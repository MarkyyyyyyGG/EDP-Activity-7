namespace EDP_Act4_Fin
{
    partial class PasswordChange
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordChange));
            this.label1 = new System.Windows.Forms.Label();
            this.back = new System.Windows.Forms.Button();
            this.newpw = new System.Windows.Forms.Label();
            this.newPw_txtbox = new System.Windows.Forms.TextBox();
            this.confirmPw_txtbox = new System.Windows.Forms.TextBox();
            this.confirmpw = new System.Windows.Forms.Label();
            this.reset_btn = new System.Windows.Forms.Button();
            this.new_pictureBox = new System.Windows.Forms.PictureBox();
            this.confirm_pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.new_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.confirm_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("SF Pro Display", 30F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(507, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(473, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password Recovery";
            // 
            // back
            // 
            this.back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(230)))), ((int)(((byte)(235)))));
            this.back.Font = new System.Drawing.Font("SF Pro Display", 15.8F, System.Drawing.FontStyle.Bold);
            this.back.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(79)))), ((int)(((byte)(90)))));
            this.back.Location = new System.Drawing.Point(646, 479);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(164, 50);
            this.back.TabIndex = 2;
            this.back.Text = "Back";
            this.back.UseVisualStyleBackColor = false;
            this.back.Click += new System.EventHandler(this.button1_Click);
            // 
            // newpw
            // 
            this.newpw.AutoSize = true;
            this.newpw.BackColor = System.Drawing.Color.Transparent;
            this.newpw.Font = new System.Drawing.Font("SF Pro Display", 20F, System.Drawing.FontStyle.Bold);
            this.newpw.ForeColor = System.Drawing.Color.White;
            this.newpw.Location = new System.Drawing.Point(317, 382);
            this.newpw.Name = "newpw";
            this.newpw.Size = new System.Drawing.Size(245, 41);
            this.newpw.TabIndex = 4;
            this.newpw.Text = "New Password";
            // 
            // newPw_txtbox
            // 
            this.newPw_txtbox.Font = new System.Drawing.Font("SF Pro Display", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newPw_txtbox.Location = new System.Drawing.Point(579, 382);
            this.newPw_txtbox.Name = "newPw_txtbox";
            this.newPw_txtbox.PasswordChar = '*';
            this.newPw_txtbox.Size = new System.Drawing.Size(401, 40);
            this.newPw_txtbox.TabIndex = 5;
            // 
            // confirmPw_txtbox
            // 
            this.confirmPw_txtbox.Font = new System.Drawing.Font("SF Pro Display", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPw_txtbox.Location = new System.Drawing.Point(579, 432);
            this.confirmPw_txtbox.Name = "confirmPw_txtbox";
            this.confirmPw_txtbox.PasswordChar = '*';
            this.confirmPw_txtbox.Size = new System.Drawing.Size(401, 40);
            this.confirmPw_txtbox.TabIndex = 7;
            // 
            // confirmpw
            // 
            this.confirmpw.AutoSize = true;
            this.confirmpw.BackColor = System.Drawing.Color.Transparent;
            this.confirmpw.Font = new System.Drawing.Font("SF Pro Display", 20F, System.Drawing.FontStyle.Bold);
            this.confirmpw.ForeColor = System.Drawing.Color.White;
            this.confirmpw.Location = new System.Drawing.Point(263, 432);
            this.confirmpw.Name = "confirmpw";
            this.confirmpw.Size = new System.Drawing.Size(299, 41);
            this.confirmpw.TabIndex = 6;
            this.confirmpw.Text = "Confirm Password";
            // 
            // reset_btn
            // 
            this.reset_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(119)))), ((int)(((byte)(242)))));
            this.reset_btn.Font = new System.Drawing.Font("SF Pro Display", 15.8F, System.Drawing.FontStyle.Bold);
            this.reset_btn.ForeColor = System.Drawing.Color.White;
            this.reset_btn.Location = new System.Drawing.Point(816, 479);
            this.reset_btn.Name = "reset_btn";
            this.reset_btn.Size = new System.Drawing.Size(164, 50);
            this.reset_btn.TabIndex = 8;
            this.reset_btn.Text = "Reset";
            this.reset_btn.UseVisualStyleBackColor = false;
            this.reset_btn.Click += new System.EventHandler(this.reset_btn_Click);
            // 
            // new_pictureBox
            // 
            this.new_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.new_pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("new_pictureBox.Image")));
            this.new_pictureBox.Location = new System.Drawing.Point(986, 376);
            this.new_pictureBox.Name = "new_pictureBox";
            this.new_pictureBox.Size = new System.Drawing.Size(51, 47);
            this.new_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.new_pictureBox.TabIndex = 9;
            this.new_pictureBox.TabStop = false;
            this.new_pictureBox.Click += new System.EventHandler(this.new_pictureBox_Click);
            // 
            // confirm_pictureBox
            // 
            this.confirm_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.confirm_pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("confirm_pictureBox.Image")));
            this.confirm_pictureBox.Location = new System.Drawing.Point(986, 425);
            this.confirm_pictureBox.Name = "confirm_pictureBox";
            this.confirm_pictureBox.Size = new System.Drawing.Size(51, 47);
            this.confirm_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.confirm_pictureBox.TabIndex = 10;
            this.confirm_pictureBox.TabStop = false;
            this.confirm_pictureBox.Click += new System.EventHandler(this.confirm_pictureBox_Click);
            // 
            // PasswordChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.confirm_pictureBox);
            this.Controls.Add(this.new_pictureBox);
            this.Controls.Add(this.reset_btn);
            this.Controls.Add(this.confirmPw_txtbox);
            this.Controls.Add(this.confirmpw);
            this.Controls.Add(this.newPw_txtbox);
            this.Controls.Add(this.newpw);
            this.Controls.Add(this.back);
            this.Controls.Add(this.label1);
            this.Name = "PasswordChange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Change";
            this.Load += new System.EventHandler(this.PasswordRecovery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.new_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.confirm_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Label newpw;
        private System.Windows.Forms.TextBox newPw_txtbox;
        private System.Windows.Forms.TextBox confirmPw_txtbox;
        private System.Windows.Forms.Label confirmpw;
        private System.Windows.Forms.Button reset_btn;
        private System.Windows.Forms.PictureBox new_pictureBox;
        private System.Windows.Forms.PictureBox confirm_pictureBox;
    }
}