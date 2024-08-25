using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
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

using System.IO;
using Microsoft.Win32;
using Path = System.IO.Path;
using System.Data;

namespace Inventory.View
{
    /// <summary>
    /// Interaction logic for Product_Details.xaml
    /// </summary>
    public partial class Product_Details : Page
    {
        private string _productID;
        private string _action;
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        string query = "";
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        string imageLocation = "D:\\Nazmul IDB\\ASPDotNet\\V3.3\\Inventory\\images\\";
        public Product_Details(string productID, string action)
        {
            InitializeComponent();
            this._productID = productID;
            this._action = action;
            itemDetailLoaded();
        }

        private void itemDetailLoaded()
        {
            switch (_action)
            {
                case "details":
                    LoadDetails();
                    break;
                case "delete":
                    LoadDelete();
                    break;
                case "create":
                    LoadCreate();
                    break;
                case "Edit":
                    LoadEdit();
                    break;
                default:
                    break;
            }
        }

        private void LoadEdit()
        {
            LoadPage();
            stp_PurchasePrice.Visibility = Visibility.Visible;
            stp_QuantityStock.Visibility = Visibility.Visible;
            populateSupplier();
            populateCatagory();
            action.Content = "Update";
            action.Click += update_Confirm;
        }

        private void update_Confirm(object sender, RoutedEventArgs e)
        {

            File.Copy(lblImagePath.Text, Path.Combine(imageLocation, Path.GetFileName(lblImagePath.Text)), true);

            if (txb_itemName.Text != null && txb_description.Text != null && cmb_supplier.Text != null && cmb_catagory.Text != null && txb_unitPrice.Text != null && txb_quantityAvailable.Text != null && txb_purchasePrice.Text != null && txb_quantityStock.Text != null)
            {
                using (con = new SqlConnection(cs))
                {
                    con.Open();

                    cmd = new SqlCommand("Update ims.Products SET ProductName=@productName, ProductCategoryID=@productCategoryID, Description=@description, Image=@image, Supplier_id=@supplierID, UnitPrice=@unitPrice, QuantityStock = @quantityStock, QuantityAvailable=@quantityAvailable, PurchasePrice=@purchasePrice " +
                        "Where item_code=@item_code)", con);
                    int? productCategoryID = GetProductCategoryID(cmb_catagory.Text);
                    int? supplierID = GetSupplierID(cmb_supplier.Text);
                    cmd.Parameters.AddWithValue("@item_code", _productID);
                    cmd.Parameters.AddWithValue("@productName", txb_itemName.Text);
                    cmd.Parameters.AddWithValue("@productCategoryID", productCategoryID);
                    cmd.Parameters.AddWithValue("@description", txb_description.Text);
                    cmd.Parameters.AddWithValue("@image", Path.GetFileName(lblImagePath.Text));
                    cmd.Parameters.AddWithValue("@supplierID", supplierID);
                    cmd.Parameters.AddWithValue("@unitPrice", txb_unitPrice.Text);
                    cmd.Parameters.AddWithValue("@quantityStock", txb_quantityStock.Text);
                    cmd.Parameters.AddWithValue("@quantityAvailable", txb_unitPrice.Text);
                    cmd.Parameters.AddWithValue("@purchasePrice", txb_purchasePrice.Text);

                    int rowCount = cmd.ExecuteNonQuery();
                    if (rowCount > 0)
                    {
                        MessageBox.Show("Product updated successfully......");
                        Return();
                    }
                    else
                    {
                        MessageBox.Show("No rows were affected. Product may not exist or no changes were made.");
                    }
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please Enter all information.....");
            }
        }


        private void LoadDelete()
        {
            stp_PurchasePrice.Visibility = Visibility.Visible;
            stp_QuantityStock.Visibility = Visibility.Visible;
            btnImgLoad.Visibility = Visibility.Hidden;
            populateSupplier();
            populateCatagory();
            LoadPage();
            txb_itemName.IsEnabled = false;
            txb_description.IsEnabled = false;
            cmb_supplier.IsEnabled= false;
            cmb_catagory.IsEnabled= false;
            txb_purchasePrice.IsEnabled = false;
            txb_unitPrice.IsEnabled=false;
            txb_quantityAvailable.IsEnabled=false;
            txb_quantityStock.IsEnabled=false;
            action.Content = "Delete";
            action.Click += delete_Confirm;
        }

        private void delete_Confirm(object sender, RoutedEventArgs e)
        {
            con = new SqlConnection(cs);
            con.Open();
            cmd = new SqlCommand("delete from ims.Products "
                                 + "WHERE item_code = @item_code", con);
            cmd.Parameters.AddWithValue("@SupplierID", _productID);
            int rowCount = cmd.ExecuteNonQuery();
            if (rowCount > 0)
            {
                MessageBox.Show("Record Deleted successfully......");

                Return();
            }
            con.Close();
        }



        private void LoadDetails()
        {
            stp_PurchasePrice.Visibility = Visibility.Visible;
            stp_QuantityStock.Visibility = Visibility.Visible;
            btnImgLoad.Visibility = Visibility.Hidden;

            populateSupplier();
            populateCatagory();
            LoadPage();
            action.Visibility = Visibility.Hidden;
            txb_itemName.IsEnabled = false;
            txb_description.IsEnabled = false;
            cmb_supplier.IsEnabled = false;
            cmb_catagory.IsEnabled = false;
            txb_purchasePrice.IsEnabled = false;
            txb_unitPrice.IsEnabled = false;
            txb_quantityAvailable.IsEnabled = false;
            txb_quantityStock.IsEnabled = false;
        }

        private void LoadPage()
        {
            con = new SqlConnection(cs);
            con.Open();

            cmd = new SqlCommand("SELECT ProductName,CategoryName,p.Description,Image,ContactFirstName+' '+ContactLastName Name," +
                "UnitPrice,QuantityStock,QuantityAvailable,PurchasePrice FROM ims.Products p "+
                "JOIN ims.Contacts c ON c.ContactID = p.Supplier_id "+
                "JOIN ims.ProductCategories pc ON pc.ProductCategoryID = p.ProductCategoryID " + 
                "WHERE item_code = @item_code", con);

            cmd.Parameters.AddWithValue("@item_code", _productID);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                txb_itemName.Text = reader["ProductName"].ToString();
                txb_description.Text = reader["Description"].ToString();
                cmb_supplier.SelectedItem = reader["Name"].ToString();
                cmb_catagory.SelectedItem = reader["CategoryName"].ToString();
                txb_purchasePrice.Text = reader["PurchasePrice"].ToString();
                txb_unitPrice.Text = reader["UnitPrice"].ToString();
                txb_quantityAvailable.Text = reader["QuantityAvailable"].ToString();
                txb_quantityStock.Text = reader["QuantityStock"].ToString();


                if (reader["Image"].ToString() == null)
                {     
                     photo.Visibility = Visibility.Hidden;

                }
                else
                {
                    string imagePath = imageLocation+ reader["Image"].ToString();
                    photo.Source = new BitmapImage(new Uri(imagePath));
                }
                
            }
            else
            {
                MessageBox.Show("Contact not found.");
            }
            con.Close();
        }

        private void populateSupplier()
        {
            con = new SqlConnection(cs);
            con.Open();
            query = "SELECT ContactFirstName+' '+ContactLastName Supplier FROM ims.Contacts where ContactTypeID=1";
            List<string> supplier = new List<string>();
            cmd = new SqlCommand(query, con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string suppliers = reader["Supplier"].ToString();
                supplier.Add(suppliers);
            }
            cmb_supplier.ItemsSource = supplier;
            con.Close();
        }

        private void populateCatagory()
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
            cmb_catagory.ItemsSource = catagories;
            con.Close();
        }

        int? GetProductCategoryID(string pcName)
        {
            int PcID; // Default value if not found
            string sql = "SELECT ProductCategoryID FROM ims.ProductCategories WHERE CategoryName = @PCName";
            con = new SqlConnection(cs);

            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@PCName", pcName);
            con.Open();

            // Execute the command
            object result = cmd.ExecuteScalar();

            // Check if the result is not null
            if (result != null && result != DBNull.Value)
            {
                // Convert the result to integer
                PcID = Convert.ToInt32(result);
                return PcID;
            }
            return null;
        }

        int? GetSupplierID(string ContactName)
        {
            int ContactID; // Default value if not found
            string sql = "SELECT ContactID FROM ims.Contacts WHERE ContactFirstName+' '+ContactLastName = @ContactName";

            con = new SqlConnection(cs);
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@ContactName", ContactName);
            con.Open();

            object result = command.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                ContactID = Convert.ToInt32(result);
                return ContactID;
            }
            return null;
        }

        private void LoadCreate()
        {
            stp_PurchasePrice.Visibility = Visibility.Visible;
       
            populateSupplier();
            populateCatagory();
            action.Content = "Save";
            action.Click += Save_Click;
 
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(lblImagePath.Text, Path.Combine(imageLocation, Path.GetFileName(lblImagePath.Text)), true);
            if (txb_itemName.Text != null && txb_description.Text != null && cmb_supplier.Text != null && cmb_catagory.Text != null && txb_unitPrice.Text != null && txb_quantityAvailable.Text != null && txb_purchasePrice.Text != null && txb_quantityStock.Text != null)
            {
                con = new SqlConnection(cs);
                con.Open();
                cmd = new SqlCommand("INSERT INTO ims.Products (ProductName,ProductCategoryID,Description,Image,Supplier_id,UnitPrice,QuantityStock,QuantityAvailable,PurchasePrice)" +
                                                       "VALUES (@productName,@productCategoryID,@description,@image,@supplierID,@unitPrice,@quantityStock,@quantityAvailable,@purchasePrice)", con);
                int? productCategoryID = GetProductCategoryID(cmb_catagory.Text);
                int? supplierID = GetSupplierID(cmb_supplier.Text);
                cmd.Parameters.AddWithValue("@productName", txb_itemName.Text);
                cmd.Parameters.AddWithValue("@productCategoryID", productCategoryID);
                cmd.Parameters.AddWithValue("@description", txb_description.Text);
                cmd.Parameters.AddWithValue("@image", Path.GetFileName(lblImagePath.Text));
                cmd.Parameters.AddWithValue("@supplierID", supplierID);
                cmd.Parameters.AddWithValue("@unitPrice", txb_unitPrice.Text);
                cmd.Parameters.AddWithValue("@quantityStock", txb_quantityStock.Text);
                cmd.Parameters.AddWithValue("@quantityAvailable", txb_unitPrice.Text);
                cmd.Parameters.AddWithValue("@purchasePrice", txb_purchasePrice.Text);

                int rowCount = cmd.ExecuteNonQuery();
                if (rowCount>1)
                {
                    MessageBox.Show("Product inserted successfully......");
                    Return();
                }
                con.Close();
                
            }
            else
            {
                MessageBox.Show("Please Enter all information.....");
            }
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            Return();
        }
        private void Return()
        {
            ManageItems manageItems = new ManageItems();
            this.NavigationService.Navigate(manageItems);
        }

        private void btnImgLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*.jpg|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() ==true) 
            {
                lblImagePath.Text = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();

                photo.Source = bitmap;
            }
        }
    }
}
