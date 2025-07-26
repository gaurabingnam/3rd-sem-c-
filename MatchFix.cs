//using Dapper;
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Windows.Forms;

//namespace WindowsFormsApp1
//{
//    public partial class MatchFix : Form
//    {
//        private string connectionString = "Data Source=SEESAM\\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";

//        public MatchFix()
//        {
//            InitializeComponent();
//        }

//        private void MatchFix_Load(object sender, EventArgs e)
//        {
//           LoadComboBoxes();
//            fetch();
//            //FetchPlayerData();
//        }

//        private void LoadComboBoxes()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    string query = "SELECT ID FROM Players";
//                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);

//                    if (dt.Rows.Count > 0)
//                    {
//                        // Bind to comboBox3 (Player 1) only
//                        comboBox3.DataSource = dt;
//                        comboBox3.DisplayMember = "ID";
//                        comboBox3.ValueMember = "ID";
//                        comboBox3.SelectedIndex = -1;
//                    }
//                    else
//                    {
//                        MessageBox.Show("No players found in the database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error loading players: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void fetch()
//        {
//            var x = comboBox3.SelectedItem as Player;
//            if (!string.IsNullOrEmpty(comboBox3.Text))
//            {
//                textBox9.Text = x.FullName;
//                textBox8.Text = x.Country;
//                textBox7.Text = x.Age?.ToString();
//            }
//}
////private void FetchPlayerData(int id)
////        {
////            try
////            {
////                using (IDbConnection db = new SqlConnection(connectionString))
////                {
////                    var player = db.QueryFirstOrDefault<Player>("SELECT ID, FullName, Country, Age FROM Players WHERE ID = @ID", new { ID = id });

////                    if (player != null)
////                    {
////                        textBox9.Text = player.FullName ?? "";
////                        textBox8.Text = player.Country ?? "";
////                        textBox7.Text = player.Age?.ToString() ?? "";
////                    }
////                    else
////                    {
////                        textBox9.Text = "";
////                        textBox8.Text = "";
////                        textBox7.Text = "";
////                        MessageBox.Show($"No player found with ID: {id}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
////                    }
////                }
////            }
////            catch (Exception ex)
////            {
////                textBox9.Text = "";
////                textBox8.Text = "";
////                textBox7.Text = "";
////                MessageBox.Show($"Error fetching player: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
////            }
////        }
//        private void panel1_Paint(object sender, PaintEventArgs e)
//        {

//        }
//        private void button1_Click(object sender, EventArgs e)
//        {
//            // Clear all textboxes and reset combo boxes
//            //textBox9.Clear();
//            //textBox8.Clear();
//            //textBox7.Clear();
//            //textBox4.Clear();
//            //textBox5.Clear();
//            //textBox6.Clear();
//            //comboBox3.SelectedIndex = -1;
//            //comboBox2.SelectedIndex = -1;
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            // Placeholder for OK button functionality
//            MessageBox.Show("Match details saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
//        {
//            fetch();
//            if (comboBox3.SelectedItem is Player selectedPlayer)
//            {
//                //FetchPlayerData(selectedPlayer.ID);
//            }
//        }

//        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
//        {

//        }

//        private void button2_Click_1(object sender, EventArgs e)
//        {

//        }
//    }
//}
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

        public MatchFix()
        {
            InitializeComponent();
            comboBox3.SelectedIndexChanged += (s, e) => fetch(comboBox3);
            comboBox2.SelectedIndexChanged += (s, e) => fetch(comboBox2);
        }

        private void MatchFix_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
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
    }
}