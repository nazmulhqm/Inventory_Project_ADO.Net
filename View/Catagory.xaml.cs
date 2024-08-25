using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Collections;

namespace Inventory.View
{
    /// <summary>
    /// Interaction logic for Catagory.xaml
    /// </summary>
    public partial class Catagory : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();


        public Catagory()
        {
            InitializeComponent();
        }

        private void CreateCatagory_Loaded(object sender, RoutedEventArgs e)
        {
            action.Content = "Save";
            back.Content = "Cancle";
            back.Click += cancle_Click;
            action.Click += save_Click;
            populateParentCatagory();
        }
        private void populateParentCatagory()
        {
            con = new SqlConnection(cs);
            con.Open();
            string query = "SELECT CategoryName FROM ims.ProductCategories";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            List<string> catagories = new List<string>();
            while (reader.Read())
            {
                string contactType = reader["CategoryName"].ToString();
                catagories.Add(contactType);
            }
            cmb_ParentCategoryType.ItemsSource = catagories;
            con.Close();
        }
        private int getParentCatagoryID(string catagoryName)
        {
            string query = "SELECT ProductCategoryID FROM ims.ProductCategories WHERE CategoryName = @CatagoryName";

            con = new SqlConnection(cs);


            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@CatagoryName", catagoryName);
         
                con.Open();
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            return 0;

          
        }
      
        private void save_Click(object sender, RoutedEventArgs e)
        {

            // Validate if required fields are not empty
            if (string.IsNullOrEmpty(Txb_CategoryName.Text))
            {
                MessageBox.Show("Please enter category name.");
                return;
            }

            // Insert the category information into the database
            string query = "INSERT INTO ims.ProductCategories (CategoryName, Description, ParentCategoriesID) " +
                           "VALUES (@CategoryName, @Description, @ParentCategory)";
            SqlConnection connection = new SqlConnection(cs);

            cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@CategoryName", Txb_CategoryName.Text);
            cmd.Parameters.AddWithValue("@Description", txb_Description.Text);
            if (cmb_ParentCategoryType.SelectedItem == null)
            {
                cmd.Parameters.AddWithValue("@ParentCategory", DBNull.Value);
            }
            else
            {
                int? parentCatagory=getParentCatagoryID(cmb_ParentCategoryType.Text);
                cmd.Parameters.AddWithValue("@ParentCategory", parentCatagory);
            }
            try
            {
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Category saved successfully.");
                    // Optionally clear the text boxes or reset the form
                    Txb_CategoryName.Text = "";
                    txb_Description.Text = "";
                    cmb_ParentCategoryType.SelectedItem = null;
                    populateParentCatagory();
                }
                else
                {
                    MessageBox.Show("Failed to save category.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cancle_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToManageItems();
            }
        }
    }
}
