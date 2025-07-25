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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            datalink();
        }
        private void datalink()
        {
            try
            {
                //string email = textBox4.Text.Trim();
                //var emailAttribute = new EmailAddressAttribute();

                //if (!emailAttribute.IsValid(email))
                //{
                //    MessageBox.Show("Invalid email format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";
                string query = "INSERT INTO signup (UserName, Name, email, Password, Contact, Address) " +
                               "VALUES (@uname, @name, @email, @password, @contact, @add)";

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uname", textBox8.Text.Trim());    // Username
                        cmd.Parameters.AddWithValue("@name", textBox1.Text.Trim());     // Full Name
                        cmd.Parameters.AddWithValue("@email", textBox3.Text.Trim());    // Email
                        cmd.Parameters.AddWithValue("@password", textBox4.Text.Trim()); // Password
                        cmd.Parameters.AddWithValue("@contact", textBox5.Text.Trim());  // Contact
                        cmd.Parameters.AddWithValue("@add", textBox6.Text.Trim());      // Address

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        con.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("User insertion failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
