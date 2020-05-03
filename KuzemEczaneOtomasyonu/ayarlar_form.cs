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
    public partial class ayarlar_form : Form
    {
        public ayarlar_form()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Veritabani.baglanti.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Veritabani.baglanti;
            bool check_durum = checkBox1.Checked==true?true:false;
            cmd.CommandText = "UPDATE tbl_ayarlar SET ayar_degeri='" + textBox1.Text + "' WHERE ayar_adi='ilac_stok_min_uyari';" +
                "UPDATE tbl_ayarlar SET ayar_degeri='" + check_durum.ToString() + "' WHERE ayar_adi='odenmeyenler_icin_uyari_veri'";
            int sonuc = cmd.ExecuteNonQuery();
            if (sonuc < 0)
            {
                MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Ayarlar Güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            Veritabani.baglanti.Close();
        }

        private void Ayarlar_form_Load(object sender, EventArgs e)
        {
            ayar_cek();
        }
        public void ayar_cek()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ayarlar where ayar_adi='ilac_stok_min_uyari'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = dr["ayar_degeri"].ToString();
            }
            Veritabani.baglanti.Close();
            cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ayarlar where ayar_adi='odenmeyenler_icin_uyari_veri'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                checkBox1.Checked = Convert.ToBoolean(dr["ayar_degeri"].ToString());
            }
            Veritabani.baglanti.Close();
        }
    }  
}
