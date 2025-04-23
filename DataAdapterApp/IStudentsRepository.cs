using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAdapterApp
{
    public interface IStudentsRepository
    {
        DataSet GetStudentsDataSet();
        void UpdateStudentsDataSet(DataSet dataSet);
    }
}
