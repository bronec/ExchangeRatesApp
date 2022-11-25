using ExchangeRatesApp.Common.Models;
using ExchangeRatesApp.Common.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeRatesServer.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CurrencyController : ControllerBase
	{
		private readonly IMemoryCache memoryCache;
		private readonly ILogger<CurrencyController> logger;


		public CurrencyController(IMemoryCache memoryCache, ILogger<CurrencyController> logger)
		{
			this.memoryCache = memoryCache;
			this.logger = logger;
		}

		[HttpPost()]
		public ActionResult<List<ExchangeRateModel>> Get(GetExchangeRatesRequestModel model)
		{
			if (!memoryCache.TryGetValue("key_currency", out List<ExchangeRateModel> exchangeRates))
			{
				return NotFound();
			}
			var name = model.Currency.ToString().ToUpper();
			var test = exchangeRates.Where(x => x.Currency == name).ToList();
			var requestedExchangeRate = exchangeRates
				.Where(x => x.Currency == name
				&& x.Date >= model.StartTime
				&& x.Date <= model.EndTime)
			.OrderBy(p => p.Date);

			logger.LogInformation("Processing GET request");
			return requestedExchangeRate.ToList();
		}
	}
}
