
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
    public partial class OduncVer : Form
    {
        private
        string uyeNo;
        string kitapNo;
        OleDbConnection baglanti;

        public OduncVer()
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
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            uyeNo = "";
            kitapNo = "";
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox6.Enabled = false;
            button2.Enabled = false;
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
            DataSet veriUye;

            uyeNo = "";
            textBox2.Clear();
            textBox3.Clear();
            button2.Enabled = false;
            
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text,"^[-0-9]*$") && textBox1.Text != "")
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
                    button2.Enabled = true;
                    textBox6.Enabled = true;

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
    


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter adaptorKitap;
            DataSet veriKitap;

            kitapNo = "";
            textBox4.Clear();
            textBox5.Clear();
            button4.Enabled = false;
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox6.Text, "^[-0-9]*$") && textBox6.Text != "")
            {



                //veri tabanı bağlantısı
                Baglan();

                //üye bilgileri
                adaptorKitap = new OleDbDataAdapter("SELECT id,kitapadi,yazar from kitap where id=" + textBox6.Text, baglanti);
                veriKitap = new DataSet();
                adaptorKitap.Fill(veriKitap, "kitap");

                if (veriKitap.Tables["kitap"].Rows.Count == 0)
                {

                    MessageBox.Show("HATA: Kitap bulunamadı!\nLütfen geçerli bir kitap numarası giriniz.", "Kitap bulunamadı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox6.Clear();

                }

                else
                {
                    textBox4.Text = veriKitap.Tables["kitap"].Rows[0][1].ToString();
                    textBox5.Text = veriKitap.Tables["kitap"].Rows[0][2].ToString();
                    kitapNo = veriKitap.Tables["kitap"].Rows[0][0].ToString();
                    button2.Enabled = false;
                    textBox6.Enabled = false;
                    button4.Enabled = true;
                }

                //Bağlantıyı sonlandır
                baglanti.Close();

            }

            else
            {

                MessageBox.Show("HATA: Lütfen geçerli bir kitap numarası giriniz.", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox6.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
                    

            if (MessageBox.Show("Üye Adı Soyadı: " + textBox2.Text +"\n\nKitap Adı: " + textBox4.Text +"\n\nYukarıdaki ödünç işlemini onaylıyor musunuz?", "Ödünç İşlemi!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)==DialogResult.Yes)
            {
                
                
                OleDbCommand komut;
                

                komut = new OleDbCommand();
                Baglan();
                komut.Connection = baglanti;
                komut.CommandText = "insert into odunc (uye_id,kitap_id, iade,tarih) values (@uyeNo, @kitapNo, 0,date())";

                komut.Parameters.AddWithValue("@uyeNo", uyeNo);
                komut.Parameters.AddWithValue("@kitapNo", kitapNo);

                komut.ExecuteNonQuery();
                baglanti.Close();

                MessageBox.Show("İşlem Tamamlandı", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                
            }
        }
    }
}
