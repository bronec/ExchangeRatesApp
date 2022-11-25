using System.Text.Json.Serialization;

namespace ExchangeRatesApp.Common.Models
{
	public class CurrencyModel
	{
		[JsonPropertyName("Cur_ID")]
		public int Id { get; set; }

		[JsonPropertyName("Cur_Abbreviation")]
		public string CurrencyName { get; set; }

		[JsonPropertyName("Cur_Scale")]
		public int CurrencyAmount { get; set; }
		[JsonPropertyName("Cur_DateStart")]
		public DateTime startCurrencyDate { get; set; }
		[JsonPropertyName("Cur_DateEnd")]
		public DateTime endCurrencyDate { get; set; }
	}
}