using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuzemEczaneOtomasyonu
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }

           public static string decimal_cevir(string sayi)
        {
            if (sayi == "")
            {
                return "";
            }
            string sonuc = Convert.ToDecimal(sayi).ToString();
            string duzgun_sayi = sonuc.Replace(',', '.');
            return duzgun_sayi;
        }
    }
}
