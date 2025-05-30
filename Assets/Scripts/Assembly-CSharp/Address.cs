using LitJson;

public class Address
{
	public int AddressID { get; set; }

	public string Addressee { get; set; }

	public string FirstLine { get; set; }

	public string SecondLine { get; set; }

	public string PostTown { get; set; }

	public string Postcode { get; set; }

	public Address()
	{
	}

	public Address(JsonData data)
	{
		if (data != null)
		{
			AddressID = (int)data["AddressID"];
			Addressee = (string)data["Addressee"];
			FirstLine = (string)data["FirstLine"];
			if (data["SecondLine"] != null)
			{
				SecondLine = (string)data["SecondLine"];
			}
			PostTown = (string)data["PostTown"];
			Postcode = (string)data["Postcode"];
		}
	}
}
