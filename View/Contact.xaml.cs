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
    public partial class Contact : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        private readonly string contactType;

        public Contact(string contactType)
        {
            InitializeComponent();
            this.contactType = contactType;
            LoadContacts();
        }

        private void LoadContacts()
        {
            switch (contactType)
            {
                case "Manufacturer":
                    LoadManufacturerContacts();
                    break;
                case "Supplier":
                    LoadSupplierContacts();
                    break;
                default:
                    MessageBox.Show("Invalid contact type.");
                    break;
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.Format("Name LIKE '%{0}%'", txbSearch.Text);
            ContactsGrid.ItemsSource = dv;
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ContactsGrid.SelectedItem;
            if (selectedRow != null)
            {
                string contactID = $"{selectedRow["Name"]}, {selectedRow["Addresses"]}, {selectedRow["Phone"]}";
                Contact_view contact_View = new Contact_view(contactID,"details");
                this.NavigationService.Navigate(contact_View);
            }
            else
            {
                MessageBox.Show("Please select a contact to view.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ContactsGrid.SelectedItem;
            if (selectedRow != null)
            {
                string contactID = $"{selectedRow["Name"]}, {selectedRow["Addresses"]}, {selectedRow["Phone"]}";
                Contact_view contact_View = new Contact_view(contactID, "delete");
                this.NavigationService.Navigate(contact_View);
            }
            else
            {
                MessageBox.Show("Please select a contact to delete.");
            }
        }
        
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ContactsGrid.SelectedItem;
            if (selectedRow != null)
            {
                string contactID = $"{selectedRow["Name"]}, {selectedRow["Addresses"]}, {selectedRow["Phone"]}";
                Contact_view contact_View = new Contact_view(contactID, "update");
                this.NavigationService.Navigate(contact_View);
            }
            else
            {
                MessageBox.Show("Please select a supplier to view.");
            }
        }

        private void LoadManufacturerContacts()
        {
            try
            {
                con = new SqlConnection(cs);
                string query = "SELECT Name, Gender, Addresses, City, Country, Phone FROM ims.vw_Manufacturers";
                cmd = new SqlCommand(query, con);
                con.Open();
                adapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                ContactsGrid.ItemsSource = dt.DefaultView;
                list.Content = "Manufacturer's List";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading manufacturer contacts: " + ex.Message);
            }
        }

        private void LoadSupplierContacts()
        {
            try
            {
                con = new SqlConnection(cs);
                string query = "SELECT Name, Gender, Addresses, City, Country, Phone FROM ims.vw_Suppliers";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);
                ContactsGrid.ItemsSource = dt.DefaultView;
                list.Content = "Supplier's List";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading supplier contacts: " + ex.Message);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string supplierID = contactType;
            Contact_view supplierViewPage = new Contact_view(supplierID, "create");
            this.NavigationService.Navigate(supplierViewPage);
        }
    }
}