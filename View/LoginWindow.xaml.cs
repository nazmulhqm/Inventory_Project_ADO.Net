using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Page
    { 
        string cs = ConfigurationManager.ConnectionStrings["InventoryDBCon"].ConnectionString;

        public LoginWindow()
        {
            InitializeComponent();
      
        }
     
        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(cs);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ims.Authenticate WHERE UserName= '" + txbUserName.Text + "' AND Password='" + txbPassword.Password + "' AND IsActive=1        ", con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                HomePage homePage = new HomePage();
                NavigationService.Navigate(homePage);
            }
            else
            {
                MessageBox.Show("Please Provide Valid Information........");
            }

        }
    }
}
