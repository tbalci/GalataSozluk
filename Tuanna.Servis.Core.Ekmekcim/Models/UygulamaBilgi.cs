using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuanna.Data.Ekmekcim;

namespace Tuanna.Servis.Core.Ekmekcim.Models
{
    public class UygulamaBilgi
    {
        public string Sozlesme { get; set; }
        public string EkmekcimNedir { get; set; }
        public List<TBLMahalle> MahalleListesi { get; set; }

        public UygulamaBilgi()
        {
            this.MahalleListesi = new List<TBLMahalle>();
        }
    }
}