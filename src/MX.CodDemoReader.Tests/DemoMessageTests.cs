using MX.CodDemoReader.Huffman;
using MX.CodDemoReader.Models;

namespace MX.CodDemoReader.Tests;

public class DemoMessageTests
{
    [Fact]
    public void PrimitiveReadsStartAtFirstByteAndUseLittleEndian()
    {
        var byteMessage = CreateMessage(0x12);
        var shortMessage = CreateMessage(0x34, 0x12);
        var intMessage = CreateMessage(0x78, 0x56, 0x34, 0x12);

        Assert.Equal(0x12, byteMessage.ReadByte());
        Assert.Equal(0x1234, shortMessage.ReadInt16());
        Assert.Equal(0x12345678, intMessage.ReadInt32());
    }

    [Fact]
    public void ReadPastEndThrowsEndOfStreamException()
    {
        var message = CreateMessage(0x12);

        _ = message.ReadByte();

        _ = Assert.Throws<EndOfStreamException>(() => message.ReadByte());
    }

    [Fact]
    public void PackedBitReadsHonorCountAndByteReadsRealign()
    {
        var message = CreateMessage(0b1010_1101, 0x34);

        Assert.Equal(0b101, message.ReadAlignedBits(3));
        Assert.Equal(0b10101, message.ReadAlignedBits(5));
        Assert.Equal(0x34, message.ReadByte());
    }

    [Fact]
    public void DecodeUsesOnlyActivePayload()
    {
        var expected = CreateMessage(0b1010_1101);
        var padded = CreateMessage(0b1010_1101, 0xff);
        padded.CurrentSize = 1;
        var tree = new HuffmanTree(HuffmanFrequencies.CallOfDuty4);

        var expectedDecoded = expected.Decode(tree);
        var paddedDecoded = padded.Decode(tree);

        Assert.Equal(expectedDecoded.Data, paddedDecoded.Data);
    }

    private static DemoMessage CreateMessage(params byte[] data)
    {
        var message = new DemoMessage(data.Length);
        data.CopyTo(message.Data, 0);
        return message;
    }
}
