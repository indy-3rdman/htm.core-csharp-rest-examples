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
//
// This example is inspired by the original python REST client example at
// https://github.com/htm-community/htm.core/blob/master/py/htm/examples/rest/client.py,
// authored by David Keeney.

using System;
using System.Threading.Tasks;

namespace SineWave
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            await ProcessAsync();
        }

        /// <summary>
        /// Setup the htm.core network and process each step until the maximum number of configures steps is reached.
        /// </summary>
        private static async Task ProcessAsync()
        {
            try
            {
                // Set the network API URL
                RestClient.Api.Init(ExampleConfiguration.BaseUrl);

                // Configure the htm.core network
                string networkId = await RestClient.Api.CreateNetworkAsync(ExampleConfiguration.NetworkConfigurationJson);

                double x = 0.00;
                for (int currentStep = 0; currentStep < ExampleConfiguration.maxStep; currentStep++)
                {
                    // get the related sine value
                    double sensorValue = Math.Sin(x);

                    // Send the sensor value into the encoder.
                    await RestClient.Api.SetEncoderValueAsync(networkId, ExampleConfiguration.EncoderRegionName, sensorValue);

                    // Execute the whole network for this step
                    await RestClient.Api.RunNetworkAsync(networkId);

                    // Get the anomaly score from the Temporal Memory
                    double anomalyScore = await RestClient.Api.GetTmAnomalyAsync(networkId, ExampleConfiguration.TmRegionName);

                    PrintResult(ExampleConfiguration.maxStep, currentStep, sensorValue, anomalyScore);

                    x += ExampleConfiguration.StepSize;
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
        /// Print the result of each step to the console
        /// </summary>
        /// <param name="maxStep"></param>
        /// <param name="currentStep"></param>
        /// <param name="sensorValue"></param>
        /// <param name="anomalyScore"></param>
        private static void PrintResult(int maxStep, int currentStep, double sensorValue, double anomalyScore)
        {
            // Note: Anomaly score will be 1 until there have been enough iterations to 'learn' the pattern.
            string iterationFormatString = $"D{maxStep.ToString().Length}";
            Console.WriteLine($"step: {currentStep.ToString(iterationFormatString)} | value: {sensorValue:+0.000;-0.000;} | anomaly: {anomalyScore:F3}");
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
