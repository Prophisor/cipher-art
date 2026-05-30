using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace CaesarCipher_app
{
    public partial class CaesarCipher : Form
    {
        // Der Schlüssel für die symmetrische Verschlüsselung (AES erfordert einen 128-, 192- oder 256-Bit-Schlüssel)
        string key = "0123456789ABCDEF"; // Ein 128-Bit-Schlüssel mit Sonderzeichen, 16 Zeichen lang
        //string key = "0$2#45&78A!CD*F01234%67"; // Ein 192-Bit-Schlüssel mit Sonderzeichen, 24 Zeichen lang
        string hashMD5 = "Password@2023$";
        public CaesarCipher()
        {
            InitializeComponent();
            CombiboxAddValues();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Caeser Cipher")
            {
                txtVerschluesselt.Text = CipherLib.EncryptCaesar(txtKlartext.Text, (int)numKey.Value);
            }
            else if (comboBox1.Text == "Substitution Cipher")
            {
                SubstitutionCipher(txtKlartext.Text);
            }
            else if (comboBox1.Text == "Matrix Cipher")
            {
                string[,] matrix = GenerateMatrixAlphabet();
                PrintMatrix(matrix);
                string[,] matrix2 = GenerateMatrixKlartext(txtKlartext.Text);
                PrintMatrix(matrix2);
                EncrptMatrix(matrix, matrix2);
            }
            else if (comboBox1.Text == "AES Cipher")
            {

                // Verschlüsseln des Klartextes
                byte[] encryptedBytes = EncryptStringToBytes_Aes(txtKlartext.Text, txthash.Text);
                string encryptedText = Convert.ToBase64String(encryptedBytes);
                // Ausgabe des verschlüsselten Textes
                Console.WriteLine("Verschlüsselter Text: " + encryptedText);
                txtVerschluesselt.Text = encryptedText;
            }
            else if (comboBox1.Text == "Base64 Cipher")
            {
                txtVerschluesselt.Text = EncryptBase64(txtKlartext.Text);
            }
            else if (comboBox1.Text == "MD5 Hash Cipher")
            {
                txtVerschluesselt.Text = EncryptMD5(txtKlartext.Text);
            }
            else if (comboBox1.Text == "MD5 Cipher")
            {
                txtVerschluesselt.Text = EncryptMD5_2(txtKlartext.Text);
            }
            else if (comboBox1.Text == "Playfair Cipher")
            {
                string[] verschluesselt = Playfair(txtKlartext.Text);
                txtVerschluesselt.Text = string.Join(" ", verschluesselt);

            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Caeser Cipher")
            {
                if ((int)numKey.Value == 0)
                {
                    var decryptedMessages = CipherLib.BruteForceAllShifts(txtVerschluesselt.Text);
                    txtKlartext.Text = string.Join(Environment.NewLine, decryptedMessages);
                }
                else
                {
                    txtKlartext.Text = CipherLib.DecryptCaesar(txtVerschluesselt.Text, (int)numKey.Value);
                }
            }
            else if (comboBox1.Text == "Matrix Cipher")
            {
                string[,] matrix = GenerateMatrixAlphabet();
                DecryptMatrix(txtVerschluesselt.Text, matrix);
            }
            else if (comboBox1.Text == "AES Cipher")
            {

                string decryptedText = DecryptStringFromBytes_Aes(Convert.FromBase64String(txtVerschluesselt.Text), txthash.Text);
                Console.WriteLine("Entschlüsselter Text: " + decryptedText);
                txtKlartext.Text = decryptedText;
            }
            else if (comboBox1.Text == "Base64 Cipher")
            {
                txtKlartext.Text = DecryptBase64(txtVerschluesselt.Text);
            }
            else if (comboBox1.Text == "MD5 Hash Cipher")
            {
                txtKlartext.Text = DecryptMD5(txtVerschluesselt.Text);
            }
            else if (comboBox1.Text == "MD5 Cipher")
            {
                txtKlartext.Text = DecryptMD5_2(txtVerschluesselt.Text);
            }
            else if (comboBox1.Text == "Playfair Cipher")
            {
                string entschluesselt = DecryptPlayfair(txtVerschluesselt.Text.Replace(" ", ""));
                txtKlartext.Text = string.Join(" ", entschluesselt);
            }


        }

        private void btnDictionary_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Caeser Cipher")
            {
                Dictionary(txtVerschluesselt.Text);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtKlartext.Clear();
            txtVerschluesselt.Clear();
        }

        private string EncryptCaeserCipher(string input, int shift)
        {
            char[] characters = input.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                if (Char.IsLetter(characters[i]))
                {
                    // char baseLetter = Char.IsUpper(characters[i]) ? 'A' : 'a';
                    char baseLetter;

                    if (Char.IsUpper(characters[i]))
                    {
                        baseLetter = 'A';
                    }
                    else
                    {
                        baseLetter = 'a';
                    }

                    // characters[i] = (char)(((characters[i] - baseLetter + shift) % 26) + baseLetter);
                    int shiftedValue = (characters[i] - baseLetter + shift) % 26;
                    characters[i] = (char)(shiftedValue + baseLetter);

                }
            }
            string ausgabe = new string(characters);
            if (!string.IsNullOrEmpty(txtKlartext.Text))
            {
                txtVerschluesselt.Text = ausgabe;
            }
            else
            {
                txtKlartext.Text = ausgabe;
            }

            return new string(characters);
            //string text = string.Concat(characters);
            //return text;
        }

        private string DecryptCaeserCipher(string input, int shift)
        {
            return EncryptCaeserCipher(input, 26 - shift); // Umkehrung der Verschlüsselung
        }

        private string Decrypt2(string input, int shift)
        {
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
            string ausgabe = new string(characters);
            if (!string.IsNullOrEmpty(txtVerschluesselt.Text))
            {
                txtKlartext.Text = ausgabe;
            }
            return new string(characters);
        }

        private void Dictionary(string message)
        {
            List<string> decryptedMessages = new List<string>();

            // Pfad zur Wörterbuchdatei
            string dictionaryFilePath = "words.txt";

            if ((int)numKey.Value == 0)
            {
                for (int shift = 1; shift <= 25; shift++)
                {
                    string decryptedMessage = Decrypt2(message, shift);
                    //Console.WriteLine($"Key {shift}: {decryptedMessage}");
                    decryptedMessages.Add(decryptedMessage);
                }
            }

            // Überprüfen, ob die Wörterbuchdatei existiert
            if (File.Exists(dictionaryFilePath))
            {
                // Wörterbuchdatei zeilenweise lesen
                string[] lines = File.ReadAllLines(dictionaryFilePath);
                txtKlartext.Text = Environment.NewLine;
                // Sätze in Wörter aufteilen und jedes Wort im Wörterbuch suchen
                foreach (string decryptedMessage in decryptedMessages)
                {
                    string[] sentences = decryptedMessage.Split('.'); // Annahme: Sätze sind durch Punkte getrennt

                    foreach (string sentence in sentences)
                    {
                        string[] words = sentence.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string word in words)
                        {
                            bool wordFound = false;
                            string searchTerm = word.ToLower();

                            // Durchsuchen des Wörterbuchs nach dem gesuchten Wort
                            foreach (string line in lines)
                            {
                                if (line.ToLower() == searchTerm)
                                {
                                    // Console.WriteLine($"Das Wort '{searchTerm}' wurde im Wörterbuch gefunden: {line}");
                                    txtKlartext.Text += line + Environment.NewLine; // Ergebnis zur Anzeige hinzufügen
                                    wordFound = true;
                                    break; // Wenn du nach dem ersten gefundenen Wort suchen möchtest, kannst du das Break verwenden
                                }
                            }

                            if (!wordFound)
                            {
                                // Console.WriteLine($"Das Wort '{searchTerm}' wurde nicht im Wörterbuch gefunden.");
                            }
                        }
                    }
                }
            }
        }

        private void CombiboxAddValues()
        {
            comboBox1.Items.Add("Caeser Cipher");
            comboBox1.Items.Add("Substitution Cipher");
            comboBox1.Items.Add("Matrix Cipher");
            comboBox1.Items.Add("Playfair Cipher");
            comboBox1.Items.Add("Base64 Cipher");
            comboBox1.Items.Add("MD5 Cipher");
            comboBox1.Items.Add("MD5 Hash Cipher");
            comboBox1.Items.Add("AES Cipher");


        }

        private string SubstitutionCipher(string text)
        {
            string Alphabet = "abcdefghiklmnopqrstuvwxyz";

            // Ein Array, um die eindeutigen Buchstaben zu speichern
            char[] buchstaben = new char[text.Length]; // Wir nehmen an, dass es nur 26 Buchstaben gibt
            int anzahlEindeutig = 0; // Um die Anzahl der eindeutigen Buchstaben zu zählen

            // Wir gehen durch jeden Buchstaben im Text
            foreach (char c in text)
            {
                // Wir ignorieren Leerzeichen
                if (c != ' ')
                {
                    // Überprüfen, ob der Buchstabe bereits im Array ist
                    bool istEindeutig = true;
                    for (int i = 0; i < anzahlEindeutig; i++)
                    {
                        if (buchstaben[i] == c)
                        {
                            istEindeutig = false;
                            break;
                        }
                    }
                    // Wenn der Buchstabe eindeutig ist, fügen wir ihn hinzu
                    if (istEindeutig)
                    {
                        buchstaben[anzahlEindeutig] = c;
                        anzahlEindeutig++;
                    }
                }
            }

            // Wir geben die Anzahl und die Liste der eindeutigen Buchstaben aus
            // Console.WriteLine("Anzahl der eindeutigen Buchstaben: " + anzahlEindeutig);
            //Console.Write("Liste der eindeutigen Buchstaben: ");
            StringBuilder stb = new StringBuilder();

            for (int i = 0; i < anzahlEindeutig; i++)
            {
                // Console.Write(buchstaben[i]);
                stb.Append(buchstaben[i]);
            }

            string result = stb.ToString();

            foreach (char letter in Alphabet)
            {
                if (!result.ToLower().Contains(letter.ToString().ToLower()))
                {
                    result += letter;
                }
            }

            //Console.WriteLine("\r\nDer Text sieht so aus: " + result);

            txtVerschluesselt.Text = result;
            return result;
        }

        private string[,] GenerateMatrixAlphabet()
        {
            string[,] matrix = new string[5, 5];
            string alphabet = "abcdefghiklmnopqrstuvwxyz";
            int index = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    // Füge das aktuelle Zeichen des Alphabets in die Matrix ein
                    matrix[i, j] = alphabet[index].ToString();

                    // Gehe zum nächsten Buchstaben im Alphabet
                    index++;

                    // Stelle sicher, dass der Index innerhalb der Grenzen des Alphabets bleibt
                    if (index >= alphabet.Length)
                    {
                        index = 0;
                    }
                }
            }
            return matrix;
        }

        private string[,] GenerateMatrixKlartext(string text)
        {
            string[,] matrix = new string[5, 5];
            string alphabet2 = SubstitutionCipher(text).Replace("j", "i");
            string alphabet = SubstitutionCipher(text).ToLower();
            Console.WriteLine(alphabet);
            Console.WriteLine(alphabet2);
            int index = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    // Füge das aktuelle Zeichen des Alphabets in die Matrix ein
                    matrix[i, j] = alphabet[index].ToString();

                    // Gehe zum nächsten Buchstaben im Alphabet
                    index++;

                    // Stelle sicher, dass der Index innerhalb der Grenzen des Alphabets bleibt
                    if (index >= alphabet.Length)
                    {
                        index = 0;
                    }
                }
            }
            return matrix;
        }

        public void PrintMatrix(string[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("row " + (i + 1) + " : ");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    // Ausgabe des eingefügten Buchstabens
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void EncrptMatrix(string[,] matrixAlphabet, string[,] matrixKlartext)
        {
            txtVerschluesselt.Clear();


            for (int i = 0; i < matrixKlartext.GetLength(0); i++)
            {
                for (int j = 0; j < matrixKlartext.GetLength(1); j++)
                {
                    for (int k = 0; k < matrixAlphabet.GetLength(0); k++)
                    {
                        for (int l = 0; l < matrixAlphabet.GetLength(1); l++)
                        {
                            if (matrixKlartext[i, j] == matrixAlphabet[k, l])
                            {
                                // Ausgabe die Position der Buchstaben
                                Console.WriteLine("die Position von " + matrixKlartext[i, j] + " ist: " + k + l);
                                //txtVerschluesselt.Text += "die Position von " + matrixKlartext[i, j] + " ist: " + k + l + Environment.NewLine; 
                                txtVerschluesselt.Text += k.ToString() + l.ToString() + " ";

                            }

                        }

                    }
                }

            }
        }

        public void DecryptMatrix(string encryptedText, string[,] matrixAlphabet)
        {
            string[] positions = encryptedText.Trim().Split(' '); // Splitte die verschlüsselten Positionen

            foreach (string pos in positions)
            {
                int row = int.Parse(pos[0].ToString()); // Extrahiere die Zeilennummer
                int col = int.Parse(pos[1].ToString()); // Extrahiere die Spaltennummer

                // Überprüfe, ob die Position innerhalb der Matrixgrenzen liegt
                if (row < matrixAlphabet.GetLength(0) && col < matrixAlphabet.GetLength(1))
                {
                    // Gib den entschlüsselten Buchstaben aus der Matrix aus
                    Console.Write(matrixAlphabet[row, col]);
                    txtKlartext.Text += matrixAlphabet[row, col];
                }
                else
                {
                    Console.Write(" "); // Wenn die Position außerhalb der Matrix liegt, gib ein Leerzeichen aus
                }
            }
            Console.WriteLine();
        }

        private byte[] EncryptStringToBytes_Aes(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(txthash.Text); // Festlegen des Schlüssels für AES
                aesAlg.Mode = CipherMode.ECB; // Betriebsmodus (Electronic Codebook - ECB, hier nur zu Demonstrationszwecken)
                aesAlg.Padding = PaddingMode.PKCS7; // Padding-Modus
                // Erstellen des Verschlüsselungstransformators
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] encrypted;

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText); // Schreibe den Klartext in den CryptoStream
                        }
                        encrypted = msEncrypt.ToArray(); // Verschlüsselte Daten aus dem MemoryStream erhalten
                    }
                }
                return encrypted;
            }
        }

        private string DecryptStringFromBytes_Aes(byte[] cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key); // Festlegen des Schlüssels für AES
                aesAlg.Mode = CipherMode.ECB; // Betriebsmodus (ECB, hier nur zu Demonstrationszwecken)
                aesAlg.Padding = PaddingMode.PKCS7; // Padding-Modus

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV); // Erstellen des Entschlüsselungstransformators

                string plaintext = null;

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd(); // Lesen und Entschlüsseln der Daten aus dem CryptoStream
                        }
                    }
                }
                return plaintext;
            }
        }

        private string EncryptBase64(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private string DecryptBase64(string decryptText)
        {
            //info: die verschlüsselte Zeichen soll ein vielfach von 4 sein.
            // das heißt die länge durch 4 muss teilbar sein. wenn nicht wir es mit ein oder zwei "=" ergänzt.
            // dass es am Ende die verschlüsselte Länge mit  "=" oder "==" teilbar durch 4 kann.
            byte[] base64EncodedBytes = Convert.FromBase64String(decryptText);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private string EncryptMD5(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);

            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = MD5.ComputeHash(Encoding.UTF8.GetBytes(txthash.Text));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform cryptoTransform = tripleDES.CreateEncryptor();
            byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);

        }

        private string DecryptMD5(string input)
        {
            byte[] data = Convert.FromBase64String(input);

            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = MD5.ComputeHash(Encoding.UTF8.GetBytes(txthash.Text));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform cryptoTransform = tripleDES.CreateDecryptor();
            byte[] result = cryptoTransform.TransformFinalBlock(data, 0, data.Length);

            return Encoding.UTF8.GetString(result);

        }

        public string EncryptMD5_2(string input)
        {
            using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(input);
                b = MD5.ComputeHash(b);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (byte x in b)

                    sb.Append(x.ToString("x2"));

                return sb.ToString();
            }
        }
        public string DecryptMD5_2(string input)
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(input);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }



        private string[] Playfair(string text)
        {
            bool found = false;
            bool found1 = false;
            bool found2 = false;
            int first1 = 0;
            int secound1 = 0;
            int first2 = 0;
            int secound2 = 0;
            List<string> listppare = new List<string>();
            string[,] matrix = GenerateMatrixKlartext("extrawurst");
            PrintMatrix(matrix);
            string ohneLeerzeichen = text.Replace(" ", "").Replace(".", "").Replace("!", "").Replace("?", "").ToLower();

            //if (ohneLeerzeichen.Length % 2 != 0)
            //{
            //    ohneLeerzeichen += "x";
            //}

            string[] paare = Regex.Split(ohneLeerzeichen, "(?<=\\G..)");


            int length = paare.Length;

            for (int i = 0; i < paare.Length; i++)
            {
                if (paare[i].Length != 0)
                {
                    if (paare[i].Length == 1)
                    {
                        paare[i] += "x";
                    }
                    else if (paare[i][0] == paare[i][1])
                    {
                        paare[i] = paare[i][0] + "x";
                    }
                }
                if (paare[i] == "")
                {
                    Array.Resize(ref paare, paare.Length - 1);
                }

            }

            // diagonal austauschen
            for (int k = 0; k < paare.Length; k++)
            {
                found = false;
                found1 = false;
                found2 = false;
                first1 = 0;
                secound1 = 0;
                first2 = 0;
                secound2 = 0;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (paare[k][0].ToString().Contains(matrix[i, j]))
                        {
                            found1 = true;
                            first1 = i;
                            secound1 = j;
                        }
                        if (paare[k][1].ToString().Contains(matrix[i, j]))
                        {
                            found2 = true;
                            first2 = i;
                            secound2 = j;
                        }
                        if (found1 && found2)
                        {


                            //  in der gleichen Zeile
                            if (first1 == first2)
                            {
                                if (secound1 != 4 && secound2 != 4)
                                {
                                    found = true;
                                    paare[k] = matrix[first1, secound1 + 1];
                                    paare[k] += matrix[first2, secound2 + 1];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                                else if (secound2 == 4)
                                {
                                    found = true;
                                    paare[k] = matrix[first1, secound1 + 1];
                                    paare[k] += matrix[first2, secound2 - secound2];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                                else
                                {
                                    found = true;
                                    paare[k] = matrix[first1, secound1 - secound1];
                                    paare[k] += matrix[first2, secound2 + 1];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                            }

                            // in der gleichen Spalte
                            if (secound1 == secound2)
                            {
                                if (first1 != 4 && first2 != 4)
                                {
                                    found = true;
                                    paare[k] = matrix[first1 + 1, secound1];
                                    paare[k] += matrix[first2 + 1, secound2];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                                else if (first2 == 4)
                                {
                                    found = true;
                                    paare[k] = matrix[first1 + 1, secound1];
                                    paare[k] += matrix[first2 - first2, secound2];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                                else
                                {
                                    found = true;
                                    paare[k] = matrix[first1 - first1, secound1];
                                    paare[k] += matrix[first2 + 1, secound2];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                            }
                            // in der Diagonalen
                            else
                            {
                                if (secound1 > secound2)
                                {
                                    found = true;
                                    paare[k] = matrix[first1, secound2];
                                    paare[k] += matrix[first2, secound1];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                                else if ((secound2 > secound1) && (first1 > first2))
                                {
                                    found = true;
                                    paare[k] = matrix[first1 - 1, secound1];
                                    paare[k] += matrix[first2 + 1, secound2];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }
                                else
                                {
                                    found = true;
                                    paare[k] = matrix[first2, secound1];
                                    paare[k] += matrix[first1, secound2];
                                    listppare.Add(paare[k]);
                                    paare[k] = "00";
                                    break;
                                }

                            }
                        }
                        if (found)
                        {
                            break;
                        }

                    }
                    if (found)
                    {
                        break;
                    }
                }
            }


            //Console.WriteLine(listppare);
            paare = listppare.ToArray();
            return paare;
        }
        private string DecryptPlayfair(string text)
        {
            string[,] matrix = GenerateMatrixKlartext("extrawurst");
            List<string> pairs = new List<string>();

            for (int i = 0; i < text.Length - 1; i += 2)
            {
                string pair = text.Substring(i, 2);
                pairs.Add(pair);
            }

            StringBuilder decryptedText = new StringBuilder();

            foreach (string pair in pairs)
            {
                int[] firstIndex = new int[2];
                int[] secondIndex = new int[2];

                bool foundFirst = false;
                bool foundSecond = false;

                // Finde Indizes des ersten Buchstabens im Paar in der Matrix
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j].Contains(pair[0]))
                        {
                            firstIndex[0] = i;
                            firstIndex[1] = j;
                            foundFirst = true;
                            break;
                        }
                    }
                    if (foundFirst) break;
                }

                // Finde Indizes des zweiten Buchstabens im Paar in der Matrix
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j].Contains(pair[1]))
                        {
                            secondIndex[0] = i;
                            secondIndex[1] = j;
                            foundSecond = true;
                            break;
                        }
                    }
                    if (foundSecond) break;
                }

                // Entschlüssele die Buchstaben
                if (firstIndex[0] == secondIndex[0]) // In derselben Zeile
                {
                    int newFirst = (firstIndex[1] - 1 + 5) % 5;
                    int newSecond = (secondIndex[1] - 1 + 5) % 5;
                    decryptedText.Append(matrix[firstIndex[0], newFirst]);
                    decryptedText.Append(matrix[secondIndex[0], newSecond]);
                }
                else if (firstIndex[1] == secondIndex[1]) // In derselben Spalte
                {
                    int newFirst = (firstIndex[0] - 1 + 5) % 5;
                    int newSecond = (secondIndex[0] - 1 + 5) % 5;
                    decryptedText.Append(matrix[newFirst, firstIndex[1]]);
                    decryptedText.Append(matrix[newSecond, secondIndex[1]]);
                }
                else // In der Diagonalen
                {
                    decryptedText.Append(matrix[firstIndex[0], secondIndex[1]]);
                    decryptedText.Append(matrix[secondIndex[0], firstIndex[1]]);
                }
            }

            return decryptedText.ToString();
        }
    }
}
