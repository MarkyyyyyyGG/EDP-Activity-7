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
    public partial class Dashboard : Form
    {
        private DatabaseManager dbManager;
        String loggedEmail = Form1.login_email;
        public Dashboard()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }
        
        private void panel5_Load(object sender, EventArgs e)
        {
            panel5.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void reportBtn_Click(object sender, EventArgs e)
        {
            var report = new Report();
            report.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var account = new Account();
            this.Hide();
            account.Show();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            if (dbManager.OpenConnection())
            {
                try
                {
                    string sqlRetrieveEmail = "SELECT username FROM accounts WHERE email = @email";
                    MySqlCommand cmdRetrieveEmail = new MySqlCommand(sqlRetrieveEmail, dbManager.connection);
                    cmdRetrieveEmail.Parameters.AddWithValue("@email", loggedEmail);
                    object result = cmdRetrieveEmail.ExecuteScalar();
                    if (result != null)
                    {
                        greet_label.Text = "Welcome, " + result.ToString(); // Convert the result to string and assign it to the textbox
                    }
                    else
                    {
                        MessageBox.Show("Email not found in the database");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving email from database: " + ex.Message);
                }
                finally
                {
                    // Do not close the connection here
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to database");
            }
            // Close the connection after retrieving email
            dbManager.CloseConnection();
        }

        private void status_btn_Click(object sender, EventArgs e)
        {
            var status = new Status();
            this.Hide();
            status.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void signoutBtn_Click(object sender, EventArgs e)
        {
            dbManager.CloseConnection();
            var loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var products = new Products();
            this.Hide();
            products.Show();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            var products = new Products();
            this.Hide();
            products.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var salary = new EmpSal();
            this.Hide();
            salary.Show();
        }
    }
}
