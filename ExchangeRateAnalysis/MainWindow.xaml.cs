using ExchangeRateAnalysis.EventArguments;
using ExchangeRateAnalysis.Services;
using ExchangeRateAnalysis.UI.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExchangeRateAnalysis
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			StartDatePicker.DisplayDate = DateTime.Now;
			EndDatePicker.DisplayDate = DateTime.Now;
			var collection = new List<CalendarDateRange>();
			collection.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now.AddYears(-5)));
			collection.Add(new CalendarDateRange(DateTime.Now.AddDays(1).Date, DateTime.MaxValue));
			foreach(var dateRange in collection)
			{
				StartDatePicker.BlackoutDates.Add(dateRange);
				EndDatePicker.BlackoutDates.Add(dateRange);
			}
			SetupViewModel();
		}

		private void SetupViewModel()
		{
			var viewModel = new MainWindowViewModel(new ExchangeRatesService());
			viewModel.ExchangeRatesUpdated += ViewModel_ExchangeRatesUpdated;
			DataContext = viewModel;
		}

		private void ViewModel_ExchangeRatesUpdated(object? sender, ExchangeRatesEventArguments e)
		{
			Chart.Reset();
			var days = e.ExchangeRates.Select(x => x.Date.ToOADate()).ToArray();
			var values = e.ExchangeRates.Select(x => decimal.ToDouble(x.Value)).ToArray();
			Chart.Plot.AddScatter(days, values);
			Chart.Plot.XAxis.TickLabelFormat("dd\\/M", dateTimeFormat: true);
			Chart.Refresh();
		}
	}
}
