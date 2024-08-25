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

namespace Inventory.View
{
    /// <summary>
    /// Interaction logic for ManageItems.xaml
    /// </summary>
    public partial class ManageItems : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        DataTable dt;
        SqlDataAdapter adapter;




        public ManageItems()
        {
            InitializeComponent();
        }

        private void addCatagory_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToCatagoryPage();
            }
        }

        private void ProdcutPage_Loaded(object sender, RoutedEventArgs e)
        {
            con = new SqlConnection(cs);

            string query = "SELECT item_code Item_Code,ProductName,pc.CategoryName,c.ContactFirstName+' '+c.ContactLastName Supplier,UnitPrice,QuantityAvailable FROM ims.Products " +
                           "Join ims.ProductCategories pc on ims.Products.ProductCategoryID=pc.ProductCategoryID " +
                           "Join ims.Contacts c on ims.Products.Supplier_id=c.ContactID";
            cmd = new SqlCommand(query, con);

            con.Open();
            adapter = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            ItemsGrid.ItemsSource = dt.DefaultView;
            con.Close();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Product_Details product_Details = new Product_Details("","create");
            this.NavigationService.Navigate(product_Details);
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected supplier's details

            DataRowView selectedRow = (DataRowView)ItemsGrid.SelectedItem;
            if (selectedRow != null)
            {
                string productID = $"{selectedRow["Item_Code"]}";
                Product_Details product_Details = new Product_Details(productID, "details");
                this.NavigationService.Navigate(product_Details);
            }
            else
            {
                MessageBox.Show("Please select a product to view.");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ItemsGrid.SelectedItem;
            if (selectedRow != null)
            {
                string productID = $"{selectedRow["Item_Code"]}";
                Product_Details product_Details = new Product_Details(productID, "Edit");
                this.NavigationService.Navigate(product_Details);
            }
            else
            {
                MessageBox.Show("Please select a product to view.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ItemsGrid.SelectedItem;
            if (selectedRow != null)
            {
                string productID = $"{selectedRow["Item_Code"]}";

                Product_Details product_Details = new Product_Details(productID, "delete");
                this.NavigationService.Navigate(product_Details);
            }
            else
            {
                MessageBox.Show("Please select a product to view.");
            }
        }
    }
}
