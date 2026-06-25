using MX.CodDemoReader.Huffman;

namespace MX.CodDemoReader.Models;

/// <summary>
/// Represents a binary demo message and provides aligned read/decode operations.
/// </summary>
/// <param name="length">The initial message length in bytes.</param>
public class DemoMessage(int length)
{
    private int _readPosition;
    private byte _readBits;

    /// <summary>
    /// Gets the underlying message data buffer.
    /// </summary>
    public byte[] Data { get; } = new byte[length];

    /// <summary>
    /// Gets or sets the active size of the message payload.
    /// </summary>
    public int CurrentSize { get; set; } = length;

    /// <summary>
    /// Gets a value indicating whether the read cursor is at the end of available data.
    /// </summary>
    public bool IsAtEndOfData => _readPosition == CurrentSize && _readBits == 0;

    /// <summary>
    /// Reads a single byte from the current aligned position.
    /// </summary>
    /// <returns>The read value, or <c>0</c> when there is insufficient data.</returns>
    public byte ReadByte()
    {
        return !AlignReadPointer(8) ? (byte)0 : (byte)(Data[_readPosition++] & 0xff);
    }

    /// <summary>
    /// Reads a 16-bit integer from the current aligned position.
    /// </summary>
    /// <returns>The read value, or <c>0</c> when there is insufficient data.</returns>
    public short ReadInt16()
    {
        return !AlignReadPointer(16) ? (short)0 : (short)((Data[_readPosition++] & 0xff) | ((Data[_readPosition++] & 0xff) << 8));
    }

    /// <summary>
    /// Reads a 32-bit integer from the current aligned position.
    /// </summary>
    /// <returns>The read value, or <c>0</c> when there is insufficient data.</returns>
    public int ReadInt32()
    {
        return !AlignReadPointer(32)
            ? 0
            : (Data[_readPosition++] & 0xff) | ((Data[_readPosition++] & 0xff) << 8) |
               ((Data[_readPosition++] & 0xff) << 16) | ((Data[_readPosition++] & 0xff) << 24);
    }

    /// <summary>
    /// Reads a null-terminated string from the message.
    /// </summary>
    /// <param name="maxLen">The maximum number of characters to read.</param>
    /// <returns>The decoded string.</returns>
    public string ReadString(int maxLen = 1024)
    {
        var len = 0;
        var buffer = new List<char>();

        do
        {
            var c = ReadByte();

            if (c <= 0)
            {
                break;
            }

            buffer.Add((char)c);
            len++;
        } while (len < maxLen);

        return new string([.. buffer]);
    }

    /// <summary>
    /// Reads a specific number of bits from the current aligned position.
    /// </summary>
    /// <param name="count">The number of bits to read.</param>
    /// <returns>The read value.</returns>
    public byte ReadAlignedBits(int count)
    {
        byte value = 0;
        int i;

        for (i = 0; i < count; i++)
        {
            value |= (byte)((byte)(Data[_readPosition] >> _readBits) & (1 << i));
            _readBits++;
            if (_readBits != 8)
            {
                continue;
            }

            _readBits = 0;
            _readPosition++;
            if (_readPosition <= CurrentSize)
            {
                continue;
            }

            _readBits = 0;
            return value;
        }
        return value;
    }

    /// <summary>
    /// Decodes the current message using the provided Huffman tree.
    /// </summary>
    /// <param name="huffmanTree">The Huffman tree used for decoding.</param>
    /// <returns>A new decoded <see cref="DemoMessage" /> instance.</returns>
    /// <exception cref="InvalidDataException">Thrown when the Huffman tree does not contain a required node.</exception>
    public DemoMessage Decode(HuffmanTree huffmanTree)
    {
        var message = new DemoMessage(CurrentSize);
        var node = huffmanTree.Root;
        var i = 0;

        while (true)
        {
            var value = ReadAlignedBits(1);

            node = (value == 0 ? node.ZeroChild : node.OneChild) ?? throw new InvalidDataException("Missing node in the tree");
            if (node.Value == null)
            {
                continue;
            }

            if (message.CurrentSize >= message.Data.Length)
            {
                break;
            }

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