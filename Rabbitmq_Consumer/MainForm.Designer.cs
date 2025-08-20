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
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(101, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "接收订单队列消息";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(452, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "接收库存队列消息";
			// 
			// listBox_OrderMessages
			// 
			this.listBox_OrderMessages.FormattingEnabled = true;
			this.listBox_OrderMessages.ItemHeight = 12;
			this.listBox_OrderMessages.Location = new System.Drawing.Point(15, 53);
			this.listBox_OrderMessages.Name = "listBox_OrderMessages";
			this.listBox_OrderMessages.Size = new System.Drawing.Size(329, 244);
			this.listBox_OrderMessages.TabIndex = 2;
			// 
			// listBox_InventoryMessages
			// 
			this.listBox_InventoryMessages.FormattingEnabled = true;
			this.listBox_InventoryMessages.ItemHeight = 12;
			this.listBox_InventoryMessages.Location = new System.Drawing.Point(454, 53);
			this.listBox_InventoryMessages.Name = "listBox_InventoryMessages";
			this.listBox_InventoryMessages.Size = new System.Drawing.Size(329, 244);
			this.listBox_InventoryMessages.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(835, 450);
			this.Controls.Add(this.listBox_InventoryMessages);
			this.Controls.Add(this.listBox_OrderMessages);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "消息消费者";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox listBox_OrderMessages;
		private System.Windows.Forms.ListBox listBox_InventoryMessages;
	}
}

