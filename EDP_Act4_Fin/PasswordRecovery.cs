using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace EDP_Act4_Fin
{
    public partial class PasswordRecovery : Form
    {
        String OTPcode;
        public static string to;

   
        public PasswordRecovery()
        {
            InitializeComponent();
            verify_btn.Enabled = false;

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

        private void submit_Click(object sender, EventArgs e)
        {

            if (OTPcode == (code_txtbox.Text).ToString()) 
            {
                to = email_txtbox.Text;
                var passwordChange = new PasswordChange();
                this.Hide();
                passwordChange.Show();
            }
            else
            {
                MessageBox.Show("Wrong Code Input", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private async void getcode_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (string.IsNullOrEmpty(email_txtbox.Text))
                {
                    MessageBox.Show("Enter Email!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                verify_btn.Enabled = true;

                string from, pass, messageBody;
                Random rand = new Random();
                OTPcode = rand.Next(100000, 999999).ToString(); // Generate a 6-digit OTP

                MailMessage message = new MailMessage();
                from = "sterriton0@gmail.com"; // Change this to your email address
                pass = "ldnakwsdvsfeasfz"; // Change this to your email password
                messageBody = "Your OTP code is " + OTPcode + ".";
                message.To.Add(email_txtbox.Text); // Set the recipient email address
                message.From = new MailAddress(from);
                message.Body = messageBody;
                message.Subject = "Forgot Password Code";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(from, pass);
                await Task.Run(() => smtp.Send(message)); // Send email asynchronously
                MessageBox.Show("Code Sent Successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default; // Revert cursor to default
            }
        }


    }
}
