using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using winNorthwindEF.Data;
using winNorthwindEF.Models;

namespace winNorthwindEF
{
    public partial class frmProducts : Form
    {
        private static int id = 0;
        Product? objProducto = null;

        public frmProducts()
        {
            InitializeComponent();
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void Refrescar()
        {
            using(var context = new NorthwindContext())
            {
                var lst = from p in context.Products
                          orderby p.ProductName ascending
                          select new { p.ProductId, p.ProductName, p.UnitPrice };
                dgvProducts.DataSource = lst.ToList();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            using(var context = new NorthwindContext())
            {
                objProducto = new Product();
                objProducto.ProductName = txtProductName.Text;
                objProducto.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                context.Products.Add(objProducto);
                context.SaveChanges();
                Limpiar();
                MessageBox.Show("Registro actualizado");
                Refrescar();
            }
        }

        private void Limpiar()
        {
            txtProductName.Text = String.Empty;
            txtUnitPrice.Text = String.Empty;
            id = 0;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if(row.Index == e.RowIndex)
                {
                    id = int.Parse(row.Cells[0].Value.ToString());
                    ObtenerDatos(id);
                }
            }
        }

        private void ObtenerDatos(int key)
        {
            using(var context = new NorthwindContext())
            {
                objProducto = context.Products.Find(key);
                txtProductName.Text = objProducto.ProductName;
                txtUnitPrice.Text = objProducto.UnitPrice.ToString();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if(id != 0)
            {
                using(var context = new NorthwindContext())
                {
                    objProducto.ProductName = txtProductName.Text;
                    objProducto.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                    context.Entry(objProducto).State = EntityState.Modified;
                    context.SaveChanges();
                    Limpiar();
                    MessageBox.Show("Registro actualizado");
                    Refrescar();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if(id != 0)
            {
                using(var context =new NorthwindContext())
                {
                    Product oProducto = context.Products.Find(id);
                    context.Products.Remove(oProducto);
                    context.SaveChanges();
                }
                Limpiar();
                MessageBox.Show("Registro eliminado");
                Refrescar();
            }
        }
    }
}
