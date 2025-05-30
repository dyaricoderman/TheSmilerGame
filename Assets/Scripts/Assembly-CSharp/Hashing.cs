using System.Security.Cryptography;
using System.Text;

public class Hashing
{
	public static string MD5(string phrase)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] inputArray = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(phrase));
		return byteArrayToString(inputArray);
	}

	public static string SHA1(string phrase)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
		byte[] inputArray = sHA1CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(phrase));
		return byteArrayToString(inputArray);
	}

	public static string SHA256(string phrase)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		SHA256Managed sHA256Managed = new SHA256Managed();
		byte[] inputArray = sHA256Managed.ComputeHash(uTF8Encoding.GetBytes(phrase));
		return byteArrayToString(inputArray);
	}

	public static string SHA384(string phrase)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		SHA384Managed sHA384Managed = new SHA384Managed();
		byte[] inputArray = sHA384Managed.ComputeHash(uTF8Encoding.GetBytes(phrase));
		return byteArrayToString(inputArray);
	}

	public static string SHA512(string phrase)
	{
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		SHA512Managed sHA512Managed = new SHA512Managed();
		byte[] inputArray = sHA512Managed.ComputeHash(uTF8Encoding.GetBytes(phrase));
		return byteArrayToString(inputArray);
	}

	public static string byteArrayToString(byte[] inputArray)
	{
		StringBuilder stringBuilder = new StringBuilder(string.Empty);
		for (int i = 0; i < inputArray.Length; i++)
		{
			stringBuilder.Append(inputArray[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
