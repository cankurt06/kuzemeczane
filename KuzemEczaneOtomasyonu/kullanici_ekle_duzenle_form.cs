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
    public partial class kullanici_ekle_duzenle_form : Form
    {
        public static string islem_tipi { get; set; }
        public static string kullanici_id { get; set; }
        public kullanici_ekle_duzenle_form()
        {
            InitializeComponent();
        }

        private void Kullanici_ekle_duzenle_Load(object sender, EventArgs e)
        {
            if(islem_tipi=="guncelleme")
            {
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_kullanicilar where kullanici_id=" + kullanici_id;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["kullanici_girisadi"].ToString();
                    textBox4.Text = Sifreleme.Sifre_Coz(dr["kullanici_sifre"].ToString());
                    textBox2.Text= dr["kullanici_adi"].ToString();
                    textBox3.Text= dr["kullanici_soyadi"].ToString();
                }
                Veritabani.baglanti.Close();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string kullanici_girisadi = textBox1.Text.ToString().Trim();
            string kullanici_adi = textBox2.Text.ToString().TrimStart().TrimEnd();
            string kullanici_soyadi = textBox3.Text.ToString().TrimStart().TrimEnd();
            string sifre = textBox4.Text.ToString().Trim();
            if (kullanici_girisadi!="" && kullanici_adi != "" && kullanici_soyadi != "" && sifre != "")
            {
                if(islem_tipi=="yeni")
                {
                    SqlDataReader dr;
                    SqlCommand cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_kullanicilar where kullanici_girisadi='" + kullanici_girisadi + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Aynı Kullanıcı Adında Başka Bir Kullanıcı Var", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Veritabani.baglanti.Close();
                    }
                    else
                    {
                        Veritabani.baglanti.Close();
                        Veritabani.baglanti.Open();
                        cmd.Connection = Veritabani.baglanti;
                        cmd.CommandText = "INSERT INTO tbl_kullanicilar (kullanici_girisadi,kullanici_sifre,kullanici_adi,kullanici_soyadi) VALUES('"+ kullanici_girisadi+ "','" + Sifreleme.Sifrele(sifre) + "','" + kullanici_adi + "','" + kullanici_soyadi + "')";
                        int sonuc= cmd.ExecuteNonQuery();
                        if(sonuc<0)
                        {
                            MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);    
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı Eklendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.Close();
                        }
                    }
                    Veritabani.baglanti.Close();
                }
                else if(islem_tipi == "guncelleme")
                {
                    if (kullanici_id==LoginKullanici.kullanici_id.ToString() || LoginKullanici.kullanici_adi=="admin")
                    {
                    SqlDataReader dr;
                    SqlCommand cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_kullanicilar where kullanici_girisadi='" + kullanici_girisadi + "' and kullanici_id<>"+ kullanici_id;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Aynı Kullanıcı Adında Başka Bir Kullanıcı Var", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Veritabani.baglanti.Close();
                    }
                    else
                    {
                        Veritabani.baglanti.Close();
                        Veritabani.baglanti.Open();
                        cmd.Connection = Veritabani.baglanti;
                        cmd.CommandText = "UPDATE tbl_kullanicilar SET kullanici_girisadi='" + kullanici_girisadi + "',kullanici_sifre='" + Sifreleme.Sifrele(sifre) + "',kullanici_adi='" + kullanici_adi + "',kullanici_soyadi='" + kullanici_soyadi + "' WHERE kullanici_id="+kullanici_id;
                        int sonuc = cmd.ExecuteNonQuery();
                        if (sonuc < 0)
                        {
                            MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı Güncellendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.Close();
                        }
                    }
                    Veritabani.baglanti.Close();

                }   
                    else
                    {
                        MessageBox.Show("Kendinizden Başka Bir Kullanıcıyı Düzenleyemezsiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
