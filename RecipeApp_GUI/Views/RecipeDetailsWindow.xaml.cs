using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RecipeApp_GUI.Views;
using RecipeApp_GUI.Utilities;
using RecipeApp_GUI.ViewModel;
namespace RecipeApp_GUI.Views;

/// <summary>
/// Interaction logic for RecipeDetailsWindow.xaml
/// </summary>
public partial class RecipeDetailsWindow : Window
{
    public RecipeDetailsWindow(Recipe recipe)
    {
        InitializeComponent();
        DataContext = recipe;
        
     
    }
   
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
