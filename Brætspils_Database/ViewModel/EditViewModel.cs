using Brætspils_Database.Command;
using Brætspils_Database.Data;
using Brætspils_Database.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        //GameItemViewModel "is the same" as the Model, however it includes more code to update Bindings.
        //It is made so to not "Break" the MVVM pattern.

        private readonly IGameDataProvider _gameDataProvider;

        //The Commands that are put into the properties 
        public EditViewModel(IGameDataProvider gameDataProvider)
        {
            this._gameDataProvider = gameDataProvider;
            AddCommand = new DelegateCommand(Add, CanAdd);
            EditCommand = new DelegateCommand(Edit, CanEdit);
            DeleteCommand = new DelegateCommand(Delete, CanDelete);
            SaveCommand = new DelegateCommand(Save, CanSave);
            UndoCommand = new DelegateCommand(Undo, CanUndo);
        }

        //Instantiate's ObservableCollection of type GameItemViewModel called Games
        public ObservableCollection<GameItemViewModel> Games { get; } = new();

        private GameItemViewModel? _latestSavedGame;
        private GameItemViewModel? _selectedGame;
        //Property to select game
        public GameItemViewModel? SelectedGame
        {
            //get => _selectedGame;
            get
            {
                return _selectedGame;
            }
            set
            {
                _selectedGame = value;
                OnPropertyChanged();
                CallOnCanExecuteChanged();
            }
        }
        
        //Used as a 'Switch' to enable and disable buttons, and the viewlist from interaction 
        private bool _setIsReadOnlyAndIsEnabled = true;

        public bool SetIsReadOnlyAndIsEnabled
        {
            get => _setIsReadOnlyAndIsEnabled;
            set
            {
                _setIsReadOnlyAndIsEnabled = value;
                OnPropertyChanged();

            }
        }

        //Loads Data from Database into OberservableCollection Games, using GetAllAsync method
        public async override Task LoadAsync()
        {
            if (Games.Any())
            {
                return;
            }
            var games = await _gameDataProvider.GetAllAsync();
            if (games is not null)
            {
                foreach (var game in games)
                {
                    Games.Add(new GameItemViewModel(game));
                }
            }
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

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;

            string sqlQuary = $"INSERT INTO Games (Titel) VALUES (@Titel)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuary, conn))
                {
                    cmd.Parameters.AddWithValue("@Titel", game.Titel);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            SetIsReadOnlyAndIsEnabled = false;
            CallOnCanExecuteChanged();
        }
        private bool CanAdd (object? parameter)
        {
            return SetIsReadOnlyAndIsEnabled is true;
        }
        //Edit the Selected Instance
        private void Edit(object? parameter)
        {
            _latestSavedGame = _selectedGame;
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
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;

                string sqlQuary = $"DELETE FROM Games WHERE ID = @Id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlQuary, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", SelectedGame.Id);

                        cmd.CommandType = System.Data.CommandType.Text;

                        conn.Open();

                        cmd.ExecuteNonQuery();
                    }
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
            var ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataBaseConnection"].ConnectionString;

            string sqlQuary = $"UPDATE Games SET Titel = @Titel, Players = @Players, Playtime = @Time WHERE ID = @Id";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuary, conn))
                {
                    cmd.Parameters.AddWithValue("@Titel", SelectedGame.Titel != null ? SelectedGame.Titel : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Players", SelectedGame.Players != null ? SelectedGame.Players : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Time", SelectedGame.Time != null ? SelectedGame.Time : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ID", SelectedGame.Id);

                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();

                    cmd.ExecuteNonQuery();

                }
            }
            SetIsReadOnlyAndIsEnabled = true;
            CallOnCanExecuteChanged();
        }
        private bool CanSave(object? parameter)
        {
            return SelectedGame != null && SetIsReadOnlyAndIsEnabled is not true && SelectedGame.Titel is not null;
        }
        //Undo the Changes back to before the Edit
        private void Undo(object? parameter)
        {
            //Needs Fixing
            _selectedGame = _latestSavedGame;
            OnPropertyChanged();
            SetIsReadOnlyAndIsEnabled = true;
        }
        private bool CanUndo(object? parameter)
        {
            return SetIsReadOnlyAndIsEnabled is not true;
        }
    }
}
