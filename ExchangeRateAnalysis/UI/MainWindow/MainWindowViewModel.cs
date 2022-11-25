using ExchangeRateAnalysis.EventArguments;
using ExchangeRateAnalysis.Model;
using ExchangeRateAnalysis.Services;
using ExchangeRatesApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ExchangeRateAnalysis.UI.MainWindow
{
	public class MainWindowViewModel : BaseViewModel
	{
		private readonly IExchangeRatesService service;
		private DateTime? startDate;
		private DateTime? endDate;
		private CurrencyDisplayModel? selectedCurrency;
		private ICommand submit;

		public MainWindowViewModel(IExchangeRatesService service)
		{
			this.service = service;
		}

		public event EventHandler<ExchangeRatesEventArguments> ExchangeRatesUpdated;

		public List<CurrencyDisplayModel> Currencies
			=> new List<CurrencyDisplayModel>()
			{
				new CurrencyDisplayModel(Currency.USD),
				new CurrencyDisplayModel(Currency.EUR),
				new CurrencyDisplayModel(Currency.RUB)
			};

		public CurrencyDisplayModel? SelectedCurrency
		{
			get => selectedCurrency;
			set
			{
				selectedCurrency = value;
				OnPropertyChanged();
			}
		}

		public DateTime? StartDate
		{
			get => startDate;
			set
			{
				startDate = value;
				OnPropertyChanged();
			}
		}

		public DateTime? EndDate
		{
			get => endDate;
			set
			{
				endDate = value;
				OnPropertyChanged();
			}
		}

		public ICommand Submit
			=> submit = submit ?? new AsyncRelayCommand(async obj =>
			{
				var exchangeRates = await service.GetExchangeRatesAsync(SelectedCurrency.Value, StartDate.Value, EndDate.Value);
				ExchangeRatesUpdated?.Invoke(this, new ExchangeRatesEventArguments(exchangeRates));
			}, 
				obj => StartDate.HasValue && EndDate.HasValue && SelectedCurrency != null);
	}
}
