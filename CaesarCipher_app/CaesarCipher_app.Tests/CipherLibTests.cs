using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CaesarCipher_app;
using System.Collections.Generic;

namespace CaesarCipher_app.Tests
{
    [TestClass]
    public class CipherLibTests
    {
        [TestMethod]
        public void EncryptCaesar_Shift3_Works()
        {
            string input = "abc XYZ";
            string expected = "def ABC";
            string actual = CipherLib.EncryptCaesar(input, 3);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecryptCaesar_Shift3_Works()
        {
            string input = "def ABC";
            string expected = "abc XYZ";
            string actual = CipherLib.DecryptCaesar(input, 3);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BruteForceAllShifts_Returns25Entries()
        {
            var results = CipherLib.BruteForceAllShifts("abc");
            Assert.AreEqual(25, results.Count);
        }

        [TestMethod]
        public void EncryptCaesar_NullInput_ReturnsEmpty()
        {
            string actual = CipherLib.EncryptCaesar(null, 5);
            Assert.AreEqual(string.Empty, actual);
        }
    }
}
