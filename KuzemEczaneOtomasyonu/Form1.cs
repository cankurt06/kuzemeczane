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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear(); // Panel'in içini temizliyoruz..
            satis_form frm2 = new satis_form();
            frm2.TopLevel = false;
            panel2.Controls.Add(frm2); // panel1 içerisinde formu ekledik

            frm2.Show(); // formu gösterdik. Ancak buraya dikakt. ShowDialog(); olarak değil Show(); olarak açıyoruz.
            frm2.Dock = DockStyle.Fill; // Açılan formun paneli doldurmasını sağladık.
            frm2.BringToFront(); // formu panel içinde en öne getirdik
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear(); // Panel'in içini temizliyoruz..
            ilaclar_form frm2 = new ilaclar_form();
            frm2.TopLevel = false;
            panel2.Controls.Add(frm2); // panel1 içerisinde formu ekledik

            frm2.Show(); // formu gösterdik. Ancak buraya dikakt. ShowDialog(); olarak değil Show(); olarak açıyoruz.
            frm2.Dock = DockStyle.Fill; // Açılan formun paneli doldurmasını sağladık.
            frm2.BringToFront(); // formu panel içinde en öne getirdik

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            const string message = "Programdan çıkmak istiyor musunuz ?";
            const string caption = "Çıkış";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear(); // Panel'in içini temizliyoruz..
            kullanicilar_form frm2 = new kullanicilar_form();
            frm2.TopLevel = false;
            panel2.Controls.Add(frm2); // panel1 içerisinde formu ekledik

            frm2.Show(); // formu gösterdik. Ancak buraya dikakt. ShowDialog(); olarak değil Show(); olarak açıyoruz.
            frm2.Dock = DockStyle.Fill; // Açılan formun paneli doldurmasını sağladık.
            frm2.BringToFront(); // formu panel içinde en öne getirdik
        }

        private void Button7_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear(); // Panel'in içini temizliyoruz..
            hastalar_form frm2 = new hastalar_form();
            frm2.TopLevel = false;
            panel2.Controls.Add(frm2); // panel1 içerisinde formu ekledik

            frm2.Show(); // formu gösterdik. Ancak buraya dikakt. ShowDialog(); olarak değil Show(); olarak açıyoruz.
            frm2.Dock = DockStyle.Fill; // Açılan formun paneli doldurmasını sağladık.
            frm2.BringToFront(); // formu panel içinde en öne getirdik
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (LoginKullanici.kullanici_adi == "admin")
            {
                panel2.Controls.Clear(); // Panel'in içini temizliyoruz..
                ayarlar_form frm2 = new ayarlar_form();
                frm2.TopLevel = false;
                panel2.Controls.Add(frm2); // panel1 içerisinde formu ekledik

                frm2.Show(); // formu gösterdik. Ancak buraya dikakt. ShowDialog(); olarak değil Show(); olarak açıyoruz.
                frm2.Dock = DockStyle.Fill; // Açılan formun paneli doldurmasını sağladık.
                frm2.BringToFront(); // formu panel içinde en öne getirdik
            }
               else
            {
                MessageBox.Show("Ayarları Sadece Admin Kullanıcı Değiştirebilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear(); // Panel'in içini temizliyoruz..
            odemeler_form frm2 = new odemeler_form();
            frm2.TopLevel = false;
            panel2.Controls.Add(frm2); // panel1 içerisinde formu ekledik

            frm2.Show(); // formu gösterdik. Ancak buraya dikakt. ShowDialog(); olarak değil Show(); olarak açıyoruz.
            frm2.Dock = DockStyle.Fill; // Açılan formun paneli doldurmasını sağladık.
            frm2.BringToFront(); // formu panel içinde en öne getirdik
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            stok_kontrol();
            odeme_bekleyen();
        }

        private void stok_kontrol()
        {
            int stok_kontrol = -1;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ayarlar where ayar_adi='ilac_stok_min_uyari'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                stok_kontrol = Convert.ToInt32(dr["ayar_degeri"]);
            }
            Veritabani.baglanti.Close();
            if (stok_kontrol >= 0)
            {
                cmd = new SqlCommand();
                Veritabani.baglanti.Open();
                cmd.Connection = Veritabani.baglanti;
                cmd.CommandText = "SELECT ilac.barkod_no AS barkod,ilac.ilac_adi AS ilac_adi, (SELECT SUM(gelen_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-(SELECT SUM(giden_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-IIF((SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id ) IS NULL,0,(SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id )) AS stokta_kalan_miktar FROM tbl_ilaclar AS ilac where (SELECT SUM(gelen_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-(SELECT SUM(giden_miktar) FROM tbl_ilac_stok where ilac_id=ilac.ilac_id)-IIF((SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id ) IS NULL,0,(SELECT SUM(adet) from tbl_satislar where ilac_id=ilac.ilac_id ))<=" + stok_kontrol;
                dr = cmd.ExecuteReader();
                string sonuc = "";
                while (dr.Read())
                {
                    sonuc += dr["barkod"].ToString() +" "+ dr["ilac_adi"].ToString() + ",Kalan Adet:" + dr["stokta_kalan_miktar"].ToString() + "\n \n";
                }
                MessageBox.Show(sonuc, "Stok Uyarısı",MessageBoxButtons.OK,MessageBoxIcon.Information);
                Veritabani.baglanti.Close();
            }
        }

        private void odeme_bekleyen()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand();
            Veritabani.baglanti.Open();
            cmd.Connection = Veritabani.baglanti;
            cmd.CommandText = "SELECT * FROM tbl_ayarlar where ayar_adi='odenmeyenler_icin_uyari_veri'  and ayar_degeri='true'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                    Veritabani.baglanti.Close();
                    Veritabani.baglanti.Open();
                    cmd.Connection = Veritabani.baglanti;
                    cmd.CommandText = "SELECT COUNT(odeme_id) AS odeme_bekleyen_sayi FROM tbl_odemeler where odeme_durumu<>'Ödeme Alındı'";
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string sonuc = dr["odeme_bekleyen_sayi"].ToString();
                        MessageBox.Show("Ödeme Bekleyen Toplamda " + sonuc + " Satış Var.", "Ödeme Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
            }
            Veritabani.baglanti.Close();
        }
    }
}
