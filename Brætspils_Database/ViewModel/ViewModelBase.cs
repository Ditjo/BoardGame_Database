using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Brætspils_Database.ViewModel
{
    //'INotifyPropertyChanged' is used to update the UI
    // Or tell the UI to update
    // It's implemintet in ViewModleBase Class for all other ViewModels to inherit

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        //Choose View Related
        public virtual Task LoadAsync() => Task.CompletedTask;
    }
}
