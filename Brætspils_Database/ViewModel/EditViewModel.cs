using Brætspils_Database.Command;
using Brætspils_Database.Data;
using Brætspils_Database.Model;
using Brætspils_Database.Sql_Querys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.ViewModel
{
    public class EditViewModel : ViewModelBase
    {
        //Commands properties The Buttons bind too

        public DelegateCommand AddCommand { get; }
        public DelegateCommand EditCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand UndoCommand { get; }

        public DelegateCommand ReloadListCommand { get; }


        //GameItemViewModel "is the same" as the Model, however it includes more code to update Bindings.
        //It is made so to not "Break" the MVVM pattern.

        //private readonly IGameDataProvider _gameDataProvider;

        //The Commands that are put into the properties 
        public EditViewModel(/*IGameDataProvider gameDataProvider*/)
        {
            //this._gameDataProvider = gameDataProvider;
            AddCommand = new DelegateCommand(Add, CanAdd);
            EditCommand = new DelegateCommand(Edit, CanEdit);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            SaveCommand = new DelegateCommand(Save, CanSave);
            UndoCommand = new DelegateCommand(Undo, CanUndo);
            ReloadListCommand = new DelegateCommand(ReloadAsync);
        }

        private GameItemViewModel? _latestSavedGame;
        private GameItemViewModel? _selectedGame;

        public ObservableCollection<GameItemViewModel> Games { get; } = new();

        //Property to select game
        public GameItemViewModel? SelectedGame
        {
            get
            {
                return _selectedGame;
            }
            set
            {
                if (SelectedGame != value)
                {
                    _selectedGame = value;
                    OnPropertyChanged();
                    CallOnCanExecuteChanged();
                }

            }
        }

        //Used as a 'Switch' to enable and disable buttons, and the viewlist from interaction 
        private bool _setIsReadOnlyAndIsEnabled = true;

        public bool SetIsReadOnlyAndIsEnabled
        {
            get => _setIsReadOnlyAndIsEnabled;
            set
            {
                if (SetIsReadOnlyAndIsEnabled != value)
                {
                    _setIsReadOnlyAndIsEnabled = value;
                    OnPropertyChanged();
                }

            }
        }

        //Loads Data from Database into OberservableCollection Games, using GetAllAsync method
        public async override Task LoadAsync()
        {
            if (Games.Any())
            {
                return;
            }
            using (EF_Connection connection = new EF_Connection())
            {
                //var games = await _gameDataProvider.GetAllAsync(SqlStatment.SqlQuarySelectAll);
                var games = await connection.GetAllGamesAsync(SqlStatment.SqlQuarySelectAll);
                if (games is not null)
                {
                    foreach (var game in games)
                    {
                        Games.Add(new GameItemViewModel(game));
                    }
                }
            }
        }
        //Reload list of games
        private async void ReloadAsync(object? parameter)
        {
            Games.Clear();
            await LoadAsync();
            OnPropertyChanged(nameof(Games));
            System.Diagnostics.Debug.WriteLine("Reloading...");
        }

        //Call for the commands to update their CanExecute Method
        public void CallOnCanExecuteChanged()
        {
            AddCommand.OnCanExecuteChanged();
            EditCommand.OnCanExecuteChanged();
            DeleteCommand.OnCanExecuteChanged();
            SaveCommand.OnCanExecuteChanged();
            UndoCommand.OnCanExecuteChanged();
        }

        //Creats new Instance of Game. Using Game to create new GameItemViewModel called viewModel
        //Adds viewModel to Games Obs.Collect. & Selects the Game in UI.
        private void Add(object? parameter)
        {
            var game = new Game { Titel = "New" };
            var viewModel = new GameItemViewModel(game);
            Games.Add(viewModel);
            SelectedGame = viewModel;

            //using (Connection connector = new Connection())
            //{
            //    //string par = "@Titel";
            //    //List<Game> list = new List<Game>();
            //    //list.Add(game);
            //    //list.Add(new SqlParameter($"{par}", SelectedGame.Titel));

            //    game.Id = connector.AddNewInstance(game);
                
            //}
            using (EF_Connection connection = new())
            {
                game.Id = connection.AddNewInstance(game);
            }
            OnPropertyChanged(nameof(SelectedGame));
            SetIsReadOnlyAndIsEnabled = false;
            CallOnCanExecuteChanged();
        }

        private bool CanAdd(object? parameter)
        {
            return SetIsReadOnlyAndIsEnabled is true;
        }

        //Edit the Selected Instance
        private void Edit(object? parameter)
        {
            _latestSavedGame = new GameItemViewModel(new Game()
            {
                Id = SelectedGame.Id,
                Players = SelectedGame.Players,
                Time = SelectedGame.Time,
                Titel = SelectedGame.Titel
            });
            SetIsReadOnlyAndIsEnabled = false;
            CallOnCanExecuteChanged();
        }

        private bool CanEdit(object? parameter)
        {
            return SelectedGame is not null && SetIsReadOnlyAndIsEnabled is true;
        }

        //Delete Command to delete games from the list
        private void Delete(object? parameter)
        {
            if (SelectedGame is not null)
            {
                
                using (EF_Connection Connector = new())
                {
                    Game game = new();
                    game.Id = SelectedGame.Id;
                    game.Players = SelectedGame.Players;
                    game.Time = SelectedGame.Time;
                    game.Titel = SelectedGame.Titel;

                    string par = "@Id";
                    List<SqlParameter> list = new List<SqlParameter>();
                    list.Add(new SqlParameter($"{par}", SelectedGame.Id));
                    Connector.DeleteInstance(game);
                }
                Games.Remove(SelectedGame);
                SelectedGame = null;

                SetIsReadOnlyAndIsEnabled = true;
                CallOnCanExecuteChanged();
            }
        }

        private bool CanDelete(object? parameter)
        {
            return SetIsReadOnlyAndIsEnabled is not true;
        }

        //Save the changes made doing Edit
        private void Save(object? parameter)
        {
            if (SelectedGame is not null)
            {
                using (EF_Connection Connector = new())
                {
                    Game game = new();
                    game.Id = SelectedGame.Id;
                    game.Players = SelectedGame.Players;
                    game.Time = SelectedGame.Time;
                    game.Titel = SelectedGame.Titel;

                    string par = "@Id";
                    List<SqlParameter> list = new List<SqlParameter>();
                    list.Add(new SqlParameter($"{par}", SelectedGame.Id));
                    Connector.UpdateInstance(game);
                }
            }
            SetIsReadOnlyAndIsEnabled = true;
            CallOnCanExecuteChanged();
        }

        private bool CanSave(object? parameter)
        {
            return SelectedGame != null && SetIsReadOnlyAndIsEnabled is not true && !string.IsNullOrEmpty(SelectedGame.Titel);
        }

        //Undo the Changes back to before the Edit
        private void Undo(object? parameter)
        {
            if (SelectedGame is not null)
            {
                _selectedGame = _latestSavedGame;
                GameItemViewModel game = Games.Where(x => x.Id == _selectedGame.Id)
                    .FirstOrDefault();
                var index = Games.IndexOf(game);
                Games[index] = _selectedGame;
            }
            SetIsReadOnlyAndIsEnabled = true;
            CallOnCanExecuteChanged();
        }

        private bool CanUndo(object? parameter)
        {
            return SetIsReadOnlyAndIsEnabled is not true;
        }
    }
}
