using System;
using System.Text;
using SevenZip.Compression.LZMA;

public class Compression
{
	public static string Compress(string text)
	{
		return Convert.ToBase64String(SevenZipHelper.Compress(Encoding.UTF8.GetBytes(text)));
	}

	public static string Decompress(string compressedText)
	{
		return Encoding.UTF8.GetString(SevenZipHelper.Decompress(Convert.FromBase64String(compressedText)));
	}
}
