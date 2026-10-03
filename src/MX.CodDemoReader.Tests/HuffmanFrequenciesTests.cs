using System.Security.Cryptography;
using MX.CodDemoReader.Huffman;

namespace MX.CodDemoReader.Tests;

public class HuffmanFrequenciesTests
{
    [Fact]
    public void ProtocolFrequencyTablesHaveExpectedContents()
    {
        var quake3 = HuffmanFrequencies.Quake3;
        var callOfDuty4 = HuffmanFrequencies.CallOfDuty4;

        Assert.Equal(256, quake3.Length);
        Assert.Equal(256, callOfDuty4.Length);
        Assert.Equal(
            "1CBDCA8D6C9BB22247525A4807C5E18CEC9B87336C56016424EECD879B505ABF",
            CalculateHash(quake3));
        Assert.Equal(
            "F358065D8DAB6D7FB923C5F272B48C7899E88DA5CDF5D5E39A8E41ACE9BC13AC",
            CalculateHash(callOfDuty4));
    }

    [Fact]
    public void ProtocolFrequencyTablesReturnIsolatedArrays()
    {
        var quake3 = HuffmanFrequencies.Quake3;
        var callOfDuty4 = HuffmanFrequencies.CallOfDuty4;

        quake3[0] = 0;
        callOfDuty4[0] = 0;

        Assert.NotSame(quake3, HuffmanFrequencies.Quake3);
        Assert.NotSame(callOfDuty4, HuffmanFrequencies.CallOfDuty4);
        Assert.Equal(250315, HuffmanFrequencies.Quake3[0]);
        Assert.Equal(274054, HuffmanFrequencies.CallOfDuty4[0]);
    }

    private static string CalculateHash(IEnumerable<int> values)
    {
        var bytes = values.SelectMany(BitConverter.GetBytes).ToArray();
        return Convert.ToHexString(SHA256.HashData(bytes));
    }
}
