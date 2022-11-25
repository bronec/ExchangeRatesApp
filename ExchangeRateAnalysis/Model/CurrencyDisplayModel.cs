using ExchangeRatesApp.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ExchangeRateAnalysis.Model
{
	public class CurrencyDisplayModel
	{
		public CurrencyDisplayModel(Currency value)
		{
			Value = value;
		}

		public Currency Value { get; set; }

		public string Name 
			=> Value.GetType()
				.GetMember(Value.ToString())
				.First()
				.GetCustomAttribute<DisplayAttribute>()
				.GetName();
	}
}
