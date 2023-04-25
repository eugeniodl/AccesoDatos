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
        //Conexión global a SQL Server
        string connectionString = ConfigurationManager.ConnectionStrings["winSQLClient.Properties.Settings.Con"].ConnectionString;

        public frmProducts()
        {
            InitializeComponent();
            GetProducts();
        }

        private void GetProducts()
        {
            string queryString = "SELECT ProductID, UnitPrice, ProductName " +
    "FROM Products ORDER BY UnitPrice DESC;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgvProducts.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                txtProductID.Text = row.Cells["ProductID"].Value.ToString();
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtUnitPrice.Text = row.Cells["UnitPrice"].Value.ToString();
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            InsertProducts();
        }

        private void InsertProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string queryString = "INSERT INTO Products(ProductName,UnitPrice) VALUES('" + txtProductName.Text
                        + "', " + txtUnitPrice.Text + ")";
                    SqlCommand cmd = new SqlCommand(queryString, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto guardado correctamente");
                    GetProducts();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string queryString = "UPDATE Products SET ProductName = '" + txtProductName.Text
                        + "', UnitPrice = " + txtUnitPrice.Text + " WHERE ProductID = " + txtProductID.Text;
                    SqlCommand cmd = new SqlCommand(queryString, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto actualizado correctamente");
                    GetProducts();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DeleteProducts();
        }

        private void DeleteProducts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string queryString = "DELETE FROM Products WHERE ProductId = " + txtProductID.Text;
                    SqlCommand cmd = new SqlCommand(queryString, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto eliminado correctamente");
                    GetProducts();
                }
                catch (InvalidCastException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
