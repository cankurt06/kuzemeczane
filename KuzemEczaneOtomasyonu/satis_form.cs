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
using System.Configuration;

namespace KuzemEczaneOtomasyonu
{
    public partial class satis_form : Form
    {
        public satis_form()
        {
            InitializeComponent();
        }
        private void satis_form_Load(object sender, EventArgs e)
        {
            satislariGetir();
        }
        private void satislariGetir()
        {
            Veritabani.baglanti.Open();
            string kayit = "SELECT satislar.satis_id AS 'Islem Id',hastalar.tc_kimlik AS 'T.C. Kimlik',CONCAT(hastalar.adi,' ',hastalar.soyadi) AS 'Adı Soyadı',(SELECT COUNT(satis_id) FROM tbl_satislar where satis_id=satislar.satis_id or master_satis=satislar.satis_id) AS 'İlaç Adeti (Kalem Bazlı)',(SELECT SUM(adet) FROM tbl_satislar where satis_id=satislar.satis_id or master_satis=satislar.satis_id) AS 'Toplam İlaç Adeti',CONCAT(odemeler.toplam_tutar,' TL') AS 'Toplam Tutar',odemeler.odeme_tipi AS 'Ödeme Tipi',odemeler.odeme_durumu AS 'Ödeme Durumu',satislar.islem_zamani AS 'İşlem Zamanı',CONCAT(kullanici.kullanici_adi,' ',kullanici.kullanici_soyadi) AS 'İşlemi Yapan Personel' " +
                "FROM tbl_satislar AS satislar LEFT JOIN tbl_ilaclar AS ilac ON satislar.ilac_id=ilac.ilac_id " +
                "LEFT JOIN tbl_hastalar AS hastalar ON satislar.hasta_id=hastalar.hasta_id " +
                "LEFT JOIN tbl_odemeler AS odemeler ON satislar.satis_id=odemeler.satis_id " +
                "LEFT JOIN tbl_kullanicilar AS kullanici ON satislar.kullanici_id=kullanici.kullanici_id " +
                "LEFT JOIN tbl_ilac_fiyatlari AS fiyat ON satislar.ilac_id=fiyat.ilac_id AND fiyat.fiyat_id=(SELECT TOP(1) fiyat_id FROM tbl_ilac_fiyatlari WHERE ilac_id=satislar.ilac_id ORDER BY fiyat_tarihi DESC) WHERE satislar.master_satis IS NULL";
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
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            satis_ekle_duzenle_form frm = new satis_ekle_duzenle_form();
            frm.Name = "satis_ekle_form";
            if (Application.OpenForms["satis_ekle_form"] == null)
            {
                satis_ekle_duzenle_form.islem_tipi = "yeni";
                frm.Text = "Satış Ekle";
                frm.Show();
            }
            else
            {
                MessageBox.Show("Satış Ekleme Ekranı Zaten Açık.");
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                int satir_index = dataGridView1.SelectedCells[0].RowIndex;
                string id = dataGridView1.Rows[satir_index].Cells[0].Value.ToString();
                string message = "Satış Silinecek Onaylıyor Musunuz ?";
                string caption = "Satış Sil";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (LoginKullanici.kullanici_adi == "admin")
                        satis_sil(id);
                    else
                        MessageBox.Show("Yetkisiz İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void satis_sil(string id)
        {
            Veritabani.baglanti.Open();
            string sqlCommandText = "DELETE FROM tbl_satislar where satis_id=" + id;
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Veritabani.baglanti);
            sqlCommand.ExecuteNonQuery();
            Veritabani.baglanti.Close();
            satislariGetir();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                 int satir_index = dataGridView1.SelectedCells[0].RowIndex;
                string id = dataGridView1.Rows[satir_index].Cells[0].Value.ToString();
                satis_ekle_duzenle_form frm = new satis_ekle_duzenle_form();
                frm.Name = "satis_detayi_form";
                if (Application.OpenForms["satis_detayi_form"] == null)
                {
                    satis_ekle_duzenle_form.islem_tipi = "detay";
                    satis_ekle_duzenle_form.satis_id = id;
                    frm.Text = "Satış Detayı";
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Satış Detay Ekranı Zaten Açık.");
                }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string arama = textBox1.Text.ToString();
            if (arama != "")
            {
                Veritabani.baglanti.Open();
                string kayit = "SELECT satislar.satis_id AS 'Islem Id',hastalar.tc_kimlik AS 'T.C. Kimlik',CONCAT(hastalar.adi,' ',hastalar.soyadi) AS 'Adı Soyadı',(SELECT COUNT(satis_id) FROM tbl_satislar where satis_id=satislar.satis_id or master_satis=satislar.satis_id) AS 'İlaç Adeti (Kalem Bazlı)',(SELECT SUM(adet) FROM tbl_satislar where satis_id=satislar.satis_id or master_satis=satislar.satis_id) AS 'Toplam İlaç Adeti',CONCAT(odemeler.toplam_tutar,' TL') AS 'Toplam Tutar',odemeler.odeme_tipi AS 'Ödeme Tipi',odemeler.odeme_durumu AS 'Ödeme Durumu',satislar.islem_zamani AS 'İşlem Zamanı',CONCAT(kullanici.kullanici_adi,' ',kullanici.kullanici_soyadi) AS 'İşlemi Yapan Personel' " +
    "FROM tbl_satislar AS satislar LEFT JOIN tbl_ilaclar AS ilac ON satislar.ilac_id=ilac.ilac_id " +
    "LEFT JOIN tbl_hastalar AS hastalar ON satislar.hasta_id=hastalar.hasta_id " +
    "LEFT JOIN tbl_odemeler AS odemeler ON satislar.satis_id=odemeler.satis_id " +
    "LEFT JOIN tbl_kullanicilar AS kullanici ON satislar.kullanici_id=kullanici.kullanici_id " +
    "LEFT JOIN tbl_ilac_fiyatlari AS fiyat ON satislar.ilac_id=fiyat.ilac_id AND fiyat.fiyat_id=(SELECT TOP(1) fiyat_id FROM tbl_ilac_fiyatlari WHERE ilac_id=satislar.ilac_id ORDER BY fiyat_tarihi DESC) WHERE satislar.master_satis IS NULL AND (hastalar.adi LIKE '%" + arama + "%' OR hastalar.soyadi LIKE '%" + arama + "%' OR hastalar.tc_kimlik LIKE '%" + arama + "%')";//musteriler tablosundaki aramaya göre kayıtları çekecek olan sql sorgusu.
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
                button6.Visible = true;
            }
            else
            {
                MessageBox.Show("Lütfen Aranacak Sözcüğü Yazın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {

            satislariGetir();
            this.button6.Visible = false;
            textBox1.Text = "";
        }
    }
}
