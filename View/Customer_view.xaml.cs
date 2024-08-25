using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace Inventory.View
{
    /// <summary>
    /// Interaction logic for Supplier_view.xaml
    /// </summary>
    public partial class Customer_view : Page
    {
        private string customerID;
        private string actiontype;
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        string query="";
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();

        public Customer_view(string customerID, string action)
        {
            InitializeComponent();
            this.customerID = customerID;
            this.actiontype = action;
            customerDetailLoaded();
        }

        private void customerDetailLoaded()
        {
            switch (actiontype)
            {
                case "details":
                    LoadDetails();
                    break;
                case "delete":
                    LoadDelete();
                    break;
                case "update":
                    LoadUpdate();
                    break;
                case "create":
                    LoadCreate();
                    break;
                default:
                    break;
            }
        }

        private void LoadUpdate()
        {
            action.Content = "Update";
            action.Click += UpdateConfirmClick;

            LoadPage();
            populateCity();
        }

        private void LoadDetails()
        {
            LoadPage();
            action.Visibility = Visibility.Hidden;

            Txb_FirstName.IsEnabled = false;
            txb_Address.IsEnabled = false;
            cmb_city.IsEnabled = false;
            txb_Phone.IsEnabled = false;
            txb_Email.IsEnabled = false;
            rb_Male.IsEnabled = false;
            rb_Male.IsEnabled = false;
        }
        private void LoadDelete()
        {
            LoadPage();


            action.Content = "DELETE";
            action.Click += DeleteConfirmClick;

            Txb_FirstName.IsEnabled = false;
            txb_Address.IsEnabled = false;
            cmb_city.IsEnabled = false;
            txb_Phone.IsEnabled = false;
            txb_Email.IsEnabled = false;
            rb_Male.IsEnabled = false;
            rb_Female.IsEnabled = false;
        }

        private void priviousPage(object sender, RoutedEventArgs e)
        {
            Return();
        }

        private void Return() 
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToCustomerPage();
            }
        }

        private void UpdateConfirmClick(object sender, RoutedEventArgs e)
        {

            string firstName = Txb_FirstName.Text;
            string address = txb_Address.Text;
            string city = cmb_city.Text;
            string phone = txb_Phone.Text;
            string email = txb_Email.Text;
            string gender;

            if (rb_Male.IsChecked == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            int cityID = GetCityID(city);

            if (Txb_FirstName != null && txb_Address != null && cmb_city != null && txb_Email != null)
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("UPDATE ims.Customers " +
                                    "SET CustomerName=@firstName," +
                                    "Gender=@gender,Addresses=@addresses,City=@city,Phone=@phone,Email=@email " +
                                    "WHERE CONCAT(CustomerName, ', ', Addresses, ', ', Phone) = @customerID", con);
                cmd.Parameters.AddWithValue("@customerID", customerID);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@addresses", address);
                cmd.Parameters.AddWithValue("@city", cityID);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);

                int rowCount = cmd.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    MessageBox.Show("Record updated successfully......");
                    Return();
                }
                con.Close();
            }
            else
            {
                MessageBox.Show("Please Enter the Name and DOB.....");
            }
        }

        private void populateCity()
        {
            con = new SqlConnection(cs);
            con.Open();
            query = "SELECT CityID, City+', '+Country City FROM ims.Cities";
            List<string> city = new List<string>();
            SqlCommand command = new SqlCommand(query, con);
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string cities = reader["City"].ToString();
                city.Add(cities);
            }
            cmb_city.ItemsSource = city;
            con.Close();
        }

        private void LoadCreate()
        {
            action.Content = "Create";
            action.Click += CreateConfirmClick;
            populateCity();
        }

        int GetCityID(string cityNme)
        {
            int CityID = 0; // Default value if not found
            string sql = "SELECT CityID FROM ims.Cities WHERE City+', '+Country = @CityName";
            con = new SqlConnection(cs);
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@CityName", cityNme);
            con.Open();

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                CityID = Convert.ToInt32(result);
            }
            return CityID;
        }

        private void CreateConfirmClick(object sender, RoutedEventArgs e)
        {
            string firstName = Txb_FirstName.Text;
            string address = txb_Address.Text;
            string city = cmb_city.Text;
            string phone = txb_Phone.Text;
            string email = txb_Email.Text;
            string gender;

            if (rb_Male.IsChecked==true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            int cityID = GetCityID(city);

            if (Txb_FirstName.Text != null && txb_Address.Text != null && cmb_city.Text != null && txb_Email.Text != null)
            {
                con = new SqlConnection(cs);
                con.Open();

                cmd = new SqlCommand("INSERT INTO ims.Customers(CustomerName,Gender,Addresses,City,Phone,Email)" +
                                    "values(@firstName,@gender,@addresses,@city,@phone,@email)", con);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@addresses", address);
                cmd.Parameters.AddWithValue("@city", cityID);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);

                int rowCount = cmd.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    MessageBox.Show("Record inserted successfully......");
                    Return();
                }
                con.Close();
            }
            else
            {
                MessageBox.Show("Please Enter the Name and DOB.....");
            }

        }

        private void LoadPage()
        {
            back.Content = "Return";
            back.Click += priviousPage;
            populateCity();
            query = "SELECT CustomerName,Gender, Addresses, a.City + ', ' + a.Country as City, Phone, Email FROM ims.Customers c " +
                           " Join ims.Cities a ON c.City = a.CityID" +
                           " WHERE CONCAT(CustomerName, ', ', Addresses, ', ', Phone) = @SupplierID";

            con = new SqlConnection(cs);

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@SupplierID", customerID);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    Txb_FirstName.Text = reader["CustomerName"].ToString();
                    txb_Address.Text = reader["Addresses"].ToString();
                    cmb_city.SelectedItem = reader["City"].ToString();
                    txb_Phone.Text = reader["Phone"].ToString();
                    txb_Email.Text = reader["Email"].ToString();
                    contactTypeInfo.Content = "Customer";
                    contactTypeInfo.Content += "'s Information";

                    string gender = reader["Gender"].ToString();
                    if (gender == "Male")
                    {
                        rb_Male.IsChecked = true;
                        rb_Female.Visibility = Visibility.Hidden;
                    }
                    else if (gender == "Female")
                    {
                        rb_Female.IsChecked = true;
                        rb_Male.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    MessageBox.Show("Customer not found.");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }



        private void DeleteConfirmClick(object sender, RoutedEventArgs e)
        {
            con = new SqlConnection(cs);
            con.Open();
            cmd = new SqlCommand("delete from ims.Customers "
                                 +"WHERE CONCAT(CustomerName, ', ', Addresses, ', ', Phone) = @SupplierID", con);
            cmd.Parameters.AddWithValue("@SupplierID", customerID);
            int rowCount = cmd.ExecuteNonQuery();
            if (rowCount > 0)
            {
                MessageBox.Show("Record Deleted successfully......");

                Return();
            }
            con.Close();    
        }
    }
}