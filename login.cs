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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";

                var user = new
                {
                    Username = textBox1.Text,
                    Password = textBox2.Text
                };

                using (var connection = new SqlConnection(conStr))
                {
                    string query = "SELECT COUNT(1) FROM signup WHERE Username = @Username AND Password = @Password";
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        int count = (int)command.ExecuteScalar();

                        if (count == 1)
                        {
                            MessageBox.Show("Login successful!");
                            this.Hide();
                            var dashboard = new Dadhboard();
                            dashboard.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Username or Password is incorrect.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dash = new Form1();
            this.Hide();
            dash.ShowDialog();
            this.Show();
        }
    }
}
