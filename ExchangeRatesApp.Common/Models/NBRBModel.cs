using System.Text.Json.Serialization;

namespace ExchangeRatesApp.Common.Models
{
	public class NBRBModel
	{
		[JsonPropertyName("Cur_ID")]
		public int Id { get; set; }
		public System.DateTime Date{ get; set; }
		[JsonPropertyName("Cur_OfficialRate")]
		public decimal ExchangeRate { get; set; }
	}
}
