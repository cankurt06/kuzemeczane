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
    public partial class ilac_fiyat_ekle : Form
    {
        public static string ilac_id { get; set; }
        public ilac_fiyat_ekle()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string fiyat = Program.decimal_cevir(textBox1.Text);
                string islem_zamani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "INSERT INTO tbl_ilac_fiyatlari (ilac_id,perakende_fiyati,fiyat_tarihi) VALUES(" + Convert.ToInt32(ilac_id) + "," + fiyat + ",'" + islem_zamani + "')";
                int sonuc = cmd.ExecuteNonQuery();
                if (sonuc < 0)
                {
                    Veritabani.baglanti.Close();
                    MessageBox.Show("Hatalı İşlem", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Veritabani.baglanti.Close();
                    MessageBox.Show("Fiyat Eklendi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
