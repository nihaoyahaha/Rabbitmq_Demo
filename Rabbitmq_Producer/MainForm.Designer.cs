namespace Rabbitmq_Producer
{
	partial class MainForm
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
			this.txt_Message = new System.Windows.Forms.TextBox();
			this.btn_send = new System.Windows.Forms.Button();
			this.cmb_RoutingKey = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btn_sendPicture = new System.Windows.Forms.Button();
			this.btn_openFile = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// txt_Message
			// 
			this.txt_Message.Location = new System.Drawing.Point(15, 20);
			this.txt_Message.Multiline = true;
			this.txt_Message.Name = "txt_Message";
			this.txt_Message.Size = new System.Drawing.Size(484, 155);
			this.txt_Message.TabIndex = 0;
			// 
			// btn_send
			// 
			this.btn_send.Location = new System.Drawing.Point(410, 195);
			this.btn_send.Name = "btn_send";
			this.btn_send.Size = new System.Drawing.Size(89, 23);
			this.btn_send.TabIndex = 1;
			this.btn_send.Text = "发送消息";
			this.btn_send.UseVisualStyleBackColor = true;
			this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
			// 
			// cmb_RoutingKey
			// 
			this.cmb_RoutingKey.FormattingEnabled = true;
			this.cmb_RoutingKey.Location = new System.Drawing.Point(12, 27);
			this.cmb_RoutingKey.Name = "cmb_RoutingKey";
			this.cmb_RoutingKey.Size = new System.Drawing.Size(237, 20);
			this.cmb_RoutingKey.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "选择路由规则";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Image = global::Rabbitmq_Producer.Properties.Resources.car;
			this.pictureBox1.Location = new System.Drawing.Point(15, 59);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(271, 266);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// btn_sendPicture
			// 
			this.btn_sendPicture.Location = new System.Drawing.Point(410, 302);
			this.btn_sendPicture.Name = "btn_sendPicture";
			this.btn_sendPicture.Size = new System.Drawing.Size(89, 23);
			this.btn_sendPicture.TabIndex = 1;
			this.btn_sendPicture.Text = "发送图片";
			this.btn_sendPicture.UseVisualStyleBackColor = true;
			this.btn_sendPicture.Click += new System.EventHandler(this.btn_sendPicture_Click);
			// 
			// btn_openFile
			// 
			this.btn_openFile.Location = new System.Drawing.Point(15, 30);
			this.btn_openFile.Name = "btn_openFile";
			this.btn_openFile.Size = new System.Drawing.Size(75, 23);
			this.btn_openFile.TabIndex = 5;
			this.btn_openFile.Text = "选择图片";
			this.btn_openFile.UseVisualStyleBackColor = true;
			this.btn_openFile.Click += new System.EventHandler(this.btn_openFile_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txt_Message);
			this.groupBox1.Controls.Add(this.btn_send);
			this.groupBox1.Location = new System.Drawing.Point(12, 65);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(517, 235);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "发送文本";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.pictureBox1);
			this.groupBox2.Controls.Add(this.btn_sendPicture);
			this.groupBox2.Controls.Add(this.btn_openFile);
			this.groupBox2.Location = new System.Drawing.Point(12, 318);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(517, 339);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "发送图片";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(547, 683);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmb_RoutingKey);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "消息生产者";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txt_Message;
		private System.Windows.Forms.Button btn_send;
		private System.Windows.Forms.ComboBox cmb_RoutingKey;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btn_sendPicture;
		private System.Windows.Forms.Button btn_openFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}

