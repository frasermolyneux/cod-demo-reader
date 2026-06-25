namespace MX.CodDemoReader.Huffman;

/// <summary>
/// Builds and exposes a Huffman decoding tree from a 256-entry frequency table.
/// </summary>
/// <param name="frequencies">The input frequency table.</param>
public class HuffmanTree(int[] frequencies)
{
    /// <summary>
    /// Gets the root node of the decoding tree.
    /// </summary>
    public HuffmanNode Root { get; } = BuildTree(frequencies);

    private static HuffmanNode BuildTree(int[] frequencies)
    {
        if (frequencies.Length != 256)
        {
            throw new ArgumentException("Frequencies should be a sequence of 256 numbers.", nameof(frequencies));
        }

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