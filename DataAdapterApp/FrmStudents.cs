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
    public partial class FrmStudents : Form
    {
        private readonly IStudentsRepository _studentsRepository;
        private DataSet dataSet;
        public FrmStudents()
        {
            InitializeComponent();
            string connectionString =
                ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
            _studentsRepository = new StudentsRepository(connectionString);
        }

        private void FrmStudents_Load(object sender, EventArgs e)
        {
            try
            {
                dataSet = _studentsRepository.GetStudentsDataSet();
                dgvStudents.DataSource = dataSet;
                dgvStudents.DataMember = "Students";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _studentsRepository.UpdateStudentsDataSet(dataSet);
                MessageBox.Show("Update successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating students: {ex.Message}");
            }
        }
    }
}
