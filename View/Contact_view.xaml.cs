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
    public partial class Contact_view : Page
    {
        private readonly string contactID;
        private readonly string actiontype;
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        string query="";
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();

        public Contact_view(string contactID, string action)
        {
            InitializeComponent();
            this.contactID = contactID;
            this.actiontype = action;
            
            contatDetailLoaded();
        }

        private void contatDetailLoaded()
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
                    MessageBox.Show("Error");
                    break;
            }
        }


        private void LoadPage()
        {
            back.Visibility = Visibility.Visible;
            back.Content = "Return";
            back.Click += priviousPage;
            populateCity();
            populateContactType();
            query = "SELECT ContactFirstName, ContactLastName, ContactTypeName,Gender, Addresses, a.City + ', ' + a.Country as City, Phone, Email FROM ims.Contacts c Join ims.ContactTypes ct" +
                           " ON c.ContactTypeID = ct.ContactTypeID Join ims.Cities a " +
                           "  ON c.City = a.CityID" +
                           " WHERE CONCAT(ContactFirstName, ' ', ContactLastName, ', ', Addresses, ', ', Phone) = @ContactID";

            con = new SqlConnection(cs);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ContactID", contactID);

            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    Txb_FirstName.Text = reader["ContactFirstName"].ToString();
                    txb_LastName.Text = reader["ContactLastName"].ToString();
                    cmb_ContactType.SelectedItem = reader["ContactTypeName"].ToString();
                    txb_Address.Text = reader["Addresses"].ToString();
                    cmb_city.SelectedItem = reader["City"].ToString();
                    txb_Phone.Text = reader["Phone"].ToString();
                    txb_Email.Text = reader["Email"].ToString();
                    contactTypeInfo.Content = reader["ContactTypeName"].ToString();
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
                    MessageBox.Show("Contact not found.");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadUpdate()
        {
            action.Content = "Update";
            action.Click += UpdateConfirmClick;

            LoadPage();
            populateCity();
            populateContactType();
        }

        private void LoadDetails()
        {
            LoadPage();
            action.Visibility = Visibility.Hidden;

            Txb_FirstName.IsEnabled = false;
            txb_LastName.IsEnabled = false;
            cmb_ContactType.IsEnabled = false;
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
            txb_LastName.IsEnabled = false;
            cmb_ContactType.IsEnabled = false;
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
                mainWindow.NavigateToContactsPage(cmb_ContactType.Text);
            }
        }

        private void UpdateConfirmClick(object sender, RoutedEventArgs e)
        {

            string firstName = Txb_FirstName.Text;
            string lastName = txb_LastName.Text;
            string contactType = cmb_ContactType.Text;
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

            int contactTypeID = GetContactTypeID(contactType, cs);
            int cityID = GetCityID(city, cs);

            if (Txb_FirstName != null && txb_LastName != null && cmb_ContactType != null && txb_Address != null && cmb_city != null && txb_Email != null)
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("UPDATE ims.Contacts " +
                                    "SET ContactFirstName=@firstName,ContactLastName=@lastName,ContactTypeID=@contactTypeID," +
                                    "Gender=@gender,Addresses=@addresses,City=@city,Phone=@phone,Email=@email " +
                                    "WHERE CONCAT(ContactFirstName, ' ', ContactLastName, ', ', Addresses, ', ', Phone) = @SupplierID", con);
                cmd.Parameters.AddWithValue("@SupplierID", contactID);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@contactTypeID", contactTypeID);
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

        private void populateContactType()
        {
            con = new SqlConnection(cs);
            con.Open();
            query = "SELECT ContactTypeID, ContactTypeName FROM ims.ContactTypes";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<string> contactTypes = new List<string>();
            while (reader.Read())
            {
                string contactType = reader["ContactTypeName"].ToString();
                contactTypes.Add(contactType);
            }
            cmb_ContactType.ItemsSource = contactTypes;
            con.Close();
        }

        private void LoadCreate()
        {
            action.Content = "Create";
            action.Click += CreateConfirmClick;
            populateCity();
            populateContactType();
        }

        int GetContactTypeID(string contactTypeName, string connectionString)
        {
            int contactTypeID = 0; 
            query = "SELECT ContactTypeID FROM ims.ContactTypes WHERE ContactTypeName = @ContactTypeName";

            con = new SqlConnection(cs);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ContactTypeName", contactTypeName);
            con.Open();

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                contactTypeID = Convert.ToInt32(result);
            }
            con.Close();

            return contactTypeID;
        }

        int GetCityID(string cityNme, string connectionString)
        {
            int CityID = 0;
            string sql = "SELECT CityID FROM ims.Cities WHERE City+', '+Country = @CityName";

            con = new SqlConnection(cs);
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@CityName", cityNme);

            con.Open();
            object result = command.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                CityID = Convert.ToInt32(result);
            }
            con.Close();
            return CityID;
        }

        private void CreateConfirmClick(object sender, RoutedEventArgs e)
        {
            string firstName = Txb_FirstName.Text;
            string lastName = txb_LastName.Text;
            string contactType = cmb_ContactType.Text;
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

            int contactTypeID = GetContactTypeID(contactType,cs);
            int cityID = GetCityID(city, cs);

            if (Txb_FirstName.Text != null && txb_LastName.Text != null && cmb_ContactType.Text != null && txb_Address.Text != null && cmb_city.Text != null && txb_Email.Text != null)
            {
                con = new SqlConnection(cs);
                con.Open();

                cmd = new SqlCommand("INSERT INTO ims.Contacts(ContactFirstName,ContactLastName,ContactTypeID,Gender,Addresses,City,Phone,Email)" +
                                    "values(@firstName,@lastName,@contactTypeID,@gender,@addresses,@city,@phone,@email)", con);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@contactTypeID", contactTypeID);
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

        private void DeleteConfirmClick(object sender, RoutedEventArgs e)
        {
            con = new SqlConnection(cs);
            con.Open();
            cmd = new SqlCommand("delete from ims.Contacts "
                                 +"WHERE CONCAT(ContactFirstName, ' ', ContactLastName, ', ', Addresses, ', ', Phone) = @SupplierID", con);
            cmd.Parameters.AddWithValue("@SupplierID", contactID);
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