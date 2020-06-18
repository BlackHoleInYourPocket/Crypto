using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto.Hash
{
    public partial class Hash : Form
    {
        private const int BLOCK_SIZE = 16;
        private const int BLOCK_BIT_SIZE = BLOCK_SIZE * 8;
        private const int HASH_BLOCK_SIZE = BLOCK_SIZE / 4;
        private const int HASH_BLOCK_BIT_SIZE = HASH_BLOCK_SIZE * 8;
        private List<String> HashFile = new List<string>();
        string filePath = string.Empty;
        public int paralelcount = 0;
        public Hash()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (inputText.Text.Equals(""))
            {
                Message(Messages.NullInputWarning, MessagesType.Warning);
                return;
            }
            StartHashing();
        }

        #region Hashing
        private void StartHashing()
        {
            try
            {
                string[] lines = File.ReadAllLines("Hash.txt");
                foreach (string line in lines)
                {
                    HashFile.Add(line);
                }
            }
            catch (IOException e)
            {
                Message(Messages.ReadFileError, MessagesType.Error);
                return;
            }
            string text = string.Empty;
            string result = string.Empty;
            using (BinaryReader reader = new BinaryReader(File.Open(inputText.Text, FileMode.Open)))
            {
                text = reader.ReadString();
                reader.Close();
            }
            int taskNumber = (text.Length / 100000) + 1;
            List<String> hashresults = new List<string>();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < taskNumber; i++)
            {
                if (taskNumber - 1 == i)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        string hash = ParallelHash(text, paralelcount * 100000, text.Length);
                        hashresults.Add(hash);
                    }));
                }
                else
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        string hash = ParallelHash(text, paralelcount * 100000, text.Length);
                        paralelcount++;
                        hashresults.Add(hash);
                    }));
                }
            }

            Task.WaitAll(tasks.ToArray());
            result = hashresults[0];
            for (int i = 1; i < hashresults.Count; i++)
            {
                result = XorOperator(result, hashresults[i]);
            }
            result = BinaryToString(result);
            hashText.Text = result;
        }

        private string ParallelHash(string text, int startPos, int endPos)
        {
            string binaryHash = String.Empty;
            string lastHash = String.Empty;
            string hash = String.Empty;
            bool flag = false;
            for (int i = startPos; i < endPos; i += BLOCK_SIZE)
            {
                if (flag) break;
                string block = "";
                try
                {
                    block = text.Substring(i, BLOCK_SIZE);
                }
                catch (ArgumentOutOfRangeException)
                {
                    block = text.Substring(i);
                    flag = true;
                }
                string bit = ConvertToBit(block);

                if (bit.Equals(""))
                {
                    Message(Messages.UnexpectedError, MessagesType.Error);
                    return "";
                }
                if (bit.Length < BLOCK_BIT_SIZE)
                {
                    bit = bit.PadRight(BLOCK_BIT_SIZE, '0');
                }
                else if (bit.Length > BLOCK_BIT_SIZE)
                {
                    bit = bit.Substring(0, BLOCK_BIT_SIZE);
                }

                List<String> hashBlocks = new List<string>();
                for (int j = 0; j < bit.Length; j += HASH_BLOCK_BIT_SIZE)
                {
                    try
                    {
                        hashBlocks.Add(bit.Substring(j, HASH_BLOCK_BIT_SIZE));
                    }
                    catch (Exception e)
                    {
                        Message(Messages.UnexpectedError, MessagesType.Error);
                        return "";
                    }
                }
                binaryHash = CreateHash(hashBlocks);
                if (!lastHash.Equals(String.Empty))
                {
                    hash = XorOperator(binaryHash, lastHash);
                }
                else
                {
                    hash = binaryHash;
                }
                lastHash = binaryHash;
            }
            return lastHash;
        }
        private string ConvertToBit(string block)
        {
            var builder = new StringBuilder();
            try
            {
                foreach (char c in block)
                    builder.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            catch (Exception)
            {
                return "";
            }
            return builder.ToString();
        }

        private string CreateHash(List<String> hashBlocks)
        {
            int[] shifts = new int[] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14,
                20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11,
                16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
            string A = hashBlocks[0];
            string B = hashBlocks[1];
            string C = hashBlocks[2];
            string D = hashBlocks[3];
            int g = 0;
            string func = String.Empty;
            for (int i = 0; i < 64; i++)
            {
                if (i <= 15)
                {
                    func = OrOperator(AndOperator(B, C), AndOperator(NotOperator(B), D));
                    g = i;
                }
                if (i > 15 && i <= 31)
                {
                    func = OrOperator(AndOperator(D, B), AndOperator(NotOperator(D), C));
                    g = ((5 * i) + 1) % 16;
                }
                if (i > 31 && i <= 47)
                {
                    func = XorOperator(XorOperator(B, C), D);
                    g = ((3 * i) + 5) % 16;
                }
                if (i > 47 && i <= 63)
                {
                    func = XorOperator(C, OrOperator(B, NotOperator(D)));
                    g = ((7 * i)) % 16;
                }
                string tempA = A;
                string tempB = B;
                string tempC = C;
                string tempD = D;
                func = XorOperator(func, A);
                func = XorOperator(func, HashFile[g]);
                A = tempD;
                D = tempC;
                C = tempB;
                for (int j = 0; j < shifts[i]; j++)
                {
                    func = ShiftLeft(func);
                }
                B = func;
            }
            string result = String.Concat(A, B, C, D);
            for (int i = 0; i < Count1s(result); i++)
            {
                result = ShiftLeft(result);
            }
            return result;
        }
        public string BinaryToString(string data)
        {
            return string.Join("", Enumerable.Range(0, data.Length / 8)
                        .Select(i => Convert.ToByte(data.Substring(i * 8, 8), 2)
                        .ToString("X2")));
        }
        private int Count1s(string bits)
        {
            return bits.Count(x => x == '1');
        }
        private int Count0s(string bits)
        {
            return bits.Count(x => x == '0');
        }
        #endregion

        #region Operators
        public string ShiftLeft(string data)
        {
            string retVal = String.Empty;
            retVal += data[data.Length - 1];
            for (int i = 0; i < data.Length - 1; i++)
            {
                retVal += data[i];
            }
            return retVal;
        }
        public string ShiftRight(string data)
        {
            string retVal = String.Empty;
            for (int i = 0; i < data.Length - 1; i++)
            {
                retVal += data[i];
            }
            retVal += data[data.Length - 1];
            return retVal;
        }

        public string OrOperator(string first, string second)
        {
            string retVal = String.Empty;
            for (int i = 0; i < first.Length; i++)
            {
                retVal += (char)(first.ToCharArray()[i] | second.ToCharArray()[i]);
            }
            return retVal;
        }

        public string AndOperator(string first, string second)
        {
            string retVal = String.Empty;
            for (int i = 0; i < first.Length; i++)
            {
                retVal += (char)(first.ToCharArray()[i] & second.ToCharArray()[i]);
            }
            return retVal;
        }
        public string XorOperator(string first, string second)
        {
            string or = OrOperator(first, second);
            string and = AndOperator(first, second);
            string andNot = NotOperator(and);
            string orandandnot = AndOperator(or, andNot);
            return orandandnot;
        }
        public string NotOperator(string data)
        {
            string retVal = String.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Equals('0')) retVal += '1';
                else retVal += 0;
            }
            return retVal;
        }
        #endregion

        #region Log
        private void Message(Messages messages, MessagesType messagesType)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])messages
                        .GetType()
                        .GetField(messages.ToString())
                        .GetCustomAttributes(typeof(DescriptionAttribute), false);
            string messagesString = attributes.Length > 0 ? attributes[0].Description : string.Empty;

            MessageBoxIcon icon = MessageBoxIcon.None;
            string title = "";
            switch (messagesType)
            {
                case MessagesType.Warning:
                    icon = MessageBoxIcon.Warning;
                    title = "Warning";
                    break;
                case MessagesType.Error:
                    icon = MessageBoxIcon.Error;
                    title = "Error";
                    break;
                case MessagesType.Information:
                    icon = MessageBoxIcon.Information;
                    title = "Information";
                    break;
            }
            MessageBox.Show(messagesString, title, MessageBoxButtons.OK, icon);
        }
        public enum Messages
        {
            [Description("Please fill the input.")]
            NullInputWarning = 1,
            [Description("Something went wrong")]
            UnexpectedError = 2,
            [Description("Hash.txt file can't read. Check file.")]
            ReadFileError = 3,
        }
        public enum MessagesType
        {
            Warning = 1,
            Error = 2,
            Information = 3
        }
        #endregion

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                inputText.Text = openFileDialog1.FileName;
            }
        }
    }
}
