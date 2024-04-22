using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EDP_Act4_Fin
{
    public partial class PasswordChange : Form
    {
        private DatabaseManager dbManager;
        String email = PasswordRecovery.to;
        public PasswordChange()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void PasswordRecovery_Load(object sender, EventArgs e)
        {
            BackgroundImage = System.Drawing.Image.FromFile("D:\\Downloads\\Sterriton games magazine (1)\\2.png");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var signup_form = new signup();
            this.Hide();
            signup_form.Show();       
        }

        private void reset_btn_Click(object sender, EventArgs e)
        {
            String pw = newPw_txtbox.Text;
            String cpw = confirmPw_txtbox.Text;

            if (pw.Length < 8 || cpw.Length < 8)
            {
                MessageBox.Show("Password too short!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                newPw_txtbox.Text = null;
                confirmPw_txtbox.Text = null;
                return;
            }

            if (newPw_txtbox.Text == confirmPw_txtbox.Text)
            {
                string newPassword = newPw_txtbox.Text;

                // Hash the new password using SHA-256
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
                    string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "");

                    // Update the user's password in the database
                    dbManager.OpenConnection();
                    string sql = "UPDATE accounts SET password = @password WHERE email = @email";
                    MySqlCommand cmd = new MySqlCommand(sql, dbManager.connection);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@email", email); // Use the retrieved username
                    cmd.ExecuteNonQuery();
                    dbManager.CloseConnection();

                    MessageBox.Show("Password Reset Successful", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void new_pictureBox_Click(object sender, EventArgs e)
        {
            if (newPw_txtbox.PasswordChar == '*')
            {
                newPw_txtbox.PasswordChar = '\0';
            }
            else
            {
                newPw_txtbox.PasswordChar = '*';
            }
        }

        private void confirm_pictureBox_Click(object sender, EventArgs e)
        {
            if (confirmPw_txtbox.PasswordChar == '*')
            {
                confirmPw_txtbox.PasswordChar = '\0';
            }
            else
            {
                confirmPw_txtbox.PasswordChar = '*';
            }
        }
    }
}
