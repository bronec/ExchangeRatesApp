namespace ExchangeRatesApp.Common.Models
{
	public class ExchangeRateModel
	{
		public string Currency { get; set; }
		public DateTime Date { get; set; }
		public decimal Value { get; set; }
		public int Amount { get; set; }
	}
}