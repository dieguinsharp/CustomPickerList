using CustomSelectableList.Core;
using CustomSelectableList.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CustomSelectableList.Custom.ViewModel {
    public class SelectableList : BaseViewModel {

        public RelayCommand SelectableCommand { get; set; }

        private ValidateCase _validateCase;

        private ObservableCollection<Model.User> items;
        public ObservableCollection<Model.User> Items {
            get { return items; }
            set {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public SelectableList (IEnumerable<Model.User> items, ValidateCase validateCase) {
            _validateCase = validateCase;
            SelectableCommand = new RelayCommand(user => SelectableItem(user as Model.User));
            Items = new ObservableCollection<Model.User>(items);
        }

        public void SelectableItem (Model.User user) {
            if(_validateCase.DoValidate(user))
                this.Items.Remove(user);
        }

        public void OpenSelectableList () {
            var page = new Custom.SelectableList();
            page.DataContext = this;

            page.Show();
        }

    }

    public class Validate {
        private Action<Model.User> _execute { get; set; }
        private Func<object, bool> _validation;

        public Validate (Action<Model.User> execute, Func<object, bool> validation) {
            _execute = execute;
            _validation = validation;
        }

        public bool DoVerify (Model.User user) {

            if(_validation(user)) {
                _execute?.Invoke(user);
                return true;
            }
            
            return false;

        } 
    }

    public class ValidateCase {

        private List<Validate> _trueValidate;
        private List<Validate> _falseValidate;

        public ValidateCase (List<Validate> trueValidate, List<Validate> falseValidate) {
            _trueValidate = trueValidate;
            _falseValidate = falseValidate;
        }

        public bool DoValidate (Model.User user) {

            var falseCase = ValidateFalseCase(user);

            bool trueCase = false;
            if(!falseCase) {
                trueCase = ValidateTrueCase(user);
            }

            return trueCase;
        }

        private bool ValidateTrueCase(Model.User user) {
            bool alreadyValidated = false;

            _trueValidate.ForEach(tv => {     
                if(!alreadyValidated) {
                    alreadyValidated = tv.DoVerify(user);
                }           
            });

            return alreadyValidated;
        }

        private bool ValidateFalseCase(Model.User user) {
            bool alreadyValidated = false;
            _falseValidate.ForEach(tv => {
                if(!alreadyValidated)
                    alreadyValidated = tv.DoVerify(user);
            });

            return alreadyValidated;
        }
    }
}
