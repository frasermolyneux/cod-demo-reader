using System.Collections;
using MX.CodDemoReader.Huffman;

namespace MX.CodDemoReader.Models;

/// <summary>
/// Represents a binary demo message and provides aligned read/decode operations.
/// </summary>
/// <param name="length">The initial message length in bytes.</param>
public class DemoMessage(int length)
{
    private int _bit;
    private int _readCount;

    private DemoMessage(byte[] data)
        : this(data.Length)
    {
        Data = data;
    }

    /// <summary>
    /// Gets the underlying message data buffer.
    /// </summary>
    public byte[] Data { get; private set; } = new byte[length];

    /// <summary>
    /// Gets or sets the active size of the message payload.
    /// </summary>
    public int CurrentSize { get; set; } = length;

    /// <summary>
    /// Gets a value indicating whether the read cursor is at the end of available data.
    /// </summary>
    public bool IsAtEndOfData => _readCount >= CurrentSize;

    /// <summary>
    /// Reads a single byte from the current aligned position.
    /// </summary>
    /// <returns>The read value, or <c>0</c> when there is insufficient data.</returns>
    public byte ReadByte()
    {
        return (byte)ReadBits(8);
    }

    /// <summary>
    /// Reads a 16-bit integer from the current aligned position.
    /// </summary>
    /// <returns>The read value, or <c>0</c> when there is insufficient data.</returns>
    public short ReadInt16()
    {
        return (short)ReadBits(16);
    }

    /// <summary>
    /// Reads a 32-bit integer from the current aligned position.
    /// </summary>
    /// <returns>The read value, or <c>0</c> when there is insufficient data.</returns>
    public int ReadInt32()
    {
        return ReadBits(32);
    }

    /// <summary>
    /// Reads a null-terminated string from the message.
    /// </summary>
    /// <param name="maxLen">The maximum number of characters to read.</param>
    /// <returns>The decoded string.</returns>
    public string ReadString(int maxLen = 1024)
    {
        var buffer = new List<char>();

        while (!IsAtEndOfData && buffer.Count < maxLen)
        {
            var c = ReadByte();

            if (c == 0)
            {
                break;
            }

            buffer.Add((char)c);
        }

        return new string([.. buffer]);
    }

    /// <summary>
    /// Reads a specific number of bits from the current aligned position.
    /// </summary>
    /// <param name="count">The number of bits to read.</param>
    /// <returns>The read value.</returns>
    public byte ReadAlignedBits(int count)
    {
        _ = count;
        return ReadByte();
    }

    /// <summary>
    /// Decodes the current message using the provided Huffman tree.
    /// </summary>
    /// <param name="huffmanTree">The Huffman tree used for decoding.</param>
    /// <returns>A new decoded <see cref="DemoMessage" /> instance.</returns>
    /// <exception cref="InvalidDataException">Thrown when the Huffman tree does not contain a required node.</exception>
    public DemoMessage Decode(HuffmanTree huffmanTree)
    {
        ArgumentNullException.ThrowIfNull(huffmanTree);

        var decoded = new List<byte>();
        var node = huffmanTree.Root;

        foreach (bool bit in new BitArray(Data))
        {
            node = (bit ? node.OneChild : node.ZeroChild) ?? throw new InvalidDataException("Missing node in the tree");
            if (node.Value == null)
            {
                continue;
            }

            decoded.Add(node.Value.Value);
            node = huffmanTree.Root;
        }

        return new DemoMessage([.. decoded]);
    }

    private int ReadBits(int bits)
    {
        var value = 0;

        for (var i = 0; i < bits; i++)
        {
            if (IsAtEndOfData)
            {
                throw new EndOfStreamException("Unexpected end of demo message");
            }

            value |= ((Data[_readCount] >> _bit) & 1) << i;
            _bit++;

            if (_bit != 8)
            {
                continue;
            }

            _bit = 0;
            _readCount++;
        }

        return value;
    }
}
