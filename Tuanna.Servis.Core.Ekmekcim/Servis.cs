using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Web;
using Tuanna.Data.Ekmekcim;
using Tuanna.Servis.Core.Ekmekcim.Models;

namespace Tuanna.Servis.Core.Ekmekcim
{
    public class Servis
    {
        public object UygulamaBilgileriniGetir()
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            UygulamaBilgi UygulamaBilgi = new UygulamaBilgi();
            using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
            {
                try
                {
                    string SozlemeFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Dokuman/Sozlesme.html");
                    string EkmekcimNedirFile = System.Web.Hosting.HostingEnvironment.MapPath("~/Dokuman/EkmekcimNedir.html");
                    UygulamaBilgi.EkmekcimNedir = System.IO.File.ReadAllText(EkmekcimNedirFile);
                    UygulamaBilgi.Sozlesme = System.IO.File.ReadAllText(SozlemeFile);
                    UygulamaBilgi.MahalleListesi = VeriTabani.TBLMahalle.ToList();
                    Sonuc.Durum = true;
                    Sonuc.Mesaj = "Bilgiler Alındı";
                    return new
                    {
                        Sonuc = Sonuc,
                        Uygulama = UygulamaBilgi
                    };
                }
                catch (Exception Hata)
                {
                    Sonuc.Durum = false;
                    Sonuc.Mesaj = "Hata Oluştu";
                    Sonuc.Hata = Hata;
                    return new
                    {
                        Sonuc = Sonuc,
                        Uygulama = UygulamaBilgi
                    };
                }
            }
        }

        public object UyeOl(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                UyeOlBilgi KullaniciBilgi = Nesne.ToModel<UyeOlBilgi>();
                TBLUye Uye = new TBLUye();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    var QSonuc = VeriTabani.TBLUye.Where(x => x.AdSoyad == KullaniciBilgi.AdSoyad).ToList();
                    if (QSonuc.Count == 0)
                    {
                        try
                        {
                            Uye.AdSoyad = KullaniciBilgi.AdSoyad;
                            Uye.Cinsiyet = Convert.ToBoolean(KullaniciBilgi.Cinsiyet);
                            Uye.ApartmanAd = KullaniciBilgi.ApartmanAd;
                            Uye.ApartmanNo = KullaniciBilgi.ApartmanNo;
                            Uye.Cadde = KullaniciBilgi.Cadde;
                            Uye.Cep = KullaniciBilgi.Cep;
                            Uye.Mail = KullaniciBilgi.Mail;
                            Uye.Mahalle = 0;
                            Uye.OnayDurum = false;
                            Uye.IslemTarih = DateTime.Now.ToString();
                            Sonuc.Durum = true;
                            Sonuc.Mesaj = "Üye Kaydı Başarıyla Yapıldı. Yöneticiler tarafından onaylandığında aktif olacaktır.";
                            VeriTabani.TBLUye.Add(Uye);
                            VeriTabani.SaveChanges();
                        }
                        catch (Exception Hata)
                        {
                            Sonuc.Durum = true;
                            Sonuc.Mesaj = "Üye Kaydı Yapılamadı.";
                            Sonuc.Hata = Hata;
                        }
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = string.Format("{0} kullanıcısı Bulunmaktadır.Lütfen Başka bir isim yazınız.! ", KullaniciBilgi.AdSoyad);
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Uye = Uye
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
        }

        public object KisiBilgileriniGetir(string NoTxt)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                int No = Convert.ToInt32(NoTxt);
                KullaniciBilgi KullaniciBilgi = new KullaniciBilgi();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    var QSonuc = VeriTabani.TBLUye.Find(No);
                    if (QSonuc != null)
                    {
                        Sonuc.Durum = true;
                        Sonuc.Mesaj = "İşlem Giriş Yaptınız.";
                        KullaniciBilgi.AdSoyad = QSonuc.AdSoyad;
                        KullaniciBilgi.No = QSonuc.SatirNo;
                        KullaniciBilgi.Mail = QSonuc.Mail;
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = "Kullanıcı Yok";
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Kullanici = KullaniciBilgi
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
        }

        public object GirisYap(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                Kullanici Kullanici = Nesne.ToModel<Kullanici>();
                KullaniciBilgi KullaniciBilgi = new KullaniciBilgi();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    var QSonuc = VeriTabani.TBLUye.Where(x => x.AdSoyad == Kullanici.KullaniciAd && x.Sifre == Kullanici.Sifre).ToList();
                    if (QSonuc.Count > 0)
                    {
                        Tuanna.Data.Ekmekcim.TBLUye Uye = QSonuc.First();
                        if (Uye.OnayDurum)
                        {
                            Sonuc.Durum = true;
                            Sonuc.Mesaj = "İşlem Giriş Yaptınız.";
                            KullaniciBilgi.AdSoyad = Uye.AdSoyad;
                            KullaniciBilgi.No = Uye.SatirNo;
                            KullaniciBilgi.Mail = Uye.Mail;
                            if (VeriTabani.TBLAnlikBakiye.Where(x => x.UyeFirma == Uye.SatirNo).Count() < 1)
                            {
                                TBLAnlikBakiye Bakiye = new TBLAnlikBakiye();
                                Bakiye.IslemTarih = DateTime.Now.ToString();
                                Bakiye.Bakiye = 0;
                                Bakiye.UyeFirma = 0;
                                VeriTabani.TBLAnlikBakiye.Add(Bakiye);
                                VeriTabani.SaveChanges();
                            }
                        }
                        else
                        {
                            Sonuc.Durum = false;
                            Sonuc.Mesaj = "Kullanıcı Onaylı Değil!";
                        }
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = "Kullanıcı Yok";
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Kullanici = KullaniciBilgi
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
        }

        public bool IsTimeOfDayBetween(DateTime time,
                                      TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime == startTime)
            {
                return true;
            }
            else if (endTime < startTime)
            {
                return time.TimeOfDay <= endTime ||
                    time.TimeOfDay >= startTime;
            }
            else
            {
                return time.TimeOfDay >= startTime &&
                    time.TimeOfDay <= endTime;
            }
        }

        public object UrunEklenmeZamanAraligiSorgula()
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                bool SaatDurumu = IsTimeOfDayBetween(DateTime.Now, new TimeSpan(22, 00, 00), new TimeSpan(7, 59, 59));
                Sonuc.Durum = true;
                Sonuc.Mesaj = "Başarıyla Tamamlandı";
                return new
                {
                    Sonuc = Sonuc,
                    SaatDurumu = SaatDurumu
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
            return Sonuc;
        }

        public object SepeteUrunEkle(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            TBLSiparis Sip = new TBLSiparis();
            try
            {
                SepetUrunEkleBilgi SepetUrunEkleBilgi = Nesne.ToModel<SepetUrunEkleBilgi>();

                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    var Kullanici = VeriTabani.TBLUye.Find(SepetUrunEkleBilgi.UserNo);
                    var Urun = VeriTabani.TBLUrun.Find(SepetUrunEkleBilgi.UrunNo);
                    if (Kullanici != null)
                    {
                        if (Urun != null)
                        {
                            bool EnAzBirStokEksikUrunVarmi = false;
                            string Mesaj = "";
                            TBLAnlikStok StkDrm = null;
                            if (VeriTabani.TBLAnlikStok.Where(x => x.Urun == Urun.SatirNo).Count() > 0)
                            {
                                StkDrm = VeriTabani.TBLAnlikStok.Where(x => x.Urun == Urun.SatirNo).First();
                            }
                            if (StkDrm == null)
                            {
                                EnAzBirStokEksikUrunVarmi = true;
                                Mesaj = string.Format("{0} ürününden stokta 0 adet bulunmaktadır.", Urun.UrunAd) + Environment.NewLine;
                            }
                            else
                            {
                                if (SepetUrunEkleBilgi.Adet > StkDrm.Adet)
                                {
                                    EnAzBirStokEksikUrunVarmi = true;
                                    Mesaj = string.Format("{0} ürününden stokta {1} adet bulunmaktadır.", Urun.UrunAd, StkDrm.Adet) + Environment.NewLine;
                                }
                            }

                            if (!EnAzBirStokEksikUrunVarmi)
                            {
                                bool SaatDurumu = IsTimeOfDayBetween(DateTime.Now, new TimeSpan(22, 00, 00), new TimeSpan(7, 59, 59));
                                //bool SaatDurumu = IsTimeOfDayBetween(DateTime.Now, new TimeSpan(7, 59, 59), new TimeSpan(22, 00, 00));
                                DateTime Zaman = DateTime.Now;
                                if (SaatDurumu)
                                {
                                    Zaman = Zaman.AddDays(1);
                                }

                                var SncListe = VeriTabani.TBLSiparis.Where(x => (x.Urun == Urun.SatirNo) && (x.Uye == Kullanici.SatirNo && x.OnaylandiMi == false)).ToList();
                                foreach (var item in SncListe)
                                {
                                    if (Convert.ToDateTime(item.IslemTarih).Date == Zaman.Date)
                                    {
                                        Sip = item;
                                        break;
                                    }
                                }

                                if (SncListe.Count() > 0)
                                {
                                    Sip = SncListe.First();
                                    Sip.Adet += SepetUrunEkleBilgi.Adet;
                                    Sip.IslemTarih = Zaman.ToString();
                                    VeriTabani.SaveChanges();
                                }
                                else
                                {
                                    Sip.Uye = Kullanici.SatirNo;
                                    Sip.Urun = Urun.SatirNo;
                                    Sip.GorulduMu = false;
                                    Sip.OnaylandiMi = false;
                                    Sip.IslemTarih = Zaman.ToString();
                                    Sip.Teslimat = false;
                                    Sip.Adet = SepetUrunEkleBilgi.Adet;
                                    VeriTabani.TBLSiparis.Add(Sip);
                                    VeriTabani.SaveChanges();
                                }
                                Sonuc.Durum = true;
                                Sonuc.Mesaj = "İşlem Başarıyla Eklendi.";
                            }
                            else
                            {
                                Sonuc.Durum = false;
                                Sonuc.Mesaj = Mesaj;
                            }
                        }
                        else
                        {
                            Sonuc.Durum = false;
                            Sonuc.Mesaj = "Ürün Yok";
                        }
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = "Kullanıcı Yok";
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Siparis = Sip
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;

                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
        }

        public object SiparisOnay(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                List<SiparisOnayInputSepetBilgi> SiparisOnayInputSepetBilgiListesi = Nesne.ToModel<List<SiparisOnayInputSepetBilgi>>();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    bool EnAzBirStokEksikUrunVarmi = false;
                    string Mesaj = "";
                    var SiparisStokUrunListesi = SiparisOnayInputSepetBilgiListesi.GroupBy(x => x.UrunNo).Select(
                        Bilgi =>
                        new
                        {
                            Bilgi.Key,
                            Adet = Bilgi.Sum(lg => lg.Adet)
                        }
                        );
                    foreach (var SipOnay in SiparisStokUrunListesi)
                    {
                        TBLAnlikStok StkDrm = null;
                        TBLUrun Urn = null;
                        if (VeriTabani.TBLAnlikStok.Where(x => x.Urun == SipOnay.Key).Count() > 0)
                        {
                            StkDrm = VeriTabani.TBLAnlikStok.Where(x => x.Urun == SipOnay.Key).First();
                        }
                        Urn = VeriTabani.TBLUrun.Find(SipOnay.Key);
                        if (StkDrm == null)
                        {
                            EnAzBirStokEksikUrunVarmi = true;
                            if (Urn != null)
                            {
                                Mesaj = string.Format("{0} ürününden stokta 0 adet bulunmaktadır.", Urn.UrunAd) + Environment.NewLine;
                            }
                            break;
                        }
                        else
                        {
                            if (StkDrm.Adet < SipOnay.Adet)
                            {
                                EnAzBirStokEksikUrunVarmi = true;
                                Mesaj = string.Format("{0} ürününden stokta {1} adet bulunmaktadır.", Urn.UrunAd, StkDrm.Adet) + Environment.NewLine;
                                break;
                            }
                        }
                    }
                    if (!EnAzBirStokEksikUrunVarmi)
                    {
                        bool TumUrunleriAlmaBakiyeDurumu = false;
                        int SUserNo = 0;
                        foreach (var SipOnay in SiparisOnayInputSepetBilgiListesi)
                        {
                            SUserNo = SipOnay.UserNo;
                            break;
                        }

                        TBLAnlikBakiye Bakiye = null;
                        if (VeriTabani.TBLAnlikBakiye.Where(x => x.UyeFirma == SUserNo).Count() > 0)
                        {
                            Bakiye = VeriTabani.TBLAnlikBakiye.Where(x => x.UyeFirma == SUserNo).First();
                        }

                        decimal HesapTutar = 0;
                        foreach (var SipOnay in SiparisOnayInputSepetBilgiListesi)
                        {
                            SUserNo = SipOnay.UserNo;
                            TBLSiparis Sip = VeriTabani.TBLSiparis.Find(SipOnay.SipNo);
                            if (Sip != null)
                            {
                                TBLUrun Urn = VeriTabani.TBLUrun.Find(Sip.Urun);
                                if (Urn != null)
                                {
                                    HesapTutar += (Sip.Adet * Urn.UrunFiyat);
                                }
                            }
                        }

                        if (Bakiye != null)
                        {
                            if (Bakiye.Bakiye - HesapTutar > -1)
                            {
                                TumUrunleriAlmaBakiyeDurumu = true;
                            }
                            else
                            {
                                TumUrunleriAlmaBakiyeDurumu = false;
                                Mesaj = string.Format("Bakiye Yeterli Değil! Siparişleriniz {0} TL - Bakiyeniz {1} TL", HesapTutar.ToString("#.##"), Bakiye.Bakiye.ToString("#.##"));
                            }
                        }
                        else
                        {
                            Mesaj = string.Format("Bakiye Yeterli Değil! Siparişleriniz {0} TL - Bakiyeniz {1} TL", HesapTutar.ToString("#.##"), 0.ToString("#.##"));
                        }

                        if (TumUrunleriAlmaBakiyeDurumu)
                        {
                            int SipUserNo = 0;
                            foreach (var SipOnay in SiparisOnayInputSepetBilgiListesi)
                            {
                                SipUserNo = SipOnay.UserNo;
                                TBLAnlikBakiye IslemBakiye = null;
                                if (VeriTabani.TBLAnlikBakiye.Where(x => x.UyeFirma == SipUserNo).Count() > 0)
                                {
                                    IslemBakiye = VeriTabani.TBLAnlikBakiye.Where(x => x.UyeFirma == SipUserNo).First();
                                }
                                if (IslemBakiye == null)
                                {
                                    Sonuc.Durum = false;
                                    Sonuc.Mesaj = "Bakiyeniz Tükendi. Bazı ürünleri alamadınız.";
                                    break;
                                }
                                else
                                {
                                    TBLSiparis Sip = VeriTabani.TBLSiparis.Find(SipOnay.SipNo);
                                    if (Sip != null)
                                    {
                                        TBLUrun Urn = VeriTabani.TBLUrun.Find(Sip.Urun);
                                        if (Urn != null)
                                        {
                                            decimal Tutar = (Sip.Adet * Urn.UrunFiyat);
                                            if ((IslemBakiye.Bakiye - Tutar) > -1)
                                            {
                                                TBLAnlikStok StkDrm = null;
                                                if (VeriTabani.TBLAnlikStok.Where(x => x.Urun == Sip.Urun).Count() > 0)
                                                {
                                                    StkDrm = VeriTabani.TBLAnlikStok.Where(x => x.Urun == Sip.Urun).First();
                                                }
                                                if (StkDrm != null)
                                                {
                                                    DateTime Zaman = DateTime.Now;
                                                    if (Convert.ToDateTime(Sip.IslemTarih).Date == Zaman.Date)
                                                    {
                                                        IslemBakiye.Bakiye = IslemBakiye.Bakiye - (Sip.Adet * Urn.UrunFiyat);
                                                        VeriTabani.SaveChanges();
                                                        Sip.Adet = SipOnay.Adet;
                                                        Sip.OnaylandiMi = true;
                                                        VeriTabani.SaveChanges();
                                                        StkDrm.Adet = StkDrm.Adet - Sip.Adet;
                                                        VeriTabani.SaveChanges();
                                                        Sonuc.Durum = true;
                                                        Sonuc.Mesaj = "Sipariş Tarihi Bu Gün Olan Siparişiniz Onaylandı.!";
                                                    }
                                                }
                                                else
                                                {
                                                    Sonuc.Durum = false;
                                                    Sonuc.Mesaj = "Stok Tükendi. Bazı ürünleri alamadınız.";
                                                }
                                                Sonuc.Durum = false;
                                                Sonuc.Mesaj = "Bakiyeniz Tükendi. Bazı ürünleri alamadınız.";
                                                break;
                                            }
                                            else
                                            {
                                                Sonuc.Durum = false;
                                                Sonuc.Mesaj = "Bakiyeniz Tükendi. Bazı ürünleri alamadınız.";
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (Sonuc.Durum)
                            {
                                var SilinecekOnaylanmayanSipListesi = VeriTabani.TBLSiparis.Where(x => x.Uye == SipUserNo && x.OnaylandiMi == false).ToList();
                                foreach (var SipOnaytem in SilinecekOnaylanmayanSipListesi)
                                {
                                    TBLSiparis SipDel = VeriTabani.TBLSiparis.Find(SipOnaytem.SatirNo);
                                    if (SipDel != null)
                                    {
                                        VeriTabani.TBLSiparis.Remove(SipDel);
                                        VeriTabani.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Sonuc.Durum = false;
                            Sonuc.Mesaj = Mesaj;
                        }
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = Mesaj;
                    }
                }
                return new
                {
                    Sonuc = Sonuc
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
        }

        public object SiparisSil(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    TBLSiparis Sip = VeriTabani.TBLSiparis.Find(Convert.ToInt32(Nesne));
                    if (Sip != null)
                    {
                        VeriTabani.TBLSiparis.Remove(Sip);
                        VeriTabani.SaveChanges();
                    }

                    Sonuc.Durum = true;
                    Sonuc.Mesaj = "Ürün Sepetten Silindi";
                }
                return new
                {
                    Sonuc = Sonuc
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc
                };
            }
        }

        public object SiparisUrunListesiGuncelle(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                KullaniciSepetBilgi KullaniciSepetBilgi = Nesne.ToModel<KullaniciSepetBilgi>();

                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    var Kullanici = VeriTabani.TBLUye.Find(KullaniciSepetBilgi.UserNo);
                    if (Kullanici != null)
                    {
                        Sonuc.Durum = true;
                        Sonuc.Mesaj = "İşlem Başarılı";
                        var SiparisListesi = VeriTabani.TBLSiparis.
                        Join(VeriTabani.TBLUrun,
                        Siparis => Siparis.Urun, Urun => Urun.SatirNo,
                        (Siparis, Urun) => new
                        {
                            SipNo = Siparis.SatirNo,
                            UserNo = Siparis.Uye,
                            UrunAd = Urun.UrunAd,
                            UrunFiyat = Urun.UrunFiyat,
                            GorulduMu = Siparis.GorulduMu,
                            IslemTarih = Siparis.IslemTarih,
                            OnaylandiMi = Siparis.OnaylandiMi,
                            Adet = Siparis.Adet,
                            Teslimat = Siparis.Teslimat,
                            Tutar = Urun.UrunFiyat * Siparis.Adet
                        }).Where(x => x.UserNo == KullaniciSepetBilgi.UserNo && x.OnaylandiMi == true).OrderByDescending(x => x.SipNo).ToList()
                        .Select((UrunBilgi, index) => new
                        {
                            SatirNo = index + 1,
                            SipNo = UrunBilgi.SipNo,
                            UserNo = UrunBilgi.UserNo,
                            UrunAd = UrunBilgi.UrunAd,
                            UrunFiyat = UrunBilgi.UrunFiyat,
                            GorulduMu = UrunBilgi.GorulduMu,
                            IslemTarih = UrunBilgi.IslemTarih,
                            OnaylandiMi = UrunBilgi.OnaylandiMi,
                            Adet = UrunBilgi.Adet,
                            Teslimat = UrunBilgi.Teslimat,
                            Tutar = UrunBilgi.Tutar
                        })
                        .ToList();

                        List<SiparisUrunBilgi> SiparisUrunListesi = new List<SiparisUrunBilgi>();
                        foreach (var item in SiparisListesi)
                        {
                            DateTime? Tarih = null;
                            try
                            {
                                Tarih = Convert.ToDateTime(item.IslemTarih.ToString());
                            }
                            catch (Exception Hata)
                            {
                                Tarih = null;
                            }
                            if (Tarih != null)
                            {
                                string TxtTarih = Tarih.Value.ToShortDateString();
                                var Snc = SiparisUrunListesi.Where(x => x.Ad == TxtTarih);
                                if (Snc.Count() == 0)
                                {
                                    SiparisUrunListesi.Add(new SiparisUrunBilgi() { Ad = TxtTarih });
                                }
                                SiparisUrunBilgi SiparisUrunBilgi = SiparisUrunListesi[SiparisUrunListesi.Count - 1];
                                SiparisUrunBilgi.UrunListesi.Add(new SiparisUrunBilgiItem()
                                {
                                    SipNo = item.SipNo,
                                    Adet = item.Adet,
                                    GorulduMu = item.GorulduMu,
                                    IslemTarih = item.IslemTarih,
                                    OnaylandiMi = item.OnaylandiMi,
                                    SatirNo = item.SatirNo,
                                    Teslimat = item.Teslimat,
                                    Tutar = item.Tutar,
                                    UrunAd = item.UrunAd,
                                    UrunFiyat = item.UrunFiyat,
                                    UserNo = item.UserNo
                                }
                                );
                            }
                        }
                        return new
                        {
                            Sonuc = Sonuc,
                            Liste = SiparisUrunListesi
                        };
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = "Kullanıcı Yok";
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Siparis = new TBLSiparis()
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;

                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc,
                    Siparis = new TBLSiparis()
                };
            }
        }

        public object SepetUrunListesiGuncelle(string Nesne)
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                KullaniciSepetBilgi KullaniciSepetBilgi = Nesne.ToModel<KullaniciSepetBilgi>();

                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    var Kullanici = VeriTabani.TBLUye.Find(KullaniciSepetBilgi.UserNo);
                    if (Kullanici != null)
                    {
                        Sonuc.Durum = true;
                        Sonuc.Mesaj = "İşlem Başarılı";
                        var SiparisListesi = VeriTabani.TBLSiparis.
                        Join(VeriTabani.TBLUrun,
                        Siparis => Siparis.Urun, Urun => Urun.SatirNo,
                        (Siparis, Urun) => new
                        {
                            SipNo = Siparis.SatirNo,
                            UserNo = Siparis.Uye,
                            UrunNo = Urun.SatirNo,
                            UrunAd = Urun.UrunAd,
                            UrunFiyat = Urun.UrunFiyat,
                            GorulduMu = Siparis.GorulduMu,
                            IslemTarih = Siparis.IslemTarih,
                            OnaylandiMi = Siparis.OnaylandiMi,
                            Adet = Siparis.Adet,
                            Teslimat = Siparis.Teslimat,
                            Tutar = Urun.UrunFiyat * Siparis.Adet
                        }).Where(x => x.UserNo == KullaniciSepetBilgi.UserNo && x.OnaylandiMi == false).OrderByDescending(x => x.SipNo).ToList()
                        .Select((UrunBilgi, index) => new
                        {
                            SatirNo = index + 1,
                            SipNo = UrunBilgi.SipNo,
                            UrunNo = UrunBilgi.UrunNo,
                            UserNo = UrunBilgi.UserNo,
                            UrunAd = UrunBilgi.UrunAd,
                            UrunFiyat = UrunBilgi.UrunFiyat,
                            GorulduMu = UrunBilgi.GorulduMu,
                            IslemTarih = UrunBilgi.IslemTarih,
                            OnaylandiMi = UrunBilgi.OnaylandiMi,
                            Adet = UrunBilgi.Adet,
                            Teslimat = UrunBilgi.Teslimat,
                            Tutar = UrunBilgi.Tutar
                        })
                        .ToList();
                        return new
                        {
                            Sonuc = Sonuc,
                            Liste = SiparisListesi
                        };
                    }
                    else
                    {
                        Sonuc.Durum = false;
                        Sonuc.Mesaj = "Kullanıcı Yok";
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Siparis = new TBLSiparis()
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;

                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new
                {
                    Sonuc = Sonuc,
                    Siparis = new TBLSiparis()
                };
            }
        }

        public object TumKullanicilariGetir()
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                Sonuc.Durum = true;
                Sonuc.Mesaj = "İşlem Başarılı";
                List<Tuanna.Data.Ekmekcim.TBLUye> Uyeler = new List<Data.Ekmekcim.TBLUye>();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    Uyeler = VeriTabani.TBLUye.ToList();
                }
                return new
                {
                    Sonuc = Sonuc,
                    Kullanicilar = Uyeler
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new { Sonuc = Sonuc };
            }
        }

        public object SliderListesiniGetir()
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                Sonuc.Durum = true;
                Sonuc.Mesaj = "İşlem Başarılı";
                List<SliderItem> Liste = new List<SliderItem>()
                        {
                            new SliderItem() {Resim=string.Format("http://ekmekcim.net/Slayt/{0}","Slide1.jpg"),Ifade="Slayt Ifade 1"},
                            new SliderItem() {Resim=string.Format("http://ekmekcim.net/Slayt/{0}","Slide2.jpg"),Ifade="Slayt Ifade 2"},
                            new SliderItem() {Resim=string.Format("http://ekmekcim.net/Slayt/{0}","Slide3.jpg"),Ifade="Slayt Ifade 3"}
                        };

                return new
                {
                    Sonuc = Sonuc,
                    Liste = Liste
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new { Sonuc = Sonuc };
            }
        }

        public object KategoriListesiniGetir()
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                Sonuc.Durum = true;
                Sonuc.Mesaj = "İşlem Başarılı";
                List<Kategori> Liste = new List<Kategori>() { };
                List<Tuanna.Data.Ekmekcim.TBLAltKategori> Kategoriler = new List<Data.Ekmekcim.TBLAltKategori>();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    Kategoriler = VeriTabani.TBLAltKategori.ToList();

                    foreach (var item in Kategoriler)
                    {
                        Liste.Add(new Kategori()
                        {
                            Ad = item.AltKategoriAd,
                            Resim = string.Format("http://www.ekmekcim.net/images/kategori/{0}.jpg", item.SatirNo.ToString()),
                            No = item.SatirNo,
                            Aciklama = string.Format("{0} adet ürün bulunmaktadır", VeriTabani.TBLUrun.Where(x => x.AltKategori == item.SatirNo).Count())
                        });
                    }
                }
                return new
                {
                    Sonuc = Sonuc,
                    Liste = Liste
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new { Sonuc = Sonuc };
            }
        }

        public object UrunListesiniGetir()
        {
            Tuanna.Lib.IslemDurum Sonuc = new Lib.IslemDurum(false, "");
            try
            {
                Sonuc.Durum = true;
                Sonuc.Mesaj = "İşlem Başarılı";
                List<Urun> Liste = new List<Urun>() { };
                List<Tuanna.Data.Ekmekcim.TBLUrun> Urunler = new List<Data.Ekmekcim.TBLUrun>();
                using (Tuanna.Data.Ekmekcim.ekmekcimnetEntities VeriTabani = new Data.Ekmekcim.ekmekcimnetEntities())
                {
                    Urunler = VeriTabani.TBLUrun.ToList();
                }
                foreach (var item in Urunler)
                {
                    Liste.Add(new Urun()
                    {
                        Ad = item.UrunAd,
                        Resim = string.Format("http://ekmekcim.net/images/125X125/{0}", item.UrunResim),
                        No = item.SatirNo,
                        Fiyat = item.UrunFiyat,
                        KategoriNo = item.AltKategori
                    });
                }
                return new
                {
                    Sonuc = Sonuc,
                    Liste = Liste
                };
            }
            catch (Exception Hata)
            {
                Sonuc.Durum = false;
                Sonuc.Mesaj = "Hata Oluştu";
                Sonuc.Hata = Hata;
                return new { Sonuc = Sonuc };
            }
        }
    }
}