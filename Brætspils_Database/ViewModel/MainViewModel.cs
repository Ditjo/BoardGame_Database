using Brætspils_Database.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        private ViewModelBase? _selectedViewModel;
		

		public MainViewModel(
			EditViewModel editViewModel,
			ListViewModel listViewModel,
			FrontpageViewModel frontpageViewModel)
		{
			this.EditViewModel = editViewModel;
			this.ListViewModel = listViewModel;
			this.FrontpageViewModel = frontpageViewModel;
			SelectedViewModel = editViewModel;
			SelectViewModelCommand = new DelegateCommand(SelectViewModel);
		}

		private async void SelectViewModel(object? parameter)
		{
			SelectedViewModel = parameter as ViewModelBase;
			await LoadAsync();
		}

		public ViewModelBase? SelectedViewModel
		{
			get =>_selectedViewModel; 
			set 
			{
				_selectedViewModel = value;
				OnPropertyChanged();
			}
		}
		public FrontpageViewModel FrontpageViewModel { get; }
		public EditViewModel EditViewModel { get; }
		public ListViewModel ListViewModel { get; }
		public DelegateCommand SelectViewModelCommand { get; }

		public async override Task LoadAsync()
		{
			if(SelectedViewModel is not null)
			{
				await SelectedViewModel.LoadAsync();
			}
		}
	}
}
