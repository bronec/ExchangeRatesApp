using ExchangeRatesApp.Common.Models;
using System;
using System.Collections.Generic;

namespace ExchangeRateAnalysis.EventArguments
{
	public class ExchangeRatesEventArguments : EventArgs
	{
		public ExchangeRatesEventArguments(List<ExchangeRateModel> exchangeRates)
		{
			ExchangeRates = exchangeRates;
		}

		public List<ExchangeRateModel> ExchangeRates { get; }
	}
}
