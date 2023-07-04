using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeApp_GUI.Utilities;
using System.Windows.Input;

namespace RecipeApp_GUI.ViewModel
{
    //This class controls the page navigation function
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; onPropertyChanged(); }   
        }
        //Objects are created using Icommand so that it uses the code in RelayCommand
        public ICommand HomeCommand { get; set; }
        public ICommand AddRecipeCommand { get; set; }
        public ICommand RecipeListCommand { get; set; }
        //Create a new instance of that object on our main window so that it will display.
        private void Home(object obj) => CurrentView = new HomeVM();
        private void AddRecipe(object obj) => CurrentView = new AddRecipeVM();
        private void RecipeList(object obj) => CurrentView = new RecipeListVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            AddRecipeCommand = new RelayCommand(AddRecipe);
            RecipeListCommand = new RelayCommand(RecipeList);

            //Startup Page
            CurrentView = new HomeVM();
        }
    }
}
