using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormEdit : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-4CN6F62\SQLEXPRESS;Initial Catalog=Crud;Integrated Security=True");
        SqlCommand cum;
        User[] CurrentUsers = new User[0];

        public FormEdit(User newUser)
        {
            InitializeComponent();

            string Reader = "SELECT * FROM Users WHERE Login=@Login";
            SqlCommand Command = new SqlCommand(Reader, con);
            Command.Parameters.AddWithValue("@Login", newUser.Login);
            con.Open();
            SqlDataReader reader = Command.ExecuteReader();
            while (reader.Read())
            {
                BoxLogin.Text = reader["Login"].ToString();
                BoxPassword.Text = reader["Password"].ToString();
                BoxName.Text = (string)reader["Fname"];
                BoxPatronymic.Text = (string)reader["Mname"];
                BoxSurname.Text = (string)reader["Lname"];
            }
            reader.Close();
            con.Close();

            string[] items = {"Администратор","Менеджер","Кладовщик" };
            BoxRole.Items.AddRange(items);
            BoxRole.SelectedIndex = newUser.Role - 1;
        }

        private void BtnBackWards_Click(object sender, EventArgs e)
        {
            MainForm Form = new MainForm();
            this.Hide();
            Form.Show();
        }

        private void FormCE_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                cum = new SqlCommand("UPDATE Users SET Login=@Login, Password=@Password, Fname=@Fname, Mname=@Mname, Lname=@Lname, Role=@Role WHERE Login = @Login", con);
                cum.Parameters.AddWithValue("@login", BoxLogin.Text);
                cum.Parameters.AddWithValue("@Password", BoxPassword.Text);
                cum.Parameters.AddWithValue("@Fname", BoxName.Text);
                cum.Parameters.AddWithValue("@Mname", BoxPatronymic.Text);
                cum.Parameters.AddWithValue("@Lname", BoxSurname.Text);
                cum.Parameters.AddWithValue("@Role", BoxRole.SelectedIndex + 1);
                con.Open();
                cum.ExecuteNonQuery();
                Change();
            }

            catch (Exception)
            {
                MessageBox.Show("Данный логин занят!", "Warning");
            }

            finally
            {
                con.Close();
            }
        }

        private async void Change()
        {
            BtnAdd.Enabled = false;
            BtnAdd.Text = "Данные изменены";
            await Task.Delay(2000);
            BtnAdd.Enabled = true;
            BtnAdd.Text = "Изменить";
        }
    }
}
