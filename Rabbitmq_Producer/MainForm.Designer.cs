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
			this.SuspendLayout();
			// 
			// txt_Message
			// 
			this.txt_Message.Location = new System.Drawing.Point(24, 27);
			this.txt_Message.Multiline = true;
			this.txt_Message.Name = "txt_Message";
			this.txt_Message.Size = new System.Drawing.Size(409, 155);
			this.txt_Message.TabIndex = 0;
			// 
			// btn_send
			// 
			this.btn_send.Location = new System.Drawing.Point(358, 222);
			this.btn_send.Name = "btn_send";
			this.btn_send.Size = new System.Drawing.Size(75, 23);
			this.btn_send.TabIndex = 1;
			this.btn_send.Text = "发送消息";
			this.btn_send.UseVisualStyleBackColor = true;
			this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
			// 
			// cmb_RoutingKey
			// 
			this.cmb_RoutingKey.FormattingEnabled = true;
			this.cmb_RoutingKey.Location = new System.Drawing.Point(22, 225);
			this.cmb_RoutingKey.Name = "cmb_RoutingKey";
			this.cmb_RoutingKey.Size = new System.Drawing.Size(237, 20);
			this.cmb_RoutingKey.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 207);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 12);
			this.label1.TabIndex = 3;
			this.label1.Text = "选择路由规则";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(488, 331);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmb_RoutingKey);
			this.Controls.Add(this.btn_send);
			this.Controls.Add(this.txt_Message);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "消息生产者";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txt_Message;
		private System.Windows.Forms.Button btn_send;
		private System.Windows.Forms.ComboBox cmb_RoutingKey;
		private System.Windows.Forms.Label label1;
	}
}

