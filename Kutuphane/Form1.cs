using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Kutuphane
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Ana Menü";

            OleDbConnection baglanti;
            OleDbDataAdapter adaptorOkuyan;
            OleDbDataAdapter adaptorKitap;
            OleDbDataAdapter adaptorUyeSayisi;
            OleDbDataAdapter adaptorKitapSayisi;
            OleDbDataAdapter adaptorOduncSayisi;
            OleDbDataAdapter adaptorIadeSayisi;

            DataSet veriOkuyan;
            DataSet veriKitap;
            DataSet veriUyeSayisi;
            DataSet veriKitapSayisi;
            DataSet veriOduncSayisi;
            DataSet veriIadeSayisi;


            //veri tabanı bağlantısı
            baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0; Data Source=../../../Database/Kutuphane.mdb");
            baglanti.Open();


            //En Çok kitap okuyan 10 üye
            adaptorOkuyan = new OleDbDataAdapter("SELECT top 10 o.uye_id as `Üye No`, e.ad as `Adı`, e.soyad as `Soyadı` ,sum(k.sayfasayisi) as `Okunan Sayfa`  FROM (odunc o inner join uye e on e.id=o.uye_id) inner join kitap k on k.id=o.kitap_id group by o.uye_id, e.ad, e.soyad order by sum(k.sayfasayisi) desc", baglanti);
            veriOkuyan = new DataSet();
            adaptorOkuyan.Fill(veriOkuyan, "enCokKitapOkuyan");

            dataGridView1.DataSource = veriOkuyan.Tables["enCokKitapOkuyan"];
            dataGridView1.Columns[0].Width = 53;
            dataGridView1.Columns[1].Width = 145;
            dataGridView1.Columns[2].Width = 145;
            dataGridView1.Columns[3].Width = 70;

            //En çok okunan 10 kitap
            adaptorKitap = new OleDbDataAdapter("SELECT top 10 k.kitapadi as `Kitap Adı`, count(o.kitap_id) as `Okunma Sayısı` FROM odunc o inner join kitap k on k.id=o.kitap_id group by k.kitapadi, k.yazar order by count(o.kitap_id) desc", baglanti);
            veriKitap = new DataSet();
            adaptorKitap.Fill(veriKitap, "enCokOkunanKitap");

            dataGridView2.DataSource = veriKitap.Tables["enCokOkunanKitap"];
            
            dataGridView2.Columns[0].Width = 330;
            dataGridView2.Columns[1].Width = 85;


            //Üye Sayısını label3'e yazdır
            adaptorUyeSayisi = new OleDbDataAdapter("SELECT count(*) from uye", baglanti);
            veriUyeSayisi = new DataSet();
            adaptorUyeSayisi.Fill(veriUyeSayisi, "uyeSayisi");

            label3.Text = veriUyeSayisi.Tables["uyeSayisi"].Rows[0][0].ToString();


            //Kitap Sayısını label4'e yazdır
            adaptorKitapSayisi = new OleDbDataAdapter("SELECT count(*) from kitap", baglanti);
            veriKitapSayisi = new DataSet();
            adaptorKitapSayisi.Fill(veriKitapSayisi, "kitapSayisi");

            label4.Text = veriKitapSayisi.Tables["kitapSayisi"].Rows[0][0].ToString();

            //Ödünç Sayısını label5'e yazdır
            adaptorOduncSayisi = new OleDbDataAdapter("SELECT count(*) from odunc", baglanti);
            veriOduncSayisi = new DataSet();
            adaptorOduncSayisi.Fill(veriOduncSayisi, "oduncSayisi");

            label5.Text = veriOduncSayisi.Tables["oduncSayisi"].Rows[0][0].ToString();

            //İade Bekleyen Sayısını label6'ya yazdır
            adaptorIadeSayisi = new OleDbDataAdapter("SELECT count(*) from odunc where iade=0", baglanti);
            veriIadeSayisi = new DataSet();
            adaptorIadeSayisi.Fill(veriIadeSayisi, "IadeSayisi");

            label6.Text = veriIadeSayisi.Tables["IadeSayisi"].Rows[0][0].ToString();

            //Bağlantıyı sonlandır
            baglanti.Close();


        }


        protected override void OnClosed(EventArgs e)
        {
            Application.Exit();
        }

        private void ödünçVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            OduncVer f2 = new OduncVer(); 
            f2.ShowDialog();
        }

        private void kitapİadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            KitapIade f3 = new KitapIade();
            f3.ShowDialog();
        }

        private void kitapEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            KitapEkle f4 = new KitapEkle();
            f4.ShowDialog();
        }

        private void üyeEklemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            UyeEkle f5 = new UyeEkle();
            f5.ShowDialog();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            OduncVer f2 = new OduncVer();
            f2.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            KitapIade f3 = new KitapIade();
            f3.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            UyeEkle f5 = new UyeEkle();
            f5.ShowDialog();
        }

        private void ödünçİşlemleriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void iadeSüresiGeçenlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f20 = new Form2();
            f20.ShowDialog();
        }

        private void iadeSüresiGeçenKitaplarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f20 = new Form2();
            f20.ShowDialog();
        }

        private void listelemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UyeListesiForm f21 = new UyeListesiForm();
            f21.ShowDialog();
        }

        private void üyeListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UyeListesiForm f21 = new UyeListesiForm();
            f21.ShowDialog();
        }

        private void kitapListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KitapListesiForm f21 = new KitapListesiForm();
            f21.ShowDialog();
        }

        private void listeleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KitapListesiForm f21 = new KitapListesiForm();
            f21.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            KitapListesiForm f21 = new KitapListesiForm();
            f21.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UyeListesiForm f21 = new UyeListesiForm();
            f21.ShowDialog();
        }

        private void ödünçVerilenlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f23 = new Form3();
            f23.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form3 f23 = new Form3();
            f23.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form4 f24 = new Form4();
            f24.ShowDialog();
        }

        private void iadeBekleyenKitaplarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f24 = new Form4();
            f24.ShowDialog();
        }

        private void enÇokOkuyanlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 f25 = new Form5();
            f25.ShowDialog();
        }

        private void enÇokOkunanKitaplarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f26 = new Form6();
            f26.ShowDialog();
        }
    }
}
