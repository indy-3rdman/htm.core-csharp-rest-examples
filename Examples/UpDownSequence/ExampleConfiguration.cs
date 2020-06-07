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

namespace UpDownSequence
{
    /// <summary>
    /// Configuration options related to the htm.core REST-/NetworkAPI and Regions used in this example
    /// </summary>
    /// <remarks>
    /// Instead of using hard-coded values for the configuration options, you might want to use
    /// <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1">appsettings.json</see>
    /// or something similar.
    /// </remarks>
    internal static class ExampleConfiguration
    {
        /// <summary>
        /// This example is using a sequence of numbers which the Temporal Memory is supposed to learn.
        /// </summary>
        internal static readonly int[] CycleArry = { 0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1 };

        /// <summary>
        /// The number of times the sequence is fed into the Temporal Memory
        /// </summary>
        /// <remarks>
        /// Since the sequence used is very simple, the TM should have learned it after the second cycle already, given the correct parameters are supplied.
        /// </remarks>
        internal static readonly int Cycles = 4;

        /// <summary>
        /// The URL of the htm.core REST. API
        /// </summary>
        /// <remarks>
        /// For details please visit <see href="https://github.com/htm-community/htm.core/blob/master/docs/NetworkAPI_REST.md" />
        /// </remarks>
        internal static readonly string BaseUrl = "http://localhost:8050/network";

        /// <summary>
        /// The column size of the encoder which is automatically forwarded to the other regions (the Temporal Memory in this case).
        /// </summary>
        internal static readonly int ColumnSize = 8;

        /// <summary>
        /// Number of cells per columns that are used by the Temporal Memory algorithm.
        /// </summary>
        /// <remarks>
        /// Please visit <see href="https://3rdman.de/2020/04/hierarchical-temporal-memory-high-order-sequence-memory/">this article</see>
        /// for more details about the Temporal Memory parameters.
        /// </remarks>
        internal static readonly int CellsPerColumn = 2;

        /// <summary>
        /// Name that is used to identify the ScalarEncodeRegion used in this example
        /// </summary>
        internal static readonly string EncoderRegionName = "encoder";

        /// <summary>
        /// The name of the Temporal Memory region that is used in this example
        /// </summary>
        internal static readonly string TmRegionName = "tm";

        /// <summary>
        /// A JSON string to configure a htm.core network with its different regions and connections.
        /// </summary>
        internal static readonly string NetworkConfigurationJson =
            $@"{{network: [
                    {{ addRegion: {{ name: ""{EncoderRegionName}"", type: ""ScalarEncoderRegion"", params: {{size: {ColumnSize}, activeBits: 1, minValue: 0, maxValue: 7 }} }} }},
                    {{ addRegion: {{ name: ""{TmRegionName}"", type: ""TMRegion"", params: {{ cellsPerColumn: {CellsPerColumn}, orColumnOutputs: false, activationThreshold: 1, minThreshold: 1, initialPermanence: 0.4}} }} }},
                    {{ addLink: {{ src: ""{EncoderRegionName}.encoded"", dest: ""{TmRegionName}.bottomUpIn""}} }}
            ]}}";

        /// <summary>
        /// The console output is paused for this number of milliseconds, before the next result is displayed.
        /// </summary>
        internal static readonly int DisplaySleepMilliseconds = 250;
    }
}
