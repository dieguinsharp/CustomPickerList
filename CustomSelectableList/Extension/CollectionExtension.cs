using CustomSelectableList.Custom.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CustomSelectableList.Extension {
    public static class CollectionExtension {
        public static void OpenSelectableList (this IEnumerable<Model.User> items, ValidateCase validateCase) {
            var selectableList = new SelectableList(items, validateCase);
            selectableList.OpenSelectableList();
        }
    }
}
