namespace StockTracker.UserControls
{
    partial class ctrLogin
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblLogin = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            lblPassword = new Label();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // lblLogin
            // 
            lblLogin.AutoSize = true;
            lblLogin.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblLogin.Location = new Point(198, 24);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(84, 37);
            lblLogin.TabIndex = 0;
            lblLogin.Text = "Login";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblUsername.Location = new Point(117, 83);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(91, 23);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtUsername.Location = new Point(117, 109);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(252, 34);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.Location = new Point(117, 180);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(252, 34);
            txtPassword.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPassword.Location = new Point(117, 154);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(84, 23);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password:";
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnLogin.Location = new Point(189, 235);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(105, 34);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // ctrLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblUsername);
            Controls.Add(lblLogin);
            Name = "ctrLogin";
            Size = new Size(484, 305);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLogin;
        private Label lblUsername;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblPassword;
        private Button btnLogin;
    }
}
