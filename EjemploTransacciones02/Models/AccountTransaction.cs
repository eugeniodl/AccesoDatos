using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploTransacciones02.Models
{
    public class AccountTransaction
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? AccountNumber { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
    }
}
