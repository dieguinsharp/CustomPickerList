using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CustomSelectableList.ViewModel {
    public class BaseViewModel : INotifyPropertyChanged {

        private string title;
        public string Title { get { return title; } set { title = value; OnPropertyChanged(nameof(Title)); } }

        public BaseViewModel () {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null) {
            if(propertyName != null) {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
