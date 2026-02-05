namespace MX.CodDemoReader.Huffman
{
    public class HuffmanNode
    {
        public HuffmanNode? ZeroChild { get; set; }
        public HuffmanNode? OneChild { get; set; }
        public byte? Value { get; set; }
        public int Frequency { get; set; }
    }
}