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
    public partial class satis_ekle_duzenle_form : Form
    {
        public static string islem_tipi { get; set; }
        public static string satis_id { get; set; }

        public satis_ekle_duzenle_form()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Lütfen Sadece Sayı Girin");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
            string tc_kimlik = textBox1.Text.ToString().Trim();
            if (tc_kimlik.Length == 11 && islem_tipi=="yeni")
            {
                    SqlDataReader dr;
                    SqlCommand cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_hastalar where tc_kimlik=" + tc_kimlik;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox2.Text = dr["adi"].ToString(); ;
                        textBox3.Text = dr["soyadi"].ToString(); 
                        dateTimePicker1.Text = dr["dogum_tarihi"].ToString();
                        textBox4.Text = dr["adres"].ToString();
                        textBox5.Text = dr["telefon"].ToString();
                        textBox6.Text = dr["sgk_guvencesi"].ToString();
                    }
                    Veritabani.baglanti.Close();
              
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string tc_kimlik = textBox1.Text.Trim();
            int hasta_id = 0;
            if (tc_kimlik.Length < 11)
            {
                MessageBox.Show("TC Kimlik Alanı Boş Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text.Trim().Length < 1)
            {
                MessageBox.Show("Ad Alanı Boş Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text.Trim().Length < 1)
            {
                MessageBox.Show("Soyadı Alanı Boş Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox1.Text.Trim().Length < 1)
            {
                MessageBox.Show("Ödeme Tipi Alanı Boş Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox2.Text.Trim().Length < 1)
            {
                MessageBox.Show("Ödeme Durumu Alanı Boş Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if (dataGridView1.RowCount < 1)
            {
                MessageBox.Show("Lütfen İlaç Ekleyin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_hastalar where tc_kimlik=" + tc_kimlik;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    hasta_id = Convert.ToInt32(dr["hasta_id"].ToString());
                    Veritabani.baglanti.Close();
                }
                else
                {
                    Veritabani.baglanti.Close();
                    Veritabani.baglanti.Open();
                    cmd = new SqlCommand();
                    cmd.Connection = Veritabani.baglanti;
                    string dogum_tarihi = Convert.ToDateTime(dateTimePicker1.Text.ToString()).ToString("yyyy-MM-dd");
                    cmd.CommandText = "INSERT INTO tbl_hastalar (tc_kimlik,adi,soyadi,dogum_tarihi,adres,telefon,sgk_guvencesi)  OUTPUT INSERTED.hasta_id VALUES('" + tc_kimlik.ToString() + "','"+textBox2.Text+ "','" + textBox3.Text + "','" + dogum_tarihi + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "')";
                    int yeni_hasta_id = Convert.ToInt32(cmd.ExecuteScalar());
                    if (yeni_hasta_id < 0)
                    {
                        MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        hasta_id = Convert.ToInt32(yeni_hasta_id.ToString());
                    }
                    Veritabani.baglanti.Close();
                }
                Veritabani.baglanti.Close();
                Veritabani.baglanti.Open();
                cmd = new SqlCommand();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "UPDATE tbl_satislar SET hasta_id="+hasta_id+" where satis_id="+satis_id+" or master_satis="+ satis_id;
                cmd.ExecuteNonQuery();
                Veritabani.baglanti.Close();
                Veritabani.baglanti.Open();
                cmd = new SqlCommand();
                cmd.Connection = Veritabani.baglanti;
                string islem_tarihi = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                cmd.CommandText = "INSERT INTO tbl_odemeler (kullanici_id,satis_id,odeme_tipi,toplam_tutar,odeme_durumu,islem_zamani)  OUTPUT INSERTED.odeme_id VALUES(" + LoginKullanici.kullanici_id + "," + satis_id + ",'" + comboBox1.Text + "',(SELECT SUM(toplam_tutar) FROM tbl_satislar where satis_id="+satis_id+" or master_satis="+satis_id+"),'" + comboBox2.Text + "','" + islem_tarihi + "')";
                int yeni_odeme_id = Convert.ToInt32(cmd.ExecuteScalar());
                if (yeni_odeme_id < 0)
                {
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Ödeme Alındı", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Veritabani.baglanti.Close();
                    this.Close();
                }
                
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "DELETE FROM tbl_satislar WHERE satis_id=" + satis_id+" OR master_satis="+ satis_id;
            int sonuc = cmd.ExecuteNonQuery();
            Veritabani.baglanti.Close();
            this.Close();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            yeni_satir_ekle(Convert.ToInt32(satis_id));
        }

        public void get_satis(int master_id)
        {
            if (Veritabani.baglanti.State == System.Data.ConnectionState.Open)
                Veritabani.baglanti.Close();

            Veritabani.baglanti.Open();
            string kayit = "SELECT satislar.satis_id AS 'Islem Id',ilac.barkod_no AS 'Barkod No',ilac.ilac_adi AS 'İlaç Adı',satislar.adet AS Adet,IIF(fiyat.perakende_fiyati IS NOT NULL,CONCAT(fiyat.perakende_fiyati,' TL'),NULL) AS 'Perakande Fiyat',IIF(satislar.toplam_tutar IS NOT NULL,CONCAT(satislar.toplam_tutar,' TL'),NULL) AS 'Perakende Toplam Tutar' " +
                "FROM tbl_satislar AS satislar LEFT JOIN tbl_ilaclar AS ilac ON satislar.ilac_id=ilac.ilac_id " +
                "LEFT JOIN tbl_ilac_fiyatlari AS fiyat ON satislar.ilac_id=fiyat.ilac_id AND fiyat.fiyat_id=(SELECT TOP(1) fiyat_id FROM tbl_ilac_fiyatlari WHERE ilac_id=satislar.ilac_id ORDER BY fiyat_tarihi DESC) WHERE satis_id=" + master_id + " OR master_satis=" + master_id + " ORDER BY satislar.satis_id ASC";
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
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT CONCAT(SUM(toplam_tutar),' TL') AS ToplamTutar FROM tbl_satislar where satis_id=" + master_id + " or master_satis="+ master_id;
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox7.Text = dr["ToplamTutar"].ToString();

            }
            Veritabani.baglanti.Close();
        }

        public void yeni_satir_ekle(int master_id)
        {
            Veritabani.baglanti.Open();
            if (master_id == 0)
            {
                string islem_zamani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "INSERT INTO tbl_satislar (kullanici_id,islem_zamani)  OUTPUT INSERTED.satis_id VALUES(" + LoginKullanici.kullanici_id+",'"+ islem_zamani + "')";
                int yeni_satis_id = Convert.ToInt32(cmd.ExecuteScalar());
                if (yeni_satis_id < 0)
                {
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    satis_id = yeni_satis_id.ToString();
                    get_satis(yeni_satis_id);
                }
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "INSERT INTO tbl_satislar (kullanici_id,master_satis) VALUES(" + LoginKullanici.kullanici_id + ","+ satis_id + ")";
                int sonuc = cmd.ExecuteNonQuery();
                if (sonuc < 0)
                {
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    get_satis(master_id);
                }
            }
            Veritabani.baglanti.Close();
        }

        private void Satis_ekle_duzenle_form_Load(object sender, EventArgs e)
        {
            if (islem_tipi == "yeni")
            {
                satis_id = "0";
            }
            else if (islem_tipi == "detay")
            {
                button1.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                dataGridView1.ReadOnly = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox6.ReadOnly = true;
                dateTimePicker1.Enabled = false;
                button3.Visible = true;
                string hasta_id = "";
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_satislar where satis_id=" + satis_id;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    hasta_id = dr["hasta_id"].ToString();
                }
                Veritabani.baglanti.Close();
                try
                {
                    cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_hastalar where hasta_id=" + hasta_id;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox1.Text = dr["tc_kimlik"].ToString(); 
                        textBox2.Text = dr["adi"].ToString(); 
                        textBox3.Text = dr["soyadi"].ToString();
                        dateTimePicker1.Text = dr["dogum_tarihi"].ToString();
                        textBox4.Text = dr["adres"].ToString();
                        textBox5.Text = dr["telefon"].ToString();
                        textBox6.Text = dr["sgk_guvencesi"].ToString();
                    }
                }
                catch
                {
                    MessageBox.Show("Hasta Bilgisi Bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Veritabani.baglanti.Close();
                try
                {
                    cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_odemeler where satis_id=" + satis_id;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        comboBox1.Text = dr["odeme_tipi"].ToString();
                        comboBox2.Text = dr["odeme_durumu"].ToString();
                    }
                }
                catch
                {
                    MessageBox.Show("Ödeme Bilgisi Bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Veritabani.baglanti.Close();
                get_satis(Convert.ToInt32(satis_id));
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                string ilac_barkod = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_ilaclar where barkod_no='" + ilac_barkod + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string ilac_id=dr["ilac_id"].ToString();
                    Veritabani.baglanti.Close();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "UPDATE tbl_satislar SET ilac_id=" + ilac_id + ",adet=IIF(adet IS NULL,1,adet),toplam_tutar=IIF(toplam_tutar IS NULL,(SELECT TOP(1) perakende_fiyati FROM tbl_ilac_fiyatlari WHERE ilac_id="+ ilac_id + " ORDER BY fiyat_tarihi DESC)*IIF(adet IS NULL,1,adet),toplam_tutar) WHERE satis_id=" + id;
                    int sonuc = cmd.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Girdiğininiz Barkod Numarasına Ait İlaç Bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Veritabani.baglanti.Close();
                }
            }
            if (e.ColumnIndex == 3)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                string adet = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (Convert.ToInt32(adet)>0)
                {
                    SqlCommand cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "UPDATE tbl_satislar SET adet=" + adet + ",toplam_tutar=(SELECT TOP(1) perakende_fiyati FROM tbl_ilac_fiyatlari WHERE ilac_id=tbl_satislar.ilac_id ORDER BY fiyat_tarihi DESC)*"+adet+" WHERE satis_id=" + id;
                    int sonuc = cmd.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Lütfen Geçerli Bir Adet Girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Veritabani.baglanti.Close();
                }
            }
            get_satis(Convert.ToInt32(satis_id));
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            int row_index = -1;
            try
            {
                 row_index = dataGridView1.SelectedCells[0].RowIndex;
            }
            catch
            {
                MessageBox.Show("Lütfen Silinecek Satırı Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (row_index >=0)
            {
                object islem_id = dataGridView1.Rows[row_index].Cells[0].Value;
                if (islem_id != null)
                {
                    SqlCommand cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "UPDATE tbl_satislar SET ilac_id=NULL,adet=NULL,toplam_tutar=null WHERE satis_id=" + islem_id.ToString(); ;
                    int sonuc = cmd.ExecuteNonQuery();
                    get_satis(Convert.ToInt32(satis_id));
                }
              else
                {
                    MessageBox.Show("Boş Satırı Silemezsiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen Silinecek Satırı Seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
