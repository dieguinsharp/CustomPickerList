using CustomSelectableList.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CustomSelectableList.Extension;
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

            var validateAdd1 = new Validate<Model.User>(_ => Add(_), _ => (Items.Count + 1) <= 15);
            var validateNotAdd1 = new Validate<Model.User>(_ => ShowErrorMore15Item(), _ => (Items.Count + 1) > 15);

            var validadeAdd2 = new Validate<Model.User>(_ => Add(_), _ => !Items.Any(a => a.ID == (_).ID));
            var validateNotAdd2 = new Validate<Model.User>(_ => ShowErrorSameItem(), _ => Items.Any(a => a.ID == (_).ID)); 

            var users = GetUsers(20);
            users.OpenSelectableList(new ValidateCase<Model.User>(
                new List<Validate<Model.User>>() { validateAdd1, validadeAdd2, },
                new List<Validate<Model.User>>() { validateNotAdd1, validateNotAdd2, }
            ));
        }

        public void ShowErrorMore15Item () {
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
