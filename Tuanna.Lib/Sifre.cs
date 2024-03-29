using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Tuanna.Lib
{
    // Şifreleme ve çözme işlemleri için kullandığımız sınıfımız, Rijndael algoritmasını temel alır.
    public class Sifre:IDisposable
    {
        private string _Key = "sifrelenecekkritersifresi";
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        #region Yapılandırcılar
        public Sifre()
        {
        }
        public Sifre(string Sifre)
        {
            this._Key = Sifre;
        }
        #endregion
        public string Sifrele(string SifrelenecekTxt)
        {
            if((SifrelenecekTxt != "") & (SifrelenecekTxt != null))
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                RijndaelCipher.Padding = PaddingMode.PKCS7;
                byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(SifrelenecekTxt);
                byte[] Salt = Encoding.ASCII.GetBytes(Key.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Key, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(PlainText, 0, PlainText.Length);
                cryptoStream.FlushFinalBlock();
                byte[] CipherBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string EncryptedData = Convert.ToBase64String(CipherBytes);
                return EncryptedData;
            }
            return "";
        }

        public string Coz(string CozulecekVeri)
        {
            if((CozulecekVeri != "") & (CozulecekVeri != null))
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                RijndaelCipher.Padding = PaddingMode.PKCS7;
                byte[] EncryptedData = Convert.FromBase64String(CozulecekVeri);
                byte[] Salt = Encoding.ASCII.GetBytes(Key.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Key, Salt);
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                byte[] PlainText = new byte[EncryptedData.Length];
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
                return DecryptedData;
            }
            else
            {
                return "";
            }
        }
       #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}