using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace Tuanna.Lib
{
    public class LDapYonetici
    {
        private string _Ldap = "";
        public string Ldap
        {
            get
            {
                return _Ldap;
            }
            set
            {
                _Ldap = value;
            }
        }

        public LDapYonetici(string Ldap)
        {
            this.Ldap ="LDAP://"+ Ldap;
        }


        public List<string> KullaniciBilgileriniVer(string KullaniciAd,string Sifre)
        {
            List<string> Sonuc = new List<string>();
            try
            {
                DirectoryEntry objDe = new DirectoryEntry(Ldap, KullaniciAd, Sifre);
                DirectorySearcher searcher = new DirectorySearcher(objDe);
                searcher.Filter = "(&(objectClass=person)(sAMAccountName=" + KullaniciAd + "))";
                searcher.PropertiesToLoad.Add("givenname");
                searcher.PropertiesToLoad.Add("sn");
                searcher.PropertiesToLoad.Add("userPrincipalName");
                SortOption Srt;
                Srt = new SortOption("sn", System.DirectoryServices.SortDirection.Ascending);
                searcher.Sort = Srt;
                SearchResult Result = searcher.FindOne();
                DirectoryEntry userEntry = null;
                userEntry = Result.GetDirectoryEntry();
                if (userEntry != null)
                {
                    Sonuc.Add(KullaniciAd);
                    Sonuc.Add( userEntry.Properties["givenname"].Value.ToString());
                    Sonuc.Add( userEntry.Properties["sn"].Value.ToString());
                    Sonuc.Add( userEntry.Properties["userPrincipalName"].Value.ToString());
                }
                else
                {
                    Sonuc = new List<string>();
                }
            }
            catch (Exception Hata)
            {
                Sonuc = new List<string>();
            }
            return Sonuc;
        }

        public bool GirisYap(string KullaniciAd, string Sifre)
        {
            try
            {
                DirectoryEntry objDe = new DirectoryEntry(Ldap, KullaniciAd, Sifre);
                DirectorySearcher searcher = new DirectorySearcher(objDe);
                searcher.Filter = "(&(objectClass=person)(sAMAccountName=" + KullaniciAd + "))";
                SearchResult Result = searcher.FindOne();
                DirectoryEntry userEntry = null;
                userEntry = Result.GetDirectoryEntry();
                if (userEntry != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool SifreDegisitir(string KulAd, string EskiSifre, string YeniSifre)
        {
            bool Sonuc = false;
            DirectoryEntry objDe = new DirectoryEntry(Ldap, KulAd, EskiSifre);
            DirectorySearcher searcher = new DirectorySearcher(objDe);
            searcher.Filter = "(&(objectClass=person)(sAMAccountName=" + KulAd + "))";
            searcher.PropertiesToLoad.Add("cn");
            searcher.PropertiesToLoad.Add("sn");
            SortOption Srt;
            Srt = new SortOption("sn", System.DirectoryServices.SortDirection.Ascending);
            searcher.Sort = Srt;
            SearchResult Result = searcher.FindOne();
            DirectoryEntry userEntry = null;
            userEntry = Result.GetDirectoryEntry();
            try
            {
                if (userEntry != null)
                {
                    userEntry.Invoke("ChangePassword", new object[] { EskiSifre, YeniSifre });
                    userEntry.CommitChanges();
                    Sonuc = true;
                }
                else
                {
                    Sonuc = false;
                }
            }
            catch (Exception Hata)
            {
                Sonuc = false;
            }
            return Sonuc;
        }
    }
}

