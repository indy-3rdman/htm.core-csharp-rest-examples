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

namespace SineWave
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
        /// Each step is increased by that amount.
        /// </summary>
        internal static readonly double StepSize = 0.01;

        /// <summary>
        // Number of steps until the example stops processing
        /// </summary>
        internal static readonly int maxStep = 10000;

        /// <summary>
        /// The URL of the htm.core REST. API
        /// </summary>
        /// <remarks>
        /// For details please visit <see href="https://github.com/htm-community/htm.core/blob/master/docs/NetworkAPI_REST.md" />
        /// </remarks>
        internal static readonly string BaseUrl = "http://localhost:8050/network";

        /// <summary>
        /// The column size that is used by the Spatial Pooler the Temporal Memory.
        /// </summary>
        internal static readonly int ColumnCount = 2048;

        /// <summary>
        /// Number of cells per columns that are used by the Temporal Memory algorithm.
        /// </summary>
        /// <remarks>
        /// Please visit <see href="https://3rdman.de/2020/04/hierarchical-temporal-memory-high-order-sequence-memory/">this article</see>
        /// for more details about the Temporal Memory parameters.
        /// </remarks>
        internal static readonly int CellsPerColumn = 8;

        /// <summary>
        /// Name that is used to identify the ScalarEncodeRegion used in this example
        /// </summary>
        internal static readonly string EncoderRegionName = "encoder";

        /// <summary>
        /// The name of the Spatial Pooler region that is used in this example
        /// </summary>
        internal static readonly string SpRegionName = "sp";

        /// <summary>
        /// The name of the Temporal Memory region that is used in this example
        /// </summary>
        internal static readonly string TmRegionName = "tm";

        /// <summary>
        /// A JSON string to configure a htm.core network with its different regions and connections.
        /// </summary>
        internal static readonly string NetworkConfigurationJson =
            $@"{{network: [
                    {{ addRegion: {{ name: ""{EncoderRegionName}"", type: ""RDSEEncoderRegion"", params: {{ size: 1000, sparsity: 0.02, radius: 0.03, seed: 2020, noise: 0.01 }} }} }},
                    {{ addRegion: {{ name: ""{SpRegionName}"", type: ""SPRegion"", params: {{ columnCount: {ColumnCount}, globalInhibition: true}} }} }},
                    {{ addRegion: {{ name: ""{TmRegionName}"", type: ""TMRegion"", params: {{ cellsPerColumn: {CellsPerColumn}, orColumnOutputs: true }} }} }},
                    {{ addLink: {{ src: ""{EncoderRegionName}.encoded"", dest: ""{SpRegionName}.bottomUpIn""}} }},
                    {{ addLink: {{ src: ""{SpRegionName}.bottomUpOut"", dest: ""{TmRegionName}.bottomUpIn""}} }}
            ]}}";

    }
}
