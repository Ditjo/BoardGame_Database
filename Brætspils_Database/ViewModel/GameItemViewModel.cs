using Brætspils_Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Brætspils_Database.ViewModel
{
    //Uses the Model as base to the ViewModel.
    //You get the model object and are able to get and set the different fields of the object.
    // The 'OnPropertyChanged()' calls for the UI to update.
    public class GameItemViewModel : ValidationViewModelBase
    {
        private readonly Game _model;

        public GameItemViewModel(Game model)
        {
            this._model = model;
        }

        public int Id => _model.Id;

        public string? Titel
        {
            get => _model.Titel;
            set 
            { 
                _model.Titel = value;
                OnPropertyChanged();
                if (string.IsNullOrEmpty(_model.Titel))
               {
                    AddError("Titel is required");
                    
                }
                else
                {
                    ClearErrors();
                }
            }

        }
        public string? Players
        {
            get => _model.Players;
            set
            {
                _model.Players = value;
                OnPropertyChanged();
            }

        }
        public string? Time
        {
            get => _model.Time;
            set
            {
                _model.Time = value;
                OnPropertyChanged();
            }

        }

    }
}
