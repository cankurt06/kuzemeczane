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
    public partial class hasta_ekle_duzenle_form : Form
    {
        public static string islem_tipi { get; set; }
        public static string hasta_id { get; set; }
        public hasta_ekle_duzenle_form()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string tc_kimlik = textBox1.Text.ToString().Trim();
            string adi = textBox2.Text.ToString();
            string soyadi = textBox3.Text.ToString();
            string dogum_tarihi = Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd"); ;
            string adres = textBox4.Text.ToString();
            string telefon = textBox5.Text.ToString();
            string sgk_guvencesi = textBox6.Text.ToString();
            if (tc_kimlik != "" && adi != "" && soyadi != "" && dogum_tarihi != "")
            {
                if (islem_tipi == "yeni")
                {
                    SqlDataReader dr;
                    SqlCommand cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_hastalar where tc_kimlik='" + tc_kimlik + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Aynı TC'ye Sahip Başka Bir Hasta Var", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Veritabani.baglanti.Close();
                    }
                    else
                    {
                        Veritabani.baglanti.Close();
                        Veritabani.baglanti.Open();
                        cmd.Connection = Veritabani.baglanti;
                        cmd.CommandText = "INSERT INTO tbl_hastalar (tc_kimlik,adi,soyadi,dogum_tarihi,adres,telefon,sgk_guvencesi) VALUES('" + tc_kimlik + "','" + adi + "','" + soyadi + "','" + dogum_tarihi + "','" + adres + "','" + telefon + "','" + sgk_guvencesi + "')";
                        int sonuc = cmd.ExecuteNonQuery();
                        if (sonuc < 0)
                        {
                            MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Hasta Eklendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.Close();
                        }
                    }
                    Veritabani.baglanti.Close();
                }
                else if (islem_tipi == "guncelleme")
                {
                    if (LoginKullanici.kullanici_adi == "admin")
                    {
                        SqlDataReader dr;
                        SqlCommand cmd = new SqlCommand();
                        Veritabani.baglanti.Open();
                        cmd.Connection = Veritabani.baglanti;
                        cmd.CommandText = "SELECT * FROM tbl_hastalar where tc_kimlik='" + tc_kimlik + "' and hasta_id<>" + hasta_id;
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Aynı TC'ye Sahip Başka Bir Hasta Var", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Veritabani.baglanti.Close();
                        }
                        else
                        {
                            Veritabani.baglanti.Close();
                            Veritabani.baglanti.Open();
                            cmd.Connection = Veritabani.baglanti;
                            cmd.CommandText = "UPDATE tbl_hastalar SET tc_kimlik='" + tc_kimlik + "',adi='" + adi + "',soyadi='" + soyadi + "',dogum_tarihi='" + dogum_tarihi + "',adres='" + adres + "',telefon='" + telefon + "',sgk_guvencesi='" + sgk_guvencesi + "' WHERE hasta_id=" + hasta_id;
                            int sonuc = cmd.ExecuteNonQuery();
                            if (sonuc < 0)
                            {
                                MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Hasta Güncellendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                Veritabani.baglanti.Close();
                                this.Close();
                            }
                        }
                        Veritabani.baglanti.Close();

                    }
                    else
                    {
                        MessageBox.Show("Yetkisiz İşlem.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show("Lütfen Boş Alan Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Hasta_ekle_duzenle_form_Load(object sender, EventArgs e)
        {
            if (islem_tipi == "guncelleme")
            {
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_hastalar where hasta_id=" + hasta_id;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["tc_kimlik"].ToString();
                    textBox2.Text = dr["adi"].ToString();
                    textBox3.Text = dr["soyadi"].ToString();
                    dateTimePicker1.Text= dr["dogum_tarihi"].ToString();
                    textBox4.Text = dr["adres"].ToString();
                    textBox5.Text = dr["telefon"].ToString();
                    textBox6.Text = dr["sgk_guvencesi"].ToString();

                }
                Veritabani.baglanti.Close();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
