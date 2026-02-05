namespace MX.CodDemoReader.Huffman
{
    public class HuffmanTree
    {
        public HuffmanTree(int[] frequencies)
        {
            Root = BuildTree(frequencies);
        }

        public HuffmanNode Root { get; }

        private static HuffmanNode BuildTree(int[] frequencies)
        {
            if (frequencies.Length != 256)
                throw new Exception("Frequencies should be a sequence of 256 numbers.");

            var nodes = new Queue<HuffmanNode>(frequencies.Select((freq, value) => new HuffmanNode
            {
                Value = (byte)value,
                Frequency = freq
            }).OrderBy(node => node.Frequency));

            while (nodes.Count > 1)
            {
                var first = nodes.Dequeue();
                var second = nodes.Dequeue();

                var newNode = new HuffmanNode
                {
                    OneChild = first,
                    ZeroChild = second,
                    Frequency = first.Frequency + second.Frequency
                };

                var reorderedList = nodes.ToList();
                reorderedList.Add(newNode);
                nodes = new Queue<HuffmanNode>(reorderedList.OrderBy(n => n.Frequency));
            }

            return nodes.Dequeue();
        }
    }
}