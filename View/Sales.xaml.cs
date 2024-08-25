using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Sales : Page
    {
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;
        string query = "";
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
        private DataTable selectedProductsTable = new DataTable();


        public Sales()
        {
            InitializeComponent();
            populateProduct();
            populateCity();
            populateDiscount();
            InitializeSelectedProductsTable();
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

        private void populateDiscount()
        {
            con = new SqlConnection(cs);
            con.Open();
            query = "SELECT DiscountName FROM ims.Discounts";
            List<string> discounts = new List<string>();
            cmd = new SqlCommand(query, con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string discount = reader["DiscountName"].ToString();
                discounts.Add(discount);
            }
            cmb_discount.ItemsSource = discounts;
            con.Close();
        }

        int GetDiscountID(string discountName)
        {
            int discountID = 0; // Default value if not found
            string sql = "SELECT DiscountID FROM ims.Discounts WHERE DiscountName = @discountName";
            con = new SqlConnection(cs);
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@discountName", discountName);
            con.Open();

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                discountID = Convert.ToInt32(result);
            }
            return discountID;
        }

        private void populateProduct()
        {
            con = new SqlConnection(cs);
            con.Open();
            query = "SELECT PRODUCTNAME FROM ims.PRODUCTS WHERE QuantityAvailable>0";
            List<string> products = new List<string>();
            cmd = new SqlCommand(query, con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string product = reader["PRODUCTNAME"].ToString();
                products.Add(product);
            }
            cmbAvailableProducts.ItemsSource = products;
            con.Close();
        }

        int GetProductID(string ProductName)
        {

            int ProductId = 0;
            query = "SELECT ProductID FROM ims.Products WHERE ProductName = @ProductName";
            con = new SqlConnection(cs);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ProductName", ProductName);
            con.Open();

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                ProductId = Convert.ToInt32(result);
            }
            con.Close();
            return ProductId;
        }

        private bool checkQuantity(int productID, int requestedQuantity)
        {
            con.Open();
            string query = "SELECT QuantityAvailable FROM ims.Products WHERE ProductID = @ProductID";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@ProductID", productID);
            int availableQuantity = (int)command.ExecuteScalar();
            con.Close();
            if (availableQuantity >= requestedQuantity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private decimal GetUnitPrice(int productID)
        {
            decimal unitPrice = 0;
            con = new SqlConnection(cs);
            string query = "SELECT UnitPrice FROM ims.Products WHERE ProductID = @ProductID";
            cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@ProductID", productID);
            con.Open();
            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                unitPrice = Convert.ToDecimal(result);
            }

            return unitPrice;
        }

        private void InitializeSelectedProductsTable()
        {
            selectedProductsTable.Columns.Add("ProductID", typeof(int));
            selectedProductsTable.Columns.Add("ProductName", typeof(string));
            selectedProductsTable.Columns.Add("Quantity", typeof(int));
            selectedProductsTable.Columns.Add("UnitPrice", typeof(decimal));
            selectedProductsTable.Columns.Add("Discount", typeof(string));
            selectedProductsTable.Columns.Add("TotalAmount", typeof(decimal));
        }
        private int GetDiscountPercentage(int discountId)
        {
            int discountPercentage = 0; // Default value if not found
            string sql = "SELECT DiscountPercentage FROM ims.Discounts WHERE  DiscountID = @discountID";
            con = new SqlConnection(cs);
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@discountID", discountId);
            con.Open();

            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                discountPercentage = Convert.ToInt32(result);
            }
            return discountPercentage;
        }

        private decimal price(int productID, int quantity, decimal unitPrice, int discountId)
        {
            decimal discountPercentage = GetDiscountPercentage(discountId);
            decimal discountAmount = (quantity * unitPrice * discountPercentage) / 100;
            decimal totalPrice = quantity * unitPrice - discountAmount;

            return totalPrice;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            string selectedProduct;
            selectedProduct = cmbAvailableProducts.Text;
            string discount = cmb_discount.Text;
            int discountId = GetDiscountID(discount);
            int discountPercentage = GetDiscountPercentage(discountId);
            int ProductID = Convert.ToInt32(GetProductID(selectedProduct));
            decimal unitPrice = GetUnitPrice(ProductID);

            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to add.");
                return;
            }

            if (!int.TryParse(txtProductQuantity.Text, out int quantity) || quantity <= 0)
            {
                if (checkQuantity(ProductID, quantity) == false)
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }
            }

            decimal totalAmount = price(ProductID, quantity, unitPrice, discountPercentage);

            selectedProductsTable.Rows.Add(ProductID, selectedProduct, quantity, unitPrice, discount, totalAmount);

            dgSelectedProducts.ItemsSource = selectedProductsTable.DefaultView;

            cmbAvailableProducts.SelectedItem = null;
            txtProductQuantity.Clear();
        }

        private int InsertCustomers(string name, string gender, string addresses, int city, string phone, string email)
        {
            int customerID = 0;
            query = "INSERT INTO ims.Customers (CustomerName, Gender, Addresses, City, Phone, Email) VALUES (@CustomerName, @Gender, @Addresses, @City, @Phone, @Email); SELECT SCOPE_IDENTITY();";

            con = new SqlConnection(cs);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@CustomerName", name);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@Addresses", addresses);
            cmd.Parameters.AddWithValue("@City", city);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.Parameters.AddWithValue("@Email", email);
            try
            {
                con.Open();
                customerID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Customer: " + ex.Message);
            }
            return customerID;
        }

        private int InsertSales(int customerID, DateTime saleDate)
        {
            int orderID = 0;
            query = "INSERT INTO ims.Sales (CustomerID, SalesDate) VALUES (@CustomerID, @SaleDate); SELECT SCOPE_IDENTITY();";

            con = new SqlConnection(cs);
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SaleDate", saleDate);
            try
            {
                con.Open();
                orderID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Sale: " + ex.Message);
            }
            return orderID;
        }

        private void InsertOrderDetail(int orderID, int productID, string discount, int quantity, decimal unitPrice, decimal totalAmount)
        {
            int discountID = GetDiscountID(discount);

            string query = "INSERT INTO ims.SalesDetails (SaleID, ProductID, DiscountID, Quantity, UnitPrice, TotalAmount) " +
                           " VALUES (@OrderID, @ProductID, @DiscountID, @Quantity, @UnitPrice, @TotalAmount); " +
                           " UPDATE ims.Products SET QuantityAvailable = QuantityAvailable - @Quantity " +
                           " WHERE ProductID = @ProductID ";

            SqlConnection con = new SqlConnection(cs);
            SqlCommand command = new SqlCommand(query, con);

            command.Parameters.AddWithValue("@OrderID", orderID);
            command.Parameters.AddWithValue("@ProductID", productID);
            command.Parameters.AddWithValue("@DiscountID", discountID);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@UnitPrice", unitPrice);
            command.Parameters.AddWithValue("@TotalAmount", totalAmount);

            try
            {
                con.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting order detail: " + ex.Message);
            }


        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                DataRowView dataRowView = button.DataContext as DataRowView;
                if (dataRowView != null)
                {
                    selectedProductsTable.Rows.Remove(dataRowView.Row);
                }
            }
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            string name = cmb_ContactID.Text;
            string city = Convert.ToString(cmb_city.SelectedItem);
            string address = txb_address.Text;
            string phone = txb_Phone.Text;
            string email = txb_email.Text;
            string gender;
            if (rb_male.IsChecked == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }
            int cityID = GetCityID(city);


            int customerID = InsertCustomers(name, gender, address, cityID, phone, email);

            DateTime orderDate = dpOrderDate.DisplayDate;
            // Insert a new row into the ims.Orders table
            int orderID = InsertSales(customerID, orderDate);

            // Iterate through the rows in the data grid
            foreach (DataRowView rowView in dgSelectedProducts.ItemsSource)
            {
                DataRow row = rowView.Row;

                string discount = (string)row["Discount"];

                int productID = (int)row["ProductID"];
                int quantity = (int)row["Quantity"];
                decimal unitPrice = (decimal)row["UnitPrice"];
                decimal totalAmount = (decimal)row["TotalAmount"];

                // Insert a new row into the ims.OrderDetails table
                InsertOrderDetail(orderID, productID, discount, quantity, unitPrice, totalAmount);
            }
            MessageBox.Show("Order confirmed successfully.");
            MainWindow main = new MainWindow();
            main.NavigateToSalesHistory();
        }
    }
}
