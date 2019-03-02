namespace ExportXDUTimeTable
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
            this.lblPwd = new System.Windows.Forms.Label();
            this.lblVerify = new System.Windows.Forms.Label();
            this.txtStuID = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtVerify = new System.Windows.Forms.TextBox();
            this.picVerify = new System.Windows.Forms.PictureBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblNotice = new System.Windows.Forms.Label();
            this.chkChangeTime = new System.Windows.Forms.CheckBox();
            this.lblTimeA = new System.Windows.Forms.Label();
            this.dtpTimeA = new System.Windows.Forms.DateTimePicker();
            this.lblTimeB = new System.Windows.Forms.Label();
            this.dtpTimeB = new System.Windows.Forms.DateTimePicker();
            this.lblSource = new System.Windows.Forms.Label();
            this.rbCpdaily = new System.Windows.Forms.RadioButton();
            this.rbEhall = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picVerify)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStuID
            // 
            this.lblStuID.AutoSize = true;
            this.lblStuID.Location = new System.Drawing.Point(12, 24);
            this.lblStuID.Name = "lblStuID";
            this.lblStuID.Size = new System.Drawing.Size(88, 30);
            this.lblStuID.TabIndex = 0;
            this.lblStuID.Text = "学号:";
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(12, 72);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(88, 30);
            this.lblPwd.TabIndex = 1;
            this.lblPwd.Text = "密码:";
            // 
            // lblVerify
            // 
            this.lblVerify.AutoSize = true;
            this.lblVerify.Location = new System.Drawing.Point(12, 120);
            this.lblVerify.Name = "lblVerify";
            this.lblVerify.Size = new System.Drawing.Size(118, 30);
            this.lblVerify.TabIndex = 2;
            this.lblVerify.Text = "验证码:";
            this.lblVerify.Visible = false;
            // 
            // txtStuID
            // 
            this.txtStuID.Location = new System.Drawing.Point(136, 12);
            this.txtStuID.Name = "txtStuID";
            this.txtStuID.Size = new System.Drawing.Size(269, 42);
            this.txtStuID.TabIndex = 3;
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(136, 60);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(269, 42);
            this.txtPwd.TabIndex = 4;
            this.txtPwd.UseSystemPasswordChar = true;
            // 
            // txtVerify
            // 
            this.txtVerify.Location = new System.Drawing.Point(136, 108);
            this.txtVerify.Name = "txtVerify";
            this.txtVerify.Size = new System.Drawing.Size(138, 42);
            this.txtVerify.TabIndex = 5;
            this.txtVerify.Visible = false;
            // 
            // picVerify
            // 
            this.picVerify.Location = new System.Drawing.Point(280, 108);
            this.picVerify.Name = "picVerify";
            this.picVerify.Size = new System.Drawing.Size(125, 57);
            this.picVerify.TabIndex = 6;
            this.picVerify.TabStop = false;
            this.picVerify.Visible = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(278, 171);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(127, 50);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "获取";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblNotice
            // 
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(12, 283);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(1048, 180);
            this.lblNotice.TabIndex = 8;
            this.lblNotice.Text = "Tips：\r\n1. 输入学号和密码，选择数据源后点击“获取”，如果一切正常，则会在桌面\r\n生成一个 [你的学号]_[学期代码].ics 文件。用你喜欢的日历软件导" +
    "入即可。\r\n2. 根据学校安排，秋季学期开学后的一段时间改为冬季作息时间，夏季学期\r\n期末的一段时间改为春夏秋作息时间。请自行设定何时更改。\r\n3. 如果出现确" +
    "实学号和密码正确，但无法获取到课表的情况，请联系作者。";
            // 
            // chkChangeTime
            // 
            this.chkChangeTime.AutoSize = true;
            this.chkChangeTime.Checked = true;
            this.chkChangeTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeTime.Location = new System.Drawing.Point(411, 12);
            this.chkChangeTime.Name = "chkChangeTime";
            this.chkChangeTime.Size = new System.Drawing.Size(231, 34);
            this.chkChangeTime.TabIndex = 9;
            this.chkChangeTime.Text = "区分作息时间";
            this.chkChangeTime.UseVisualStyleBackColor = true;
            // 
            // lblTimeA
            // 
            this.lblTimeA.AutoSize = true;
            this.lblTimeA.Location = new System.Drawing.Point(412, 72);
            this.lblTimeA.Name = "lblTimeA";
            this.lblTimeA.Size = new System.Drawing.Size(358, 30);
            this.lblTimeA.TabIndex = 10;
            this.lblTimeA.Text = "春夏秋作息时间开始日期:";
            // 
            // dtpTimeA
            // 
            this.dtpTimeA.Location = new System.Drawing.Point(776, 60);
            this.dtpTimeA.Name = "dtpTimeA";
            this.dtpTimeA.Size = new System.Drawing.Size(267, 42);
            this.dtpTimeA.TabIndex = 11;
            this.dtpTimeA.Value = new System.DateTime(2019, 5, 2, 0, 0, 0, 0);
            // 
            // lblTimeB
            // 
            this.lblTimeB.AutoSize = true;
            this.lblTimeB.Location = new System.Drawing.Point(412, 120);
            this.lblTimeB.Name = "lblTimeB";
            this.lblTimeB.Size = new System.Drawing.Size(328, 30);
            this.lblTimeB.TabIndex = 12;
            this.lblTimeB.Text = "冬季作息时间开始日期:";
            // 
            // dtpTimeB
            // 
            this.dtpTimeB.Location = new System.Drawing.Point(776, 108);
            this.dtpTimeB.Name = "dtpTimeB";
            this.dtpTimeB.Size = new System.Drawing.Size(267, 42);
            this.dtpTimeB.TabIndex = 13;
            this.dtpTimeB.Value = new System.DateTime(2018, 10, 8, 0, 0, 0, 0);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(12, 229);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(118, 30);
            this.lblSource.TabIndex = 14;
            this.lblSource.Text = "数据源:";
            // 
            // rbCpdaily
            // 
            this.rbCpdaily.AutoSize = true;
            this.rbCpdaily.Checked = true;
            this.rbCpdaily.Location = new System.Drawing.Point(136, 227);
            this.rbCpdaily.Name = "rbCpdaily";
            this.rbCpdaily.Size = new System.Drawing.Size(170, 34);
            this.rbCpdaily.TabIndex = 15;
            this.rbCpdaily.TabStop = true;
            this.rbCpdaily.Text = "今日校园";
            this.rbCpdaily.UseVisualStyleBackColor = true;
            // 
            // rbEhall
            // 
            this.rbEhall.AutoSize = true;
            this.rbEhall.Location = new System.Drawing.Point(312, 225);
            this.rbEhall.Name = "rbEhall";
            this.rbEhall.Size = new System.Drawing.Size(260, 34);
            this.rbEhall.TabIndex = 16;
            this.rbEhall.Text = "一站式服务大厅";
            this.rbEhall.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 486);
            this.Controls.Add(this.rbEhall);
            this.Controls.Add(this.rbCpdaily);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.dtpTimeB);
            this.Controls.Add(this.lblTimeB);
            this.Controls.Add(this.dtpTimeA);
            this.Controls.Add(this.lblTimeA);
            this.Controls.Add(this.chkChangeTime);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.picVerify);
            this.Controls.Add(this.txtVerify);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtStuID);
            this.Controls.Add(this.lblVerify);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.lblStuID);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出课程表 - 西安电子科技大学";
            ((System.ComponentModel.ISupportInitialize)(this.picVerify)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStuID;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.Label lblVerify;
        private System.Windows.Forms.TextBox txtStuID;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtVerify;
        private System.Windows.Forms.PictureBox picVerify;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblNotice;
        private System.Windows.Forms.CheckBox chkChangeTime;
        private System.Windows.Forms.Label lblTimeA;
        private System.Windows.Forms.DateTimePicker dtpTimeA;
        private System.Windows.Forms.Label lblTimeB;
        private System.Windows.Forms.DateTimePicker dtpTimeB;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.RadioButton rbCpdaily;
        private System.Windows.Forms.RadioButton rbEhall;
    }
}

