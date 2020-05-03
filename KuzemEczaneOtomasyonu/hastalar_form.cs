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
    public partial class hastalar_form : Form
    {
        public hastalar_form()
        {
            InitializeComponent();
        }

        private void Hastalar_form_Load(object sender, EventArgs e)
        {
            get_hastalar();
        }
        public void get_hastalar()
        {
            Veritabani.baglanti.Open();
            string kayit = "SELECT hasta_id AS 'Id',tc_kimlik AS 'TC Kimlik',adi AS 'Adı',soyadi AS 'Soyadı'" +
                ", FORMAT(dogum_tarihi, 'dd.MM.yyyy') AS 'Doğum Tarihi',adres AS Adres,telefon as Telefon,sgk_guvencesi AS 'SGK Güvencesi' FROM tbl_hastalar";
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

        private void Button3_Click(object sender, EventArgs e)
        {
            string id = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            string adi = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            string soyadi = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
            string message = adi + " " + soyadi + " İsimli Hasta Tüm Kayıtlarıyla Birlikte Silinecek Onaylıyor Musunuz ?";
            string caption = "Hasta Sil";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (LoginKullanici.kullanici_adi == "admin")
                    hasta_sil(id);
                else
                    MessageBox.Show("Yetkisiz İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void hasta_sil(string id)
        {
            Veritabani.baglanti.Open();
            string sqlCommandText = "DELETE FROM tbl_hastalar where hasta_id=" + id;
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Veritabani.baglanti);
            sqlCommand.ExecuteNonQuery();
            Veritabani.baglanti.Close();
            MessageBox.Show("Hasta Silindi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            get_hastalar();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            hasta_ekle_duzenle_form frm = new hasta_ekle_duzenle_form();
            frm.Name = "hasta_ekle_form";
            if (Application.OpenForms["hasta_ekle_form"] == null)
            {
                hasta_ekle_duzenle_form.islem_tipi = "yeni";
                frm.Text = "Hasta Ekle";
                frm.Show();
            }
            else
            {
                MessageBox.Show("Hasta Ekle Ekranı Zaten Açık.");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string id = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            if (id != "")
            {
                hasta_ekle_duzenle_form frm = new hasta_ekle_duzenle_form();
                frm.Name = "hasta_duzenle_form";
                if (Application.OpenForms["hasta_duzenle_form"] == null)
                {
                    hasta_ekle_duzenle_form.islem_tipi = "guncelleme";
                    hasta_ekle_duzenle_form.hasta_id = id;
                    frm.Text = "Hasta Güncelleme";
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Hasta Düzenleme Ekranı Zaten Açık.");
                }
            }
            else
            {
                MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
