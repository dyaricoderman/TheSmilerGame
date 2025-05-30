using System;
using LitJson;

public class Prize
{
	public string Code { get; set; }

	public PrizeDescription PrizeDescription { get; set; }

	public DateTime? Used { get; set; }

	public Prize()
	{
	}

	public Prize(JsonData data)
	{
		Code = (string)data["Code"];
		PrizeDescription = new PrizeDescription(data["PrizeDescription"]);
	}
}
