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
    public partial class ilac_stok_ekle : Form
    {
        public static string ilac_id { get; set; }
        public ilac_stok_ekle()
        {
            InitializeComponent();
        }

        private void İlac_stok_ekle_Load(object sender, EventArgs e)
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ecza_depo_firmalari";
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
            string stok = textBox1.Text.Trim();
            string firma_adi = comboBox1.Text;
            string firma_id = "";
            int gelen_miktar = 0;
            int giden_miktar = 0;
            string islem_zamani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ecza_depo_firmalari where firma_adi='"+firma_adi+"'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                firma_id=dr["ecza_depo_firma_id"].ToString();
            }
            Veritabani.baglanti.Close();
            if (stok != "" && firma_adi!="" && firma_id!="")
            {
                if (stok.Substring(0, 1) == "-")
                {
                    gelen_miktar = 0;
                    giden_miktar = Math.Abs(Convert.ToInt32(stok));
                }
                else
                {
                    gelen_miktar = Convert.ToInt32(stok);
                    giden_miktar = 0;
                }
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "INSERT INTO tbl_ilac_stok (ilac_id,ecza_depo_firma_id,gelen_miktar,giden_miktar,tarih) VALUES(" + Convert.ToInt32(ilac_id) + "," + Convert.ToInt32(firma_id) + "," + gelen_miktar + "," + giden_miktar + ",'" + islem_zamani + "')";
                int sonuc = cmd.ExecuteNonQuery();
                if (sonuc < 0)
                {
                    Veritabani.baglanti.Close();
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Veritabani.baglanti.Close();
                    MessageBox.Show("Stok Eklendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen Boş Alan Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
