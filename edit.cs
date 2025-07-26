using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class edit : Form
    {
        private string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";
        private string connectionString;

      
        public edit()
        {
            InitializeComponent();
            this.Load += edit_Load;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged; // ✅ attach event handler
        }

        private void edit_Load(object sender, EventArgs e)
        {
            LoadPlayerIDs();
            LoadAllData();
        }

        private void LoadPlayerIDs()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(conStr))
                {
                    var players = db.Query<Player>("SELECT ID, FullName FROM Players").ToList();
                    comboBox1.DataSource = players;
                    comboBox1.DisplayMember = "ID";
                    comboBox1.ValueMember = "ID";
                    comboBox1.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading player IDs: " + ex.Message);
            }
        }

        // ✅ Triggered when a new ID is selected from ComboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // ✅ Fetch and fill player data
        private void FetchPlayerData(int id)
        {
            LoadAllData();
            try
            {
                using (IDbConnection db = new SqlConnection(conStr))
                {
                    var player = db.QueryFirstOrDefault<Player>("SELECT * FROM Players WHERE ID = @ID", new { ID = id });

                    if (player != null)
                    {
                        textBox1.Text = player.FullName ?? "";
                        textBox4.Text = player.Country ?? "";
                        textBox5.Text = player.Contact ?? "";
                        textBox6.Text = player.Email ?? "";
                        textBox7.Text = player.Age?.ToString() ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching player: " + ex.Message);
            }
        }

        // ✅ Save/Update logic
        private void button2_Click(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem is Player selectedPlayer))
            {
                MessageBox.Show("Select a valid player.");
                return;
            }

            // Email validation
            if (!string.IsNullOrWhiteSpace(textBox6.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(textBox6.Text, emailPattern))
                {
                    MessageBox.Show("Invalid email format.");
                    return;
                }
            }

            // Contact validation
            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                string contactPattern = @"^[\d\s\-\+\(\)]+$";
                if (!Regex.IsMatch(textBox5.Text, contactPattern))
                {
                    MessageBox.Show("Invalid contact number.");
                    return;
                }
            }

            var updatedPlayer = new
            {
                ID = selectedPlayer.ID,
                FullName = textBox1.Text.Trim(),
                Country = textBox4.Text.Trim(),
                Contact = textBox5.Text.Trim(),
                Email = textBox6.Text.Trim(),
                Age = int.TryParse(textBox7.Text.Trim(), out int age) ? age : (int?)null
            };

            try
            {
                string query = @"
                    UPDATE Players
                    SET FullName = @FullName,
                        Country = @Country,
                        Contact = @Contact,
                        Email = @Email,
                        Age = @Age
                    WHERE ID = @ID";

                using (var db = new SqlConnection(conStr))
                {
                    int result = db.Execute(query, updatedPlayer);

                    if (result > 0)
                    {
                        MessageBox.Show("Player updated successfully!");
                        LoadPlayerIDs(); // reload list
                        FetchPlayerData(updatedPlayer.ID); // reload selected data
                    }
                    else
                    {
                        MessageBox.Show("Failed to update player.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving player: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is Player selectedPlayer)
            {
                FetchPlayerData(selectedPlayer.ID);
            }
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

    }

  
}
