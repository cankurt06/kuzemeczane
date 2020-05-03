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
    public partial class odeme_duzenle_form : Form
    {
        public static int odeme_id { get; set; }
        public odeme_duzenle_form()
        {
            InitializeComponent();
        }

        private void Odeme_duzenle_form_Load(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_odemeler where odeme_id=" + odeme_id;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    comboBox1.Text = dr["odeme_tipi"].ToString();
                    comboBox2.Text = dr["odeme_durumu"].ToString();
                }
                Veritabani.baglanti.Close();
            }
            catch
            {
                MessageBox.Show("Ödeme Bilgisi Bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Veritabani.baglanti.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "UPDATE tbl_odemeler SET odeme_tipi='" + comboBox1.Text + "',odeme_durumu='"+ comboBox2.Text + "' where odeme_id=" + odeme_id;
            int sonuc = cmd.ExecuteNonQuery();
            if (sonuc < 0)
            {
                MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Ödeme Güncellendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Veritabani.baglanti.Close();
                this.Close();
            }
            Veritabani.baglanti.Close();
        }
    }
}
