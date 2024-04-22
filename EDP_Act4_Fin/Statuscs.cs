using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace EDP_Act4_Fin
{
    public partial class Status : Form
    {
        private DataTable originalDataTable;
        private DatabaseManager dbManager;

        public Status()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void Status_Load(object sender, EventArgs e)
        {
            // Set background image layout
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Load data into DataGridView
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                // Open database connection
                if (dbManager.OpenConnection())
                {
                    // Create SQL query to select all users
                    string sqlQuery = "SELECT * FROM accounts";
                    

                    // Create MySqlCommand object with SQL query and connection
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, dbManager.connection);

                    // Create DataAdapter to fetch data from database
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    // Create DataTable to store the fetched data
                    originalDataTable = new DataTable();

                    // Fill the DataTable with data from the DataAdapter
                    adapter.Fill(originalDataTable);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = originalDataTable;

                    // Hide the password column
                    dataGridView1.Columns["password"].Visible = false;

                    // Close database connection
                    dbManager.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Failed to connect to database");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user data: " + ex.Message);
            }
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /*private void search_txtbox_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = search_txtbox.Text.Trim().ToLower();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string username = row.Cells["username"].Value.ToString().ToLower();

                // Check if the username contains the search term
                if (username.Contains(searchTerm))
                {
                    row.Visible = true; // Show the row if it matches the search term
                }
                else
                {
                    row.Visible = false; // Hide the row if it does not match the search term
                }
            }
        }*/

        private void search_txtbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the search keyword from the TextBox
                string keyword = search_txtbox.Text.Trim().ToLower();

                // Clear the current selection to avoid InvalidOperationException
                dataGridView1.ClearSelection();

                // Filter the DataGridView based on the search keyword
                if (!string.IsNullOrEmpty(keyword))
                {
                    // Create a DataView based on the original DataTable
                    DataView dataView = new DataView(originalDataTable);

                    // Apply a RowFilter to dynamically filter the data
                    dataView.RowFilter = $"username LIKE '%{keyword}%'";

                    // Assign the DataView to the DataGridView
                    dataGridView1.DataSource = dataView.ToTable();
                }
                else
                {
                    // If the search keyword is empty, reset the DataGridView to the original data
                    dataGridView1.DataSource = originalDataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void activate_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the value of the ID column from the selected row
                int userId = Convert.ToInt32(selectedRow.Cells["id_account"].Value);

                // Update the status of the selected user to "Active" in the database
                if (UpdateUserStatus(userId, "Active"))
                {
                    MessageBox.Show("User activated successfully.");
                    // Refresh the DataGridView to reflect the changes
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Failed to activate user.");
                }
            }
            else
            {
                MessageBox.Show("Please select a user to activate.");
            }
        }

        private bool UpdateUserStatus(int userId, string status)
        {

            try
            {
                if (dbManager.OpenConnection())
                {
                    string sqlUpdateStatus = "UPDATE accounts SET status = @status WHERE id_account = @userId";
                    MySqlCommand cmdUpdateStatus = new MySqlCommand(sqlUpdateStatus, dbManager.connection);
                    cmdUpdateStatus.Parameters.AddWithValue("@status", status);
                    cmdUpdateStatus.Parameters.AddWithValue("@userId", userId);
                    int rowsAffected = cmdUpdateStatus.ExecuteNonQuery();
                    dbManager.CloseConnection();
                    return rowsAffected > 0; // Check if any rows were affected
                }
                else
                {
                    MessageBox.Show("Failed to connect to database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user status: " + ex.Message);
                return false;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Populate textboxes and combobox with the details from the selected row
                email_txtbox.Text = selectedRow.Cells["email"].Value.ToString();
                username_txtbox.Text = selectedRow.Cells["username"].Value.ToString();
                phone_txtbox.Text = selectedRow.Cells["phone"].Value.ToString();

                // Check if the bday value is DBNull.Value before converting it to DateTime
                if (selectedRow.Cells["bday"].Value != DBNull.Value)
                {
                    DateTime selectedDate = Convert.ToDateTime(selectedRow.Cells["bday"].Value);
                    string formattedDate = selectedDate.ToString("yyyy-MM-dd");
                    birthday.Value = DateTime.ParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                else
                {
                    // Handle the case when bday is null (e.g., set a default value for the DateTimePicker)
                    birthday.Value = DateTime.Today; // You can set any default value here
                }

                region_txtbox.Text = selectedRow.Cells["region"].Value.ToString();
                province_txtbox.Text = selectedRow.Cells["province"].Value.ToString();
                city_txtbox.Text = selectedRow.Cells["city"].Value.ToString();
                barangay_txtbox.Text = selectedRow.Cells["barangay"].Value.ToString();
                postal_txtbox.Text = selectedRow.Cells["postal_code"].Value.ToString();
                role_comBox.Text = selectedRow.Cells["role"].Value.ToString();

                // Fetch password from the database based on the selected user ID
                string password = GetPasswordFromDatabase(Convert.ToInt32(selectedRow.Cells["id_account"].Value));

                // Check if the password is not null or empty
                if (!string.IsNullOrEmpty(password))
                {
                    // Set the password textbox value
                    password_txtbox.Text = password;
                    // Disable the password textbox
                    password_txtbox.Enabled = false;
                }
                else
                {
                    // Clear the password textbox value
                    password_txtbox.Clear();
                    // Enable the password textbox
                    password_txtbox.Enabled = true;
                }
            }
        }


        private string GetPasswordFromDatabase(int userId)
        {
            try
            {
                string password = null;

                if (dbManager.OpenConnection())
                {
                    string sqlQuery = "SELECT password FROM accounts WHERE id_account = @userId";
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, dbManager.connection);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    object result = cmd.ExecuteScalar();

                    // Check if the result is not null and convert it to string
                    if (result != null)
                    {
                        password = result.ToString();
                    }

                    dbManager.CloseConnection();
                }

                return password;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching password from database: " + ex.Message);
                return null;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the value of the ID column from the selected row
                int userId = Convert.ToInt32(selectedRow.Cells["id_account"].Value);

                // Update the status of the selected user to "Inactive" in the database
                if (UpdateUserStatus(userId, "Inactive"))
                {
                    MessageBox.Show("User deactivated successfully.");
                    // Refresh the DataGridView to reflect the changes
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Failed to deactivate user.");
                }
            }
            else
            {
                MessageBox.Show("Please select a user to deactivate.");
            }
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            var dashboard = new Dashboard();
            this.Hide();
            dashboard.Show();
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Get the value of the ID column from the selected row
                int userId = Convert.ToInt32(selectedRow.Cells["id_account"].Value);
                // Retrieve values from textboxes
                string username = username_txtbox.Text;
                string phone = phone_txtbox.Text;
                DateTime bday = birthday.Value;
                string region = region_txtbox.Text;
                string province = province_txtbox.Text;
                string city = city_txtbox.Text;
                //string password = GetSHA256Hash(password_txtbox.Text);
                string barangay = barangay_txtbox.Text;
                string postalCode = postal_txtbox.Text;

                // Retrieve selected role from combobox
                string role = role_comBox.SelectedItem.ToString();

                // Update user details in the database
                if (UpdateUserDetails(userId, username, phone, bday, region, province, city, barangay, postalCode, role))
                {
                    MessageBox.Show("User details updated successfully.");
                    // Refresh the DataGridView to reflect the changes
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Failed to update user details.");
                }
            }
            else
            {
                MessageBox.Show("Please select a user to update.");
            }
        }

        private bool UpdateUserDetails(int userId, string username, string phone, DateTime bday, string region, string province, string city, string barangay, string postalCode, string role)
        {
            try
            {
                if (dbManager.OpenConnection())
                {
                    string sqlUpdateDetails = "UPDATE accounts SET username = @username, phone = @phone, bday = @bday, region = @region, province = @province, city = @city, barangay = @barangay, postal_code = @postalCode, role = @role WHERE id_account = @userId";
                    MySqlCommand cmdUpdateDetails = new MySqlCommand(sqlUpdateDetails, dbManager.connection);
                    cmdUpdateDetails.Parameters.AddWithValue("@username", username);
                    cmdUpdateDetails.Parameters.AddWithValue("@phone", phone);
                    cmdUpdateDetails.Parameters.AddWithValue("@bday", bday);
                    cmdUpdateDetails.Parameters.AddWithValue("@region", region);
                    cmdUpdateDetails.Parameters.AddWithValue("@province", province);
                    cmdUpdateDetails.Parameters.AddWithValue("@city", city);
                    cmdUpdateDetails.Parameters.AddWithValue("@barangay", barangay);
                    cmdUpdateDetails.Parameters.AddWithValue("@postalCode", postalCode);
                    cmdUpdateDetails.Parameters.AddWithValue("@role", role);
                    cmdUpdateDetails.Parameters.AddWithValue("@userId", userId);
                    int rowsAffected = cmdUpdateDetails.ExecuteNonQuery();
                    dbManager.CloseConnection();
                    return rowsAffected > 0; // Check if any rows were affected
                }
                else
                {
                    MessageBox.Show("Failed to connect to database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user details: " + ex.Message);
                return false;
            }
        }

        private string GetSHA256Hash(string input)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = algorithm.ComputeHash(inputBytes);
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "");
            return hashString;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (password_txtbox.PasswordChar == '*')
            {
                password_txtbox.PasswordChar = '\0';
            }
            else
            {
                password_txtbox.PasswordChar = '*';
            }
        }

        private bool AddNewUser(string email, string username, string phone, DateTime bday, string region, string province, string city, string barangay, string postalCode, string role, string password)
        {
            // Validate email format
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if the email already exists in the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["email"].Value != null && row.Cells["email"].Value.ToString().Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Email already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            // Hash the password
            string hashedPassword = GetSHA256Hash(password);

            try
            {
                if (dbManager.OpenConnection())
                {
                    string sqlInsertUser = "INSERT INTO accounts (email, username, phone, password, bday, region, province, city, barangay, postal_code, role) VALUES (@email, @username, @phone, @password, @bday, @region, @province, @city, @barangay, @postalCode, @role)";
                    MySqlCommand cmdInsertUser = new MySqlCommand(sqlInsertUser, dbManager.connection);
                    cmdInsertUser.Parameters.AddWithValue("@email", email);
                    cmdInsertUser.Parameters.AddWithValue("@username", username);
                    cmdInsertUser.Parameters.AddWithValue("@phone", phone);
                    cmdInsertUser.Parameters.AddWithValue("@password", hashedPassword);
                    cmdInsertUser.Parameters.AddWithValue("@bday", bday);
                    cmdInsertUser.Parameters.AddWithValue("@region", region);
                    cmdInsertUser.Parameters.AddWithValue("@province", province);
                    cmdInsertUser.Parameters.AddWithValue("@city", city);
                    cmdInsertUser.Parameters.AddWithValue("@barangay", barangay);
                    cmdInsertUser.Parameters.AddWithValue("@postalCode", postalCode);
                    cmdInsertUser.Parameters.AddWithValue("@role", role);
                    int rowsAffected = cmdInsertUser.ExecuteNonQuery();
                    dbManager.CloseConnection();
                    return rowsAffected > 0; // Check if any rows were affected
                }
                else
                {
                    MessageBox.Show("Failed to connect to database");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding new user: " + ex.Message);
                return false;
            }
        }


        // Method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private void add_btn_Click(object sender, EventArgs e)
        {
            // Retrieve values from textboxes
            string email = email_txtbox.Text;
            string username = username_txtbox.Text;
            string phone = phone_txtbox.Text;
            DateTime bday = birthday.Value;
            string region = region_txtbox.Text;
            string province = province_txtbox.Text;
            string city = city_txtbox.Text;
            string pw = password_txtbox.Text;
            string password = password_txtbox.Text; // Hash the password
            string barangay = barangay_txtbox.Text;
            string postalCode = postal_txtbox.Text;

            // Retrieve selected role from combobox

            // Validate password length
            if (pw.Length < 8)
            {
                MessageBox.Show("Password length should be at least 8 characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string role = role_comBox.SelectedItem.ToString();

            // Insert new user details into the database
            if (AddNewUser(email,username, phone, bday, region, province, city, barangay, postalCode, role, password))
            {
                MessageBox.Show("New user added successfully.");
                // Refresh the DataGridView to reflect the changes
                LoadUserData();
            }
            else
            {
                //MessageBox.Show("Failed to add new user.");
            }
        }

    }
}
