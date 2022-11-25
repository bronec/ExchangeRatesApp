using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExchangeRateAnalysis.UI
{
	public class AsyncRelayCommand : ICommand
	{
		private readonly Func<object, bool> canExecute;
		private readonly Func<object, Task> execute;

		public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public event EventHandler? CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object? parameter)
		{
			try
			{
				return canExecute == null || canExecute(parameter);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		public async void Execute(object? parameter)
		{
			try
			{
				await execute?.Invoke(parameter);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
