using System;
using LitJson;

public class Winner
{
	public Prize Prize { get; set; }

	public Address Address { get; set; }

	public DateTime? Dispatched { get; set; }

	public Winner()
	{
	}

	public Winner(JsonData data)
	{
		Prize = new Prize(data["Prize"]);
		Address = new Address(data["Address"]);
		if (data["Dispatched"] != null)
		{
			Dispatched = DateTime.Parse((string)data["Dispatched"]);
		}
	}
}
