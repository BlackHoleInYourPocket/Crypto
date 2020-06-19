using System;
using System.Collections;
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
        private List<BitArray> HashFile = new List<BitArray>();
        string filePath = string.Empty;
        public int paralelcount = -1;
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
                    HashFile.Add(new BitArray(line.Select(c => c == '1').ToArray()));
                }
            }
            catch (IOException e)
            {
                Message(Messages.ReadFileError, MessagesType.Error);
                return;
            }

            string result = string.Empty;
            BitArray hash = ReadBit(inputText.Text);
            result = BinaryToString(hash);
            hashText.Text = result;
        }
        private BitArray ReadBit(string path)
        {
            BitArray binaryHash = null;
            BitArray lastHash = null;
            BitArray hash = null;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] blocks;
                BitArray bits = null;
                using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                {
                    blocks = br.ReadBytes(BLOCK_SIZE);
                    while (blocks.Length > 0)
                    {
                        if (blocks.Length < BLOCK_SIZE)
                        {
                            List<byte> list = blocks.ToList();
                            while (list.Count < BLOCK_SIZE)
                            {
                                list.Add(0);
                            }
                            blocks = new byte[list.Count];
                            blocks = list.ToArray();
                        }
                        bits = new BitArray(blocks);
                        List<BitArray> hashBlocks = new List<BitArray>();
                        for (int j = 0; j < 4; j++)
                        {
                            try
                            {
                                BitArray addedBit = new BitArray(HASH_BLOCK_BIT_SIZE);
                                for (int i = 0; i < HASH_BLOCK_BIT_SIZE; i++)
                                {
                                    addedBit[i] = bits[i + (32 * j)];
                                }
                                hashBlocks.Add(addedBit);
                            }
                            catch (Exception e)
                            {
                                Message(Messages.UnexpectedError, MessagesType.Error);
                            }
                        }
                        binaryHash = CreateHash(hashBlocks);
                        try
                        {
                            hash = binaryHash.Xor(lastHash);
                        }
                        catch (Exception)
                        {
                            hash = binaryHash;
                        }
                        lastHash = binaryHash;
                        blocks = br.ReadBytes(BLOCK_SIZE);
                    }
                }
            }
            return hash;
        }
        private BitArray CreateHash(List<BitArray> hashBlocks)
        {
            int[] shifts = new int[] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 5, 9, 14,
                20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11,
                16, 23, 4, 11, 16, 23, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };
            BitArray A = hashBlocks[0];
            BitArray B = hashBlocks[1];
            BitArray C = hashBlocks[2];
            BitArray D = hashBlocks[3];
            int g = 0;
            BitArray func = null;
            for (int i = 0; i < 64; i++)
            {
                if (i <= 15)
                {
                    BitArray first = B.And(C);
                    BitArray second = B.Not().And(D);
                    func = first.Or(second);
                    g = i;
                }
                if (i > 15 && i <= 31)
                {
                    BitArray first = D.And(B);
                    BitArray second = D.Not().And(C);
                    func = first.Or(second);
                    g = ((5 * i) + 1) % 16;
                }
                if (i > 31 && i <= 47)
                {
                    func = B.Xor(C).Xor(D);
                    g = ((3 * i) + 5) % 16;
                }
                if (i > 47 && i <= 63)
                {
                    BitArray first = B.Or(D.Not());
                    func = C.Xor(first);
                    g = ((7 * i)) % 16;
                }
                func = func.Xor(A);
                func = func.Xor(HashFile[g]);
                A = new BitArray(D);
                D = new BitArray(C);
                C = new BitArray(B);
                func = ShiftLeft(func, shifts[i]);
                //func = func.LeftShift(shifts[i]);
                B = new BitArray(func);
            }
            BitArray result = new BitArray(128);
            for (int i = 0; i < 128; i++)
            {
                if (i >= 0 && i < 32) result[i] = A[i];
                if (i >= 32 && i < 63) result[i] = B[i % 32];
                if (i >= 64 && i < 96) result[i] = C[i % 32];
                if (i >= 96 && i < 128) result[i] = D[i % 32];
            }
            //result = result.LeftShift(Count1s(result));
            result = ShiftLeft(result, Count1s(result));
            return result;
        }
        public string BinaryToString(BitArray bits)
        {
            StringBuilder sb = new StringBuilder(bits.Length / 4);

            for (int i = 0; i < bits.Length; i += 4)
            {
                int v = (bits[i] ? 8 : 0) |
                        (bits[i + 1] ? 4 : 0) |
                        (bits[i + 2] ? 2 : 0) |
                        (bits[i + 3] ? 1 : 0);

                sb.Append(v.ToString("x1")); // Or "X1"
            }

            String result = sb.ToString();
            return result;
        }
        public BitArray ShiftLeft(BitArray data, int shiftCount)
        {
            BitArray retVal = new BitArray(data.Length);
            for (int i = shiftCount; i < data.Length; i++)
            {
                retVal[i - shiftCount] = data[i];
            }
            for (int i = 0; i < shiftCount; i++)
            {
                retVal[(retVal.Length) - (shiftCount - i)] = data[i];
            }
            return retVal;
        }
        private int Count1s(BitArray bits)
        {
            return (from bool m in bits
                    where m
                    select m).Count();
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
