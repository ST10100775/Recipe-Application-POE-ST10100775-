using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using RecipeApp_GUI.Utilities;

namespace RecipeApp_GUI.Views
{
    public partial class RecipeList : UserControl
    {
        //RecipeList & filtered list.
        //filterRecipeList = search for a recipe with the search box
        public ObservableCollection<RecipeFile> recipeList { get; set; }
        public ObservableCollection<RecipeFile> filteredRecipeList { get; set; }
        private string searchText;
        //The XAML bind the data points to file
        public RecipeList()
        {
            InitializeComponent();
            recipeList = new ObservableCollection<RecipeFile>();
            filteredRecipeList = new ObservableCollection<RecipeFile>();
            DataContext = this;

            Loaded += RecipeList_Loaded;
        }

        private void RecipeList_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRecipeFiles();
        }

        private void LoadRecipeFiles()
        {
            // Recipe folder path location.
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recipes");

            try
            {
                // This retrieves JSON files in folder
                string[] filePaths = Directory.GetFiles(folderPath, "*.json");

                // Clear existing recipe list
                recipeList.Clear();

                foreach (string filePath in filePaths)
                {
                    // Read the contents of the file and deserialize data
                    string fileContents = File.ReadAllText(filePath);
                    Recipe recipe = JsonConvert.DeserializeObject<Recipe>(fileContents);

                    // New RecipeFile object adds  to collection, Obtain data from the file path
                    RecipeFile recipeFile = new RecipeFile
                    {
                        RecipeName = Path.GetFileNameWithoutExtension(filePath),
                        FilePath = filePath,
                        DateCreated = File.GetCreationTime(filePath),
                        DateModified = File.GetLastWriteTime(filePath),
                        Calorie = CalculateTotalCalories(filePath),
                        FoodGroup = GetFoodGroups(filePath)
                    };
                    recipeList.Add(recipeFile);
                }

                // Sorts recipes in our DataGrid in alphabetical order.
                recipeList = new ObservableCollection<RecipeFile>(recipeList.OrderBy(r => r.RecipeName));
                filteredRecipeList = recipeList;
                recipeDataGrid.ItemsSource = filteredRecipeList; // Updates the ItemsSource property

            }
            catch (Exception ex)
            {
                // Error message if the page can't find our folder or if the JSON information is wrong
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred while loading recipe files: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }

        private int CalculateTotalCalories(string filePath)
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                Recipe recipe = JsonConvert.DeserializeObject<Recipe>(fileContents);

                int totalCalories = 0;
                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    if (int.TryParse(ingredient.Calorie, out int calorie))
                    {
                        totalCalories += calorie;
                    }
                }
                return totalCalories;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while calculating total calories: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return 0;
            }
        }

        private string GetFoodGroups(string filePath)
        {
            try
            {
                string fileContents = File.ReadAllText(filePath);
                Recipe recipe = JsonConvert.DeserializeObject<Recipe>(fileContents);

                return string.Join(", ", recipe.Ingredients.Select(i => i.FoodGroup).Distinct());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while retrieving food groups: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return string.Empty;
            }
        }

        //Display a message box for the recipe we selected (double click it).
        
        private void recipeDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecipeFile selectedRecipe = recipeDataGrid.SelectedItem as RecipeFile;
            if (selectedRecipe != null)
            {
                string filePath = selectedRecipe.FilePath;

                try
                {
                    string fileContents = File.ReadAllText(filePath);
                    Recipe recipe = JsonConvert.DeserializeObject<Recipe>(fileContents);

                    RecipeDetailsWindow detailsWindow = new RecipeDetailsWindow(recipe); // Pass the Recipe object
                    detailsWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while reading the file: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }





        //Edit authorization the contents of our JSON file 
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            RecipeFile selectedRecipe = recipeDataGrid.SelectedItem as RecipeFile;
            if (selectedRecipe != null)
            {
                // Access the file path
                string filePath = selectedRecipe.FilePath;

                try
                {
                    Process.Start("notepad.exe", filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while opening the recipe file: {ex.Message}\n\n{ex.StackTrace}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }


        
        //Deletes the file in the folder Button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            RecipeFile selectedRecipe = recipeDataGrid.SelectedItem as RecipeFile;
            if (selectedRecipe != null)
            {
                // Access  file path
                string filePath = selectedRecipe.FilePath;

                try
                {
                    // Delete the file
                    File.Delete(filePath);

                    // Remove the deleted recipe file from the collection
                    recipeList.Remove(selectedRecipe);
                    filteredRecipeList.Remove(selectedRecipe);

                    MessageBox.Show("File successfully deleted.", "File Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"An error occurred while loading recipe file '{filePath}': {ex}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    });
                }

            }
        }
        //This will allow the user to search
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            searchText = searchTextBox.Text.ToLower();
            ApplyFilter();
        }
        //This method is our filter code that will implement the search filter logic for our data grid.
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                filteredRecipeList = recipeList;
            }
            else
            {
                filteredRecipeList = new ObservableCollection<RecipeFile>(
                    recipeList.Where(r =>
                        r.RecipeName.ToLower().Contains(searchText) ||
                        r.Calorie.ToString().Contains(searchText) ||
                        (r.FoodGroup != null && r.FoodGroup.ToLower().Contains(searchText))
                    ));
            }

            recipeDataGrid.ItemsSource = filteredRecipeList;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    // Class holds the recipe information from our JSON.
    public class RecipeFile
    {
        public string RecipeName { get; set; }
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int Calorie { get; set; }
        public string FoodGroup { get; set; }
    }

    // This recipe class structures our JSON file 
    public class Recipe
    {
        public string Name { get; set; }
        public int Calorie { get; set; }
        public string FoodGroup { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
    }

    // Define the Ingredient class to hold ingredient information
    public class Ingredient
    {
        public string Amount { get; set; }
        public string Unit { get; set; }
        public string IngName { get; set; }

        public string Calorie { get; set; }
        public string FoodGroup { get; set; }
    }
}
