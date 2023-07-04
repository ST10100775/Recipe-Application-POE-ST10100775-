using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Windows.Input;
using RecipeApp_GUI.Views;
using System.Data;
using System.Collections.ObjectModel;
using System.CodeDom;

namespace RecipeApp_GUI.ViewModel
{

   //This class = data getter and setters for our Recipe Name,Ingredients and steps
   //AddRecipe Class
    class AddRecipeVM : INotifyPropertyChanged
    {


        public string? recName { get; set; }
        
        public Ingredient Ingredients { get; set; }
       
        public stepDescription stepDescriptions { get; set; }
        public List<stepDescription> Steps { get; set; }

        public List<Ingredient> IngredientS { get; set; }
        public int TotalCalories { get; set; }
        public List<string> FoodGroups { get; set; }
        public AddRecipeVM()
        {
            this.Ingredients = new Ingredient();
            this.stepDescriptions = new stepDescription();
            recName = "";

            Steps = new List<stepDescription>();

            IngredientS = new List<Ingredient>();

            FoodGroups = new List<string>();
        }



       
        

       



        public event PropertyChangedEventHandler? PropertyChanged;
    }
    
    class Ingredient: INotifyPropertyChanged
    {

        public string? ingName { get; set; }

        public ComboBox? foodGroup { get; set; }

        public int? amount { get; set; }

        public string unit { get; set; }

        public int? calorie { get; set; }
        public Ingredient()
        {
            ingName = "";
            amount = 0;
            unit = "";
            calorie = null;
            foodGroup = new ComboBox();
            foodGroup.Items.Add("Fruit");
            foodGroup.Items.Add("Vegetables");
            foodGroup.Items.Add("Carbohydrate");
            foodGroup.Items.Add("Protein");
            foodGroup.Items.Add("Dairy");

            foodGroup.SelectedIndex = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        
    }
    class stepDescription : INotifyPropertyChanged
    {
        public string? stepDesc { get; set; }
        public stepDescription()
        {
            stepDesc = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
