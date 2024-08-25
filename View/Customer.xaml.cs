using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Configuration;

namespace Inventory.View
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Customer : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();

        public Customer()
        {
            InitializeComponent();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("Name LIKE '%{0}%'", txbSearch.Text);
            CustomersGrid.ItemsSource = dv;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
            if (selectedRow != null)
            {
                string customerID = $"{selectedRow["Name"]}, {selectedRow["Addresses"]}, {selectedRow["Phone"]}";
                Customer_view customer_View = new Customer_view(customerID, "details");
                this.NavigationService.Navigate(customer_View);
            }
            else
            {
                MessageBox.Show("Please select a supplier to view.");
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
            if (selectedRow != null)
            {
                string customerID = $"{selectedRow["Name"]}, {selectedRow["Addresses"]}, {selectedRow["Phone"]}";
                Customer_view supplierViewPage = new Customer_view(customerID, "delete");
                this.NavigationService.Navigate(supplierViewPage);
            }
            else
            {
                MessageBox.Show("Please select a supplier to view.");
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)CustomersGrid.SelectedItem;
            if (selectedRow != null)
            {
                string customerID = $"{selectedRow["Name"]}, {selectedRow["Addresses"]}, {selectedRow["Phone"]}";
                Customer_view supplierViewPage = new Customer_view(customerID, "update");
                this.NavigationService.Navigate(supplierViewPage);
            }
            else
            {
                MessageBox.Show("Please select a supplier to view.");
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string supplierID = "";
            Customer_view supplierViewPage = new Customer_view(supplierID, "create");
            this.NavigationService.Navigate(supplierViewPage);
        }

        private void Customer_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                con = new SqlConnection(cs);
                string query = "SELECT CustomerName Name, Gender, Addresses, ci.City, Country, Phone FROM ims.Customers c JOIN " +
                    "ims.Cities ci ON c.City=ci.CityID ";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                CustomersGrid.ItemsSource = dt.DefaultView;
                list.Content = "Customer's List";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer contacts: " + ex.Message);
            }
        }
    }
}