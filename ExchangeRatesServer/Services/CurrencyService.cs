using ExchangeRatesApp.Common.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace ExchangeRatesServer.Services
{
	public class CurrencyService : BackgroundService
	{
		private readonly IMemoryCache memoryCache;
		private readonly ILogger logger;
		private static int[] listCurrency = { 145, 431, 292, 451, 298, 456, };
		private static HttpClient httpClient = new()
		{
			BaseAddress = new Uri("https://www.nbrb.by"),
		};

		public CurrencyService(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					bool createOn = false;
					if(createOn == true || !(File.Exists("ExchengeRate.json")))
					{
						createOn = true;
						List<CurrencyModel> listBasicCurrencies = new List<CurrencyModel>();
						await GetListBascCurrencies(listBasicCurrencies);
						List<NBRBModel> listCurrencyNBRB = new List<NBRBModel>();
						await GetListCurrencyNBRB(listBasicCurrencies, listCurrencyNBRB);
						var listExchengeRate = listBasicCurrencies.Join(listCurrencyNBRB,
							p => p.Id,
							c => c.Id,
							(p, c) => new ExchangeRateModel()
							{
								Currency = p.CurrencyName,
								Amount = p.CurrencyAmount,
								Date = c.Date,
								Value = c.ExchangeRate
							});

						using FileStream createStream = File.Create("ExchengeRate.json");
						await JsonSerializer.SerializeAsync(createStream, listExchengeRate);
						await createStream.DisposeAsync();

						memoryCache.Set("key_currency", listExchengeRate, TimeSpan.FromMinutes(1440));
					}
					if (File.Exists("ExchengeRate.json") && createOn == false)
					{
						createOn = true;
						using FileStream openStream = File.OpenRead("ExchengeRate.json");
						var listExchengeRate = await JsonSerializer.DeserializeAsync<List<ExchangeRateModel>>
							(openStream);

						memoryCache.Set("key_currency", listExchengeRate, TimeSpan.FromMinutes(60));
					}
				}
				catch (Exception e)
				{
					logger.LogError(e.Message);
				}

				await Task.Delay(3600000, stoppingToken);
			}
		}

		private static async Task GetListBascCurrencies(List<CurrencyModel> listBasicCurrencies)
		{
			for (int i = 0; i < listCurrency.Length; i++)
			{
				listBasicCurrencies.Add(await GetCurrency(httpClient, listCurrency[i]));
			}
		}

		private static async Task GetListCurrencyNBRB(List<CurrencyModel> currenc, List<NBRBModel> listCurrencyNBRB)
		{
			foreach (var item in currenc)
			{
				if (new DateTime(2021, 07, 08) == item.endCurrencyDate)
				{
					for (int i = 1; i <= 5; i++)
					{
						listCurrencyNBRB.AddRange(await GetDynamicsCurrencyFromJsonAsync(httpClient, item.Id, item.endCurrencyDate.AddYears(-i), item.endCurrencyDate.AddYears((i - 1) * -1)));
					}
					continue;
				}
				listCurrencyNBRB.AddRange(await GetDynamicsCurrencyFromJsonAsync(httpClient, item.Id, item.startCurrencyDate, item.endCurrencyDate));
			}
		}

		static async Task<CurrencyModel> GetCurrency(HttpClient httpClient, int idСurrency)
			=> await httpClient.GetFromJsonAsync<CurrencyModel>($"/API/ExRates/currencies/{idСurrency}");

		static async Task<List<NBRBModel>> GetDynamicsCurrencyFromJsonAsync(HttpClient httpClient, int idСurrency, DateTime startDate, DateTime endTime)
		{
			using var response = await httpClient.GetAsync($"/API/ExRates/Rates/Dynamics/{idСurrency}?startDate={startDate}&endDate={endTime}");
			response.EnsureSuccessStatusCode();

			var listDynamicsCurrency = await response.Content.ReadFromJsonAsync<List<NBRBModel>>();
			return listDynamicsCurrency;
		}
	}
}
