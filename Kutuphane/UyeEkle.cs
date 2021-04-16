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
using IronBarCode;

namespace Kutuphane
{
    public partial class UyeEkle : Form
    {
        private
        OleDbConnection baglanti;
        public UyeEkle()
        {
            InitializeComponent();
        }
        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
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


        }

        public Boolean check_values()
        {
            if (textBox1.Text == "")
            {
                return false;
            }
            else if (textBox2.Text == "")
            {
                return false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, "^[-0-9]*$") || textBox3.Text == "")
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter adaptorUyeId;
            DataSet veriUyeId;
            string uyeId;

            if (check_values())
            {

                if (MessageBox.Show("Üye Adı: " + textBox1.Text +" "+ textBox2.Text + "\n\nTelefon No: " + textBox3.Text + "\n\nYukarıdaki üyeyi kaydetmek istiyor musunuz?", "Kayıt İşlemi!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {


                    OleDbCommand komut;


                    komut = new OleDbCommand();
                    Baglan();
                    komut.Connection = baglanti;
                    komut.CommandText = "insert into uye (ad, soyad,tlf) values (@ad, @soyad,@tlf)";

                    komut.Parameters.AddWithValue("@ad", textBox1.Text);
                    komut.Parameters.AddWithValue("@soyad", textBox2.Text);
                    komut.Parameters.AddWithValue("@tlf", textBox3.Text);


                    komut.ExecuteNonQuery();
                    //Son eklenen kitabın id'sini alalım.

                    adaptorUyeId = new OleDbDataAdapter("select @@IDENTITY from uye", baglanti);
                    veriUyeId = new DataSet();
                    adaptorUyeId.Fill(veriUyeId, "uye");

                    uyeId = veriUyeId.Tables["uye"].Rows[0][0].ToString();


                    baglanti.Close();

                    MessageBox.Show("İşlem Tamamlandı", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    button1.Visible = true;
                    groupBox2.Visible = true;

                    //Barkod oluşturalım.
                    var MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(uyeId, BarcodeWriterEncoding.QRCode);
                    MyBarCode.AddAnnotationTextAboveBarcode(textBox1.Text + " " + textBox2.Text);
                    MyBarCode.AddBarcodeValueTextBelowBarcode();
                    MyBarCode.SetMargins(10);
                    MyBarCode.ChangeBarCodeColor(Color.Purple);
                    // Save as HTML
                    MyBarCode.SaveAsPng("MyBarCode.png");
                    //System.Diagnostics.Process.Start("MyBarCode.png");
                    pictureBox1.ImageLocation = "MyBarCode.png";
                    clear();


                }
            }
            else
            {
                MessageBox.Show("Eksik ya da hatalı veri girişi.\nGirilen verileri kontrol ediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
            button1.Visible = false;
            groupBox2.Visible = false;
            pictureBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("MyBarCode.png");
        }
    }
}
