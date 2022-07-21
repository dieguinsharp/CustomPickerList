using CustomSelectableList.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomSelectableList.Extension;
using CustomSelectableList.Custom.ViewModel;
using System.Windows;
using System.Linq;

namespace CustomSelectableList.ViewModel {
    public class Main : BaseViewModel {

        public RelayCommand OpenCommand { get; set; }

        private ObservableCollection<Model.User> items;
        public ObservableCollection<Model.User> Items {
            get { return items; }
            set {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public Main () {
            OpenCommand = new RelayCommand(user => OpenSelectableList());
            LoadItems();
        }

        public void LoadItems () {
            Items = new ObservableCollection<Model.User>(GetUsers(10));
        }

        public void OpenSelectableList () {

            var canAdd1 = new Validate(_ => Add(_), _ => (Items.Count + 1) <= 15);
            var notAdd1 = new Validate(_ => ShowErrorMore30Item(), _ => (Items.Count + 1) > 15);

            var canAdd2 = new Validate(_ => Add(_), _ => !Items.Any(a => a.ID == (_ as Model.User).ID));
            var notAdd2 = new Validate(_ => ShowErrorSameItem(), _ => Items.Any(a => a.ID == (_ as Model.User).ID)); 

            var users = GetUsers(20);
            users.OpenSelectableList(new ValidateCase(
                new List<Validate>() { canAdd1, canAdd2 },
                new List<Validate>() { notAdd1, notAdd2 }
            ));
        }

        public void ShowErrorMore30Item () {
            MessageBox.Show("Error","Impossivel add mais que 15 items.");
        }

        public void ShowErrorSameItem () {
            MessageBox.Show("Error","Impossivel add itens iguais.");
        }

        public void Add (Model.User user) {
            Items.Add(user);
        }

        public IEnumerable<Model.User> GetUsers (int count) {
            var items = new List<Model.User>();
            for(int x = 0;x < count; x++) {
                items.Add(new Model.User() { ID = x,Name = "User " + x });
            }

            return items;
        } 
    }
}
