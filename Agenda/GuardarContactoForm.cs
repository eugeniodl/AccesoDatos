using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agenda
{
    public partial class GuardarContactoForm : Form
    {
        private int? _id;
        private readonly ContactoRepository _contactoRepository;

        public GuardarContactoForm(int? id = null)
        {
            InitializeComponent();
            _id = id;
            string connectionString =
                ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
            _contactoRepository = new ContactoRepository(connectionString);
            if (_id != null)
            {
                CargarData();
            }
        }

        private void CargarData()
        {
            try
            {
                Contacto contacto = _contactoRepository.GetValue((int)_id);
                txtNombre.Text = contacto.Nombre;
                txtApellido.Text = contacto.Apellido;
                dtpFechaNacimiento.Value = contacto.FechaNacimiento;
                txtTelefono.Text = contacto.Telefono.ToString();
                txtEmail.Text = contacto.Email;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar los datos del contacto" + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_id == null)
                {
                    _contactoRepository.Insert(new Contacto
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        FechaNacimiento = dtpFechaNacimiento.Value,
                        Telefono = Convert.ToInt32(txtTelefono.Text),
                        Email = txtEmail.Text
                    });
                }
                else
                {
                    _contactoRepository.Update(new Contacto
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        FechaNacimiento = dtpFechaNacimiento.Value,
                        Telefono = Convert.ToInt32(txtTelefono.Text),
                        Email = txtEmail.Text,
                        Id = (int)_id
                    });
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el contacto: " + ex.Message);
            }
        }
    }
}
