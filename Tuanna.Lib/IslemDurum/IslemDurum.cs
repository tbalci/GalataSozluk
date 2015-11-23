using System;

    /// <summary>
    /// Bir İşlem Yapıldığında İşlemin Başarılı Olup Olmadığı ile ilgili Bilgileri Tutarım
    /// </summary>
    /// 

namespace Tuanna.Lib
{
    public class IslemDurum
    {
        #region Enumlar
        #endregion
        #region -Alanlar-


        /// <summary>
        /// İşlem Durumu
        /// </summary>
        private bool _Durum = true;

        /// <summary>
        /// İşlem Durumu
        /// </summary>
        public bool Durum
        {
            get
            {
                return _Durum;
            }
            set
            {
                _Durum = value;
            }
        }

        /// <summary>
        /// İşlem Mesajı
        /// </summary>
        private string _Mesaj = "";

        /// <summary>
        /// İşlem Mesajı
        /// </summary>
        public string Mesaj
        {
            get
            {
                return _Mesaj;
            }
            set
            {
                _Mesaj = value;
            }
        }

        /// <summary>
        /// İşlem Degerı
        /// </summary>
        private object _Deger;

        /// <summary>
        /// İşlem Degerı
        /// </summary>
        public object Deger
        {
            get
            {
                return _Deger;
            }
            set
            {
                _Deger = value;
            }
        }

        #endregion
        #region -Yapılandırcılar-
        public IslemDurum()
        {

        }
        public IslemDurum(bool Durum, string Mesaj)
        {
            this.Durum = Durum;
            this.Mesaj = Mesaj;
        }
        public IslemDurum(bool Durum, string Mesaj,string Deger)
        {
            this.Durum = Durum;
            this.Mesaj = Mesaj;
            this.Deger = Deger;
        }


        public IslemDurum(bool Durum, string Mesaj,Exception Hata)
        {
            this.Durum = Durum;
            this.Mesaj = Mesaj;
            this.Hata = Hata;
        }

        public IslemDurum(bool Durum, string Mesaj, Exception Hata,string Deger)
        {
            this.Durum = Durum;
            this.Mesaj = Mesaj;
            this.Hata = Hata;
            this.Deger = Deger;
        }
        #endregion
        /// <summary>
        /// Dönen Hata Nesnesi
        /// </summary>
        private Exception _Hata;
        public Exception Hata
        {
            get { return _Hata; }
            set { _Hata = value; }
        }
        #region -Metodlar

        #endregion
    }

}