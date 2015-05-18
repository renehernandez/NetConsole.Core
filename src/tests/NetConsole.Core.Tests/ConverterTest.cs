using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Converters;
using NUnit.Core;
using NUnit.Framework;

namespace NetConsole.Core.Tests
{
    [TestFixture]
    public class ConverterTest
    {
        [Test]
        public void Test_TryStringSuccess()
        {
            // Arrange
            object str = "hello";

            // Act
            var tup = Converter.TryString(str);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual("hello", tup.Item2);
        }

        [Test]
        public void Test_TryStringFail()
        {
            // Arrange
            object obj = true;

            // Act
            var tup = Converter.TryString(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_ObjectTryByteSuccess()
        {
            // Arrange
            object obj = (byte) 200;

            // Act
            var tup = Converter.TryByte(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(200, tup.Item2);
        }

        [Test]
        public void Test_StringTryByteSuccess()
        {
            // Arrange
            object obj = "20";

            // Act
            var tup = Converter.TryByte(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(20, tup.Item2);
        }

        [Test]
        public void Test_ObjectTryByteFail()
        {
            // Arrance
            object obj = 18.0;

            // Act
            var tup = Converter.TryByte(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_StringTryByteFail()
        {
            // Arrange
            object obj = "halo20";

            // Act
            var tup = Converter.TryByte(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_ObjectTrySbyteSuccess()
        {
            // Arrange
            object obj = (sbyte)-128;

            // Act
            var tup = Converter.TrySbyte(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(-128, tup.Item2);
        }

        [Test]
        public void Test_StringTrySbyteSuccess()
        {
            // Arrange
            object obj = "43";

            // Act
            var tup = Converter.TrySbyte(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(43, tup.Item2);
        }

        [Test]
        public void Test_ObjectTrySbyteFail()
        {
            // Arrange
            object obj = 2500;

            // Act
            var tup = Converter.TrySbyte(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_StringTrySbyteFail()
        {
            // Arrange
            object obj = "paifbv";

            // Acts
            var tup = Converter.TrySbyte(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_ObjectTryShortSuccess()
        {
            // Arrange
            object obj = (short)0xFFF;

            // Act
            var tup = Converter.TryShort(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual((1 << 12) - 1, tup.Item2);
        }

        [Test]
        public void Test_StringTryShortSuccess()
        {
            // Arrange
            object obj = "-02578";

            // Act
            var tup = Converter.TryShort(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(-2578, tup.Item2);
        }

        [Test]
        public void Test_ObjectTryShortFail()
        {
            // Arrange
            object obj = -02578;

            // Act
            var tup = Converter.TryShort(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_StringTryShortFail()
        {
            // Arrange
            object obj = "123456789";

            // Act
            var tup = Converter.TryShort(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_ObjectTryObjectSuccess()
        {
            // Arrange
            object obj = new List<int>();

            // Act
            var tup = Converter.TryObject(typeof(List<int>), obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(obj, tup.Item2);
        }

        [Test]
        public void Test_ObjectTryObjectFail()
        {
            // Arrange
            object obj = new HashSet<int>();

            // Act
            var tup = Converter.TryObject(typeof(List<int>), obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_ObjectTryDateTimeSuccess()
        {
            // Arrange
            object obj = DateTime.Now;

            // Act
            var tup = Converter.TryDateTime(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(obj, tup.Item2);
        }

        [Test]
        public void Test_StringTryDateTimeSuccess()
        {
            // Arrange
            object obj = "1878-12-01";

            // Act
            var tup = Converter.TryDateTime(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(DateTime.Parse(obj.ToString()), tup.Item2);
        }

        [Test]
        public void Test_ObjectTryDateTimeFail()
        {
            // Arrange
            object obj = DateTime.Now.DayOfWeek;

            // Act
            var tup = Converter.TryDateTime(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_StringTryDateTimeFail()
        {
            // Arrange
            object obj = "-2004-12,01";

            // Act
            var tup = Converter.TryDateTime(obj);

            // Assert
            Assert.IsFalse(tup.Item1);
            Assert.IsNull(tup.Item2);
        }

        [Test]
        public void Test_StringTryDecimalSuccess()
        {
            // Arrange
            object obj = "-203952.7298473529";

            // Act
            var tup = Converter.TryDecimal(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(Decimal.Parse(obj.ToString()), tup.Item2);
        }

        [Test]
        public void Test_ObjectTryDecimalSuccess()
        {
            // Arrange
            object obj = new decimal(-23 << 20);

            // Act
            var tup = Converter.TryDecimal(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(obj, tup.Item2);
        }

        [Test]
        public void Test_ObjectTryBoolSuccess()
        {
            // Arrange
            object obj = true;

            // Act
            var tup = Converter.TryBool(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(obj, tup.Item2);
        }

        [Test]
        public void Test_StringTryBoolSuccess()
        {
            // Arrange
            object obj = "False";

            // Act
            var tup = Converter.TryBool(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual(false, tup.Item2);
        }

        [Test]
        public void Test_ObjectTryCharSuccess()
        {
            // Arrange
            object obj = 'c';

            // Act
            var tup = Converter.TryChar(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual('c', tup.Item2);
        }

        [Test]
        public void Test_StringTryCharSuccess()
        {
            // Arrange
            object obj = "c";

            // Act
            var tup = Converter.TryChar(obj);

            // Assert
            Assert.IsTrue(tup.Item1);
            Assert.AreEqual('c', tup.Item2);
        }


    }
}
