using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelSupport;

namespace ViewModelSupportTestApp
{
    class MainWindowVM : ViewModelBase
    {
        public string TextBoxText { get; set; } = "Enter text...";
        public string LabelText { get; set; } = "Label Text";

        public ObservableCollection<string> History { get; set; } = new ObservableCollection<string>();

        public void Exec_Enter()
        {
            LabelText = TextBoxText;
            RaisePropertyChanged("LabelText"); // strings don't seem to automatically raise their changed event, so do it manually.
            
            History.Add(TextBoxText); // Observable collections DO seem to automatically raise their changed event.
        }


    }
}
