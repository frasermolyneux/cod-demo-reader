using MX.CodDemoReader.Huffman;

namespace MX.CodDemoReader.Models
{
    public class DemoMessage
    {
        private int _readPosition;
        private byte _readBits;

        public DemoMessage(int length)
        {
            Data = new byte[length];
            CurrentSize = length;
        }

        public byte[] Data { get; }

        public int CurrentSize { get; set; }

        public bool IsAtEndOfData => _readPosition == CurrentSize && _readBits == 0;

        public byte ReadByte()
        {
            if (!AlignReadPointer(8))
                return 0;

            return (byte)(Data[_readPosition++] & 0xff);
        }

        public short ReadInt16()
        {
            if (!AlignReadPointer(16))
                return 0;

            return (short)((Data[_readPosition++] & 0xff) | ((Data[_readPosition++] & 0xff) << 8));
        }

        public int ReadInt32()
        {
            if (!AlignReadPointer(32))
                return 0;

            return (Data[_readPosition++] & 0xff) | ((Data[_readPosition++] & 0xff) << 8) |
                   ((Data[_readPosition++] & 0xff) << 16) | ((Data[_readPosition++] & 0xff) << 24);
        }

        public string ReadString(int maxLen = 1024)
        {
            var len = 0;
            var buffer = new List<char>();

            do
            {
                var c = ReadByte();

                if (c <= 0)
                    break;

                buffer.Add((char)c);
                len++;
            } while (len < maxLen);

            return new string(buffer.ToArray());
        }

        public byte ReadAlignedBits(int count)
        {
            byte value = 0;
            int i;

            for (i = 0; i < count; i++)
            {
                value |= (byte)((byte)(Data[_readPosition] >> _readBits) & 1 << i);
                _readBits++;
                if (_readBits != 8) continue;
                _readBits = 0;
                _readPosition++;
                if (_readPosition <= CurrentSize) continue;
                _readBits = 0;
                return value;
            }
            return value;
        }

        public DemoMessage Decode(HuffmanTree huffmanTree)
        {
            var message = new DemoMessage(CurrentSize);
            var node = huffmanTree.Root;
            var i = 0;

            while (true)
            {
                var value = ReadAlignedBits(1);

                node = value == 0 ? node.ZeroChild : node.OneChild;

                if (node == null)
                    throw new Exception("Missing node in the tree");

                if (node.Value == null)
                    continue;

                if (message.CurrentSize >= message.Data.Length)
                    break;

                message.Data[message.CurrentSize++] = node.Value.Value;
                node = huffmanTree.Root;
                i++;
            }

            return message;
        }

        private bool AlignReadPointer(int count)
        {
            var skip = count - _readBits;
            _readBits = 0;
            _readPosition += skip / 8;
            return skip >= 0 && _readPosition < Data.Length;
        }
    }
}