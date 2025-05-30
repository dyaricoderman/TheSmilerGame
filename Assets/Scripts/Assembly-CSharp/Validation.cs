using System.Text.RegularExpressions;

public class Validation
{
	public static bool IsEmail(string Email)
	{
		string pattern = "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
		Regex regex = new Regex(pattern);
		if (regex.IsMatch(Email))
		{
			return true;
		}
		return false;
	}

	public static bool IsName(string Name)
	{
		if (Name != string.Empty)
		{
			return true;
		}
		return false;
	}

	public static bool IsPhone(string Phone)
	{
		string pattern = "[^0-9 \\s]";
		Regex regex = new Regex(pattern);
		if (!regex.IsMatch(Phone))
		{
			return true;
		}
		return false;
	}

	public static bool IsUrl(string Url)
	{
		string pattern = "^(https?://)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\\.[a-z]{2,6})(:[0-9]{1,4})?((/?)|(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
		Regex regex = new Regex(pattern);
		if (regex.IsMatch(Url))
		{
			return true;
		}
		return false;
	}
}
