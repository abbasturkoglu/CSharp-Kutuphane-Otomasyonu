using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronBarCode;
using System.Data.OleDb;

namespace Kutuphane
{
    public partial class KitapEkle : Form

    {
        private
        OleDbConnection baglanti;
        public KitapEkle()
        {
            InitializeComponent();
            
        }
        public void Baglan()
        {

            baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0; Data Source=../../../Database/Kutuphane.mdb");
            baglanti.Open();
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }
        public void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            
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

            else if (!System.Text.RegularExpressions.Regex.IsMatch(textBox4.Text, "^[-0-9]*$") || textBox4.Text == "")
                {
                    return false;
                }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, "^[-0-9]*$") || textBox5.Text == "")
            {
                return false;
            }
            else
                {
                    return true;
                }

        }


        private void button1_Click(object sender, EventArgs e)
        {
           
            System.Diagnostics.Process.Start("MyBarCode.png");
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter adaptorKitapId;
            DataSet veriKitapId;
            string kitapId;

            if (check_values())
            {

                if (MessageBox.Show("Kitap Adı: " + textBox1.Text + "\n\nYazarı: " + textBox2.Text + "\n\nYukarıdaki kitabı kaydetmek istiyor musunuz?", "Kayıt İşlemi!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {


                    OleDbCommand komut;


                    komut = new OleDbCommand();
                    Baglan();
                    komut.Connection = baglanti;
                    komut.CommandText = "insert into kitap (kitapadi, yazar,yayintarihi,sayfasayisi,adet) values (@kitapadi, @yazar,@yayintarihi,@sayfasayisi,@adet)";

                    komut.Parameters.AddWithValue("@kitapadi", textBox1.Text);
                    komut.Parameters.AddWithValue("@yazar", textBox2.Text);
                    komut.Parameters.AddWithValue("@yayintarihi", textBox3.Text);
                    komut.Parameters.AddWithValue("@sayfasayisi", textBox4.Text);
                    komut.Parameters.AddWithValue("@adet", textBox5.Text);

                    komut.ExecuteNonQuery();
                    //Son eklenen kitabın id'sini alalım.

                    adaptorKitapId = new OleDbDataAdapter("select @@IDENTITY from kitap", baglanti);
                    veriKitapId = new DataSet();
                    adaptorKitapId.Fill(veriKitapId, "kitap");

                    kitapId = veriKitapId.Tables["kitap"].Rows[0][0].ToString();


                    baglanti.Close();

                    MessageBox.Show("İşlem Tamamlandı", "İşlem", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    button1.Visible = true;
                    groupBox2.Visible = true;

                    //Barkod oluşturalım.
                    var MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(kitapId, BarcodeWriterEncoding.QRCode);
                    MyBarCode.AddAnnotationTextAboveBarcode(textBox1.Text);
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
            else {
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
    }
}
