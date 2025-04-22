using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
            if(id != null )
            {

            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    Contacto contacto = new Contacto
                    {
                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        FechaNacimiento = dtpFechaNacimiento.Value,
                        Telefono = Convert.ToInt32(txtTelefono.Text),
                        Email = txtEmail.Text
                    };

                    if (_id == null) // Nuevo contacto
                    {
                        
                    }
                    else // Editar contacto
                    {
                        contacto.Id = (int)_id;
                        
                    }

                    DialogResult = DialogResult.OK; // Indica que se guardó correctamente
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el contacto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar formato del teléfono (solo números)
            if (!int.TryParse(txtTelefono.Text, out _))
            {
                MessageBox.Show("El teléfono debe contener solo números.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validar formato de email
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("El correo electrónico no es válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
