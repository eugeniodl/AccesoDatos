using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winSQLClient
{
    public partial class frmProducts : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;

        public frmProducts()
        {
            InitializeComponent();
            GetProducts();
        }

        private void GetProducts()
        {
            string queryString = "SELECT ProductID, UnitPrice, ProductName FROM " +
    "Products WHERE UnitPrice > @pricePoint ORDER BY UnitPrice DESC;";

            int paramValue = 30;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@pricePoint", paramValue);
                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvPRoducts.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
