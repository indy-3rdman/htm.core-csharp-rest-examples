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

using System.Collections.Generic;
using System.Linq;
using NumSharp;

namespace SdrLite
{
    /// <summary>
    /// A simplified SDR implementation, providing just the most necessary functionality required to work with the REST client example.
    /// </summary>
    /// <remarks>
    /// Utilizing <see href="https://github.com/SciSharp/NumSharp">NumSharp</see>, a NumPy implementation for .NET.
    /// </remarks>
    public class Sdr
    {
        private NDArray _nd;

        /// <summary>
        /// Create a new Sparse Distributed Representation based on the dimensions provided.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <remarks>
        /// Transpose is used to simulate column-major order, as currently NumSharp does not seem to have a way to set this.
        /// </remarks>
        public Sdr(int[] dimensions)
        {
            this._nd = np.transpose(np.zeros(dimensions, NPTypeCode.Byte));
        }

        /// <summary>
        /// Sets the active bits in the flattened <see cref="NDArray"/>, based on its indexes.
        /// </summary>
        /// <param name="indices"></param>
        public void SetSparse(List<int> indices)
        {
            Shape shape = this._nd.Shape;

            NDArray flatArray = np.zeros(shape, NPTypeCode.Byte).flatten();

            foreach (int index in indices)
            {
                flatArray[index] = 1;
            }

            this._nd = flatArray.reshape(shape);
        }

        /// <summary>
        /// Gets the indexes of the active bits within the flattened <see cref="NDArray"/>.
        /// </summary>
        /// <returns></returns>
        public List<int> GetSparse()
        {
            var sparseList = new List<int>();
            NDArray flatArray = this._nd.flatten();

            for (int i = 0; i < flatArray.size; i++)
            {
                if (flatArray[i] == 1)
                {
                    sparseList.Add(i);
                }
            }

            return sparseList;
        }

        /// <summary>
        /// Sets all bits in the flattened <see cref="NDArray"/> to either 0 (inactive), or 1 (active)
        /// </summary>
        /// <param name="dense">The flattened bytes list</param>
        public void SetDense(List<byte> dense)
        {
            Shape shape = this._nd.Shape;

            NDArray flatArray = np.array<byte>(dense);
            this._nd = flatArray.reshape(shape);
        }

        /// <summary>
        /// Gets all bits of the flattened <see cref="NDArray"/>.
        /// </summary>
        /// <returns>The flattened bytes list</returns>
        public List<byte> GetDense()
        {
            return this._nd.flatten().ToArray<byte>().ToList();
        }

        /// <summary>
        /// Gets the value of the byte at the coordinate position.
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns>The value of the byte at the position identified by the coordinates list.</returns>
        /// <remarks>
        /// Using transpose to simulate column-major order, as currently NumSharp does not seem to have a way to set this.
        /// </remarks>
        public byte At(List<int> coordinates)
        {
            int[] indices = coordinates.ToArray();
            NDArray ndArray = np.transpose(this._nd).GetData(indices).flatten();
            return ndArray.Data<byte>()[0];
        }

        /// <summary>
        /// Takes the dense representation of the <see cref="NDArray"/> and turns it into a string. Values are separate by comma.
        /// </summary>
        /// <returns>A string representation of all bits in the SDR.</returns>
        public string StringifyDense()
        {
            return $"[{string.Join(",", this.GetDense())}]";
        }

        /// <summary>
        /// Takes the sparse representation of the <see cref="NDArray"/> and turns it into a string. Values are separate by comma.
        /// </summary>
        /// <returns>Only the active bit indexes of the SDR as a string.</returns>
        public override string ToString()
        {
            return $"[{string.Join(",", this.GetSparse())}]";
        }
    }
}
