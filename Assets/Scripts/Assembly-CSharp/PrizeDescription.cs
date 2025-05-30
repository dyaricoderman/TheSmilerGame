using LitJson;

public class PrizeDescription
{
	public string Name { get; set; }

	public string ShortDescription { get; set; }

	public string LongDescription { get; set; }

	public int IconID { get; set; }

	public PrizeDescription()
	{
	}

	public PrizeDescription(JsonData data)
	{
		Name = (string)data["Name"];
		ShortDescription = (string)data["ShortDescription"];
		LongDescription = (string)data["LongDescription"];
		IconID = (int)data["IconID"];
	}
}
