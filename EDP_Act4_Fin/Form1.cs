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
using System.Security.Cryptography;


namespace EDP_Act4_Fin
{
    public partial class Form1 : Form
    {
        public static string login_email;

        private DatabaseManager dbManager;
        public Form1()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //BackgroundImage = System.Drawing.Image.FromFile("D:\\Downloads\\Sterriton games magazine (1)\\1.png");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var signup_form = new signup();
            signup_form.Show();
            this.Hide();

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string email = email_txtbox.Text;
            login_email = email_txtbox.Text;
            string password = password_txtbox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dbManager.OpenConnection())
            {
                try
                {
                    // Retrieve password and status from the database based on the email
                    string sqlRetrieveData = "SELECT password, status, role FROM accounts WHERE email = @email";
                    MySqlCommand cmdRetrieveData = new MySqlCommand(sqlRetrieveData, dbManager.connection);
                    cmdRetrieveData.Parameters.AddWithValue("@email", email);
                    MySqlDataReader reader = cmdRetrieveData.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedPasswordHash = reader["password"].ToString();
                        string userStatus = reader["status"].ToString();
                        string userRole = reader["role"].ToString();

                        if (userStatus == "Active")
                        {
                            // Hash the entered password
                            HashAlgorithm algorithm = SHA256.Create();
                            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                            byte[] hashBytes = algorithm.ComputeHash(inputBytes);
                            string enteredPasswordHash = BitConverter.ToString(hashBytes).Replace("-", "");

                            // Compare the hashed entered password with the stored hashed password
                            if (storedPasswordHash == enteredPasswordHash)
                            {
                                // Redirect users based on their role
                                if (userRole == "Admin")
                                {
                                    var dashboard = new Dashboard();
                                    this.Hide();
                                    dashboard.Show();
                                }
                                else if (userRole == "User")
                                {
                                    var homepage = new Homepage();
                                    this.Hide();
                                    homepage.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Unknown user role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Incorrect email or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("User is inactive", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("User not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Login failed: " + ex.Message);
                }
                finally
                {
                    // Close the reader and connection
                    dbManager.CloseConnection();
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
        }


        private void pw_recover_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var passwordrecovery = new PasswordRecovery();
            passwordrecovery.Show();
            this.Hide();
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
    }
}
