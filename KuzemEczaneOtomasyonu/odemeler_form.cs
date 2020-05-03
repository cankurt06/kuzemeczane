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
    public partial class odemeler_form : Form
    {
        public odemeler_form()
        {
            InitializeComponent();
        }

        private void Odemeler_form_Load(object sender, EventArgs e)
        {
            odemeler_getir();
        }

        public void odemeler_getir()
        {

            Veritabani.baglanti.Open();
            string kayit = "SELECT odeme.odeme_id AS 'Ödeme Id',odeme.satis_id AS 'Satış Id',odeme.odeme_tipi AS 'Ödeme Tipi'," +
                "CONCAT(odeme.toplam_tutar,' TL') AS 'Toplam Tutar',odeme.odeme_durumu AS 'Ödeme Durumu',odeme.islem_zamani AS 'İşlem Zamanı'," +
                "CONCAT(kullanici.kullanici_adi,' ',kullanici.kullanici_soyadi) AS 'İşlem Yapan Personel' FROM tbl_odemeler AS odeme LEFT JOIN tbl_kullanicilar AS kullanici ON odeme.kullanici_id=kullanici.kullanici_id";
            //musteriler tablosundaki tüm kayıtları çekecek olan sql sorgusu.
            SqlCommand komut = new SqlCommand(kayit, Veritabani.baglanti);
            //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
            SqlDataAdapter da = new SqlDataAdapter(komut);
            //SqlDataAdapter sınıfı verilerin databaseden aktarılması işlemini gerçekleştirir.
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
            dataGridView2.DataSource = dt;
            //Formumuzdaki DataGridViewin veri kaynağını oluşturduğumuz tablo olarak gösteriyoruz.
            Veritabani.baglanti.Close();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int satir_index = dataGridView2.SelectedCells[1].RowIndex;
                string id = dataGridView2.Rows[satir_index].Cells[1].Value.ToString();
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

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                int satir_index = dataGridView2.SelectedCells[0].RowIndex;
                string id = dataGridView2.Rows[satir_index].Cells[0].Value.ToString();
                string message = "Ödeme Silinecek Onaylıyor Musunuz ?";
                string caption = "Ödeme Sil";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (LoginKullanici.kullanici_adi == "admin")
                        odeme_sil(id);
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
        private void odeme_sil(string id)
        {
            Veritabani.baglanti.Open();
            string sqlCommandText = "DELETE FROM tbl_odemeler where odeme_id=" + id;
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Veritabani.baglanti);
            sqlCommand.ExecuteNonQuery();
            Veritabani.baglanti.Close();
            odemeler_getir();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                int satir_index = dataGridView2.SelectedCells[0].RowIndex;
                string id = dataGridView2.Rows[satir_index].Cells[0].Value.ToString();
                odeme_duzenle_form frm = new odeme_duzenle_form();
                frm.Name = "odeme_duzenle_form";
                if (Application.OpenForms["odeme_duzenle_form"] == null)
                {
                    odeme_duzenle_form.odeme_id = Convert.ToInt32(id);
                    frm.Text = "Ödeme Düzenle";
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Ödeme Düzenle Ekranı Zaten Açık.");
                }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
