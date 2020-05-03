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
    public partial class ilac_ekle_duzenle_form : Form
    {
        public static string islem_tipi { get; set; }
        public static string ilac_id { get; set; }
        public ilac_ekle_duzenle_form()
        {
            InitializeComponent();
        }

        private void İlac_ekle_duzenle_form_Load(object sender, EventArgs e)
        {
            if (islem_tipi == "guncelleme")
            {
                SqlDataReader dr;
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT tbl_ilaclar.*,tbl_ilac_firmalari.firma_adi AS firma_adi FROM tbl_ilaclar LEFT JOIN tbl_ilac_firmalari ON tbl_ilaclar.firma_id=tbl_ilac_firmalari.firma_id where tbl_ilaclar.ilac_id=" + ilac_id;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["barkod_no"].ToString();
                    textBox2.Text = dr["ilac_adi"].ToString();
                    comboBox1.SelectedText = dr["firma_adi"].ToString();
                    textBox3.Text = dr["kdv_orani"].ToString();
                    textBox4.Text = dr["kurum_iskontosu"].ToString();
                    textBox5.Text = dr["eczaci_kari"].ToString();
                }
                Veritabani.baglanti.Close();
            }
            ilac_firmalari_getir();
        }

        public void ilac_firmalari_getir()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ilac_firmalari";
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["firma_adi"]);
            }
            Veritabani.baglanti.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string barkod_no = textBox1.Text.ToString().Trim();
            string ilac_adi = textBox2.Text.ToString().TrimStart().TrimEnd();
            string firma_adi = comboBox1.Text.ToString();
            string firma_id =null;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ilac_firmalari where firma_adi='"+ firma_adi + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                firma_id = dr["firma_id"].ToString();
            }
            Veritabani.baglanti.Close();
            string kdv = Program.decimal_cevir(textBox3.Text.ToString().TrimStart().TrimEnd());
            string iskonto = Program.decimal_cevir(textBox4.Text.ToString().TrimStart().TrimEnd());
            string kar = Program.decimal_cevir(textBox5.Text.ToString().TrimStart().TrimEnd());
            if (barkod_no != "" && ilac_adi != "" && firma_adi != "")
            {
                if (islem_tipi == "yeni")
                {
                     cmd = new SqlCommand();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT * FROM tbl_ilaclar where barkod_no='" + barkod_no + "'";
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Aynı Barkod Numarasına Ait Başka İlaç Var.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Veritabani.baglanti.Close();
                    }
                    else
                    {
                        Veritabani.baglanti.Close();
                        Veritabani.baglanti.Open();
                        cmd.Connection = Veritabani.baglanti;
                        cmd.CommandText = "INSERT INTO tbl_ilaclar (barkod_no,ilac_adi,firma_id,kdv_orani,kurum_iskontosu,eczaci_kari) VALUES('" + barkod_no + "','" +ilac_adi + "',"+ firma_id + ","+kdv+","+ iskonto + ","+kar+")";
                        int sonuc = cmd.ExecuteNonQuery();
                        if (sonuc < 0)
                        {
                            MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("İlaç Eklendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            this.Close();
                        }
                    }
                    Veritabani.baglanti.Close();
                }
                else if (islem_tipi == "guncelleme")
                {
                         cmd = new SqlCommand();
                        Veritabani.baglanti.Open();
                        cmd.Connection = Veritabani.baglanti;
                        cmd.CommandText = "SELECT * FROM tbl_ilaclar where barkod_no='" + barkod_no + "' and ilac_id <>" + ilac_id;
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            MessageBox.Show("Aynı Barkod Numarasına Ait Başka İlaç Var.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Veritabani.baglanti.Close();
                        }
                        else
                        {
                            Veritabani.baglanti.Close();
                            Veritabani.baglanti.Open();
                            cmd.Connection = Veritabani.baglanti;
                            cmd.CommandText = "UPDATE tbl_ilaclar SET barkod_no='" + barkod_no + "',ilac_adi='" + ilac_adi + "',firma_id=" + Convert.ToInt32(firma_id) + ",kdv_orani=" + kdv + ",kurum_iskontosu="+iskonto+ ",eczaci_kari="+kar+" WHERE ilac_id=" + Convert.ToInt32(ilac_id);
                            int sonuc = cmd.ExecuteNonQuery();
                            if (sonuc < 0)
                            {
                                MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("İlaç Güncellendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.Close();
                            }
                        }
                        Veritabani.baglanti.Close();

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
