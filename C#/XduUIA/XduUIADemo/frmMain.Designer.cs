namespace XduUIADemo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStuID = new System.Windows.Forms.Label();
            this.lblVerify = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtStuID = new System.Windows.Forms.TextBox();
            this.txtVerify = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.pbVerify = new System.Windows.Forms.PictureBox();
            this.btnLoginIds = new System.Windows.Forms.Button();
            this.btnLoginApp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbVerify)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStuID
            // 
            this.lblStuID.AutoSize = true;
            this.lblStuID.Location = new System.Drawing.Point(13, 25);
            this.lblStuID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStuID.Name = "lblStuID";
            this.lblStuID.Size = new System.Drawing.Size(88, 30);
            this.lblStuID.TabIndex = 8;
            this.lblStuID.Text = "学号:";
            // 
            // lblVerify
            // 
            this.lblVerify.AutoSize = true;
            this.lblVerify.Location = new System.Drawing.Point(13, 126);
            this.lblVerify.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVerify.Name = "lblVerify";
            this.lblVerify.Size = new System.Drawing.Size(118, 30);
            this.lblVerify.TabIndex = 9;
            this.lblVerify.Text = "验证码:";
            this.lblVerify.Visible = false;
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(13, 78);
            this.lblPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(88, 30);
            this.lblPass.TabIndex = 10;
            this.lblPass.Text = "密码:";
            // 
            // txtStuID
            // 
            this.txtStuID.Location = new System.Drawing.Point(104, 17);
            this.txtStuID.Name = "txtStuID";
            this.txtStuID.Size = new System.Drawing.Size(336, 42);
            this.txtStuID.TabIndex = 11;
            // 
            // txtVerify
            // 
            this.txtVerify.Location = new System.Drawing.Point(134, 123);
            this.txtVerify.Name = "txtVerify";
            this.txtVerify.Size = new System.Drawing.Size(185, 42);
            this.txtVerify.TabIndex = 12;
            this.txtVerify.Visible = false;
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(104, 70);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(336, 42);
            this.txtPwd.TabIndex = 13;
            this.txtPwd.UseSystemPasswordChar = true;
            // 
            // pbVerify
            // 
            this.pbVerify.Location = new System.Drawing.Point(325, 123);
            this.pbVerify.Name = "pbVerify";
            this.pbVerify.Size = new System.Drawing.Size(115, 47);
            this.pbVerify.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbVerify.TabIndex = 14;
            this.pbVerify.TabStop = false;
            this.pbVerify.Visible = false;
            // 
            // btnLoginIds
            // 
            this.btnLoginIds.Location = new System.Drawing.Point(18, 171);
            this.btnLoginIds.Name = "btnLoginIds";
            this.btnLoginIds.Size = new System.Drawing.Size(152, 66);
            this.btnLoginIds.TabIndex = 15;
            this.btnLoginIds.Text = "登录 IDS";
            this.btnLoginIds.UseVisualStyleBackColor = true;
            this.btnLoginIds.Click += new System.EventHandler(this.btnLoginIds_Click);
            // 
            // btnLoginApp
            // 
            this.btnLoginApp.Location = new System.Drawing.Point(235, 171);
            this.btnLoginApp.Name = "btnLoginApp";
            this.btnLoginApp.Size = new System.Drawing.Size(205, 66);
            this.btnLoginApp.TabIndex = 16;
            this.btnLoginApp.Text = "登录 i 西电";
            this.btnLoginApp.UseVisualStyleBackColor = true;
            this.btnLoginApp.Click += new System.EventHandler(this.btnLoginApp_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 255);
            this.Controls.Add(this.btnLoginApp);
            this.Controls.Add(this.lblStuID);
            this.Controls.Add(this.lblVerify);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.txtStuID);
            this.Controls.Add(this.txtVerify);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.pbVerify);
            this.Controls.Add(this.btnLoginIds);
            this.Name = "frmMain";
            this.Text = "XduUIA Demo";
            ((System.ComponentModel.ISupportInitialize)(this.pbVerify)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStuID;
        private System.Windows.Forms.Label lblVerify;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtStuID;
        private System.Windows.Forms.TextBox txtVerify;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.PictureBox pbVerify;
        private System.Windows.Forms.Button btnLoginIds;
        private System.Windows.Forms.Button btnLoginApp;
    }
}

