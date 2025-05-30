using System;
using System.Collections;
using System.Collections.Generic;
using CompilerGenerated;
using UnityEngine;

[Serializable]
public class PlayerPrefsX : MonoBehaviour
{
	[NonSerialized]
	private static int endianDiff1;

	[NonSerialized]
	private static int endianDiff2;

	[NonSerialized]
	private static int idx;

	[NonSerialized]
	private static byte[] byteBlock;

	public static bool SetBool(string name, bool value)
	{
		bool flag;
		try
		{
			PlayerPrefs.SetInt(name, value ? 1 : 0);
		}
		catch (Exception)
		{
			flag = false;
			goto IL_0028;
		}
		int result = 1;
		goto IL_0029;
		IL_0029:
		return (byte)result != 0;
		IL_0028:
		result = (flag ? 1 : 0);
		goto IL_0029;
	}

	public static bool GetBool(string name)
	{
		return PlayerPrefs.GetInt(name) == 1;
	}

	public static bool GetBool(string name, bool defaultValue)
	{
		return (!PlayerPrefs.HasKey(name)) ? defaultValue : GetBool(name);
	}

	public static bool SetVector2(string key, Vector2 vector)
	{
		return SetFloatArray(key, new float[2] { vector.x, vector.y });
	}

	public static Vector2 GetVector2(string key)
	{
		float[] floatArray = GetFloatArray(key);
		return (floatArray.Length >= 2) ? new Vector2(floatArray[0], floatArray[1]) : Vector2.zero;
	}

	public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
		return (!PlayerPrefs.HasKey(key)) ? defaultValue : GetVector2(key);
	}

	public static bool SetVector3(string key, Vector3 vector)
	{
		return SetFloatArray(key, new float[3] { vector.x, vector.y, vector.z });
	}

	public static Vector3 GetVector3(string key)
	{
		float[] floatArray = GetFloatArray(key);
		return (floatArray.Length >= 3) ? new Vector3(floatArray[0], floatArray[1], floatArray[2]) : Vector3.zero;
	}

	public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
		return (!PlayerPrefs.HasKey(key)) ? defaultValue : GetVector3(key);
	}

	public static bool SetQuaternion(string key, Quaternion vector)
	{
		return SetFloatArray(key, new float[4] { vector.x, vector.y, vector.z, vector.w });
	}

	public static Quaternion GetQuaternion(string key)
	{
		float[] floatArray = GetFloatArray(key);
		return (floatArray.Length >= 4) ? new Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]) : Quaternion.identity;
	}

	public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
	{
		return (!PlayerPrefs.HasKey(key)) ? defaultValue : GetQuaternion(key);
	}

	public static bool SetColor(string key, Color color)
	{
		return SetFloatArray(key, new float[4] { color.r, color.g, color.b, color.a });
	}

	public static Color GetColor(string key)
	{
		float[] floatArray = GetFloatArray(key);
		return (floatArray.Length >= 4) ? new Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]) : new Color(0f, 0f, 0f, 0f);
	}

	public static Color GetColor(string key, Color defaultValue)
	{
		return (!PlayerPrefs.HasKey(key)) ? defaultValue : GetColor(key);
	}

	public static bool SetBoolArray(string key, bool[] boolArray)
	{
		int result;
		if (boolArray.Length == 0)
		{
			Debug.LogError("The bool array cannot have 0 entries when setting " + key);
			result = 0;
		}
		else
		{
			byte[] array = new byte[(boolArray.Length + 7) / 8 + 5];
			array[0] = Convert.ToByte(ArrayType.Bool);
			BitArray bitArray = new BitArray(boolArray);
			bitArray.CopyTo(array, 5);
			Initialize();
			ConvertInt32ToBytes(boolArray.Length, array);
			result = (SaveBytes(key, array) ? 1 : 0);
		}
		return (byte)result != 0;
	}

	public static bool[] GetBoolArray(string key)
	{
		bool[] result;
		if (PlayerPrefs.HasKey(key))
		{
			byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
			if (array.Length < 6)
			{
				Debug.LogError("Corrupt preference file for " + key);
				result = new bool[0];
			}
			else if (array[0] != 2)
			{
				Debug.LogError(key + " is not a boolean array");
				result = new bool[0];
			}
			else
			{
				Initialize();
				byte[] array2 = new byte[array.Length - 5];
				Array.Copy(array, 5, array2, 0, array2.Length);
				BitArray bitArray = new BitArray(array2);
				bitArray.Length = ConvertBytesToInt32(array);
				bool[] array3 = new bool[bitArray.Count];
				bitArray.CopyTo(array3, 0);
				result = array3;
			}
		}
		else
		{
			result = new bool[0];
		}
		return result;
	}

	public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
	{
		bool[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetBoolArray(key);
		}
		else
		{
			bool[] array = new bool[defaultSize];
			int i = 0;
			bool[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static bool SetStringArray(string key, string[] stringArray)
	{
		int result;
		if (stringArray.Length == 0)
		{
			Debug.LogError("The string array cannot have 0 entries when setting " + key);
			result = 0;
		}
		else
		{
			byte[] array = new byte[stringArray.Length + 1];
			array[0] = Convert.ToByte(ArrayType.String);
			Initialize();
			int num = 0;
			while (true)
			{
				if (num < stringArray.Length)
				{
					if (stringArray[num] == null)
					{
						Debug.LogError("Can't save null entries in the string array when setting " + key);
						result = 0;
						break;
					}
					if (stringArray[num].Length > 255)
					{
						Debug.LogError("Strings cannot be longer than 255 characters when setting " + key);
						result = 0;
						break;
					}
					int num3;
					int num2 = (idx = (num3 = idx) + 1);
					array[num3] = (byte)stringArray[num].Length;
					num++;
					continue;
				}
				bool flag;
				try
				{
					PlayerPrefs.SetString(key, Convert.ToBase64String(array) + "|" + string.Join(string.Empty, stringArray));
				}
				catch (Exception)
				{
					flag = false;
					goto IL_0101;
				}
				result = 1;
				break;
				IL_0101:
				result = (flag ? 1 : 0);
				break;
			}
		}
		return (byte)result != 0;
	}

	public static string[] GetStringArray(string key)
	{
		string[] result;
		if (PlayerPrefs.HasKey(key))
		{
			string text = PlayerPrefs.GetString(key);
			int num = text.IndexOf("|"[0]);
			if (num < 4)
			{
				Debug.LogError("Corrupt preference file for " + key);
				result = new string[0];
			}
			else
			{
				byte[] array = Convert.FromBase64String(text.Substring(0, num));
				if (array[0] != 3)
				{
					Debug.LogError(key + " is not a string array");
					result = new string[0];
				}
				else
				{
					Initialize();
					int num2 = array.Length - 1;
					string[] array2 = new string[num2];
					int num3 = num + 1;
					int num4 = 0;
					while (true)
					{
						if (num4 >= num2)
						{
							result = array2;
							break;
						}
						int num6;
						int num5 = (idx = (num6 = idx) + 1);
						int num7 = array[num6];
						if (num3 + num7 > text.Length)
						{
							Debug.LogError("Corrupt preference file for " + key);
							result = new string[0];
							break;
						}
						array2[num4] = text.Substring(num3, num7);
						num3 += num7;
						num4++;
					}
				}
			}
		}
		else
		{
			result = new string[0];
		}
		return result;
	}

	public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
	{
		string[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetStringArray(key);
		}
		else
		{
			string[] array = new string[defaultSize];
			int i = 0;
			string[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static bool SetIntArray(string key, int[] intArray)
	{
		return SetValue(key, intArray, ArrayType.Int32, 1, _0024adaptor_0024__PlayerPrefsX_SetIntArray_0024callable2_0024239_61___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00240.Adapt(ConvertFromInt));
	}

	public static bool SetFloatArray(string key, float[] floatArray)
	{
		return SetValue(key, floatArray, ArrayType.Float, 1, _0024adaptor_0024__PlayerPrefsX_SetFloatArray_0024callable4_0024243_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00241.Adapt(ConvertFromFloat));
	}

	public static bool SetVector2Array(string key, Vector2[] vector2Array)
	{
		return SetValue(key, vector2Array, ArrayType.Vector2, 2, _0024adaptor_0024__PlayerPrefsX_SetVector2Array_0024callable6_0024247_67___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00242.Adapt(ConvertFromVector2));
	}

	public static bool SetVector3Array(string key, Vector3[] vector3Array)
	{
		return SetValue(key, vector3Array, ArrayType.Vector3, 3, _0024adaptor_0024__PlayerPrefsX_SetVector3Array_0024callable8_0024251_67___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00243.Adapt(ConvertFromVector3));
	}

	public static bool SetQuaternionArray(string key, Quaternion[] quaternionArray)
	{
		return SetValue(key, quaternionArray, ArrayType.Quaternion, 4, _0024adaptor_0024__PlayerPrefsX_SetQuaternionArray_0024callable10_0024255_73___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00244.Adapt(ConvertFromQuaternion));
	}

	public static bool SetColorArray(string key, Color[] colorArray)
	{
		return SetValue(key, colorArray, ArrayType.Color, 4, _0024adaptor_0024__PlayerPrefsX_SetColorArray_0024callable12_0024259_63___0024__PlayerPrefsX_SetValue_0024callable0_0024262_117___00245.Adapt(ConvertFromColor));
	}

	private static bool SetValue(string key, IList array, ArrayType arrayType, int vectorNumber, __PlayerPrefsX_SetValue_0024callable0_0024262_117__ convert)
	{
		int result;
		if (array.Count == 0)
		{
			Debug.LogError("The " + arrayType.ToString() + " array cannot have 0 entries when setting " + key);
			result = 0;
		}
		else
		{
			byte[] array2 = new byte[4 * array.Count * vectorNumber + 1];
			array2[0] = Convert.ToByte(arrayType);
			Initialize();
			for (int i = 0; i < array.Count; i++)
			{
				convert(array, array2, i);
			}
			result = (SaveBytes(key, array2) ? 1 : 0);
		}
		return (byte)result != 0;
	}

	private static void ConvertFromInt(int[] array, byte[] bytes, int i)
	{
		ConvertInt32ToBytes(array[i], bytes);
	}

	private static void ConvertFromFloat(float[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i], bytes);
	}

	private static void ConvertFromVector2(Vector2[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].x, bytes);
		ConvertFloatToBytes(array[i].y, bytes);
	}

	private static void ConvertFromVector3(Vector3[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].x, bytes);
		ConvertFloatToBytes(array[i].y, bytes);
		ConvertFloatToBytes(array[i].z, bytes);
	}

	private static void ConvertFromQuaternion(Quaternion[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].x, bytes);
		ConvertFloatToBytes(array[i].y, bytes);
		ConvertFloatToBytes(array[i].z, bytes);
		ConvertFloatToBytes(array[i].w, bytes);
	}

	private static void ConvertFromColor(Color[] array, byte[] bytes, int i)
	{
		ConvertFloatToBytes(array[i].r, bytes);
		ConvertFloatToBytes(array[i].g, bytes);
		ConvertFloatToBytes(array[i].b, bytes);
		ConvertFloatToBytes(array[i].a, bytes);
	}

	public static int[] GetIntArray(string key)
	{
		List<int> list = new List<int>();
		GetValue(key, list, ArrayType.Int32, 1, _0024adaptor_0024__PlayerPrefsX_GetIntArray_0024callable14_0024312_53___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00246.Adapt(ConvertToInt));
		return list.ToArray();
	}

	public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
	{
		int[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetIntArray(key);
		}
		else
		{
			int[] array = new int[defaultSize];
			int i = 0;
			int[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static float[] GetFloatArray(string key)
	{
		List<float> list = new List<float>();
		GetValue(key, list, ArrayType.Float, 1, _0024adaptor_0024__PlayerPrefsX_GetFloatArray_0024callable16_0024329_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00247.Adapt(ConvertToFloat));
		return list.ToArray();
	}

	public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
	{
		float[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetFloatArray(key);
		}
		else
		{
			float[] array = new float[defaultSize];
			int i = 0;
			float[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static Vector2[] GetVector2Array(string key)
	{
		List<Vector2> list = new List<Vector2>();
		GetValue(key, list, ArrayType.Vector2, 2, _0024adaptor_0024__PlayerPrefsX_GetVector2Array_0024callable18_0024346_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00248.Adapt(ConvertToVector2));
		return list.ToArray();
	}

	public static Vector2[] GetVector2Array(string key, Vector2 defaultValue, int defaultSize)
	{
		Vector2[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetVector2Array(key);
		}
		else
		{
			Vector2[] array = new Vector2[defaultSize];
			int i = 0;
			Vector2[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static Vector3[] GetVector3Array(string key)
	{
		List<Vector3> list = new List<Vector3>();
		GetValue(key, list, ArrayType.Vector3, 3, _0024adaptor_0024__PlayerPrefsX_GetVector3Array_0024callable20_0024363_59___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___00249.Adapt(ConvertToVector3));
		return list.ToArray();
	}

	public static Vector3[] GetVector3Array(string key, Vector3 defaultValue, int defaultSize)
	{
		Vector3[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetVector3Array(key);
		}
		else
		{
			Vector3[] array = new Vector3[defaultSize];
			int i = 0;
			Vector3[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static Quaternion[] GetQuaternionArray(string key)
	{
		List<Quaternion> list = new List<Quaternion>();
		GetValue(key, list, ArrayType.Quaternion, 4, _0024adaptor_0024__PlayerPrefsX_GetQuaternionArray_0024callable22_0024380_65___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002410.Adapt(ConvertToQuaternion));
		return list.ToArray();
	}

	public static Quaternion[] GetQuaternionArray(string key, Quaternion defaultValue, int defaultSize)
	{
		Quaternion[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetQuaternionArray(key);
		}
		else
		{
			Quaternion[] array = new Quaternion[defaultSize];
			int i = 0;
			Quaternion[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	public static Color[] GetColorArray(string key)
	{
		List<Color> list = new List<Color>();
		GetValue(key, list, ArrayType.Color, 4, _0024adaptor_0024__PlayerPrefsX_GetColorArray_0024callable24_0024397_55___0024__PlayerPrefsX_GetValue_0024callable1_0024412_116___002411.Adapt(ConvertToColor));
		return list.ToArray();
	}

	public static Color[] GetColorArray(string key, Color defaultValue, int defaultSize)
	{
		Color[] result;
		if (PlayerPrefs.HasKey(key))
		{
			result = GetColorArray(key);
		}
		else
		{
			Color[] array = new Color[defaultSize];
			int i = 0;
			Color[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				array2[i] = defaultValue;
			}
			result = array;
		}
		return result;
	}

	private static void GetValue(string key, IList list, ArrayType arrayType, int vectorNumber, __PlayerPrefsX_GetValue_0024callable1_0024412_116__ convert)
	{
		if (!PlayerPrefs.HasKey(key))
		{
			return;
		}
		byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
		if ((array.Length - 1) % (vectorNumber * 4) != 0)
		{
			Debug.LogError("Corrupt preference file for " + key);
			return;
		}
		if ((ArrayType)array[0] != arrayType)
		{
			Debug.LogError(key + " is not a " + arrayType.ToString() + " array");
			return;
		}
		Initialize();
		int num = (array.Length - 1) / (vectorNumber * 4);
		for (int i = 0; i < num; i++)
		{
			convert(list, array);
		}
	}

	private static void ConvertToInt(List<int> list, byte[] bytes)
	{
		list.Add(ConvertBytesToInt32(bytes));
	}

	private static void ConvertToFloat(List<float> list, byte[] bytes)
	{
		list.Add(ConvertBytesToFloat(bytes));
	}

	private static void ConvertToVector2(List<Vector2> list, byte[] bytes)
	{
		list.Add(new Vector2(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToVector3(List<Vector3> list, byte[] bytes)
	{
		list.Add(new Vector3(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToQuaternion(List<Quaternion> list, byte[] bytes)
	{
		list.Add(new Quaternion(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	private static void ConvertToColor(List<Color> list, byte[] bytes)
	{
		list.Add(new Color(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
	}

	public static void ShowArrayType(string key)
	{
		byte[] array = Convert.FromBase64String(PlayerPrefs.GetString(key));
		if (array.Length > 0)
		{
			ArrayType arrayType = (ArrayType)array[0];
			Debug.Log(key + " is a " + arrayType.ToString() + " array");
		}
	}

	private static void Initialize()
	{
		if (BitConverter.IsLittleEndian)
		{
			endianDiff1 = 0;
			endianDiff2 = 0;
		}
		else
		{
			endianDiff1 = 3;
			endianDiff2 = 1;
		}
		if (byteBlock == null)
		{
			byteBlock = new byte[4];
		}
		idx = 1;
	}

	private static bool SaveBytes(string key, byte[] bytes)
	{
		bool flag;
		try
		{
			PlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
		}
		catch (Exception)
		{
			flag = false;
			goto IL_0021;
		}
		int result = 1;
		goto IL_0022;
		IL_0022:
		return (byte)result != 0;
		IL_0021:
		result = (flag ? 1 : 0);
		goto IL_0022;
	}

	private static void ConvertFloatToBytes(float f, byte[] bytes)
	{
		byteBlock = BitConverter.GetBytes(f);
		ConvertTo4Bytes(bytes);
	}

	private static float ConvertBytesToFloat(byte[] bytes)
	{
		ConvertFrom4Bytes(bytes);
		return BitConverter.ToSingle(byteBlock, 0);
	}

	private static void ConvertInt32ToBytes(int i, byte[] bytes)
	{
		byteBlock = BitConverter.GetBytes(i);
		ConvertTo4Bytes(bytes);
	}

	private static int ConvertBytesToInt32(byte[] bytes)
	{
		ConvertFrom4Bytes(bytes);
		return BitConverter.ToInt32(byteBlock, 0);
	}

	private static void ConvertTo4Bytes(byte[] bytes)
	{
		bytes[idx] = byteBlock[endianDiff1];
		bytes[idx + 1] = byteBlock[1 + endianDiff2];
		bytes[idx + 2] = byteBlock[2 - endianDiff2];
		bytes[idx + 3] = byteBlock[3 - endianDiff1];
		idx += 4;
	}

	private static void ConvertFrom4Bytes(byte[] bytes)
	{
		byteBlock[endianDiff1] = bytes[idx];
		byteBlock[1 + endianDiff2] = bytes[idx + 1];
		byteBlock[2 - endianDiff2] = bytes[idx + 2];
		byteBlock[3 - endianDiff1] = bytes[idx + 3];
		idx += 4;
	}

	public virtual void Main()
	{
	}
}
