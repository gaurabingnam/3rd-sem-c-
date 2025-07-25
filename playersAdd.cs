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

        //private void modify()
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(textBox1.Text))
        //        {
        //            MessageBox.Show("Full Name is required.");
        //            return;
        //        }

        //        if (comboBox1.SelectedItem == null || !(comboBox1.SelectedItem is Player selectedPlayer))
        //        {
        //            MessageBox.Show("Please select a valid player ID to modify.");
        //            return;
        //        }

        //        if (!string.IsNullOrWhiteSpace(textBox6.Text))
        //        {
        //            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        //            if (!Regex.IsMatch(textBox6.Text, emailPattern))
        //            {
        //                MessageBox.Show("Invalid email format.");
        //                return;
        //            }
        //        }

        //        if (!string.IsNullOrWhiteSpace(textBox5.Text))
        //        {
        //            string contactPattern = @"^[\d\s\-\+\(\)]+$";
        //            if (!Regex.IsMatch(textBox5.Text, contactPattern))
        //            {
        //                MessageBox.Show("Invalid contact number.");
        //                return;
        //            }
        //        }

        //        var player = new
        //        {
        //            ID = selectedPlayer.ID,
        //            FullName = textBox1.Text,
        //            Country = textBox4.Text,
        //            Contact = textBox5.Text,
        //            Email = textBox6.Text,
        //            Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
        //        };

        //        string query = @"
        //            UPDATE Players
        //            SET FullName = @FullName, Country = @Country, Contact = @Contact, Email = @Email, Age = @Age
        //            WHERE ID = @ID";

        //        using (var connection = new SqlConnection(conStr))
        //        {
        //            connection.Open();
        //            using (var transaction = connection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    using (var command = new SqlCommand(query, connection, transaction))
        //                    {
        //                        command.Parameters.AddWithValue("@ID", player.ID);
        //                        command.Parameters.AddWithValue("@FullName", player.FullName);
        //                        command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
        //                        command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
        //                        command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
        //                        command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

        //                        int rowsAffected = command.ExecuteNonQuery();
        //                        transaction.Commit();

        //                        if (rowsAffected > 0)
        //                        {
        //                            MessageBox.Show("Player updated successfully!");
        //                            ClearFields();
        //                            FetchPlayerData();
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("No player found with that ID.");
        //                        }
        //                    }
        //                }
        //                catch (SqlException ex)
        //                {
        //                    transaction.Rollback();
        //                    if (ex.Number == 2627 || ex.Number == 2601)
        //                    {
        //                        MessageBox.Show("Email already exists.");
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show($"Database error: {ex.Message}");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}");
        //    }
        //}

        //private void FetchPlayerData()
        //{
        //    try
        //    {
        //        using (IDbConnection db = new SqlConnection(conStr))
        //        {
        //            db.Open();
        //            var data = db.Query<Player>("SELECT ID, FullName, Country, Contact, Email, Age FROM Players").ToList();

        //            if (data.Any())
        //            {
        //                comboBox1.DataSource = data;
        //                comboBox1.ValueMember = "ID";
        //                comboBox1.DisplayMember = "ID";
        //                comboBox1.SelectedIndex = -1;
        //            }
        //            else
        //            {
        //                comboBox1.DataSource = null;
        //                MessageBox.Show("No players found.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error fetching players: {ex.Message}");
        //    }
        //}

        //private void FetchDataInForm()
        //{
        //    if (comboBox1.SelectedItem is Player player)
        //    {
        //        textBox1.Text = player.FullName ?? string.Empty;
        //        textBox4.Text = player.Country ?? string.Empty;
        //        textBox5.Text = player.Contact ?? string.Empty;
        //        textBox6.Text = player.Email ?? string.Empty;
        //        textBox7.Text = player.Age?.ToString() ?? string.Empty;
        //    }
        //}

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
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
