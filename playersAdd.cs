using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class playersAdd : Form
    {
        string action;
        private string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";

        public playersAdd()
        {
            InitializeComponent();
            this.Load += playersAdd_Load;
        }

        private void playersAdd_Load(object sender, EventArgs e)
        {
            LoadAllData();
        }

        private void button2_Click(object sender, EventArgs e)
         {
            save();
            LoadAllData();
        //    if (action == "add")
        //    {
        //        save();
        //    }
        //    else if (action == "edit")
        //    {
        //        modify();
        //    }
        //    else if (action == "delete")
        //    {
        //        // delete();
        //    }
        }

        private void save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Full Name is required.");
                    return;
                }

                var player = new
                {
                    FullName = textBox1.Text,
                    Country = textBox4.Text,
                    Contact = textBox5.Text,
                    Email = textBox6.Text,
                    Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
                };

                string query = @"
                    INSERT INTO Players (FullName, Country, Contact, Email, Age)
                    VALUES (@FullName, @Country, @Contact, @Email, @Age)";

                using (var connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", player.FullName);
                        command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Player added successfully!");
                            ClearFields();
                           // FetchPlayerData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add player.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // FetchDataInForm();
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
          //  comboBox1.SelectedIndex = -1;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ClearFields();
            action = "add";
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
          //  action = "edit";
          // FetchPlayerData();
          //  FetchDataInForm();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearFields();
            action = "delete";
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
          //  FetchPlayerData();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            save();
            LoadAllData();
        }

        private void LoadAllData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    string query = @"SELECT ID, FullName, Country, Contact, Email, Age FROM Players";
                    var players = connection.Query<Player>(query).ToList();
                    dataGridView1.DataSource = players;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        connection.Open();
                        string deleteQuery = "DELETE FROM Players WHERE ID = @ID";
                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ID", selectedId);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record deleted successfully!");
                                LoadAllData(); // Reload data after deletion
                            }
                            else
                            {
                                MessageBox.Show("No record deleted.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting data: " + ex.Message);
            }
        }
    }

    // ✅ Required Player model for Dapper and comboBox1 binding
    //public class Player
    //{
    //    public int ID { get; set; }
    //    public string FullName { get; set; }
    //    public string Country { get; set; }
    //    public string Contact { get; set; }
    //    public string Email { get; set; }
    //    public int? Age { get; set; }
    //}
}
