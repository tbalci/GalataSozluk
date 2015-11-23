using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace Tuanna.Lib
{
    public class Dosya:IDisposable
    {
        public FileInfo DosyaBilgisiVer(string DosyaYolu)
        {
          return new System.IO.FileInfo(DosyaYolu);
        }
        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
