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

namespace Translator
{
    public partial class AddWord : Form
    {
        static SqlConnection sqlConnection = new SqlConnection(@"Data Source=LAPTOP-AOALA04D\SEPTSQL;Initial Catalog=Dictionary;Trusted_Connection=True");
        public AddWord()
        {
            InitializeComponent();
        }

        void FillDataGridView()
        {
            sqlConnection.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("View", sqlConnection);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ta", textBox4.Text);
            DataTable dtb = new DataTable();
            sqlDa.Fill(dtb);
            dataGridView.DataSource = dtb;

            dataGridView.Columns[0].HeaderCell.Value = "Số thứ tự";
            dataGridView.Columns[1].HeaderCell.Value = "Tiếng Anh";
            dataGridView.Columns[2].HeaderCell.Value = "Nghĩa";
            dataGridView.Columns[3].HeaderCell.Value = "Quá khứ";
            dataGridView.Columns[4].HeaderCell.Value = "Quá khứ phân từ";
            sqlConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                if (button1.Text == "Thêm")
                {
                    SqlCommand cmd = new SqlCommand("AddAndEdit", sqlConnection);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@moded", "Add");
                    cmd.Parameters.AddWithValue("@stt", textBox5.Text);
                    cmd.Parameters.AddWithValue("@ta", textBox6.Text);
                    cmd.Parameters.AddWithValue("@tv", textBox1.Text);
                    cmd.Parameters.AddWithValue("@q1", textBox3.Text);
                    cmd.Parameters.AddWithValue("@q2", textBox2.Text);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("AddAndEdit", sqlConnection);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@moded", "Edit");
                    cmd.Parameters.AddWithValue("@stt", textBox5.Text);
                    cmd.Parameters.AddWithValue("@ta", textBox6.Text);
                    cmd.Parameters.AddWithValue("@tv", textBox1.Text);
                    cmd.Parameters.AddWithValue("@q1", textBox3.Text);
                    cmd.Parameters.AddWithValue("@q2", textBox2.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Saved!!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                sqlConnection.Close();
                FillDataGridView();
            }
        }

        private void AddWord_Load(object sender, EventArgs e)
        {
            FillDataGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Delete", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@stt", textBox5.Text);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            sqlConnection.Open();
            textBox5.Text = dataGridView.CurrentRow.Cells[0].Value.ToString();
            textBox6.Text = dataGridView.CurrentRow.Cells[1].Value.ToString();
            textBox1.Text = dataGridView.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView.CurrentRow.Cells[3].Value.ToString();
            textBox2.Text = dataGridView.CurrentRow.Cells[4].Value.ToString();
            button1.Text = "Cập nhật";
            button2.Enabled = true;

            sqlConnection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox6.Clear();
            textBox5.Clear();
            button1.Text = "Thêm";
        }
    }
}
