namespace ExchangeRatesApp.Common.Models.Requests
{
	public class GetExchangeRatesRequestModel
	{
		public Currency Currency { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}
}
