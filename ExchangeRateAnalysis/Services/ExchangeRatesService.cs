using ExchangeRatesApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Json;
using ExchangeRatesApp.Common.Models.Requests;

namespace ExchangeRateAnalysis.Services
{
	public class ExchangeRatesService : IExchangeRatesService
	{
		private const string HostName = "https://localhost:444/";
		private HttpClient httpClient = new HttpClient();

		public async Task<List<ExchangeRateModel>> GetExchangeRatesAsync(Currency currency, DateTime startDate, DateTime endDate)
		{
			try
			{
				var request = new GetExchangeRatesRequestModel()
				{
					StartTime = startDate,
					EndTime = endDate,
					Currency = currency
				};
				using (var response = await httpClient.PostAsJsonAsync(GetUrl($"currency"), request))
				{
					if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
					{
						throw new WebException(response.StatusCode.ToString());
					}
					return await response.Content.ReadFromJsonAsync<List<ExchangeRateModel>>();
				}
			}
			catch (Exception e)
			{
				throw;
			}
		}

		private string GetUrl(string resource)
			=> HostName + resource;
	}
}
