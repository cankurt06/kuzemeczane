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
    public partial class kullanicilar_form : Form
    {
        public kullanicilar_form()
        {
            InitializeComponent();
        }

        private void Kullanicilar_Load(object sender, EventArgs e)
        {
            get_kullanicilar();
        }
        public void get_kullanicilar()
        {
                Veritabani.baglanti.Open();
                string kayit = "SELECT kullanici_id AS 'Id',kullanici_girisadi AS 'Kullanıcı Adı',kullanici_adi AS 'Adı',kullanici_soyadi AS 'Soyadı' FROM tbl_kullanicilar";
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
            string message = adi+" "+soyadi+" Kullanıcısı Silinecek Onaylıyor Musunuz ?";
            string caption = "Kullanıcı Sil";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (LoginKullanici.kullanici_adi == "admin")
                    kullanici_sil(id);
                else if(LoginKullanici.kullanici_id== Convert.ToInt32(id))
                    MessageBox.Show("Kendi Kullanıcınızı Silemezsiniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Yetkisiz İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void kullanici_sil(string id)
        {
            Veritabani.baglanti.Open();
            string sqlCommandText = "DELETE FROM tbl_kullanicilar where kullanici_id="+id;
            SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Veritabani.baglanti);
            sqlCommand.ExecuteNonQuery();
            Veritabani.baglanti.Close();
            get_kullanicilar();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            kullanici_ekle_duzenle_form frm = new kullanici_ekle_duzenle_form();
            frm.Name = "kullanici_ekle_form";
            if (Application.OpenForms["kullanici_ekle_form"] == null)
            {
                kullanici_ekle_duzenle_form.islem_tipi = "yeni";
                frm.Text = "Kullanıcı Ekle";
                frm.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı Ekle Ekranı Zaten Açık.");
            }
          
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string id = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
            if(id!="")
            {
                kullanici_ekle_duzenle_form frm = new kullanici_ekle_duzenle_form();
                frm.Name = "kullanici_duzenle_form";
                if (Application.OpenForms["kullanici_duzenle_form"] == null)
                {
                    kullanici_ekle_duzenle_form.islem_tipi = "guncelleme";
                    kullanici_ekle_duzenle_form.kullanici_id = id;
                    frm.Text = "Kullanıcı Güncelleme";
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Düzenleme Ekranı Zaten Açık.");
                }            }
            else
            {
                MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
