using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace EDP_Act4_Fin
{
    public partial class Account : Form
    {
        private DatabaseManager dbManager;
        String loggedEmail = Form1.login_email;
        public Account()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            email_txtbox.Enabled = false;
        }

        private void Account_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            email_txtbox.Text = loggedEmail;

            if (dbManager.OpenConnection())
            {
                try
                {
                    string sqlRetrieveUserInfo = "SELECT username, phone, bday, region, province, city, barangay, postal_code FROM accounts WHERE email = @email";
                    MySqlCommand cmdRetrieveUserInfo = new MySqlCommand(sqlRetrieveUserInfo, dbManager.connection);
                    cmdRetrieveUserInfo.Parameters.AddWithValue("@email", loggedEmail);
                    MySqlDataReader reader = cmdRetrieveUserInfo.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate textboxes with retrieved user information
                        username_txtbox.Text = reader.GetString("username");
                        phone_txtbox.Text = reader.GetString("phone");
                        DateTime bday = reader.GetDateTime("bday");
                        birthday.Value = bday;
                        region_txtbox.Text = reader.GetString("region");
                        province_txtbox.Text = reader.GetString("province");
                        city_txtbox.Text = reader.GetString("city");
                        barangay_txtbox.Text = reader.GetString("barangay");
                        postal_txtbox.Text = reader.GetString("postal_code");
                    }
                    else
                    {
                        MessageBox.Show("User information not found in the database");
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                   
                    dbManager.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
        }


        private void save_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = birthday.Value;

            // Convert the DateTime value to the format understood by MySQL
            string formattedDate = selectedDate.ToString("yyyy-MM-dd");
            if (dbManager.OpenConnection())
            {
                try
                {
                    // Check if email exists
                    string sqlCheckEmail = "SELECT COUNT(*) FROM accounts WHERE email = @loggedEmail";
                    MySqlCommand cmdCheckEmail = new MySqlCommand(sqlCheckEmail, dbManager.connection);
                    cmdCheckEmail.Parameters.AddWithValue("@loggedEmail", loggedEmail);
                    int emailCount = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());

                    if (emailCount > 0)
                    {
                        // Email exists, proceed with update
                        string sqlUpdateInfo = "UPDATE accounts SET username = @username, phone = @phone, bday = @bday, region = @region, province = @province, city = @city, barangay = @barangay, postal_code = @postal_code WHERE email = @loggedEmail";
                        MySqlCommand cmdUpdateInfo = new MySqlCommand(sqlUpdateInfo, dbManager.connection);
                        cmdUpdateInfo.Parameters.AddWithValue("@username", username_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@phone", phone_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@bday", formattedDate);
                        cmdUpdateInfo.Parameters.AddWithValue("@region", region_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@province", province_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@city", city_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@barangay", barangay_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@postal_code", postal_txtbox.Text);
                        cmdUpdateInfo.Parameters.AddWithValue("@loggedEmail", loggedEmail);

                        int rowsAffected = cmdUpdateInfo.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Information updated successfully");
                        }
                        else
                        {
                            MessageBox.Show("No rows updated");
                        }
                    }
                    else
                    {
                        // Email doesn't exist in the database
                        MessageBox.Show("Email not found in the database");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating information: " + ex.Message);
                }
                finally
                {
                    dbManager.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
        }

        private void back_btn_Click(object sender, EventArgs e)
        {
            var dashboard = new Dashboard();
            this.Hide();
            dashboard.Show();
        }
    }
}
