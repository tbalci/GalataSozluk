using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO.Compression;
using System.Reflection;
using System.ComponentModel;
using HtmlAgilityPack;

namespace Tuanna.Lib
{
   public class Araclar:IDisposable
    {

        public HtmlLink CreateFaviconLink(string FilePath)
        {
            var link = new HtmlLink();
            link.Attributes.Add("rel", "shortcut icon");
            link.Href = link.ResolveUrl(FilePath);
            return link;
        }

        public DataTable HtmlTxtToTable(string Html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(Html);
            var nodes = doc.DocumentNode.SelectNodes("//table/tr");
            var table = new DataTable("TBLVERI");
            var headers = nodes[0]
                .Elements("th")
                .Select(th => th.InnerText.Trim());
            foreach (var header in headers)
            {
                table.Columns.Add(header);
            }
            var rows = nodes.Skip(1).Select(tr => tr
                .Elements("td")
                .Select(td => td.InnerText.Trim())
                .ToArray());
            foreach (var row in rows)
            {
                table.Rows.Add(row);
            }
            return table;
        }



        public Dictionary<int, string> TipListesiGetir(Type enumType)
        {
            Array Values = Enum.GetValues(enumType);
            string[] names = Enum.GetNames(enumType);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            for (int i = 0; i <= names.GetUpperBound(0); i++)
                dic.Add(Convert.ToInt32(Enum.Parse(enumType, names[i])), GetDescription((Enum)Enum.Parse(enumType, names[i])));
            return dic;
        }

        public string GetDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string strvalue = string.Empty;
                if (string.IsNullOrEmpty(strvalue))
                    strvalue = (attributes.Length > 0) ? attributes[0].Description : value.ToString();
                return strvalue;
            }
            catch
            {
                return null;
            }
        }


        public HtmlLink CreateCssLink(string cssFilePath, string media)
        {
            var link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Href = link.ResolveUrl(cssFilePath);
            if (string.IsNullOrEmpty(media))
            {
                media = "all";
            }

            link.Attributes.Add("media", media);
            return link;
        }

        public HtmlGenericControl CreateJavaScriptLink(string scriptFilePath)
        {
            var script = new HtmlGenericControl();
            script.TagName = "script";
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", script.ResolveUrl(scriptFilePath));

            return script;
        }

        public HtmlGenericControl CreateJavaScriptLink(string scriptFilePath, string Tip = "")
        {
            var script = new HtmlGenericControl();
            script.TagName = "script";
            if (Tip != "")
            {
                script.Attributes.Add("type", "text/javascript");
            }
            script.Attributes.Add("src", script.ResolveUrl(scriptFilePath));
            return script;
        }

        #region "Wininet Referans ediliyor..."
        [DllImport("wininet.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool InternetGetConnectedState(ref int lpSFlags, int dwReserved);

        #endregion
        static long lngFlags = 0;
        #region "Bağlantı Durumu Sorgulanıyor..."
        public bool BaglantiDurumu()
        {
            int temp_int = (int)lngFlags;
            if (InternetGetConnectedState(ref temp_int, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion 


      

       /// <summary>
       /// Function to get byte array from a file
       /// </summary>
       /// <param name="DosyaAd">File name to get byte array</param>
       /// <returns>Byte Array</returns>
       /// 

       public byte[] DosyadanOku(string DosyaAd)
       {
           byte[] Sonuc = null;
           System.IO.FileStream _FileStream = new System.IO.FileStream(DosyaAd, System.IO.FileMode.Open, System.IO.FileAccess.Read);
           // attach filestream to binary reader
           System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);
           try
           {
               // Open file for reading
               // get total byte length of the file
               long _TotalBytes = new System.IO.FileInfo(DosyaAd).Length;
               // read entire file into buffer
               Sonuc = _BinaryReader.ReadBytes((Int32)_TotalBytes);
               // close file reader
           }
           catch (Exception _Exception)
           {
               Sonuc = null;
           }
           finally
           {
               if (_FileStream!=null)
               {
                   _FileStream.Close();
                   _FileStream.Dispose();    
               }
               if (_BinaryReader!=null)
               {
                   _BinaryReader.Close();
               }
           }

           return Sonuc;
       }
       public byte[] DosyadanOku(Stream Kaynak)
       {
           byte[] Sonuc = null;
           System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(Kaynak);
           try
           {
               long _TotalBytes = Kaynak.Length;
               Sonuc = _BinaryReader.ReadBytes((Int32)_TotalBytes);
           }
           catch (Exception Hata)
           {
               Sonuc = null;
           }
           finally
           {
               if (_BinaryReader != null)
               {
                   _BinaryReader.Close();
               }
           }

           return Sonuc;
       }

     
       public  DataSet TextDataSeteCevir(string Veri)
       {
           StringReader objReader = new StringReader(Veri);
           DataSet dsData = new DataSet();
           dsData.ReadXml(objReader);
           return dsData;
       }


       public string DataSetiTextCevir(DataSet KaynakData)
       {
           MemoryStream objStream = new MemoryStream();
           KaynakData.WriteXml(objStream);
           XmlTextWriter objXmlWriter = new XmlTextWriter(objStream, Encoding.UTF8);
           objStream = (MemoryStream)objXmlWriter.BaseStream;
           UTF8Encoding objEncoding = new UTF8Encoding();
           return objEncoding.GetString(objStream.ToArray());
       }

       public string TurkceKarakter(string text)
       {
           text = text.Replace("İ", "\u0130");
           text = text.Replace("ı", "\u0131");
           text = text.Replace("Ş", "\u015e");
           text = text.Replace("ş", "\u015f");
           text = text.Replace("Ğ", "\u011e");
           text = text.Replace("ğ", "\u011f");
           text = text.Replace("Ö", "\u00d6");
           text = text.Replace("ö", "\u00f6");
           text = text.Replace("ç", "\u00e7");
           text = text.Replace("Ç", "\u00c7");
           text = text.Replace("ü", "\u00fc");
           text = text.Replace("Ü", "\u00dc");
           return text;

       }
       public int RastgeleSayiVer(int AltLimit, int UstLimit)
       {
           Random random = new Random();
           return random.Next(AltLimit, UstLimit);
       }
       public int RastgeleSayiVer(int AltLimit, int UstLimit, List<int> Liste)
       {
           Random random = new Random();
           int Sayi = -1;
           while (!Liste.Contains(Sayi))
           {
               Sayi = random.Next(AltLimit, UstLimit);
           }
           return Sayi;
       }
       public int RastgeleSayiVer()
       {
           Random random = new Random();
           return random.Next(0,100000000);
       }
       public int RastgeleSayiVer(List<int> Liste)
       {
           Random random = new Random();
           int Sayi = -1;
           while (!Liste.Contains(Sayi))
           {
               Sayi=random.Next(0, 100000000);
           }
           return Sayi;
       }
       /// <summary>
       /// Converts an image into an icon.
       /// </summary>
       /// <param name="img">The image that shall become an icon</param>
       /// <param name="size">The width and height of the icon. Standard
       /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
       /// <param name="keepAspectRatio">Whether the image should be squashed into a
       /// square or whether whitespace should be put around it.</param>
       /// <returns>An icon!!</returns>
       /// private int RandomNumber(int min, int max)
        public Icon MakeIcon(System.Drawing.Image img, int size, bool keepAspectRatio)
        {
            if (img==null)
            {
                return null;
            }
            Bitmap square = new Bitmap(size, size); // create new bitmap
            Graphics g = Graphics.FromImage(square); // allow drawing to it

            int x, y, w, h; // dimensions for new image

            if (!keepAspectRatio || img.Height == img.Width)
            {
                // just fill the square
                x = y = 0; // set x and y to 0
                w = h = size; // set width and height to size
            }
            else
            {
                // work out the aspect ratio
                float r = (float)img.Width / (float)img.Height;

                // set dimensions accordingly to fit inside size^2 square
                if (r > 1)
                { // w is bigger, so divide h by r
                    w = size;
                    h = (int)((float)size / r);
                    x = 0; y = (size - h) / 2; // center the image
                }
                else
                { // h is bigger, so multiply w by r
                    w = (int)((float)size * r);
                    h = size;
                    y = 0; x = (size - w) / 2; // center the image
                }
            }

            // make the image shrink nicely by using HighQualityBicubic mode
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
            g.Flush(); // make sure all drawing operations complete before we get the icon

            // following line would work directly on any image, but then
            // it wouldn't look as nice.
            return Icon.FromHandle(square.GetHicon());
        }

       public Icon ImageToIcon(System.Drawing.Image Resim,Size Boyut,System.Drawing.Imaging.ImageFormat Format)
       {

           Icon Sonuc = null;
           try
           {
               using (MemoryStream ms = new MemoryStream(ImageToByte(Resim,Format)))
               {
                   Sonuc = new Icon(ms, Boyut.Width,Boyut.Height);
               }
           }
           catch
           {
               Sonuc = null;
           }
           return Sonuc;
       }

       public string ReWriterPath(string Kategori,string No, string Baslik)
       {
           string Temp = "";
           Temp = Baslik.ToLower();
           Temp = Temp.Replace("-", ""); Temp = Temp.Replace(" ", "-");
           Temp = Temp.Replace("ç", "c"); Temp = Temp.Replace("ğ", "g");
           Temp = Temp.Replace("ı", "i"); Temp = Temp.Replace("ö", "o");
           Temp = Temp.Replace("ş", "s"); Temp = Temp.Replace("ü", "u");
           Temp = Temp.Replace("\"", ""); Temp = Temp.Replace("/", "");
           Temp = Temp.Replace("(", ""); Temp = Temp.Replace(")", "");
           Temp = Temp.Replace("{", ""); Temp = Temp.Replace("}", "");
           Temp = Temp.Replace("%", ""); Temp = Temp.Replace("&", "");
           Temp = Temp.Replace("+", ""); Temp = Temp.Replace(".", "-");
           Temp = Temp.Replace("?", ""); Temp = Temp.Replace(",", "");
           Temp = Temp.Replace("'", ""); Temp = Temp.Replace(":", "");
           Temp = Temp.Replace("#39", ""); Temp = Temp.Replace(":", "");
           return  "~/" + No + "-" + Temp + ".aspx";
       }

        public IslemDurum DosyayaKaydet(string Dosya, byte[] Veri)
        {
            IslemDurum Sonuc = new IslemDurum(false, "");
            try
            {
                System.IO.FileStream Dosyaci = new System.IO.FileStream(Dosya, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                Dosyaci.Write(Veri, 0, Veri.Length);
                Dosyaci.Close();
                Sonuc = new IslemDurum(true, "Başarılı olarak oluşturuldu");
            }
            catch (Exception Hata)
            {
                Sonuc = new IslemDurum(false, "Dosya oluşturma işleminde hata oluştu..!Ayrıntılar :\n"+Hata.Message.ToString());
            }
            return Sonuc;
        }
        public  void TabloyaSatirEkle(ref HtmlTable Tablo)
        {
            HtmlTableRow YeniSatir = new HtmlTableRow();
            if(Tablo.Rows.Count > 0)
            {

                for(int Don = 0; Don < Tablo.Rows[Tablo.Rows.Count - 1].Cells.Count; Don++)
                {
                    YeniSatir.Cells.Add(new HtmlTableCell());
                }
            }
            else
            {
                YeniSatir.Cells.Add(new HtmlTableCell());
            }
            Tablo.Rows.Add(YeniSatir);
        }

        public  void TabloyaSatirEkle(ref HtmlTable Tablo, int SatirSayisi)
        {
            for(int Don = 0; Don < SatirSayisi; Don++)
            {
                TabloyaSatirEkle(ref Tablo);
            }
        }
        public  void TabloyaYeniHucreEkle(ref HtmlTable Tablo, int SatirNo, int EklenecekHucreSayisi, bool NoWrap)
        {
            if(SatirNo < Tablo.Rows.Count)
            {
                for(int Don = 0; Don < EklenecekHucreSayisi; Don++)
                {
                    HtmlTableCell YeniHucre = new HtmlTableCell();
                    YeniHucre.NoWrap = NoWrap;
                    Tablo.Rows[SatirNo].Cells.Add(YeniHucre);
                }
            }
        }

        public  void TabloyaSatırAcVeriEkle(ref HtmlTable Tablo, string[] Veri, bool NoWrap)
        {
            TabloyaSatirEkle(ref Tablo, 1);
            TabloyaYeniHucreEkle(ref Tablo, Tablo.Rows.Count - 1, Veri.Length, true);
            for(int Don = 0; Don < Veri.Length; Don++)
            {
                Label LblVeri = new Label();
                LblVeri.Text = Veri[Don];
                LblVeri.Width = 200;
                Tablo.Rows[Tablo.Rows.Count - 1].Cells[Don].NoWrap = true;
                Tablo.Rows[Tablo.Rows.Count - 1].Cells[Don].Controls.Add(LblVeri);
            }
        }

        public  void TabloyaSatırAcVeriEkle(ref HtmlTable Tablo, System.Web.UI.Control[] Veri, bool NoWrap)
        {
            TabloyaSatirEkle(ref Tablo, 1);
            TabloyaYeniHucreEkle(ref Tablo, Tablo.Rows.Count - 1, Veri.Length, true);
            for(int Don = 0; Don < Veri.Length; Don++)
            {
                Tablo.Rows[Tablo.Rows.Count - 1].Cells[Don].NoWrap = true;
                Tablo.Rows[Tablo.Rows.Count - 1].Cells[Don].Controls.Add(Veri[Don]);
            }
        }

        public byte[] ResimiYenidenBoyutlandir(byte[] Image, int Boyut)
        {
            //ReSizeImage function um resmi ve resim'e vermek istediğim boyutu alıyor.
            //Resmimin orjinal halini editlemek için MemoryStream nesnesine atıyorum.
            using(System.Drawing.Image eskiResim = System.Drawing.Image.FromStream(new MemoryStream(Image)))
            {
                //Resmin boyutlarını hesaplamak için ise ResimBoyutHesapla Functionu içerisine Orjinal resminin
                //boyutunu ve vermek istediğim yeni boyutu gönderek yeni boyutları geri alıyorum.(bkz.ResimBoyutHesapla func.)
                Size YeniBoyut = ResimBoyutHesapla(eskiResim.Size, Boyut);
                using(Bitmap YeniImage = new Bitmap(YeniBoyut.Width, YeniBoyut.Height, PixelFormat.Format24bppRgb))
                {
                    using(Graphics gps = Graphics.FromImage(YeniImage))
                    {
                        //Şimdi elimde boyutları yeniden oluşturulmuş resmim var.Graphics nesnesine yeni
                        //resmimi verek bağzı özelliklerini veriyorum .bu şekilde resmim görünüm olarakta
                        //bozulmadan geri dönüyor..
                        gps.SmoothingMode = SmoothingMode.AntiAlias;
                        gps.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gps.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gps.DrawImage(eskiResim, new System.Drawing.Rectangle(new Point(0, 0), YeniBoyut));
                        MemoryStream MyStream = new MemoryStream();
                        YeniImage.Save(MyStream, ImageFormat.Jpeg);
                        return MyStream.GetBuffer();
                    }

                }
            }
        }
        public Size ResimBoyutHesapla(Size EskiBoyut, int Boyut)
        {
            //Eğer orjinal resmimin yüksekliği genişliğinden büyükse ,yeni oluşturucağım resmimin yüksekliğini
            //ResimEkle functionumda verdiğim boyuta eşitliyorum.Genişliği ise birkaç işlemle tekrardan hesaplıyorum.
            //Tersi durumda ise busefer genişliği ResimEkle functionumda verdiğim boyutla direkt olarak eşitliyorum.
            Size YeniBoyut = new Size();
            if(EskiBoyut.Height > EskiBoyut.Width)
            {
                YeniBoyut.Width = (int)(EskiBoyut.Width * ((float)Boyut / (float)EskiBoyut.Height));
                YeniBoyut.Height = Boyut;
            }
            else
            {
                YeniBoyut.Width = Boyut;
                YeniBoyut.Height = (int)(EskiBoyut.Height * ((float)Boyut / (float)EskiBoyut.Width));
            }
            //Bu işlermler bittikten sonra resmimin yeni boyutlarını ReSizeImage Functionuma geri gönderiyorum.
            return YeniBoyut;
        }

        public byte[] ImageToBytes(string Dosya, string[] DosyaTipi)
        {
            bool DosyaTipiDogrumu = false;
            try
            {
                FileInfo file = new FileInfo(Dosya);
                foreach(string Tip in DosyaTipi)
                {
                    if(Tip == file.Extension)
                    {
                        DosyaTipiDogrumu = true;
                        break;
                    }
                }
                if(DosyaTipiDogrumu)
                {
                    FileStream fs = new FileStream(Dosya, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] image = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();
                    return image;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public byte[] ImageToByte(System.Drawing.Image Resim, ImageFormat ResimFormat)
        {
            byte[] Sonuc;
            try
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    Resim.Save(ms, ResimFormat);
                    Sonuc = ms.ToArray();
                }
            }
            catch(Exception)
            {
                return null;
            }
            return Sonuc;
        }

        public byte[] StringToByte(string Komut)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(Komut);
        }

        public string ByteToString(byte[] Veri)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            return enc.GetString(Veri);
        }


        public string MakineIdGetir()
        {
            string Sonuc="";
            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Length>0)
                {
                    Sonuc ="TUA-"+System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].Id.ToString().Replace("}","").Replace("{","");
                }
                else
                {
                    Sonuc = "Urun Kodu Yok";
                }
            }
            catch
            {
                Sonuc = "";
            }
            return Sonuc;
        }
        //public byte[] Compress(byte[] buffer)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
        //    zip.Write(buffer, 0, buffer.Length);
        //    zip.Close();
        //    ms.Position = 0;

        //    MemoryStream outStream = new MemoryStream();

        //    byte[] compressed = new byte[ms.Length];
        //    ms.Read(compressed, 0, compressed.Length);

        //    byte[] gzBuffer = new byte[compressed.Length + 4];
        //    Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
        //    Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
        //    return gzBuffer;
        //}

        //public byte[] Decompress(byte[] gzBuffer)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    int msgLength = BitConverter.ToInt32(gzBuffer, 0);
        //    ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

        //    byte[] buffer = new byte[msgLength];

        //    ms.Position = 0;
        //    GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
        //    zip.Read(buffer, 0, buffer.Length);

        //    return buffer;
        //}

        public System.Drawing.Image ByteToImage(byte[] ByteResim)
        {
            System.Drawing.Image SonucResim;
            using(MemoryStream ms = new MemoryStream(ByteResim, 0, ByteResim.Length))
            {
                ms.Write(ByteResim, 0, ByteResim.Length);
                SonucResim = System.Drawing.Image.FromStream(ms, true);
            }
            return SonucResim;
        }

        public IslemDurum DizinSil(string DizinYol)
        {
            IslemDurum YeniIslemDurumu = new IslemDurum();
            if((DizinYol != null) && (DizinYol != ""))
            {
                try
                {
                    DirectoryInfo YeniDizinBilgisi = new DirectoryInfo(DizinYol);
                    if(YeniDizinBilgisi.Exists)
                    {
                        YeniDizinBilgisi.Delete();
                        YeniIslemDurumu.Durum = true;
                        YeniIslemDurumu.Mesaj = "Dizin Silinirken İle Oluşturuldu..!";
                        return YeniIslemDurumu;
                    }
                    else
                    {
                        YeniIslemDurumu.Durum = false;
                        YeniIslemDurumu.Mesaj = "Silinecek Bir Dizin Bulunamadı..!";
                        return YeniIslemDurumu;
                    }
                }
                catch(Exception Hata)
                {
                    YeniIslemDurumu.Durum = false;
                    YeniIslemDurumu.Mesaj = "Dizin Silinirken Hata Oluştu..! Ayrıntılar : " + Hata.Message.ToString();
                    return YeniIslemDurumu;
                }
            }
            return YeniIslemDurumu;
        }
        public IslemDurum DosyaSil(string DosyaYol)
        {
            IslemDurum YeniIslemDurumu = new IslemDurum();
            if((DosyaYol != null) && (DosyaYol != ""))
            {
                try
                {
                    FileInfo YeniDosyaBilgisi = new FileInfo(DosyaYol);
                    if(YeniDosyaBilgisi.Exists)
                    {
                        YeniDosyaBilgisi.Delete();
                        YeniIslemDurumu.Durum = true;
                        YeniIslemDurumu.Mesaj = "Dosya Silinirken İle Oluşturuldu..!";
                        return YeniIslemDurumu;
                    }
                    else
                    {
                        YeniIslemDurumu.Durum = false;
                        YeniIslemDurumu.Mesaj = "Silinecek Bir Dosya Bulunamadı..!";
                        return YeniIslemDurumu;
                    }
                }
                catch(Exception Hata)
                {
                    YeniIslemDurumu.Durum = false;
                    YeniIslemDurumu.Mesaj = "Dosya Silinirken Hata Oluştu..! Ayrıntılar : " + Hata.Message.ToString();
                    return YeniIslemDurumu;
                }
            }
            return YeniIslemDurumu;
        }
        public IslemDurum DizinOlustur(string DizinYol)
        {
            IslemDurum YeniIslemDurumu = new IslemDurum();
            if((DizinYol != null) && (DizinYol != ""))
            {
                try
                {
                    DirectoryInfo YeniDizinBilgisi = new DirectoryInfo(DizinYol);
                    if(!YeniDizinBilgisi.Exists)
                    {
                        YeniDizinBilgisi.Create();
                        YeniIslemDurumu.Durum = true;
                        YeniIslemDurumu.Mesaj = "Dizin Başarı İle Oluşturuldu..!";
                        return YeniIslemDurumu;
                    }
                    else
                    {
                        YeniIslemDurumu.Durum = false;
                        YeniIslemDurumu.Mesaj = "Dizin Daha Önceden Oluşturulmuş..!";
                        return YeniIslemDurumu;
                    }
                }
                catch(Exception Hata)
                {
                    YeniIslemDurumu.Durum = false;
                    YeniIslemDurumu.Mesaj = "Dizin Oluşturulurken Hata Oluştu..! Ayrıntılar : " + Hata.Message.ToString();
                    return YeniIslemDurumu;
                }
            }
            return YeniIslemDurumu;

        }
        private string[,] HarfDonusumTablosu = new string[,] 
        {
        {"a","a"},
        {"b","b"},
        {"c","c"},
        {"ç","c"},
        {"d","d"},
        {"e","e"},
        {"f","f"},
        {"g","g"},
        {"ğ","g"},
        {"h","h"},
        {"ı","ı"},
        {"i","ı"},
        {"j","j"},
        {"k","k"},
        {"l","l"},
        {"m","m"},
        {"n","n"},
        {"o","o"},
        {"ö","o"},
        {"p","p"},
        {"r","r"},
        {"s","s"},
        {"ş","s"},
        {"t","t"},
        {"u","u"},
        {"ü","ü"},
        {"v","v"},
        {"y","y"},
        {"z","z"},
        {"A","A"},
        {"B","B"},
        {"C","C"},
        {"Ç","C"},
        {"D","D"},
        {"E","E"},
        {"F","F"},
        {"G","G"},
        {"Ğ","G"},
        {"H","H"},
        {"I","I"},
        {"I","I"},
        {"J","J"},
        {"K","K"},
        {"L","L"},
        {"M","M"},
        {"N","N"},
        {"O","O"},
        {"Ö","O"},
        {"P","P"},
        {"R","R"},
        {"S","S"},
        {"Ş","S"},
        {"T","T"},
        {"U","U"},
        {"Ü","Ü"},
        {"V","V"},
        {"Y","Y"},
        {"Z","Z"},
        {"1","1"},
        {"2","2"},
        {"3","3"},
        {"4","4"},
        {"5","5"},
        {"6","6"},
        {"7","7"},
        {"8","8"},
        {"9","9"},
        {"0","0"},
        {",",","},
        {".","."},
        {"?","?"},
        {"!","!"},
        {"~","~"},
        };

        public string TrkToIng(string Text)
        {
            string Sonuc = "";
            char[] KarakterDizisi = Text.ToCharArray();
            foreach(char Harf in KarakterDizisi)
            {
                string SonucHarf = "";
                int keyCount = HarfDonusumTablosu.GetLength(0);
                string AraHarf = Harf.ToString();
                for(int i = 0; i < keyCount; i++)
                {
                    if(HarfDonusumTablosu[i, 0].Equals(AraHarf))
                    {
                        SonucHarf = (string)HarfDonusumTablosu[i, 1];
                    }
                }
                Sonuc += SonucHarf;
            }
            return Sonuc;
        }


        private TreeNode AltDiziniNodeSeklindeGetir(TreeNode ParentNode, string Dizin, bool DizinSecim, bool DosyaSecim)
        {
            TreeNode Node = new TreeNode();
            DirectoryInfo YeniDizinBilgi = new DirectoryInfo(Dizin);
            Node.Text = YeniDizinBilgi.Name.ToString();
            Node.Selected = DizinSecim;
            Int32 Deger = Convert.ToInt32(ParentNode.ToolTip);
            Deger++;
            Node.ToolTip = Deger.ToString();
            Node.Value = ParentNode.Value + "/" + YeniDizinBilgi.Name.ToString();
            foreach (FileInfo AktifDosya in YeniDizinBilgi.GetFiles())
            {
                TreeNode YeniNode = new TreeNode();
                YeniNode.Text = AktifDosya.Name.ToString();
                Node.ChildNodes.Add(YeniNode);
                YeniNode.Selected = DosyaSecim;
                YeniNode.Value = YeniNode.Parent.Value + "/" + AktifDosya.Name.ToString();
                YeniNode.ToolTip = Deger.ToString();
            }
            foreach (DirectoryInfo AktifDizin in YeniDizinBilgi.GetDirectories())
            {
                Node.ChildNodes.Add(AltDiziniNodeSeklindeGetir(Node, AktifDizin.FullName, DizinSecim, DosyaSecim));
            }
            return Node;
        }
        public TreeNode DiziniNodeSeklindeGetir(string Dizin, bool DizinSecim, bool DosyaSecim)
        {

            DirectoryInfo YeniDizinBilgi = new DirectoryInfo(Dizin);
            TreeNode Node = new TreeNode();
            Node.Text = "Ana Dizin - Dosyalar";
            Node.Value = YeniDizinBilgi.Parent.Name.ToString();
            Node.Selected = DizinSecim;
            Node.ToolTip = "0";
            Node.ChildNodes.Add(AltDiziniNodeSeklindeGetir(Node, Dizin, DizinSecim, DosyaSecim));
            return Node;
        }
        private void AltTreeSimileEt(ref DataTable Tablo, TreeNode Node, string IdSutun, string TextAlan, string LevelAd, string SecimSutun)
        {
            foreach (TreeNode Nd in Node.ChildNodes)
            {
                DataRow YeniSatir = Tablo.NewRow();
                YeniSatir[IdSutun] = Nd.Value;
                YeniSatir[TextAlan] = Nd.Text;
                YeniSatir[LevelAd] = Nd.ToolTip;
                YeniSatir[SecimSutun] = Nd.Selected;
                Tablo.Rows.Add(YeniSatir);
                AltTreeSimileEt(ref Tablo, Nd, IdSutun, TextAlan, LevelAd, SecimSutun);
            }
        }

        public DataTable DiziniTabloSeklindeGetir(string Dizin, bool DizinSecim, bool DosyaSecim)
        {
            DataTable Sonuc = new DataTable("Dizin Listesi");
            try
            {
                DataColumn CLMDEGER = new DataColumn("CLMDEGER");
                DataColumn CLMTEXT = new DataColumn("CLMTEXT");
                DataColumn CLMUST = new DataColumn("CLMUST");
                DataColumn CLMSEC = new DataColumn("CLMSEC");
                Sonuc.Columns.Add(CLMDEGER);
                Sonuc.Columns.Add(CLMTEXT);
                Sonuc.Columns.Add(CLMUST);
                Sonuc.Columns.Add(CLMSEC);


                TreeNode DizinNode = DiziniNodeSeklindeGetir(Dizin, DizinSecim, DosyaSecim);
                foreach (TreeNode Nd in DizinNode.ChildNodes)
                {
                    DataRow YeniSatir = Sonuc.NewRow();
                    YeniSatir["CLMDEGER"] = Nd.Value;
                    YeniSatir["CLMTEXT"] = Nd.Text;
                    YeniSatir["CLMUST"] = 0;
                    YeniSatir["CLMSEC"] = Nd.Selected;
                    Sonuc.Rows.Add(YeniSatir);
                    AltTreeSimileEt(ref Sonuc, Nd, "CLMDEGER", "CLMTEXT", "CLMUST", "CLMSEC");
                }
            }
            catch
            {
                Sonuc = new DataTable();
                DataColumn CLMDEGER = new DataColumn("CLMDEGER");
                DataColumn CLMTEXT = new DataColumn("CLMTEXT");
                DataColumn CLMUST = new DataColumn("CLMUST");
                DataColumn CLMSEC = new DataColumn("CLMSEC");
                Sonuc.Columns.Add(CLMDEGER);
                Sonuc.Columns.Add(CLMTEXT);
                Sonuc.Columns.Add(CLMUST);
                Sonuc.Columns.Add(CLMSEC);
            }
            return Sonuc;
        }

        public System.Drawing.Color[] RenkListesiVer()
        {
            return new System.Drawing.Color[] { System.Drawing.Color.Red, System.Drawing.Color.Brown, System.Drawing.Color.Blue, System.Drawing.Color.Yellow, System.Drawing.Color.Green, System.Drawing.Color.DarkCyan, System.Drawing.Color.DarkGreen, System.Drawing.Color.DarkSalmon, System.Drawing.Color.DeepPink, System.Drawing.Color.DarkGreen, System.Drawing.Color.DarkGoldenrod, System.Drawing.Color.DarkBlue, System.Drawing.Color.DarkMagenta };
        }
        public System.Drawing.Color RastGeleRenkVer()
        {
            System.Drawing.Color[] RenkListesi = RenkListesiVer();
            return RenkListesi[RastGeleSayiVer(0,RenkListesi.Length-1)];
        }
        public int RastGeleSayiVer(int Bas,int Bit)
        {
            Random Sayi = new Random();
            int RnDSayi = Sayi.Next(Bas,Bit);
            return RnDSayi;
        }

        #region IDisposable Members
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
