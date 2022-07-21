using CustomSelectableList.Core;
using CustomSelectableList.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CustomSelectableList.Extension {
    public static class CollectionExtension {
        public static void OpenSelectableList<T>(this IEnumerable<T> items, ValidateCase<T> validateCase) {
            var selectableList = new PickerViewModel<T>(items, validateCase);
            selectableList.OpenSelectableList();
        }
    }

    public class PickerViewModel<T> : BaseViewModel {

        public DelegateCommand<T> SelectableCommand { get; set; }

        private ValidateCase<T> _validateCase;

        private ObservableCollection<T> items;
        public ObservableCollection<T> Items {
            get { return items; }
            set {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public PickerViewModel (IEnumerable<T> items, ValidateCase<T> validateCase) {
            _validateCase = validateCase;
            SelectableCommand = new DelegateCommand<T>(user => SelectableItem(user));
            Items = new ObservableCollection<T>(items);
        }

        public void SelectableItem (T item) {
            if(_validateCase.DoValidate(item))
                this.Items.Remove(item);
        }

        public void OpenSelectableList () {
            var page = new Custom.PickerView();
            page.DataContext = this;

            page.Show();
        }

    }

    public class Validate<T> {
        private Action<T> _execute { get; set; }
        private Func<T, bool> _validation;

        public Validate (Action<T> execute, Func<T, bool> validation) {
            _execute = execute;
            _validation = validation;
        }

        public bool DoVerify (T item) {

            if(_validation(item)) {
                _execute?.Invoke(item);
                return true;
            }
            
            return false;

        } 
    }

    public class ValidateCase<T> {

        private List<Validate<T>> _trueValidates;
        private List<Validate<T>> _falseValidates;

        public ValidateCase (List<Validate<T>> trueValidate, List<Validate<T>> falseValidate) {
            _trueValidates = trueValidate;
            _falseValidates = falseValidate;
        }

        public bool DoValidate (T item) {

            var falseCase = ValidateCases(item, _falseValidates);

            bool trueCase = false;
            if(!falseCase) {
                trueCase = ValidateCases(item, _trueValidates);
            }

            return trueCase;
        }

        private bool ValidateCases(T item, List<Validate<T>> validates) {
            bool alreadyValidated = false;

            validates.ForEach(tv => {     
                if(!alreadyValidated) {
                    alreadyValidated = tv.DoVerify(item);
                }           
            });

            return alreadyValidated;
        }
    }
}
