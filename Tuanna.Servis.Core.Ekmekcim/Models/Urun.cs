using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuanna.Servis.Core.Ekmekcim.Models
{
    public class Urun
    {
        public int No { get; set; }
        public int KategoriNo { get; set; }
        public string Resim { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
    }
}