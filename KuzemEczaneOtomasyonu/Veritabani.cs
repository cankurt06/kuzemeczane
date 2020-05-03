using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuzemEczaneOtomasyonu
{
   
    class Veritabani
    {
        static string conString = Properties.Settings.Default.KUZEM_ECZANE_OTO_ConnectionString;
        //Bu veritabanına bağlanmak için gerekli olan bağlantı cümlemiz.app configten alıyor
        public static SqlConnection baglanti = new SqlConnection(conString);
    }
}
