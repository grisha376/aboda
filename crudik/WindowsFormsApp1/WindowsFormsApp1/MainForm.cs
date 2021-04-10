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

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-4CN6F62\SQLEXPRESS;Initial Catalog=Crud;Integrated Security=True");
        SqlCommand cum;

        public MainForm()
        {
            InitializeComponent();
        }

        private void usersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.usersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.crud_DB);

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.usersTableAdapter.Fill(this.crud_DB.Users);
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            FormCE FormCE = new FormCE();
            this.Hide();
            FormCE.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult res = new DialogResult();
                res = MessageBox.Show("Подтвердите удаление!",
                                                 "Warning",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    cum = new SqlCommand("DELETE FROM USERS WHERE Login=@Login", con);
                    cum.Parameters.AddWithValue("@Login", usersDataGridView["dataGridViewTextBoxColumn1", usersDataGridView.CurrentCell.RowIndex].Value);
                    con.Open();
                    cum.ExecuteNonQuery();
                    this.usersTableAdapter.Fill(this.crud_DB.Users);
                }
                else
                {
                    return;
                }
            }
            catch(System.InvalidOperationException)
            {
                MessageBox.Show("Данных для удаления нет!","Warning");
            }

            finally
            {
                con.Close();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            User CurrentUser = new User();
            CurrentUser.Login = Convert.ToString(usersDataGridView["dataGridViewTextBoxColumn1", usersDataGridView.CurrentCell.RowIndex].Value);
            CurrentUser.Role = Convert.ToInt32(usersDataGridView["dataGridViewTextBoxColumn6", usersDataGridView.CurrentCell.RowIndex].Value);
            FormEdit Edit = new FormEdit(CurrentUser);
            this.Hide();
            Edit.Show();
        }
    }
}
