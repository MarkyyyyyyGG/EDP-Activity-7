using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace EDP_Act4_Fin
{
    public partial class signup : Form
    {

        private DatabaseManager dbManager;
        public signup()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //BackgroundImage = System.Drawing.Image.FromFile("D:\\Downloads\\Sterriton games magazine (1)\\1.png");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

        }

        //---------------------------- SIGN UP BUTTON ----------------------------------------//
        private void button1_Click(object sender, EventArgs e)
        {
            string pw = password_txtbox.Text;
            string email = email_txtbox.Text;

            if (pw.Length < 8)
            {
                MessageBox.Show("Password too short!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                password_txtbox.Text = null;
                return;
            }
            if (!email.Contains("@"))
            {
                MessageBox.Show("Invalid Email!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                email_txtbox.Text = null;
                return;
            }

            if (string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dbManager.OpenConnection())
            {
                try
                {
                    // Check if email already exists
                    string sqlCheckEmail = "SELECT COUNT(*) FROM accounts WHERE email = @email";
                    MySqlCommand cmdCheckEmail = new MySqlCommand(sqlCheckEmail, dbManager.connection);
                    cmdCheckEmail.Parameters.AddWithValue("@email", email);
                    int emailCount = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());

                    if (emailCount > 0)
                    {
                        MessageBox.Show("Email already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        email_txtbox.Text = null;
                        password_txtbox.Text = null;
                        dbManager.CloseConnection();
                        return;
                    }

                    // Perform signup operation
                    HashAlgorithm algorithm = SHA256.Create();
                    byte[] inputBytes = Encoding.UTF8.GetBytes(pw);
                    byte[] hashBytes = algorithm.ComputeHash(inputBytes);
                    string hashString = BitConverter.ToString(hashBytes).Replace("-", "");

                    String active = "Active";
                    String role = "User";


                    string sql = "INSERT INTO accounts(email, password, status, role) VALUES(@email, @password, @status, @role)";
                    MySqlCommand cmd = new MySqlCommand(sql, dbManager.connection);
                    cmd.Parameters.AddWithValue("@email", email_txtbox.Text);
                    cmd.Parameters.AddWithValue("@status", active);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.Parameters.AddWithValue("@password", hashString);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User registered successfully!");
                    email_txtbox.Text = null;
                    password_txtbox.Text = null;

                    var signin = new Form1();
                    this.Hide();
                    signin.Show();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Registration failed: " + ex.Message);
                }
                dbManager.CloseConnection();
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
        }


        // Method to generate a random salt
        /*private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // You can adjust the size of the salt as needed
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Method to hash with SHA-256
        private byte[] HashWithSHA256(byte[] inputBytes)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(inputBytes);
            }
        }*/

 

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var login_form = new Form1();
            login_form.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(password_txtbox.PasswordChar == '*')
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
