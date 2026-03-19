using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexuSys.Helper
{
    public class CheckBoxGeneric<T>
    {
        private bool _IsChecked { get; set; }
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
            }
        }

        public T value { get; set; }
    }
}
