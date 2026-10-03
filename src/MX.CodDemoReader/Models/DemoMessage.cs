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
    /// <returns>The read value.</returns>
    /// <exception cref="EndOfStreamException">There is insufficient data remaining.</exception>
    public byte ReadByte()
    {
        AlignReadPointer();
        return (byte)ReadBits(8);
    }

    /// <summary>
    /// Reads a 16-bit integer from the current aligned position.
    /// </summary>
    /// <returns>The read value.</returns>
    /// <exception cref="EndOfStreamException">There is insufficient data remaining.</exception>
    public short ReadInt16()
    {
        AlignReadPointer();
        return (short)ReadBits(16);
    }

    /// <summary>
    /// Reads a 32-bit integer from the current aligned position.
    /// </summary>
    /// <returns>The read value.</returns>
    /// <exception cref="EndOfStreamException">There is insufficient data remaining.</exception>
    public int ReadInt32()
    {
        AlignReadPointer();
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
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="count"/> is less than one or greater than eight.
    /// </exception>
    /// <exception cref="EndOfStreamException">There is insufficient data remaining.</exception>
    public byte ReadAlignedBits(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, 8);

        return (byte)ReadBits(count);
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

        foreach (bool bit in new BitArray(Data[..CurrentSize]))
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

    internal int ReadBits(int bits)
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

    private void AlignReadPointer()
    {
        if (_bit == 0)
        {
            return;
        }

        _bit = 0;
        _readCount++;
    }
}
