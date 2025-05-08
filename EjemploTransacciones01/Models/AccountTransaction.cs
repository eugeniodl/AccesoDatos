using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EjemploTransacciones01.Models
{
    // Clase de entidad AccountTransaction
    public class AccountTransaction
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string AccountNumber { get; set; }
        [Precision(18, 2)]
        public decimal Credit { get; set; }
        [Precision(18, 2)]
        public decimal Debit { get; set; }
    }
}
