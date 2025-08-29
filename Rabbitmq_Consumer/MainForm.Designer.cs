namespace Rabbitmq_Consumer
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listBox_OrderMessages = new System.Windows.Forms.ListBox();
			this.listBox_InventoryMessages = new System.Windows.Forms.ListBox();
			this.btn_ReceiveMessage = new System.Windows.Forms.Button();
			this.btn_StopReceiveMessage = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(101, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "接收订单队列消息";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 287);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "接收库存队列消息";
			// 
			// listBox_OrderMessages
			// 
			this.listBox_OrderMessages.FormattingEnabled = true;
			this.listBox_OrderMessages.ItemHeight = 12;
			this.listBox_OrderMessages.Location = new System.Drawing.Point(14, 27);
			this.listBox_OrderMessages.Name = "listBox_OrderMessages";
			this.listBox_OrderMessages.Size = new System.Drawing.Size(560, 244);
			this.listBox_OrderMessages.TabIndex = 2;
			// 
			// listBox_InventoryMessages
			// 
			this.listBox_InventoryMessages.FormattingEnabled = true;
			this.listBox_InventoryMessages.ItemHeight = 12;
			this.listBox_InventoryMessages.Location = new System.Drawing.Point(14, 305);
			this.listBox_InventoryMessages.Name = "listBox_InventoryMessages";
			this.listBox_InventoryMessages.Size = new System.Drawing.Size(560, 244);
			this.listBox_InventoryMessages.TabIndex = 2;
			// 
			// btn_ReceiveMessage
			// 
			this.btn_ReceiveMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_ReceiveMessage.Location = new System.Drawing.Point(14, 597);
			this.btn_ReceiveMessage.Name = "btn_ReceiveMessage";
			this.btn_ReceiveMessage.Size = new System.Drawing.Size(99, 23);
			this.btn_ReceiveMessage.TabIndex = 3;
			this.btn_ReceiveMessage.Text = "开始接收消息";
			this.btn_ReceiveMessage.UseVisualStyleBackColor = true;
			this.btn_ReceiveMessage.Click += new System.EventHandler(this.btn_ReceiveMessage_Click);
			// 
			// btn_StopReceiveMessage
			// 
			this.btn_StopReceiveMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btn_StopReceiveMessage.Location = new System.Drawing.Point(164, 597);
			this.btn_StopReceiveMessage.Name = "btn_StopReceiveMessage";
			this.btn_StopReceiveMessage.Size = new System.Drawing.Size(99, 23);
			this.btn_StopReceiveMessage.TabIndex = 3;
			this.btn_StopReceiveMessage.Text = "停止接收消息";
			this.btn_StopReceiveMessage.UseVisualStyleBackColor = true;
			this.btn_StopReceiveMessage.Click += new System.EventHandler(this.btn_StopReceiveMessage_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(608, 27);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(487, 522);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(606, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 12);
			this.label3.TabIndex = 1;
			this.label3.Text = "接收图片队列";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1130, 685);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btn_StopReceiveMessage);
			this.Controls.Add(this.btn_ReceiveMessage);
			this.Controls.Add(this.listBox_InventoryMessages);
			this.Controls.Add(this.listBox_OrderMessages);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "消息消费者";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listBox_OrderMessages;
		private System.Windows.Forms.ListBox listBox_InventoryMessages;
		private System.Windows.Forms.Button btn_ReceiveMessage;
		private System.Windows.Forms.Button btn_StopReceiveMessage;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label3;
	}
}

