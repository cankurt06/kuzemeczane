using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuzemEczaneOtomasyonu
{
    public partial class ilac_stok_fiyat_bilgisi : Form
    {
        public static int ilac_id { get; set; }
        public ilac_stok_fiyat_bilgisi()
        {
            InitializeComponent();
        }

        private void İlac_stok_fiyat_bilgisi_Load(object sender, EventArgs e)
        {
            Veritabani.baglanti.Open();
            string kayit = "SELECT stok.stok_id AS 'Stok Id',firma.firma_adi AS 'Ecza Depo Firması',stok.gelen_miktar AS 'Gelen Miktar',stok.giden_miktar AS 'Giden Miktar',stok.tarih FROM tbl_ilac_stok AS stok LEFT JOIN tbl_ecza_depo_firmalari AS firma ON firma.ecza_depo_firma_id=stok.ecza_depo_firma_id where stok.ilac_id="+ilac_id;
            //musteriler tablosundaki tüm kayıtları çekecek olan sql sorgusu.
            SqlCommand komut = new SqlCommand(kayit, Veritabani.baglanti);
            //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
            SqlDataAdapter da = new SqlDataAdapter(komut);
            //SqlDataAdapter sınıfı verilerin databaseden aktarılması işlemini gerçekleştirir.
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
            dataGridView1.DataSource = dt;
            //Formumuzdaki DataGridViewin veri kaynağını oluşturduğumuz tablo olarak gösteriyoruz.
            Veritabani.baglanti.Close();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            Veritabani.baglanti.Open();
             kayit = "SELECT perakende_fiyati AS 'Perakende Satış Fiyatı',fiyat_tarihi FROM tbl_ilac_fiyatlari where ilac_id="+ilac_id+" order by fiyat_tarihi DESC";
            //musteriler tablosundaki tüm kayıtları çekecek olan sql sorgusu.
            komut = new SqlCommand(kayit, Veritabani.baglanti);
            //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
            da = new SqlDataAdapter(komut);
            //SqlDataAdapter sınıfı verilerin databaseden aktarılması işlemini gerçekleştirir.
            dt = new DataTable();
            da.Fill(dt);
            //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
            dataGridView2.DataSource = dt;
            //Formumuzdaki DataGridViewin veri kaynağını oluşturduğumuz tablo olarak gösteriyoruz.
            Veritabani.baglanti.Close();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
