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
    public partial class AgendaForm : Form
    {
        private readonly ContactoRepository _contactoRepository;

        public AgendaForm()
        {
            InitializeComponent();
            string connectionString =
                ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
            _contactoRepository = new ContactoRepository(connectionString);
            Actualizar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        private void Actualizar()
        {
            try
            {
                dgvContactos.DataSource = _contactoRepository.GetAll();
            }
            catch (Exception ex)
            {
                MostrarError("Error al obtener los datos", ex);
            }
        }

        private void MostrarError(string mensaje, Exception ex)
        {
            MessageBox.Show(mensaje + Environment.NewLine + ex.Message,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetIdSeleccionado();
            if (id != null)
            {
                try
                {
                    _contactoRepository.Delete((int)id);
                    Actualizar();
                }
                catch (Exception ex)
                {
                    MostrarError("Error al eliminar el contacto", ex);
                }
            }
        }

        private int? GetIdSeleccionado()
        {
            if (dgvContactos.SelectedRows.Count > 0 &&
                dgvContactos.SelectedRows[0].Cells[0].Value != null)
            {
                return Convert.ToInt32(dgvContactos.SelectedRows[0].Cells[0].Value);
            }
            return null;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MostrarFormularioNuevo();
        }

        private void MostrarFormularioNuevo()
        {
            GuardarContactoForm form = new GuardarContactoForm();
            form.ShowDialog();
            Actualizar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetIdSeleccionado();
            if (id != null)
            {
                MostrarFormularioEdicion((int)id);
            }            
        }

        private void MostrarFormularioEdicion(int id)
        {
            GuardarContactoForm form = new GuardarContactoForm(id);
            form.ShowDialog();
            Actualizar();
        }
    }
}
