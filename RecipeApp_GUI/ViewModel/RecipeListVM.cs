using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipeApp_GUI.Utilities;
using RecipeApp_GUI.ViewModel;
using RecipeApp_GUI.Views;

namespace RecipeApp_GUI.ViewModel
{
    class RecipeListVM
    {


        public ObservableCollection<AddRecipeVM>recipeList { get; set; }

        public ICommand showWindowCommand { get; set; }

        public RecipeListVM()
        {
            
            showWindowCommand = new RelayCommand(ShowWindow, CanShowWindow);


        }

        private bool CanShowWindow(object arg)
        {
            throw new NotImplementedException();
        }

        private void ShowWindow(object obj)
        {
            RecipeList showRecipeWin = new RecipeList();
           
        }
    }
}
