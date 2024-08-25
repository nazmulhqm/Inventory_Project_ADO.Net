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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.View
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }
        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToHomePage();
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ManageItems_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToManageItems();
            }
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToLoginPage();
            }
        }
      
        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            if(Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToCustomerPage();
            }
            
        }
        private void Supplier_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                NavigateToContactsPage("Supplier");
            }
        }
        private void Manufacturer_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                NavigateToContactsPage("Manufacturer");
            }
        }
        public void NavigateToContactsPage(string contactType)
        {
            Contact supplierPage = new Contact(contactType);
            ((MainWindow)Application.Current.MainWindow).mainFrame.NavigationService.Navigate(supplierPage);
        }

        private void OrderPlace_Click(object sender, RoutedEventArgs e)
        {
            if(Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToSales();
            }
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToReport();
            }
        }

        private void SalesHistory_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToSalesHistory();
            }
        }
    }
}
