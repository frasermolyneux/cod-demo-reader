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

    private static DemoMessage CreateMessage(params byte[] data)
    {
        var message = new DemoMessage(data.Length);
        data.CopyTo(message.Data, 0);
        return message;
    }
}
