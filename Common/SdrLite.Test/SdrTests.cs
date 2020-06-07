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
using NUnit.Framework;

namespace SdrLite.Test
{
    public class SdrTests
    {
        [Test]
        public void TestOneDimensionalSparse()
        {
            List<int> sparse = new List<int> { 0, 7 };
            Sdr sut = new Sdr(new int[] { 8 });
            sut.SetSparse(sparse);

            List<int> result = sut.GetSparse();

            Assert.AreEqual(sparse, result);
        }

        [Test]
        public void TestTwoDimensionalSparse()
        {
            List<int> sparse = new List<int> { 1, 6, 8, 63 };
            Sdr sut = new Sdr(new int[] { 8, 8 });
            sut.SetSparse(sparse);

            List<int> result = sut.GetSparse();

            Assert.AreEqual(sparse, result);
        }

        [Test]
        public void TestOneDimensionalDenseFromSparse()
        {
            List<byte> expected = new List<byte> { 1, 0, 0, 0, 0, 0, 0, 1 };
            List<int> sparse = new List<int> { 0, 7 };
            Sdr sut = new Sdr(new int[] { 8 });
            sut.SetSparse(sparse);

            List<byte> result = sut.GetDense();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestTwoDimensionalDenseFromSparse()
        {
            List<byte> expected = new List<byte> { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
            List<int> sparse = new List<int> { 0, 1, 14, 15 };
            Sdr sut = new Sdr(new int[] { 8, 2 });
            sut.SetSparse(sparse);

            List<byte> result = sut.GetDense();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestOneDimensionalSparseFromDense()
        {
            List<byte> dense = new List<byte> { 1, 1, 0, 0, 0, 0, 0, 0 };
            List<int> expected = new List<int> { 0, 1 };
            Sdr sut = new Sdr(new int[] { 8 });
            sut.SetDense(dense);

            List<int> result = sut.GetSparse();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestTwoDimensionalSparseFromDense()
        {
            List<byte> dense = new List<byte> { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
            List<int> expected = new List<int> { 0, 1, 14, 15 };
            Sdr sut = new Sdr(new int[] { 8, 2 });
            sut.SetDense(dense);

            List<int> result = sut.GetSparse();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TestAt()
        {
            List<byte> dense = new List<byte> { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
            Sdr sut = new Sdr(new int[] { 2, 8 });
            sut.SetDense(dense);

            byte element0 = sut.At(new List<int> { 0, 0 });
            byte element1 = sut.At(new List<int> { 1, 0 });
            byte element2 = sut.At(new List<int> { 0, 1 });
            byte element3 = sut.At(new List<int> { 1, 1 });
            byte element14 = sut.At(new List<int> { 0, 7 });
            byte element15 = sut.At(new List<int> { 1, 7 });

            Assert.AreEqual(dense[0], element0);
            Assert.AreEqual(dense[1], element1);
            Assert.AreEqual(dense[2], element2);
            Assert.AreEqual(dense[3], element3);
            Assert.AreEqual(dense[14], element14);
            Assert.AreEqual(dense[15], element15);
        }
    }
}
