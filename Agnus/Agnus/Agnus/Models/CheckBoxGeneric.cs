using Agnus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Models
{
    public class CheckBoxGeneric<T> : ViewModelBase
    {
        private bool _IsChecked { get; set; }
        public bool IsChecked 
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public T value { get; set; }
    }
}
