using System.Security.Cryptography;

namespace Integration;

public class Tests
{
    [Test]
    public void Setup()
    {
        byte[] keyBytes = new byte[64]; // 512 Bit = 64 Bytes
        RandomNumberGenerator.Fill(keyBytes);
        string secretKey = Convert.ToBase64String(keyBytes);
        
        Console.WriteLine(secretKey);
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}