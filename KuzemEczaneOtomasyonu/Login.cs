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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlDataReader dr;
        private void Button1_Click(object sender, EventArgs e)
        {
            string kullanici_adi = textBox1.Text.ToString().Trim();
            string sifre = textBox2.Text.ToString().Trim();            
            if(kullanici_adi!="" && sifre!="")
            {
                SqlCommand cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT * FROM tbl_kullanicilar where kullanici_girisadi='" + kullanici_adi + "' AND kullanici_sifre='" + Sifreleme.Sifrele(sifre) + "'";
                
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    LoginKullanici.kullanici_id =Convert.ToInt32(dr["kullanici_id"]);
                    LoginKullanici.kullanici_adi = dr["kullanici_girisadi"].ToString();
                    this.Hide();
                    Form1 frm = new Form1();
                    frm.Text = "Kuzem Eczane Otomasyonu V1 | " + dr["kullanici_adi"] + " " + dr["kullanici_soyadi"];
                    Veritabani.baglanti.Close();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adını ve şifrenizi kontrol ediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Veritabani.baglanti.Close();
            }
            else
            {
                MessageBox.Show("Lütfen Boş Alan Bırakmayın", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
