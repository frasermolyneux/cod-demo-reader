namespace MX.CodDemoReader.Huffman;

/// <summary>
/// Represents a node in the Huffman decoding tree.
/// </summary>
public class HuffmanNode
{
    /// <summary>
    /// Gets or sets the child node selected when reading a zero bit.
    /// </summary>
    public HuffmanNode? ZeroChild { get; set; }

    /// <summary>
    /// Gets or sets the child node selected when reading a one bit.
    /// </summary>
    public HuffmanNode? OneChild { get; set; }

    /// <summary>
    /// Gets or sets the decoded byte value for leaf nodes.
    /// </summary>
    public byte? Value { get; set; }

    /// <summary>
    /// Gets or sets the frequency used while constructing the tree.
    /// </summary>
    public int Frequency { get; set; }
}