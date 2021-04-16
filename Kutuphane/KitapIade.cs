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

namespace Kutuphane
{
    public partial class KitapIade : Form
    {
        private
        string uyeNo;
        //string kitapNo;
        OleDbConnection baglanti;

        public KitapIade()
        {
            InitializeComponent();
        }
        public void Baglan()
        {

            baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0; Data Source=../../../Database/Kutuphane.mdb");
            baglanti.Open();
        }
        public void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            
            uyeNo = "";
            //kitapNo = "";
            button1.Enabled = true;
            textBox1.Enabled = true;
            
            button4.Enabled = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OleDbDataAdapter adaptorUye;
            OleDbDataAdapter adaptorKitap;
            DataSet veriUye;
            DataSet veriKitap;

            uyeNo = "";
            textBox2.Clear();
            textBox3.Clear();
          

            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[-0-9]*$") && textBox1.Text != "")
            {



                //veri tabanı bağlantısı
                Baglan();

                //üye bilgileri
                adaptorUye = new OleDbDataAdapter("SELECT id,ad + ' ' + soyad,tlf from uye where id=" + textBox1.Text, baglanti);
                veriUye = new DataSet();
                adaptorUye.Fill(veriUye, "uye");

                if (veriUye.Tables["uye"].Rows.Count == 0)
                {

                    MessageBox.Show("HATA: Üye bulunamadı!\nLütfen geçerli bir üye numarası giriniz.", "Üye bulunamadı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();

                }

                else
                {
                    textBox2.Text = veriUye.Tables["uye"].Rows[0][1].ToString();
                    textBox3.Text = veriUye.Tables["uye"].Rows[0][2].ToString();
                    uyeNo = veriUye.Tables["uye"].Rows[0][0].ToString();

                    textBox1.Enabled = false;
                    button1.Enabled = false;


                    //Kitap bilgilerini doldur
                    adaptorKitap = new OleDbDataAdapter("SELECT k.id as `Kitap No`, k.kitapadi as `Kitap Adı`  FROM odunc o inner join kitap k on k.id=o.kitap_id where iade=0 and uye_id=" + uyeNo, baglanti);
                    veriKitap = new DataSet();
                    adaptorKitap.Fill(veriKitap, "kitap");

                    dataGridView1.DataSource = veriKitap.Tables["kitap"];

                    dataGridView1.Columns[0].Width = 85;
                    dataGridView1.Columns[1].Width = 420;


                    if (veriKitap.Tables["kitap"].Rows.Count > 0)
                    {

                        button4.Enabled = true;

                    }
                    else
                    {

                        MessageBox.Show("HATA: Üye üzerinde kitap yoktur.", "Kayıt Yok!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clear();
                    }



                }

                //Bağlantıyı sonlandır
                baglanti.Close();

            }

            else
            {

                MessageBox.Show("HATA: Lütfen geçerli bir üye numarası giriniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            OleDbDataAdapter adaptorKitap;
            DataSet veriKitap;

            int rowindex = dataGridView1.CurrentCell.RowIndex;
           
            if (MessageBox.Show("Üye Adı Soyadı: " + textBox2.Text + "\n\nKitap Adı: " + dataGridView1.Rows[rowindex].Cells[1].Value.ToString() + "\n\nYukarıdaki aide işlemini onaylıyor musunuz?", "İade İşlemi!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                OleDbCommand komut;
                komut = new OleDbCommand();
                Baglan();
                komut.Connection = baglanti;
                komut.CommandText = "update odunc set iade=1, iadetarih=date() where iade=0 and uye_id=" + uyeNo + " and kitap_id=" + dataGridView1.Rows[rowindex].Cells[0].Value.ToString();

                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("İşlem Tamamlandı", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button4.Enabled = false;


                //Kitap bilgilerini doldur
                adaptorKitap = new OleDbDataAdapter("SELECT k.id as `Kitap No`, k.kitapadi as `Kitap Adı`  FROM odunc o inner join kitap k on k.id=o.kitap_id where iade=0 and uye_id=" + uyeNo, baglanti);
                veriKitap = new DataSet();
                adaptorKitap.Fill(veriKitap, "kitap");

                dataGridView1.DataSource = veriKitap.Tables["kitap"];

                dataGridView1.Columns[0].Width = 85;
                dataGridView1.Columns[1].Width = 420;


                if (veriKitap.Tables["kitap"].Rows.Count > 0)
                {

                    button4.Enabled = true;

                }


            }
        }
    }
}
