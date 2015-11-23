using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.Common;
using System.Net.Mail;

/// <summary>
/// Summary description for EmailGondermeIslemleri
/// </summary>
/// 

namespace Tuanna.Lib
{
    public class EmailGondermeIslemleri
    {
        #region Enumlar
        #endregion
        #region -Alanlar-

        /// <summary>
        /// Server ServerAdı
        /// </summary>
        private string _ServerAd = "";

        /// <summary>
        /// Server ServerAdı
        /// </summary>
        public string ServerAd
        {
            get
            {
                return _ServerAd;
            }
            set
            {
                _ServerAd = value;
            }
        }

        /// <summary>
        /// Server ServerKullaniciAdı
        /// </summary>
        private string _ServerKullaniciAd = "";

        /// <summary>
        /// Server ServerKullaniciAdı
        /// </summary>
        public string ServerKullaniciAd
        {
            get
            {
                return _ServerKullaniciAd;
            }
            set
            {
                _ServerKullaniciAd = value;
            }
        }


        /// <summary>
        /// Server ServerSifre
        /// </summary>
        private string _ServerSifre = "";

        /// <summary>
        /// Server ServerSifre
        /// </summary>
        public string ServerSifre
        {
            get
            {
                return _ServerSifre;
            }
            set
            {
                _ServerSifre = value;
            }
        }

        /// <summary>
        /// Server ServerGonderilmeAd
        /// </summary>
        private string _ServerGonderilmeAd = "";

        /// <summary>
        /// Server ServerGonderilmeAd
        /// </summary>
        public string ServerGonderilmeAd
        {
            get
            {
                return _ServerGonderilmeAd;
            }
            set
            {
                _ServerGonderilmeAd = value;
            }
        }


        /// <summary>
        /// Server ServerPort
        /// </summary>
        private Int32 _ServerPort = 0;

        /// <summary>
        /// Server ServerPort
        /// </summary>
        public Int32 ServerPort
        {
            get
            {
                return _ServerPort;
            }
            set
            {
                if((value == 0) || (value.ToString() == ""))
                {
                    value = 25;
                }
                _ServerPort = value;
            }
        }

        private MailMessage _Mail = new MailMessage();

        public MailMessage Mail
        {
            get
            {
                return _Mail;
            }
            set
            {
                _Mail = value;
            }
        }
        private bool _SslDurum = false;

        public bool SslDurum
        {
            get
            {
                return _SslDurum;
            }
            set
            {
                _SslDurum = value;
            }
        }

        private DataTable _GondermeSonuclari = new DataTable();

        public DataTable GondermeSonuclari
        {
            get
            {
                return _GondermeSonuclari;
            }
            set
            {
                _GondermeSonuclari = value;
            }
        }


        public SmtpClient Gonderici = new SmtpClient();

        #endregion

        #region -Yapılandırcılar-

        public EmailGondermeIslemleri(string ServerAd, string KullaniciAd, string Sifre, Int32 Port,bool SslDurum)
        {
            this.ServerAd = ServerAd;
            this.ServerKullaniciAd = KullaniciAd;
            this.ServerSifre = Sifre;
            this.ServerPort = Port;
            this.SslDurum = SslDurum;
        }
        #endregion
        #region -Metodlar
        public bool Gonder()
        {
            Gonderici.Credentials = new System.Net.NetworkCredential(this.ServerKullaniciAd, this.ServerSifre);
            Gonderici.Port = this.ServerPort;
            Gonderici.Host = this.ServerAd;
            Gonderici.EnableSsl = this.SslDurum;
            try
            {
                Gonderici.Send(this.Mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
        #endregion
}