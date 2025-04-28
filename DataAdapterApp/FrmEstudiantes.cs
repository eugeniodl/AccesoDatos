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

namespace DataAdapterApp
{
    public partial class FrmEstudiantes : Form
    {
        private readonly IStudentsRepository _studentsRepository;
        private DataSet dataSet;

        public FrmEstudiantes()
        {
            InitializeComponent();
            string connectionString =
                ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
            _studentsRepository = new StudentsRepository(connectionString);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                _studentsRepository.UpdateStudentsDataSet(dataSet);
                MessageBox.Show("Estuadiantes actualizados.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar los estudiantes: {ex.Message}");
            }
        }

        private void FrmEstudiantes_Load(object sender, EventArgs e)
        {
            try
            {
                dataSet = _studentsRepository.GetStudentsDataSet();
                dgvEstudiantes.DataSource = dataSet;
                dgvEstudiantes.DataMember = "Estudiantes";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los estudiantes: {ex.Message}");
            }
        }
    }
}
