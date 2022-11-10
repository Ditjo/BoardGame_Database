using Brætspils_Database.Data;
using Brætspils_Database.Sql_Querys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.ViewModel
{
    public class FrontpageViewModel : ViewModelBase
    {
        private GameItemViewModel? _randomGame;
        //private readonly IGameDataProvider _gameDataProvider;

        //public FrontpageViewModel(IGameDataProvider gameDataProvider)
        //{
        //    _gameDataProvider = gameDataProvider;
        //}

        public ObservableCollection<GameItemViewModel> GameList { get; } = new();

        public async override Task LoadAsync()
        {
            if (GameList.Any())
            {
                GameList.Clear();
            }
            using (EF_Connection connection = new())
            {
                var games = await connection.GetAllGamesAsync(SqlStatment.SqlQuarySelectAll);
                //var games = await _gameDataProvider.GetAllAsync(SqlStatment.SqlQuarySelectAll);
                if (games is not null)
                {
                    foreach (var game in games)
                    {
                        GameList.Add(new GameItemViewModel(game));
                    }

                }
                PickRandomGame();
            }
        }

        public GameItemViewModel? RandomGame
        {
            get
            {
                //PickRandomGame();
                return _randomGame;
            }
            set
            {
                if (RandomGame != value)
                {
                    _randomGame = value;
                    OnPropertyChanged();
                    //CallOnCanExecuteChanged();
                }

            }
        }

        public void PickRandomGame()
        {
            var random = new Random();
            int game = random.Next(GameList.Count());
            RandomGame = GameList.ElementAt(game);
        }
    }
}
