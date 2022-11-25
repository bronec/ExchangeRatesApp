using ExchangeRatesApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateAnalysis.Services
{
	public interface IExchangeRatesService
	{
		Task<List<ExchangeRateModel>> GetExchangeRatesAsync(Currency currency, DateTime startDate, DateTime endDate);
	}
}
