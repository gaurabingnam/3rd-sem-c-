

using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

using WindowsFormsApp1.Model;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class MatchFix : Form
    {
        private string connectionString = "Data Source=SEESAM\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";
        //private object lable4;

        public MatchFix()
        {
            InitializeComponent();

            numericUpDown1.Maximum = 11;
            numericUpDown2.Maximum = 11;

            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;

            comboBox3.SelectedIndexChanged += (s, e) => fetch(comboBox3);
            comboBox2.SelectedIndexChanged += (s, e) => fetch(comboBox2);


        }

        private void MatchFix_Load(object sender, EventArgs e)
        {
            LoadMatchResults();
            LoadComboBoxes();
            FormatDataGridView();
        }

        private void LoadComboBoxes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, FullName, Country, Age FROM Players";
                    var players = conn.Query<Player>(query).AsList();

                    if (players.Count > 0)
                    {
                        comboBox3.DataSource = players;
                        comboBox3.DisplayMember = "ID";
                        comboBox3.ValueMember = "ID";
                        comboBox3.SelectedIndex = -1;

                        comboBox2.DataSource = players.ToList(); // Avoid sharing DataSource
                        comboBox2.DisplayMember = "ID";
                        comboBox2.ValueMember = "ID";
                        comboBox2.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show("No players found in the database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading players: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fetch(ComboBox comboBox)
        {
          
            if (comboBox.SelectedItem != null)
            {
                try
                {
                    // Get the selected Player object
                    var selectedPlayer = comboBox.SelectedItem as Player;
                    if (selectedPlayer != null && selectedPlayer.ID > 0)
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "SELECT FullName, Country, Age FROM Players WHERE ID = @ID";
                            // Use the ID property directly from the Player object
                            var player = conn.QuerySingleOrDefault<Player>(query, new { ID = selectedPlayer.ID });

                            if (player != null)
                            {
                                if (comboBox == comboBox3)
                                {
                                    textBox9.Text = player.FullName;
                                    textBox8.Text = player.Country;
                                    textBox7.Text = player.Age?.ToString();
                                }

                                else if (comboBox == comboBox2)
                                {
                                    // Use different textboxes for comboBox2
                                    textBox4.Text = player.FullName;
                                    textBox5.Text = player.Country;
                                    textBox6.Text = player.Age?.ToString();
                                }
                            }
                            else
                            {
                                if (comboBox == comboBox3)
                                {
                                    textBox9.Text = string.Empty;
                                    textBox8.Text = string.Empty;
                                    textBox7.Text = string.Empty;
                                }
                                else if (comboBox == comboBox2)
                                {
                                    textBox4.Text = string.Empty;
                                    textBox5.Text = string.Empty;
                                    textBox6.Text = string.Empty;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (comboBox == comboBox3)
                        {
                            textBox9.Text = string.Empty;
                            textBox8.Text = string.Empty;
                            textBox7.Text = string.Empty;
                        }
                        else if (comboBox == comboBox2)
                        {
                            textBox4.Text = string.Empty;
                            textBox5.Text = string.Empty;
                            textBox6.Text = string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching player data: {ex.Message}\nSelectedValue Type: {comboBox3.SelectedValue?.GetType().Name}\nSelectedItem Type: {comboBox3.SelectedItem?.GetType().Name}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (comboBox == comboBox3)
                {
                    textBox9.Text = string.Empty;
                    textBox8.Text = string.Empty;
                    textBox7.Text = string.Empty;
                }
                else if (comboBox == comboBox2)
                {
                    textBox4.Text = string.Empty;
                    textBox5.Text = string.Empty;
                    textBox6.Text = string.Empty;
                }
            }
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
         
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 11)
            {
                label4.Visible = true;
                numericUpDown2.Enabled = false;

                var winner = comboBox3.SelectedItem as Player;
                var loser = comboBox2.SelectedItem as Player;

                if (winner != null && loser != null)
                    SaveMatchToDatabase(winner, 11, loser, (int)numericUpDown2.Value); // <-- Winner: Player1, Loser: Player2
            }
            else
            {
                label4.Visible = false;
                numericUpDown2.Enabled = true;
            }
        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {
         
            if (numericUpDown2.Value == 11)
            {
                label4.Visible = true;
                numericUpDown1.Enabled = false;

                var winner = comboBox2.SelectedItem as Player;
                var loser = comboBox3.SelectedItem as Player;

                if (winner != null && loser != null)
                    SaveMatchToDatabase(winner, 11, loser, (int)numericUpDown1.Value); // <-- Winner: Player2, Loser: Player1
            }
            else
            {
                label4.Visible = false;
                numericUpDown1.Enabled = true;
            }
        }
        
        //private void SaveWinnerToDatabase(Player player, int score)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();
        //            string insertQuery = "INSERT INTO MatchResults (PlayerID, FullName, Score, Result) VALUES (@PlayerID, @FullName, @Score, @Result)";
        //            conn.Execute(insertQuery, new
        //            {
        //                PlayerID = player.ID,
        //                FullName = player.FullName,
        //                Score = score,
        //                Result = "Winner"
        //            });
        //        }

        //        MessageBox.Show($"{player.FullName} saved as Winner!", "Match Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error saving match result: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void SaveMatchToDatabase(Player winner, int winnerScore, Player loser, int loserScore)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO MatchResults (PlayerID, FullName, Score, Result) VALUES (@PlayerID, @FullName, @Score, @Result)";

                    // Insert Winner
                    conn.Execute(insertQuery, new
                    {
                        PlayerID = winner.ID,
                        FullName = winner.FullName,
                        Score = winnerScore,
                        Result = "Winner"
                    });

                    // Insert Loser
                    conn.Execute(insertQuery, new
                    {
                        PlayerID = loser.ID,
                        FullName = loser.FullName,
                        Score = loserScore,
                        Result = "Loser"
                    });
                }

                MessageBox.Show($"{winner.FullName} (Winner) and {loser.FullName} (Loser) saved!", "Match Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving match result: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
               // Clear TextBoxes
    textBox4.Clear();
    textBox5.Clear();
    textBox6.Clear();
    textBox7.Clear();
    textBox8.Clear();
    textBox9.Clear();

    // Reset ComboBoxes
    comboBox2.SelectedIndex = -1;
    comboBox3.SelectedIndex = -1;

    // Reset NumericUpDowns
    numericUpDown1.Value = 0;
    numericUpDown2.Value = 0;
    numericUpDown1.Enabled = true;
    numericUpDown2.Enabled = true;

    // Hide label showing winner (if applicable)
    label4.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadMatchResults()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM MatchResults ORDER BY ID DESC"; // optional: order by newest first
                    var results = conn.Query(query).ToList();
                    dataGridView1.DataSource = results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading match results: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormatDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

    }
}


