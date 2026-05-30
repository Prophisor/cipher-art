using System;
using System.Collections.Generic;
using System.Text;

namespace CaesarCipher_app
{
    public static class CipherLib
    {
        public static string EncryptCaesar(string input, int shift)
        {
            if (input == null) return string.Empty;
            char[] characters = input.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                if (Char.IsLetter(characters[i]))
                {
                    char baseLetter = Char.IsUpper(characters[i]) ? 'A' : 'a';
                    int shiftedValue = (characters[i] - baseLetter + shift) % 26;
                    if (shiftedValue < 0) shiftedValue += 26;
                    characters[i] = (char)(shiftedValue + baseLetter);
                }
            }

            return new string(characters);
        }

        public static string DecryptCaesar(string input, int shift)
        {
            return EncryptCaesar(input, 26 - (shift % 26));
        }

        public static string DecryptWithShift(string input, int shift)
        {
            if (input == null) return string.Empty;
            char[] characters = input.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                if (Char.IsLetter(characters[i]))
                {
                    char baseLetter = Char.IsUpper(characters[i]) ? 'A' : 'a';
                    char decryptedChar = (char)(((characters[i] - baseLetter - shift + 26) % 26) + baseLetter);
                    characters[i] = decryptedChar;
                }
            }

            return new string(characters);
        }

        public static List<string> BruteForceAllShifts(string input)
        {
            var results = new List<string>();
            for (int shift = 1; shift <= 25; shift++)
            {
                string decryptedMessage = DecryptWithShift(input, shift);
                results.Add($"Key {shift}: {decryptedMessage}");
            }
            return results;
        }
    }
}
