using System.Text;
using MX.CodDemoReader.Huffman;
using MX.CodDemoReader.Models;

namespace MX.CodDemoReader.Tests;

public class DemoReaderTests
{
    [Fact]
    public void ReadConfigurationForCallOfDuty5DemoReturnsMetadata()
    {
        using var stream = CreateCallOfDuty5Demo(
            "\\mapname\\mp_regression\\fs_game\\mods/test_mod\\g_gametype\\dm\\sv_hostname\\Test Server");
        var reader = new DemoReader(stream, GameVersion.CallOfDuty5);

        var configuration = reader.ReadConfiguration();

        Assert.Equal("mp_regression", configuration["mapname"]);
        Assert.Equal("mods/test_mod", configuration["fs_game"]);
        Assert.Equal("dm", configuration["g_gametype"]);
        Assert.Equal("Test Server", configuration["sv_hostname"]);
    }

    private static MemoryStream CreateCallOfDuty5Demo(string configuration)
    {
        var decodedMessage = new List<byte>
        {
            1,
            0,
            0,
            0,
            0,
            2,
            1,
            0,
            1
        };
        decodedMessage.AddRange(Encoding.ASCII.GetBytes(configuration));
        decodedMessage.Add(0);
        decodedMessage.Add(0);

        var compressedMessage = Encode(decodedMessage, HuffmanFrequencies.CallOfDuty4);
        var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write((byte)0);
            writer.Write(0);
            writer.Write(compressedMessage.Length + sizeof(int));
            writer.Write(0);
            writer.Write(compressedMessage);
        }

        stream.Position = 0;
        return stream;
    }

    private static byte[] Encode(IEnumerable<byte> values, int[] frequencies)
    {
        var root = BuildProtocolTree(frequencies);
        var codes = new Dictionary<byte, IReadOnlyList<bool>>();
        BuildCodes(root, [], codes);

        var bits = values.SelectMany(value => codes[value]).ToArray();
        var result = new byte[(bits.Length + 7) / 8];
        for (var i = 0; i < bits.Length; i++)
        {
            if (bits[i])
            {
                result[i / 8] |= (byte)(1 << (i % 8));
            }
        }

        return result;
    }

    private static ProtocolNode BuildProtocolTree(int[] frequencies)
    {
        var nodes = frequencies
            .Select((frequency, value) => new ProtocolNode((byte)value, frequency))
            .ToList();

        while (nodes.Count > 1)
        {
            var ordered = nodes.OrderBy(node => node.Frequency).ToArray();
            var zero = ordered[0];
            var one = ordered[1];

            _ = nodes.Remove(zero);
            _ = nodes.Remove(one);
            nodes.Add(new ProtocolNode(null, zero.Frequency + one.Frequency)
            {
                Zero = zero,
                One = one
            });
        }

        return nodes[0];
    }

    private static void BuildCodes(
        ProtocolNode node,
        IReadOnlyList<bool> prefix,
        IDictionary<byte, IReadOnlyList<bool>> codes)
    {
        if (node.Value.HasValue)
        {
            codes[node.Value.Value] = prefix;
            return;
        }

        BuildCodes(node.Zero!, [.. prefix, false], codes);
        BuildCodes(node.One!, [.. prefix, true], codes);
    }

    private sealed class ProtocolNode(byte? value, int frequency)
    {
        public byte? Value { get; } = value;
        public int Frequency { get; } = frequency;
        public ProtocolNode? Zero { get; init; }
        public ProtocolNode? One { get; init; }
    }
}
