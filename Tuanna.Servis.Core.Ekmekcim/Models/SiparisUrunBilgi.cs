using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuanna.Servis.Core.Ekmekcim.Models
{
    public class SiparisUrunBilgiItem
    {
        public int SatirNo { get; set; }
        public int SipNo { get; set; }
        public int UserNo { get; set; }

        public string UrunAd { get; set; }
        public decimal UrunFiyat { get; set; }

        public string IslemTarih { get; set; }

        public bool GorulduMu { get; set; }
        public bool OnaylandiMi { get; set; }
        public int Adet { get; set; }
        public bool Teslimat { get; set; }
        public decimal Tutar { get; set; }
    }

    public class SiparisUrunBilgi
    {
        public string Ad { get; set; }
        public List<SiparisUrunBilgiItem> UrunListesi = new List<SiparisUrunBilgiItem>();
    }
}