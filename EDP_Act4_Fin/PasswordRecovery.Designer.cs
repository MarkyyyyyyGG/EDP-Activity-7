namespace EDP_Act4_Fin
{
    partial class PasswordRecovery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordRecovery));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.getcode = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.email_txtbox = new System.Windows.Forms.TextBox();
            this.code_txtbox = new System.Windows.Forms.TextBox();
            this.code = new System.Windows.Forms.Label();
            this.verify_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("SF Pro Display", 30F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(498, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(473, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password Recovery";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(230)))), ((int)(((byte)(235)))));
            this.button1.Font = new System.Drawing.Font("SF Pro Display", 15.8F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(79)))), ((int)(((byte)(90)))));
            this.button1.Location = new System.Drawing.Point(646, 543);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 50);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // getcode
            // 
            this.getcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(119)))), ((int)(((byte)(242)))));
            this.getcode.Font = new System.Drawing.Font("SF Pro Display", 15.8F, System.Drawing.FontStyle.Bold);
            this.getcode.ForeColor = System.Drawing.Color.White;
            this.getcode.Location = new System.Drawing.Point(816, 432);
            this.getcode.Name = "getcode";
            this.getcode.Size = new System.Drawing.Size(164, 50);
            this.getcode.TabIndex = 3;
            this.getcode.Text = "Get Code";
            this.getcode.UseVisualStyleBackColor = false;
            this.getcode.Click += new System.EventHandler(this.getcode_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("SF Pro Display", 20F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(462, 382);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 41);
            this.label2.TabIndex = 4;
            this.label2.Text = "Email";
            // 
            // email_txtbox
            // 
            this.email_txtbox.Font = new System.Drawing.Font("SF Pro Display", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email_txtbox.Location = new System.Drawing.Point(579, 382);
            this.email_txtbox.Name = "email_txtbox";
            this.email_txtbox.Size = new System.Drawing.Size(401, 40);
            this.email_txtbox.TabIndex = 5;
            // 
            // code_txtbox
            // 
            this.code_txtbox.Font = new System.Drawing.Font("SF Pro Display", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_txtbox.Location = new System.Drawing.Point(579, 494);
            this.code_txtbox.Name = "code_txtbox";
            this.code_txtbox.Size = new System.Drawing.Size(401, 40);
            this.code_txtbox.TabIndex = 7;
            // 
            // code
            // 
            this.code.AutoSize = true;
            this.code.BackColor = System.Drawing.Color.Transparent;
            this.code.Font = new System.Drawing.Font("SF Pro Display", 20F, System.Drawing.FontStyle.Bold);
            this.code.ForeColor = System.Drawing.Color.White;
            this.code.Location = new System.Drawing.Point(462, 494);
            this.code.Name = "code";
            this.code.Size = new System.Drawing.Size(100, 41);
            this.code.TabIndex = 6;
            this.code.Text = "Code";
            // 
            // verify_btn
            // 
            this.verify_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(119)))), ((int)(((byte)(242)))));
            this.verify_btn.Font = new System.Drawing.Font("SF Pro Display", 15.8F, System.Drawing.FontStyle.Bold);
            this.verify_btn.ForeColor = System.Drawing.Color.White;
            this.verify_btn.Location = new System.Drawing.Point(816, 543);
            this.verify_btn.Name = "verify_btn";
            this.verify_btn.Size = new System.Drawing.Size(164, 50);
            this.verify_btn.TabIndex = 8;
            this.verify_btn.Text = "Verify";
            this.verify_btn.UseVisualStyleBackColor = false;
            this.verify_btn.Click += new System.EventHandler(this.submit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("SF Pro Display", 15F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(604, 324);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(311, 30);
            this.label3.TabIndex = 9;
            this.label3.Text = "Code will be sent via Email";
            // 
            // PasswordRecovery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.verify_btn);
            this.Controls.Add(this.code_txtbox);
            this.Controls.Add(this.code);
            this.Controls.Add(this.email_txtbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.getcode);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "PasswordRecovery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Recovery";
            this.Load += new System.EventHandler(this.PasswordRecovery_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button getcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox email_txtbox;
        private System.Windows.Forms.TextBox code_txtbox;
        private System.Windows.Forms.Label code;
        private System.Windows.Forms.Button verify_btn;
        private System.Windows.Forms.Label label3;
    }
}