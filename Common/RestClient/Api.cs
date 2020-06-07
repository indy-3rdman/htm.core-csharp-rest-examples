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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SdrLite;

namespace RestClient
{
    /// <summary>
    /// This class handles the communication with the htm.core REST API.
    /// The REST URL and NetworkAPI configuration can be adjusted vi <see cref="ExampleConfiguration"/>
    /// </summary>
    /// <remarks>
    /// For details about the API please visit <see href="https://github.com/htm-community/htm.core/blob/master/docs/NetworkAPI_REST.md" />
    /// </remarks>
    public static class Api
    {
        private static readonly HttpClient s_client = new HttpClient();
        private static string baseUrl = "http://localhost:8050/network";

        public static void Init(string baseNetworkUrl)
        {
            baseUrl = baseNetworkUrl;
        }

        /// <summary>
        /// Creates and configures a new htm.core NetworkAPI object.
        /// </summary>
        /// <param name="json">The NetworkAPI configuration string in JSON format.</param>
        /// <returns>A <see cref="Task"> object containing the NetworkAPI Id as result.</returns>
        public static async Task<string> CreateNetworkAsync(string json)
        {
            HttpResponseMessage configResponse;

            StringContent configData = new StringContent(json, Encoding.UTF8, "application/json");

            s_client.DefaultRequestHeaders.Accept.Clear();

            try
            {
                configResponse = await s_client.PostAsync(baseUrl, configData);
            }
            catch (Exception e)
            {
                throw new RestClientException($"Could not configure network: {e.Message}");
            }

            string configResult = GetContentResult(configResponse).Trim();

            return configResult;
        }

        /// <summary>
        /// Submits the to be encoded value to the REST API, via the PUT method
        /// </summary>
        /// <param name="networkId">The htm.core NetworkAPI id</param>
        /// <param name="sensedValue"></param>
        public static async Task SetEncoderValueAsync(string networkId, string regionName, double sensedValue)
        {
            string requestUrl = $"{baseUrl}/{networkId}/region/{regionName}/param/sensedValue";
            FormUrlEncodedContent httpContent = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("data", $"{sensedValue}")
                    });
            HttpResponseMessage valueResponse = await s_client.PutAsync(requestUrl, httpContent);
            string result = GetContentResult(valueResponse).Trim();
            if (result != "OK")
            {
                throw new RestClientException($"Could not set encoder value: {result}");
            }
        }

        /// <summary>
        /// Executes all components of the network
        /// </summary>
        /// <param name="networkId">The htm.core NetworkAPI id</param>
        public static async Task RunNetworkAsync(string networkId)
        {
            string runUrl = $"{baseUrl}/{networkId}/run";
            HttpResponseMessage runResponse = await s_client.GetAsync(runUrl);
            string result = GetContentResult(runResponse).Trim();
            if (result != "OK")
            {
                throw new RestClientException($"Network run error: {result}");
            }
        }

        /// <summary>
        /// Get the output of an encoder region as SDR
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="regionName"></param>
        /// <returns>A <see cref="Task"> object containing an SDR of the encoded value as result.</returns>
        public static async Task<Sdr> GetEncoderSdrOutputAsync(string networkId, string regionName)
        {
            return await GetSdrOutputAsync(networkId, regionName, "encoded");
        }

        /// <summary>
        /// Get the active cells of a Temporal Memory region as SDR
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="regionName"></param>
        /// <returns>A <see cref="Task"> object containing an SDR of the active cells as result.</returns>
        public static async Task<Sdr> GetTmActiveCellsAsync(string networkId, string regionName)
        {
            return await GetSdrOutputAsync(networkId, regionName, "activeCells");
        }

        /// <summary>
        /// Get the winner cells of a Temporal Memory region as SDR
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="regionName"></param>
        /// <returns>A <see cref="Task"> object containing an SDR of the winner cells as result.</returns>
        public static async Task<Sdr> GetTmWinnerCellsAsync(string networkId, string regionName)
        {
            return await GetSdrOutputAsync(networkId, regionName, "predictedActiveCells");
        }

        /// <summary>
        /// Get the predictive cells of a Temporal Memory region as SDR
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="regionName"></param>
        /// <returns>A <see cref="Task"> object containing an SDR of the predictive cells as result.</returns>
        public static async Task<Sdr> GetTmPredictiveCellsAsync(string networkId, string regionName)
        {
            return await GetSdrOutputAsync(networkId, regionName, "predictiveCells");
        }

        /// <summary>
        /// Get the anomaly value of the Temporal Memory region as double
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="regionName"></param>
        /// <returns>A <see cref="Task"> object containing the anomaly double as result.</returns>
        public static async Task<double> GetTmAnomalyAsync(string networkId, string regionName)
        {
            string requestUrl = $"{baseUrl}/{networkId}/region/{regionName}/output/anomaly";
            string result = await GetResponseStringAsync(requestUrl);
            dynamic anomalyScore = JRaw.Parse(result.Trim());
            double score = anomalyScore.data[0];
            return score;
        }

        public static async Task DeleteNetworkAsync(string networkId)
        {
            string requestUrl = $"{baseUrl}/{networkId}/ALL";
            HttpResponseMessage response = await s_client.DeleteAsync(requestUrl);
            string result = GetContentResult(response).Trim();
            if (result != "OK")
            {
                throw new RestClientException($"Could not delete network: {result}");
            }
        }

        /// <summary>
        /// Gets the response string of a HTTP GET request to the specified URL.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns>A <see cref="Task"> object containing the response string as result.</returns>
        private static async Task<string> GetResponseStringAsync(string requestUrl)
        {
            HttpResponseMessage response = await s_client.GetAsync(requestUrl);
            return GetContentResult(response);
        }

        /// <summary>
        /// Gets the output of a region by its output name.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="regionName"></param>
        /// <param name="outputName"></param>
        /// <returns>A <see cref="Task"> object containing an SDR of the output as result.</returns>
        private static async Task<Sdr> GetSdrOutputAsync(string networkId, string regionName, string outputName)
        {
            string requestUrl = $"{baseUrl}/{networkId}/region/{regionName}/output/{outputName}";
            return await GetSdrAsync(requestUrl);
        }

        /// <summary>
        /// Gets the <see cref="HttpResponseMessage"/> content result, or throws an <see cref="RestClientException"/> if something went wrong.
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns>A <see cref="Task"> object containing the result string of the request as result.</returns>
        private static string GetContentResult(HttpResponseMessage responseMessage)
        {
            string result = responseMessage.Content.ReadAsStringAsync().Result;

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new RestClientException($"{(int)responseMessage.StatusCode} => {result}");
            }

            if (result.StartsWith("ERROR:"))
            {
                throw new RestClientException(result);
            }

            return result;
        }

        /// <summary>
        /// Send a HTTP GET request to an API endpoint that returns an SDR as a result.
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns>A <see cref="Task"> object containing an SDR of the output as result.</returns>
        private static async Task<Sdr> GetSdrAsync(string requestUrl)
        {
            string responseString = await GetResponseStringAsync(requestUrl);
            Sdr sdr = CreateSdrFromResponse(responseString);
            return sdr;
        }

        /// <summary>
        /// Converts the response string into an SDR
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns>A SDR object</returns>
        private static Sdr CreateSdrFromResponse(string responseString)
        {
            dynamic parsedJson = JRaw.Parse(responseString.Trim());
            string sdrTypeString = ((string)parsedJson.type).Replace("SDR", string.Empty).Trim('(', ')');
            int[] dimensions = sdrTypeString.Split(',').Select(Int32.Parse).ToArray();
            int[] sparse = parsedJson.data.ToObject<int[]>();

            SdrLite.Sdr sdr = new SdrLite.Sdr(dimensions);
            sdr.SetSparse(sparse.ToList());
            return sdr;
        }
    }
}

