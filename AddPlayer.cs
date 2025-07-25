//using Dapper;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Data.SqlClient;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using WindowsFormsApp1.Model;

//namespace WindowsFormsApp1
//{

//    public partial class AddPlayer : Form
//    {
//        string action;
//        private string conStr;
//        //public class Player
//        //{
//        //    public int ID { get; set; }
//        //    public string FullName { get; set; }
//        //    public string Country { get; set; }
//        //    public string Contact { get; set; }
//        //    public string Email { get; set; }
//        //    public int? Age { get; set; }
//        //}

//        public AddPlayer()
//        {
//            InitializeComponent();
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
          
//        }

//        private void AddPlayer_Load(object sender, EventArgs e)
//        {

//        }
//        private void save()
//        {
//            try
//            {
//                string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";

//                // Validate required fields
//                if (string.IsNullOrWhiteSpace(textBox1.Text))
//                {
//                    MessageBox.Show("Full Name is required.");
//                    return;
//                }

//                // Create an object to hold form data
//                var player = new
//                {
//                    FullName = textBox1.Text,
//                    Country = textBox4.Text,
//                    Contact = textBox5.Text,
//                    Email = textBox6.Text,
//                    Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
//                };

//                // SQL query to insert data into the Players table
//                string query = @"
//                    INSERT INTO Players (FullName, Country, Contact, Email, Age)
//                    VALUES (@FullName, @Country, @Contact, @Email, @Age)";

//                using (var connection = new SqlConnection(conStr))
//                {
//                    connection.Open();
//                    using (var command = new SqlCommand(query, connection))
//                    {
//                        // Add parameters to prevent SQL injection
//                        command.Parameters.AddWithValue("@FullName", player.FullName);
//                        command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
//                        command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
//                        command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
//                        command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

//                        // Execute the query
//                        int rowsAffected = command.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Player added successfully!");
//                            // Clear form fields
//                            textBox1.Clear();
//                          //  textBox2.Clear();
//                            textBox4.Clear();
//                            textBox5.Clear();
//                            textBox6.Clear();
//                            textBox7.Clear();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Failed to add player. Please try again.");
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"An error occurred: {ex.Message}");
//            }
//        }

      

//        private void button4_Click(object sender, EventArgs e)
//        {
//            ClearFields();
//            action = "edit";
//            FetchPlayerData();
//            FetchDataInForm();
            
//        }

//        private void button5_Click(object sender, EventArgs e)
//        {
//            ClearFields();
//            action = "delete";
//        }

//        private void modify()
//        {
//            try
//            {
//                // Validate required fields
//                if (string.IsNullOrWhiteSpace(textBox1.Text))
//                {
//                    MessageBox.Show("Full Name is required.");
//                    return;
//                }

//                // Validate ID
//                if (comboBox1.SelectedItem == null || !int.TryParse(comboBox1.SelectedValue?.ToString(), out int id) || id <= 0)
//                {
//                    MessageBox.Show("Please select a valid player ID to modify.");
//                    return;
//                }

//                // Validate Email format (if provided)
//                if (!string.IsNullOrWhiteSpace(textBox6.Text))
//                {
//                    string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
//                    if (!Regex.IsMatch(textBox6.Text, emailPattern))
//                    {
//                        MessageBox.Show("Please enter a valid email address.");
//                        return;
//                    }
//                }

//                // Validate Age (if provided)
//                //if (!string.IsNullOrWhiteSpace(textBox7.Text))
//                //{
//                //    if (!int.TryParse(textBox7.Text, out int age) || age <= 0)
//                //    {
//                //        MessageBox.Show("Please enter a valid positive age.");
//                //        return;
//                //    }
//                //}

//                // Validate Contact (if provided)
//                if (!string.IsNullOrWhiteSpace(textBox5.Text))
//                {
//                    string contactPattern = @"^[\d\s\-\+\(\)]+$";
//                    if (!Regex.IsMatch(textBox5.Text, contactPattern))
//                    {
//                        MessageBox.Show("Please enter a valid contact number (digits, spaces, +, -, or parentheses).");
//                        return;
//                    }
//                }

//                // Create an object to hold form data
//                var player = new
//                {
//                    ID = id,
//                    FullName = textBox1.Text,
//                    Country = textBox4.Text,
//                    Contact = textBox5.Text,
//                    Email = textBox6.Text,
//                    Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
//                };

//                // SQL query to update the player
//                string query = @"
//                    UPDATE Players
//                    SET FullName = @FullName, Country = @Country, Contact = @Contact, Email = @Email, Age = @Age
//                    WHERE ID = @ID";

//                using (var connection = new SqlConnection(conStr))
//                {
//                    connection.Open();
//                    using (var transaction = connection.BeginTransaction())
//                    {
//                        try
//                        {
//                            using (var command = new SqlCommand(query, connection, transaction))
//                            {
//                                // Add parameters to prevent SQL injection
//                                command.Parameters.AddWithValue("@ID", player.ID);
//                                command.Parameters.AddWithValue("@FullName", player.FullName);
//                                command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
//                                command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
//                                command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
//                                command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

//                                // Execute the query
//                                int rowsAffected = command.ExecuteNonQuery();

//                                // Commit the transaction
//                                transaction.Commit();

//                                if (rowsAffected > 0)
//                                {
//                                    // Log the operation (for debugging)
//                                    Console.WriteLine($"Player modified: ID={player.ID}, FullName={player.FullName}");
//                                    MessageBox.Show($"Player (ID: {player.ID}) updated successfully!");
//                                    //  ClearFields();
//                                }
//                                else
//                                {
//                                    MessageBox.Show("No player found with the specified ID.");
//                                }
//                            }
//                        }
//                        catch (SqlException ex)
//                        {
//                            transaction.Rollback();
//                            if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
//                            {
//                                MessageBox.Show("A player with this email already exists.");
//                            }
//                            else
//                            {
//                                MessageBox.Show($"Database error: {ex.Message}");
//                            }
//                            return;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"An error occurred: {ex.Message}");
//            }
//        }
//        private void FetchPlayerData()
//        {

//            try
//            {
//                string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";
//                using (IDbConnection db = new System.Data.SqlClient.SqlConnection(conStr))
//                {
//                    db.Open();
//                    var data = db.Query<Player>("SELECT ID, FullName, Country, Contact, Email, Age FROM Players");

//                    if (data != null && data.Any())
//                    {
//                        comboBox1.DataSource = data.ToList();
//                        comboBox1.ValueMember = "ID";
//                        comboBox1.DisplayMember = "ID";
//                        comboBox1.SelectedIndex = -1; // No selection by default
//                    }
//                    else
//                    {
//                        comboBox1.DataSource = null;
//                        MessageBox.Show("No players found.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error fetching player data: {ex.Message}");
//            }
//        }

//        private void FetchDataInForm()
//        {
//            var selectedPlayer = comboBox1.SelectedItem as Player;
//            if (selectedPlayer != null)
//            {
//                // textBox2.Text = selectedPlayer.ID.ToString();
//                textBox1.Text = selectedPlayer.FullName;
//                textBox4.Text = selectedPlayer.Country;
//                textBox5.Text = selectedPlayer.Contact;
//                textBox6.Text = selectedPlayer.Email;
//                textBox7.Text = selectedPlayer.Age?.ToString();
//            }
//            else
//            {
//                ClearFields();
//            }
          

//    }

//        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
//        {

//        }

//        private void button2_Click_1(object sender, EventArgs e)
//        {
//            if (action == "add")
//            {
//                save();

//            }
//            if (action == "edit")
//            {
//               // modify();
//            }
//            if (action == "delete")
//            {


//            }
//        }
//        private void ClearFields()
//        {
//            textBox1.Clear();
           
//            textBox4.Clear();
//            textBox5.Clear();
//            textBox6.Clear();
//            textBox7.Clear();
//            comboBox1.SelectedIndex = -1;
//        }
//        private void button3_Click_1(object sender, EventArgs e)
//        {
//            ClearFields();
//            action = "add";
//        }
//    }
//}



//using Dapper;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Windows.Forms;

//namespace WindowsFormsApp1
//{
//    // Player class to map to Players table
//    public class Player
//    {
//        public int ID { get; set; }
//        public string FullName { get; set; }
//        public string Country { get; set; }
//        public string Contact { get; set; }
//        public string Email { get; set; }
//        public int? Age { get; set; }
//    }

//    public partial class AddPlayer : Form
//    {
//        private string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";
//        private string action;

//        public AddPlayer()
//        {
//            InitializeComponent();
//            FetchPlayerData(); // Load players into ComboBox on form load
//            textBox2.ReadOnly = true; // ID is read-only
//        }

//        private void ClearFields()
//        {
//            textBox1.Clear();
//            textBox2.Clear();
//            textBox4.Clear();
//            textBox5.Clear();
//            textBox6.Clear();
//            textBox7.Clear();
//            comboBox1.SelectedIndex = -1;
//        }

//        private void save()
//        {
//            try
//            {
//                // Validate required fields
//                if (string.IsNullOrWhiteSpace(textBox1.Text))
//                {
//                    MessageBox.Show("Full Name is required.");
//                    return;
//                }

//                var player = new
//                {
//                    FullName = textBox1.Text,
//                    Country = textBox4.Text,
//                    Contact = textBox5.Text,
//                    Email = textBox6.Text,
//                    Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
//                };

//                string query = @"
//                    INSERT INTO Players (FullName, Country, Contact, Email, Age)
//                    VALUES (@FullName, @Country, @Contact, @Email, @Age)";

//                using (var connection = new System.Data.SqlClient.SqlConnection(conStr))
//                {
//                    connection.Open();
//                    using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
//                    {
//                        command.Parameters.AddWithValue("@FullName", player.FullName);
//                        command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
//                        command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
//                        command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
//                        command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

//                        int rowsAffected = command.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Player added successfully!");
//                            ClearFields();
//                            FetchPlayerData(); // Refresh ComboBox
//                        }
//                        else
//                        {
//                            MessageBox.Show("Failed to add player. Please try again.");
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"An error occurred: {ex.Message}");
//            }
//        }

//        private void FetchPlayerData()
//        {
//            try
//            {
//                using (IDbConnection db = new System.Data.SqlClient.SqlConnection(conStr))
//                {
//                    db.Open();
//                    var data = db.Query<Player>("SELECT ID, FullName, Country, Contact, Email, Age FROM Players");

//                    if (data != null && data.Any())
//                    {
//                        comboBox1.DataSource = data.ToList();
//                        comboBox1.ValueMember = "ID";
//                        comboBox1.DisplayMember = "ID";
//                        comboBox1.SelectedIndex = -1; // No selection by default
//                    }
//                    else
//                    {
//                        comboBox1.DataSource = null;
//                        MessageBox.Show("No players found.");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error fetching player data: {ex.Message}");
//            }
//        }

//        private void FetchDataInForm()
//        {
//            var selectedPlayer = comboBox1.SelectedItem as Player;
//            if (selectedPlayer != null)
//            {
//               // textBox2.Text = selectedPlayer.ID.ToString();
//                textBox1.Text = selectedPlayer.FullName;
//                textBox4.Text = selectedPlayer.Country;
//                textBox5.Text = selectedPlayer.Contact;
//                textBox6.Text = selectedPlayer.Email;
//                textBox7.Text = selectedPlayer.Age?.ToString();
//            }
//            else
//            {
//                ClearFields();
//            }
//        }

//        private void comboBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            FetchDataInForm();
//        }

//        private void button4_Click(object sender, EventArgs e)
//        {
//            action = "edit";
//            if (comboBox1.SelectedItem != null)
//            {
//                FetchDataInForm();

//                MessageBox.Show("Ready to edit selected player.");
//            }
//            else
//            {
//                MessageBox.Show("Please select a player to edit.");
//            }
//        }

//        private void button5_Click(object sender, EventArgs e)
//        {
//            action = "delete";
//            if (comboBox1.SelectedItem != null)
//            {
//                FetchDataInForm();
//                MessageBox.Show("Ready to delete selected player.");
//            }
//            else
//            {
//                MessageBox.Show("Please select a player to delete.");
//            }
//        }

//        private void button3_Click(object sender, EventArgs e)
//        {
//            action = "add";
//            ClearFields();
//            MessageBox.Show("Ready to add a new player.");
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            ClearFields();
//            action = null;
//            MessageBox.Show("Fields cleared.");
//        }
//    }
//}








using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1
{
    public class Player
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
    }

    public partial class AddPlayer : Form
    {
        string action;
        private string conStr;

        public AddPlayer()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (action == "add")
            {
                save();
            }
            if (action == "edit")
            {
                modify();
            }
            if (action == "delete")
            {
            }
        }

        private void AddPlayer_Load(object sender, EventArgs e)
        {
        }

        private void save()
        {
            try
            {
                string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";

                // Validate required fields
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Full Name is required.");
                    return;
                }

                // Create an object to hold form data
                var player = new
                {
                    FullName = textBox1.Text,
                    Country = textBox4.Text,
                    Contact = textBox5.Text,
                    Email = textBox6.Text,
                    Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
                };

                // SQL query to insert data into the Players table
                string query = @"
                    INSERT INTO Players (FullName, Country, Contact, Email, Age)
                    VALUES (@FullName, @Country, @Contact, @Email, @Age)";

                using (var connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@FullName", player.FullName);
                        command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Player added successfully!");
                            // Clear form fields
                            textBox1.Clear();
                            textBox4.Clear();
                            textBox5.Clear();
                            textBox6.Clear();
                            textBox7.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add player. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields();
            action = "edit";
            FetchPlayerData();
            FetchDataInForm();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearFields();
            action = "delete";
        }

        private void modify()
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Full Name is required.");
                    return;
                }

                // Validate ID
                if (comboBox1.SelectedItem == null || !int.TryParse(comboBox1.SelectedValue?.ToString(), out int id) || id <= 0)
                {
                    MessageBox.Show("Please select a valid player ID to modify.");
                    return;
                }

                // Validate Email format (if provided)
                if (!string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!Regex.IsMatch(textBox6.Text, emailPattern))
                    {
                        MessageBox.Show("Please enter a valid email address.");
                        return;
                    }
                }

                // Validate Contact (if provided)
                if (!string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    string contactPattern = @"^[\d\s\-\+\(\)]+$";
                    if (!Regex.IsMatch(textBox5.Text, contactPattern))
                    {
                        MessageBox.Show("Please enter a valid contact number (digits, spaces, +, -, or parentheses).");
                        return;
                    }
                }

                // Create an object to hold form data
                var player = new
                {
                    ID = id,
                    FullName = textBox1.Text,
                    Country = textBox4.Text,
                    Contact = textBox5.Text,
                    Email = textBox6.Text,
                    Age = int.TryParse(textBox7.Text, out int age) ? age : (int?)null
                };

                // SQL query to update the player
                string query = @"
                    UPDATE Players
                    SET FullName = @FullName, Country = @Country, Contact = @Contact, Email = @Email, Age = @Age
                    WHERE ID = @ID";

                using (var connection = new SqlConnection(conStr))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var command = new SqlCommand(query, connection, transaction))
                            {
                                // Add parameters to prevent SQL injection
                                command.Parameters.AddWithValue("@ID", player.ID);
                                command.Parameters.AddWithValue("@FullName", player.FullName);
                                command.Parameters.AddWithValue("@Country", (object)player.Country ?? DBNull.Value);
                                command.Parameters.AddWithValue("@Contact", (object)player.Contact ?? DBNull.Value);
                                command.Parameters.AddWithValue("@Email", (object)player.Email ?? DBNull.Value);
                                command.Parameters.AddWithValue("@Age", (object)player.Age ?? DBNull.Value);

                                // Execute the query
                                int rowsAffected = command.ExecuteNonQuery();

                                // Commit the transaction
                                transaction.Commit();

                                if (rowsAffected > 0)
                                {
                                    // Log the operation (for debugging)
                                    Console.WriteLine($"Player modified: ID={player.ID}, FullName={player.FullName}");
                                    MessageBox.Show($"Player (ID: {player.ID}) updated successfully!");
                                    // ClearFields();
                                }
                                else
                                {
                                    MessageBox.Show("No player found with the specified ID.");
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
                            {
                                MessageBox.Show("A player with this email already exists.");
                            }
                            else
                            {
                                MessageBox.Show($"Database error: {ex.Message}");
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void FetchPlayerData()
        {
            try
            {
                string conStr = @"Data Source=SEESAM\SQLEXPRESS;Initial Catalog=project;Integrated Security=True;Encrypt=False";
                using (IDbConnection db = new System.Data.SqlClient.SqlConnection(conStr))
                {
                    db.Open();
                    var data = db.Query<Player>("SELECT ID, FullName, Country, Contact, Email, Age FROM Players");

                    if (data != null && data.Any())
                    {
                        comboBox1.DataSource = data.ToList();
                        comboBox1.ValueMember = "ID";
                        comboBox1.DisplayMember = "ID";
                        comboBox1.SelectedIndex = -1; // No selection by default
                    }
                    else
                    {
                        comboBox1.DataSource = null;
                        MessageBox.Show("No players found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching player data: {ex.Message}");
            }
        }

        private void FetchDataInForm()
        {
            var selectedPlayer = comboBox1.SelectedItem as Player;
            if (selectedPlayer != null)
            {
                // textBox2.Text = selectedPlayer.ID.ToString();
                textBox1.Text = selectedPlayer.FullName ?? string.Empty;
                textBox4.Text = selectedPlayer.Country ?? string.Empty;
                textBox5.Text = selectedPlayer.Contact ?? string.Empty;
                textBox6.Text = selectedPlayer.Email ?? string.Empty;
                textBox7.Text = selectedPlayer.Age?.ToString() ?? string.Empty;
            }
            else
            {
                ClearFields();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FetchDataInForm();
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ClearFields();
            action = "add";
        }
    }
}