using System.Security.Cryptography;
using MX.CodDemoReader.Huffman;

namespace MX.CodDemoReader.Tests;

public class HuffmanFrequenciesTests
{
    [Fact]
    public void ProtocolFrequencyTablesHaveExpectedContents()
    {
        Assert.Equal(256, HuffmanFrequencies.Quake3.Length);
        Assert.Equal(256, HuffmanFrequencies.CallOfDuty4.Length);
        Assert.Equal(
            "1CBDCA8D6C9BB22247525A4807C5E18CEC9B87336C56016424EECD879B505ABF",
            CalculateHash(HuffmanFrequencies.Quake3));
        Assert.Equal(
            "F358065D8DAB6D7FB923C5F272B48C7899E88DA5CDF5D5E39A8E41ACE9BC13AC",
            CalculateHash(HuffmanFrequencies.CallOfDuty4));
    }

    private static string CalculateHash(IEnumerable<int> values)
    {
        var bytes = values.SelectMany(BitConverter.GetBytes).ToArray();
        return Convert.ToHexString(SHA256.HashData(bytes));
    }
}
