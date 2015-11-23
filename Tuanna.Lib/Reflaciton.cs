using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Tuanna.Lib
{
    public class Reflaciton
    {
        public List<PropertyInfo> OzellikleriGetir(Type Tip)
        {
            List<PropertyInfo> Sonuc = new List<PropertyInfo>();
            try
            {
                PropertyInfo[] Ozellikler =Tip.GetProperties();
                Sonuc.AddRange(Ozellikler);
            }
            catch
            {
                Sonuc = new List<PropertyInfo>();
            }
            return Sonuc;
        }
        public string FullAdiniGetir(Type Tip)
        {
            return Tip.FullName;
        }

        public List<Type> AssemblyAitNesneleriGetir(Assembly Asm)
        {
            List<Type> Sonuc = new List<Type>();
            try
            {
                Sonuc = Asm.GetTypes().ToList();
            }
            catch
            {
                Sonuc = new List<Type>();
            }
            return Sonuc;
        }

        public List<Type> ArayuzeAitNesneleriGetir(Assembly Asm,string ArayuzAd)
        {
            List<Type> Sonuc = new List<Type>();
            try
            {
                 Type[] TipListesi = Asm.GetTypes();
                foreach (Type Nsn in TipListesi)
                {
                    if (Nsn.GetInterface(ArayuzAd) != null)
                    {
                        Sonuc.Add(Nsn);
                    }
                }
            }
            catch
            {
                
            }
            return Sonuc;
        }

    }
}
