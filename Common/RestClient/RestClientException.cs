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

namespace RestClient
{
    /// <summary>
    /// Used for generic handling of errors related to the communication with, and results of the htm.core REST API.
    /// </summary>
    [Serializable]
    public class RestClientException : Exception
    {
        public RestClientException()
        {
        }

        public RestClientException(string message)
            : base($"REST ERROR: {message}")
        {
        }

        public RestClientException(string message, Exception inner)
            : base($"REST ERROR: {message}", inner)
        {
        }

        protected RestClientException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
