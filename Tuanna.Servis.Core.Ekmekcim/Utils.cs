using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Tuanna.Servis.Core.Ekmekcim.Models;

namespace Tuanna.Servis.Core.Ekmekcim
{
    public class Utils
    {
        //public object UniversiteToUniversite(UniversiteTip Tip)
        //{
        //    object Sonuc = null;
        //    Tuanna.Lib.Reflaciton YeniRefArac = new Lib.Reflaciton();
        //    string Yol = HttpContext.Current.Server.MapPath("~/bin/Tuanna.Servis.Core.Ekmekcim.dll");
        //    Assembly Nsn = Assembly.LoadFrom(Yol);
        //    List<Type> UniversiteTipListesi = YeniRefArac.ArayuzeAitNesneleriGetir(Nsn, "IUniversite");
        //    foreach (Type Itm in UniversiteTipListesi)
        //    {
        //        IUniversite Sinif = (IUniversite)Nsn.CreateInstance(Itm.FullName);
        //        if (Sinif.Tip==Tip)
        //        {
        //            Sonuc = Sinif;
        //        }
        //    }
        //    return Sonuc;
        //}

        //public  static string HtmlDegerBul(string _Txt,string Baslangic_Txt, string Bitis_txt, string SonEk,int KrkSayi )
        //{
        //    string Sonuc = null;
        //    try
        //    {
        //        int Start = -1;
        //        if (Baslangic_Txt == "*")
        //        {
        //            Start = 1;
        //        }
        //        else
        //        {
        //            Start = _Txt.IndexOf(Baslangic_Txt, 0, _Txt.Length - 1);
        //        }

        //        if (Start > -1)
        //        {
        //            int End = -1;
        //            Start += KrkSayi;
        //            End = _Txt.IndexOf(Bitis_txt, Start, _Txt.Length - (Start + 1));
        //            if (End > -1)
        //            {
        //                Sonuc = _Txt.Substring(Start, End - Start);
        //                Sonuc = Sonuc.Trim();
        //                return Sonuc;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
}