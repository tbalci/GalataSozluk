using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Tuanna.Lib
{
    public class Veri:IDisposable
    {
        public void TreeyiYukle(DataTable Veri,string IdSutun,string UstSutun,string TextAlan ,ref TreeView Liste, bool IlkKayıtBosOlsun,string Lnk,string Ilkkayit)
        {
            Liste.Nodes.Clear();
            TreeNode Node = null;
            if(IlkKayıtBosOlsun)
            {
                Node = new TreeNode(Ilkkayit, "0");
                Liste.Nodes.Add(Node);
            }
            foreach(DataRow Satir in Veri.Rows)
            {
                Node = new TreeNode(Satir[TextAlan].ToString(), Satir[IdSutun].ToString());
                if(Lnk!="")
                {
                    Node.NavigateUrl = Satir[Lnk].ToString();    
                }
                
                string rootID = Satir[IdSutun].ToString();
                if(Satir[UstSutun].ToString() == "0")
                {
                    Liste.Nodes.Add(Node);
                    AltElemanlariEkle(Node, rootID, Veri,IdSutun,UstSutun,TextAlan,Lnk);
                }
            }
        }

        public DataTable LinqSutunListesiOlustur<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }
            return dataTable;
        }

        //public DataTable LINQToDataTable<T>(List<T> items)
        //{

        //    DataTable dataTable = new DataTable(typeof(T).Name);

        //    //Get all the properties

        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (PropertyInfo prop in Props)
        //    {

        //        //Setting column names as Property names

        //        dataTable.Columns.Add(prop.Name);

        //    }

        //    foreach (T item in items)
        //    {

        //        var values = new object[Props.Length];

        //        for (int i = 0; i < Props.Length; i++)
        //        {

        //            //inserting property values to datatable rows

        //            values[i] = Props[i].GetValue(item, null);

        //        }

        //        dataTable.Rows.Add(values);

        //    }

        //    //put a breakpoint here and check datatable

        //    return dataTable;

        //}

        public DataTable Distinct(DataTable dataSource,
                                string distinctColumn)
        {
            DataTable returnTable = new DataTable("distinctTable");
            /*
             * kaynak tablodaki sütunları
             * geri dönecek tabloya kopyalıyorum
             * böylece direkt satır eklenirken
             * sütunların birebir eşleşmesini sağlıyorum
             */
            foreach (DataColumn c in dataSource.Columns)
            {
                DataColumn Sutun = new DataColumn() { ColumnName = c.ColumnName, DataType = c.DataType, Caption = c.Caption };
                returnTable.Columns.Add(Sutun);
            }
            bool hasRow = false;
            foreach (DataRow selectedRow in dataSource.Rows)
            {
                /*
                 * kaynak tabloda herbir satırı seçip
                 * sonuçların bulunduğu tabloda benzerinin
                 * olup olmadığına bakıyorum
                 */
                foreach (DataRow r in returnTable.Rows)
                {
                    /*
                     * eşleşen sonuçlar bulunursa,
                     * satırın var olduğu anlamında
                     * hasRow=true yapıyorum
                     */
                    if (r[distinctColumn].ToString() ==
                        selectedRow[distinctColumn].ToString())
                    {
                        hasRow = true;
                        break;
                    }
                }
                /*
                 * Seçili satır bulunmamışsa hasRow değeri
                 * varsayılan olarak false olacağı için,
                 * satır ekleniyor
                 */
                if (hasRow == false)
                    returnTable.Rows.Add(selectedRow.ItemArray);
                hasRow = false;
            }
            return returnTable;
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            if (varlist.Count() > 0)
            {
                PropertyInfo[] oProps = null;
                if (varlist == null) return dtReturn;
                foreach (T rec in varlist)
                {
                    if ((oProps == null) && (rec != null))
                    {
                        oProps = ((Type)rec.GetType()).GetProperties();
                        foreach (PropertyInfo pi in oProps)
                        {
                            Type colType = pi.PropertyType;
                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                            == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            DataColumn Sutun = new DataColumn(pi.Name, colType);
                            Sutun.Caption = pi.Name.ToString().Replace("_", " ");
                            dtReturn.Columns.Add(Sutun);
                        }
                        break;
                    }
                }

                foreach (T rec in varlist)
                {
                    if ((oProps == null) && (rec != null))
                    {
                        oProps = ((Type)rec.GetType()).GetProperties();
                    }
                    if (oProps != null)
                    {
                        DataRow dr = dtReturn.NewRow();
                        foreach (PropertyInfo pi in oProps)
                        {
                            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                            (rec, null);
                        }
                        dtReturn.Rows.Add(dr);
                    }
                }

            }


            return dtReturn;
        }




        private void AltElemanlariEkle(TreeNode ustNod, string rootID, DataTable Veri,string IdSutun,string UstSutun,string TextAlan,string Lnk)
        {
            DataRow[] altNodlar = Veri.Select(UstSutun + " = " + rootID, UstSutun+"  ASC");
            foreach(DataRow Satir in altNodlar)
            {
                TreeNode cocukNod = new TreeNode(Satir[TextAlan].ToString(), Satir[IdSutun].ToString());
                string SatirID = Satir[IdSutun].ToString();
                if(Lnk != "")
                {
                    cocukNod.NavigateUrl = Satir[Lnk].ToString();
                }
                ustNod.ChildNodes.Add(cocukNod);
                AltElemanlariEkle(cocukNod, SatirID, Veri, IdSutun, UstSutun, TextAlan,Lnk);
            }
        }

        private void AltTreeSimileEt(ref DataTable Tablo,TreeNode Node,string IdSutun,string TextAlan,string LevelAd)
        {
                    // Alt Nodelarıda Bu Sıranın İçine Ekle
                    foreach(TreeNode Nd in Node.ChildNodes)
                    {
                        DataRow YeniSatir = Tablo.NewRow();
                        YeniSatir[IdSutun]  = Nd.Value;
                        YeniSatir[TextAlan] = Nd.Text;
                        YeniSatir[LevelAd] =Nd.Depth;
                        Tablo.Rows.Add(YeniSatir);
                        AltTreeSimileEt(ref Tablo, Nd, IdSutun, TextAlan,LevelAd);
                    }
        }
        public int HaftaninKacinciGunu(DateTime Tarih)
        {
            return (int)DateTime.Parse(Tarih.ToShortDateString()).DayOfWeek;
        }
        public int YilinKacinciGunu(DateTime Tarih)
        {
            return (int)DateTime.Parse(Tarih.ToShortDateString()).DayOfYear;
        }
        public void TreeIcinNodeListesiGetir(ref TreeView Liste,DataTable Tablo, string IdSutun, string UstSutun, string TextAlan, bool İlkKayitBosOlsun, string LevelAd,string LnkSutun)
        {
            TreeyiYukle(Tablo, IdSutun, UstSutun, TextAlan, ref Liste, İlkKayitBosOlsun,LnkSutun,"");
        }
        public TreeNodeCollection TreeIcinNodeListesiGetir(DataTable Tablo,string IdSutun,string UstSutun,string TextAlan,bool İlkKayitBosOlsun,string LevelAd,string IlkKayit)
        {
                TreeView YeniTree = new TreeView();
                TreeyiYukle(Tablo, IdSutun, UstSutun, TextAlan, ref YeniTree, İlkKayitBosOlsun,"",IlkKayit);
                return YeniTree.Nodes;
        }
        public DataTable TreeIcinSimileEt(DataTable Tablo,string IdSutun,string UstSutun,string TextAlan,bool İlkKayitBosOlsun,string LevelAd,string İlkKayitAd)
        {
                DataTable Sonuc = Tablo.Clone();
                DataColumn DCLTREELEVEL = new DataColumn(LevelAd);
                DCLTREELEVEL.ColumnName = LevelAd;
                Sonuc.Columns.Add(DCLTREELEVEL);
                try 
	            {
                
                    TreeView YeniTree = new TreeView();
                    TreeyiYukle(Tablo, IdSutun, UstSutun, TextAlan, ref YeniTree, İlkKayitBosOlsun,"",İlkKayitAd);
                    foreach(TreeNode Nd in YeniTree.Nodes)
                    {
                        DataRow YeniSatir = Sonuc.NewRow();
                        YeniSatir[IdSutun] = Nd.Value;
                        YeniSatir[TextAlan] = Nd.Text;
                        YeniSatir[LevelAd] = 0;
                        Sonuc.Rows.Add(YeniSatir);
                        AltTreeSimileEt(ref Sonuc, Nd, IdSutun, TextAlan, LevelAd);
                    }
                }
	            catch (Exception)
	            {
                    Sonuc = Tablo.Clone();
                    DCLTREELEVEL = new DataColumn(LevelAd);
                    DCLTREELEVEL.ColumnName = LevelAd;
                    Sonuc.Columns.Add(DCLTREELEVEL);
	            }
                return Sonuc;
        }
        public DataTable TablaSirala(DataTable Tablo, string SiralamaKriter)
        {
            DataTable newDT = Tablo.Clone();
            int rowCount = Tablo.Rows.Count;

            DataRow[] BulunanSatirlar = Tablo.Select(null, SiralamaKriter);
            for (int i = 0; i < rowCount; i++)
            {
                object[] arr = new object[Tablo.Columns.Count];
                for (int j = 0; j < Tablo.Columns.Count; j++)
                {
                    arr[j] = BulunanSatirlar[i][j];
                }
                DataRow data_row = newDT.NewRow();
                data_row.ItemArray = arr;
                newDT.Rows.Add(data_row);
            }

            Tablo.Rows.Clear();

            for (int i = 0; i < newDT.Rows.Count; i++)
            {
                object[] arr = new object[Tablo.Columns.Count];
                for (int j = 0; j < Tablo.Columns.Count; j++)
                {
                    arr[j] = newDT.Rows[i][j];
                }

                DataRow data_row = Tablo.NewRow();
                data_row.ItemArray = arr;
                Tablo.Rows.Add(data_row);
            }
            return Tablo;
        }

        public DataView UstenIstenenKadarSatirGetir(DataView TabloGorunumu, int AlinacakKayitSayisi)
        {
            DataTable oTbHedef = TabloGorunumu.Table.Clone();
            if (AlinacakKayitSayisi >= TabloGorunumu.Count)
            {
                AlinacakKayitSayisi = TabloGorunumu.Count;
            }
            for (int i = 0; i <= AlinacakKayitSayisi - 1; i++)
            {
                oTbHedef.ImportRow(TabloGorunumu[i].Row);
            }
            return new DataView(oTbHedef);
        }

        public DataTable UstenIstenenKadarSatirliTabloGetir(DataView TabloGorunumu, int AlinacakKayitSayisi)
        {
            DataTable oTbHedef = TabloGorunumu.Table.Clone();
            if (AlinacakKayitSayisi >= TabloGorunumu.Count)
            {
                AlinacakKayitSayisi = TabloGorunumu.Count;
            }
            for (int i = 0; i <= AlinacakKayitSayisi - 1; i++)
            {
                oTbHedef.ImportRow(TabloGorunumu[i].Row);
            }
            return oTbHedef;
        }

        public string ConvertDbTableTotmlText(DataTable CevirliCekTablo, string Baslik, string Altlik)
        {
            if (CevirliCekTablo != null)
            {
                StringBuilder Cevirici = new StringBuilder();
                //Cevirici.Append("<html>");
                //Cevirici.Append("<head>");
                //Cevirici.Append("<title>");
                //Cevirici.Append("Page-");
                //Cevirici.Append(Guid.NewGuid().ToString());
                //Cevirici.Append("</title>");
                //Cevirici.Append("</head>");
                //Cevirici.Append("<body>");
                Cevirici.Append("<table border='1px' cellpadding='5' cellspacing='0'>");
                // Cevirici.Append("style='border: solid 1px Silver; font-size: x-small;'>");
                if ((Baslik != null) && (Baslik != ""))
                {
                    Cevirici.Append("<tr>");
                    Cevirici.Append("<td>");
                    Cevirici.Append(Baslik);
                    Cevirici.Append("</td>");
                    Cevirici.Append("</tr>");
                }
                Cevirici.Append("<tr>");
                foreach (DataColumn myColumn in CevirliCekTablo.Columns)
                {
                    Cevirici.Append("<td>");
                    Cevirici.Append(myColumn.ColumnName);
                    Cevirici.Append("</td>");
                }
                Cevirici.Append("</tr>");

                foreach (DataRow myRow in CevirliCekTablo.Rows)
                {
                    Cevirici.Append("<tr>");
                    foreach (DataColumn myColumn in CevirliCekTablo.Columns)
                    {
                        Cevirici.Append("<td>");
                        Cevirici.Append(myRow[myColumn.ColumnName].ToString());
                        Cevirici.Append("</td>");
                    }
                    Cevirici.Append("</tr>");
                }
                if ((Altlik != null) && (Altlik != ""))
                {
                    Cevirici.Append("<tr>");
                    Cevirici.Append("<td>");
                    Cevirici.Append(Altlik);
                    Cevirici.Append("</td>");
                    Cevirici.Append("</tr>");
                }
                Cevirici.Append("</table>");
                //Cevirici.Append("</body>");
                //Cevirici.Append("</html>");
                return Cevirici.ToString();
            }
            return "";
        }


        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
