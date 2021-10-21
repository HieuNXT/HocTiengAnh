using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Speech;
using System.Speech.Synthesis;
using IronOcr;
using System.Net;
using System.IO;

namespace Translator
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this._comboFrom.Items.AddRange(Translator.Languages.ToArray());
            this._comboTo.Items.AddRange(Translator.Languages.ToArray());

            this._comboFrom.SelectedItem = "English";
            this._comboTo.SelectedItem = "Vietnamese";

            var synth = new SpeechSynthesizer();
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                comboBox1.Items.Add(voice.VoiceInfo.Name);
            }

            textBox1.Text = File.ReadAllText(@"Translator\Resources\ls\ls1.txt");
            textBox2.Text = File.ReadAllText(@"Translator\Resources\ls\ls2.txt");
            textBox3.Text = File.ReadAllText(@"Translator\Resources\ls\ls3.txt");
            textBox5.Text = File.ReadAllText(@"Translator\Resources\ls\ls4.txt");
            textBox7.Text = File.ReadAllText(@"Translator\Resources\ls\ls5.txt");
            textBox8.Text = File.ReadAllText(@"Translator\Resources\ls\ls6.txt");
            textBox9.Text = File.ReadAllText(@"Translator\Resources\ls\ls7.txt");
            textBox10.Text = File.ReadAllText(@"Translator\Resources\ls\ls8.txt");
            textBox11.Text = File.ReadAllText(@"Translator\Resources\ls\ls9.txt");
            textBox12.Text = File.ReadAllText(@"Translator\Resources\ls\ls10.txt");
            textBox13.Text = File.ReadAllText(@"Translator\Resources\ls\ls11.txt");
            textBox14.Text = File.ReadAllText(@"Translator\Resources\ls\ls12.txt");
            textBox15.Text = File.ReadAllText(@"Translator\Resources\ls\ls13.txt");
            textBox16.Text = File.ReadAllText(@"Translator\Resources\ls\ls14.txt");
            list();
            addmedia();
        }
        void addmedia()
        {
            string[] files = Directory.GetFiles(@"Translator\Resources");

            foreach (string f in files)
            {
                var fileItem = new filename { Title = Path.GetFileNameWithoutExtension(f), Path = Path.GetFullPath(f) };
                listBox1.Items.Add(fileItem);
            }
            listBox1.DisplayMember = "Title";
            listBox1.ValueMember = "Path";
        }
        public class filename
        {
            public string Title { get; set; }
            public string Path { get; set; }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        private void _btnTranslate_Click(object sender, EventArgs e)
        {
            if (label6.Text == "Vietnamese")
            {
                this.comboBox1.SelectedItem = "Microsoft An";
            }
            else
            {
                this.comboBox1.SelectedItem = "Microsoft Zira";
            }
            if (CheckForInternetConnection() == true)
            {
                Translator t = new Translator();

            this._editTarget.Text = string.Empty;
            this._editTarget.Update();
            // Translate the text
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this._lblStatus.Text = "Translating...";
                this._lblStatus.Update();
                this._editTarget.Text = t.Translate(this._editSourceText.Text.Trim(), (string)this._comboFrom.SelectedItem, (string)this._comboTo.SelectedItem);
                if (t.Error == null)
                {
                    this._editTarget.Update();
                }
                else
                {
                    MessageBox.Show(t.Error.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            }
            else
            {
                int index = listBox.FindString(_editSourceText.Text);

                if (index < 0)
                {
                    MessageBox.Show("Item not found.");
                    _editSourceText.Text = String.Empty;
                }
                else
                {
                    listBox.SelectedIndex = index;
                }
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoice(comboBox1.SelectedItem.ToString());
            string a = _editTarget.Text.ToString();

            synth.SpeakAsync(a);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _editSourceText.Clear();
            _editTarget.Clear();
            //pictureBox4.Image.Dispose();
            pictureBox4.Image = null;
        }
        

        private void button6_Click(object sender, EventArgs e)
        {
            string from = (string)this._comboFrom.SelectedItem;
            string to = (string)this._comboTo.SelectedItem;
            this._comboFrom.SelectedItem = to;
            this._comboTo.SelectedItem = from;

            // Reset text
            this._editSourceText.Text = this._editTarget.Text;
            this._editTarget.Text = string.Empty;
            this.Update();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
            {
                textBox6.Text = o.FileName;
                pictureBox4.ImageLocation = o.FileName;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var Ocr = new IronTesseract();
            Ocr.Language = OcrLanguage.Vietnamese;
            _editSourceText.Text = Ocr.Read(textBox6.Text).Text;
            Ocr.Language = OcrLanguage.Vietnamese;
        }

        private void _comboFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Text = _comboFrom.SelectedItem.ToString();
        }

        private void _comboTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            label6.Text = _comboTo.SelectedItem.ToString();
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        static SqlConnection sqlConnection = new SqlConnection(@"Data Source=LAPTOP-AOALA04D\SEPTSQL;Initial Catalog=Dictionary;Trusted_Connection=True");
        private void button8_Click(object sender, EventArgs e)
        {

        }
        void list()
        {
            SqlDataAdapter a = new SqlDataAdapter("Select ta from Dic ", sqlConnection);
            DataTable dataTable = new DataTable();
            a.Fill(dataTable);

            listBox.DataSource = dataTable;
            listBox.DisplayMember = "ta";
            listBox.ValueMember = "ta";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItems = listBox1.SelectedItems.Cast<filename>();
            var all = string.Join(Environment.NewLine, selectedItems.Select(x => x.Path));

            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.URL = all;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(label5.Text=="English")
            {
                _editSourceText.Text = (((DataRowView)listBox.SelectedItem).Row[0]).ToString();
                string qr = "Select tv from Dic where ta = '" + _editSourceText.Text + "'";
                SqlDataAdapter a = new SqlDataAdapter(qr, sqlConnection);
                DataTable dataTable = new DataTable();
                a.Fill(dataTable);

                _editTarget.Text = Convert.ToString(dataTable.Rows[0]["tv"]);
            }
            else
            {
                _editSourceText.Text = (((DataRowView)listBox.SelectedItem).Row[0]).ToString();
                string qr = "Select ta from Dic where tv = '" + _editSourceText.Text + "'";
                SqlDataAdapter a = new SqlDataAdapter(qr, sqlConnection);
                DataTable dataTable = new DataTable();
                a.Fill(dataTable);

                _editTarget.Text = Convert.ToString(dataTable.Rows[0]["ta"]);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddWord ad = new AddWord();
            ad.Show();
        }
    }
}
