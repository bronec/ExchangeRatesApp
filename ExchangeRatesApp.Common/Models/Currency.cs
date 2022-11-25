using System.ComponentModel.DataAnnotations;

namespace ExchangeRatesApp.Common.Models
{
	public enum Currency
	{
		[Display(ResourceType = typeof(Resources), Name = nameof(Resources.Currency_USD))]
		USD,
		[Display(ResourceType = typeof(Resources), Name = nameof(Resources.Currency_EUR))]
		EUR,
		[Display(ResourceType = typeof(Resources), Name = nameof(Resources.Currency_RUB))]
		RUB
	}
}
