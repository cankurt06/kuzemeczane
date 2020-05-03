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
    public partial class ilaclar_form : Form
    {
        public ilaclar_form()
        {
            InitializeComponent();
        }
        private void İlaclar_form_Load(object sender, EventArgs e)
        {
            ilaclariGetir();
        }
        private void ilaclariGetir()
        {
            Veritabani.baglanti.Open();
            string kayit = "SELECT ilac.ilac_id AS 'İlaç Id',ilac.barkod_no AS 'İlaç Barkodu',ilac.ilac_adi AS 'İlaç Adı',firma.firma_adi AS 'İlaç Üretici Firma',(SELECT SUM(gelen_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id) AS 'Toplam Giren Stok',(SELECT SUM(gelen_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-(SELECT SUM(giden_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-IIF((SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id ) IS NULL,0,(SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id )) AS 'Kalan Stok', " +
                "ilac.kdv_orani AS 'Kdv Oranı %',ilac.kurum_iskontosu AS 'Kurum İskonto Oranı',ilac.eczaci_kari AS 'Eczacı Karı' " +
                "from tbl_ilaclar AS ilac LEFT JOIN tbl_ilac_firmalari AS firma ON firma.firma_id=ilac.firma_id";
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
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            string arama = textBox1.Text.ToString();
            if(arama!="")
            {
                Veritabani.baglanti.Open();
                string kayit = "SELECT ilac.ilac_id AS 'İlaç Id',ilac.barkod_no AS 'İlaç Barkodu',ilac.ilac_adi AS 'İlaç Adı',firma.firma_adi AS 'İlaç Üretici Firma',(SELECT SUM(gelen_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id) AS 'Toplam Giren Stok',(SELECT SUM(gelen_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-(SELECT SUM(giden_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-IIF((SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id ) IS NULL,0,(SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id )) AS 'Kalan Stok', " +
                "ilac.kdv_orani AS 'Kdv Oranı %',ilac.kurum_iskontosu AS 'Kurum İskonto Oranı',ilac.eczaci_kari AS 'Eczacı Karı' " +
                "from tbl_ilaclar AS ilac LEFT JOIN tbl_ilac_firmalari AS firma ON firma.firma_id=ilac.firma_id where ilac.ilac_adi like '%" + arama + "%' or ilac.barkod_no like '%" + arama + "%'";
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
                button6.Visible = true;
            }
            else
            {
                MessageBox.Show("Lütfen Aranacak Sözcüğü Yazın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            ilaclariGetir();
            this.button6.Visible = false;
            textBox1.Text = "";
        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    if (id != "")
                    {
                        ilac_fiyat_ekle frm = new ilac_fiyat_ekle();
                        frm.Name = "ilac_fiyat_ekle";
                        if (Application.OpenForms["ilac_fiyat_ekle"] == null)
                        {
                            ilac_fiyat_ekle.ilac_id = id;
                            frm.Text = "İlaç Fiyat Ekleme";
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("İlaç Fiyat Ekleme Ekranı Zaten Açık.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
              string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        if (id != "")
                        {
                            ilac_stok_ekle frm = new ilac_stok_ekle();
                            frm.Name = "ilac_stok_ekle";
                            if (Application.OpenForms["ilac_stok_ekle"] == null)
                            {
                                ilac_stok_ekle.ilac_id = id;
                                frm.Text = "İlaç Stok Ekleme";
                                frm.Show();
                            }
                            else
                            {
                                MessageBox.Show("İlaç Stok Ekleme Ekranı Zaten Açık.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {

                int satir_index = dataGridView1.SelectedCells[0].RowIndex;
                string id = dataGridView1.Rows[satir_index].Cells[0].Value.ToString();
                string message = "İlaç Silinecek Onaylıyor Musunuz ?";
                string caption = "İlaç Sil";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (LoginKullanici.kullanici_adi == "admin")
                    {
                        Veritabani.baglanti.Open();
                        string sqlCommandText = "DELETE FROM tbl_ilaclar where ilac_id=" + id;
                        SqlCommand sqlCommand = new SqlCommand(sqlCommandText, Veritabani.baglanti);
                        sqlCommand.ExecuteNonQuery();
                        Veritabani.baglanti.Close();
                    }
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
                MessageBox.Show("Lütfen Satır Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                 string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                if (id != "")
                {
                    ilac_ekle_duzenle_form frm = new ilac_ekle_duzenle_form();
                    frm.Name = "ilac_duzenle_form";
                    if (Application.OpenForms["ilac_duzenle_form"] == null)
                    {
                        ilac_ekle_duzenle_form.islem_tipi = "guncelleme";
                        ilac_ekle_duzenle_form.ilac_id = id;
                        frm.Text = "İlaç Güncelleme";
                        frm.Show();
                    }
                    else
                    {
                        MessageBox.Show("İlaç Düzenleme Ekranı Zaten Açık.");
                    }
                }
                else
                {
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ilac_ekle_duzenle_form frm = new ilac_ekle_duzenle_form();
            frm.Name = "ilac_ekle_form";
            if (Application.OpenForms["ilac_ekle_form"] == null)
            {
                ilac_ekle_duzenle_form.islem_tipi = "yeni";
                frm.Text = "İlaç Ekle";
                frm.Show();
            }
            else
            {
                MessageBox.Show("İlaç Ekle Ekranı Zaten Açık.");
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            try
            {
             string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        if (id != "")
                        {
                            ilac_stok_fiyat_bilgisi frm = new ilac_stok_fiyat_bilgisi();
                            frm.Name = "ilac_stok_fiyat_bilgisi";
                            if (Application.OpenForms["ilac_stok_fiyat_bilgisi"] == null)
                            {
                                ilac_stok_fiyat_bilgisi.ilac_id = Convert.ToInt32(id);
                                frm.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString()+" Stok Fiyat Bilgi";
                                frm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Stok Fiyat Bilgi Ekranı Zaten Açık.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
            }
            catch
            {
                MessageBox.Show("Lütfen Satır Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
