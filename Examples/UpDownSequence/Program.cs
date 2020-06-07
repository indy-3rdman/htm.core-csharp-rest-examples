// htm.core C# REST examples
//
// Copyright(C) 2020, Martin Kandlbinder, https://3rdman.de
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY - without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading.Tasks;

namespace UpDownSequence
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            await ProcessAsync();
        }

        /// <summary>
        /// Setup the htm.core network and kick-off processing for each cycle
        /// </summary>
        /// <returns></returns>
        private static async Task ProcessAsync()
        {
            try
            {
                RestClient.Api.Init(ExampleConfiguration.BaseUrl);
                string networkId = await RestClient.Api.CreateNetworkAsync(ExampleConfiguration.NetworkConfigurationJson);
                for (int cycleIdx = 0; cycleIdx < ExampleConfiguration.Cycles; cycleIdx++)
                {
                    await ProcessCycleAsync(networkId, cycleIdx);
                }
                await RestClient.Api.DeleteNetworkAsync(networkId);
            }
            catch (Exception e)
            {
                PrintError(e.Message);
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Process one cycle by iterating over all values of the cycleArray.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="cycleIdx"></param>
        /// <returns></returns>
        private static async Task ProcessCycleAsync(string networkId, int cycleIdx)
        {
            int stepIdx = 1;
            foreach (int sensorValue in ExampleConfiguration.CycleArry)
            {
                DisplayData result = new DisplayData { CycleIdx = cycleIdx + 1, StepIdx = stepIdx, Value = sensorValue };

                // Encode the sensor value using a scalar encoder
                await RestClient.Api.SetEncoderValueAsync(networkId, ExampleConfiguration.EncoderRegionName, sensorValue);

                // Execute one iteration of the Network
                await RestClient.Api.RunNetworkAsync(networkId);

                // Get the active, winner and predicted cells from the Temporal Memory
                result.ActiveSdr = await RestClient.Api.GetTmActiveCellsAsync(networkId, ExampleConfiguration.TmRegionName);
                result.WinnderSdr = await RestClient.Api.GetTmWinnerCellsAsync(networkId, ExampleConfiguration.TmRegionName);
                result.PredictiveSdr = await RestClient.Api.GetTmPredictiveCellsAsync(networkId, ExampleConfiguration.TmRegionName);

                // Also get the anomaly score of the TM for this run
                result.AnomalyScore = await RestClient.Api.GetTmAnomalyAsync(networkId, ExampleConfiguration.TmRegionName);

                DisplayOutput.PrintResult(result);

                stepIdx++;
            }
        }

        /// <summary>
        /// Display errors in red
        /// </summary>
        /// <param name="errorMsg"></param>
        private static void PrintError(string errorMsg)
        {
            ConsoleColor savedColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorMsg);
            Console.ForegroundColor = savedColor;
        }
    }
}
