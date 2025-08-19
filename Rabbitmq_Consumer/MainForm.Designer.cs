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
			this.txt_OrderMessages = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txt_InventoryMessages = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txt_OrderMessages
			// 
			this.txt_OrderMessages.Location = new System.Drawing.Point(12, 53);
			this.txt_OrderMessages.Multiline = true;
			this.txt_OrderMessages.Name = "txt_OrderMessages";
			this.txt_OrderMessages.Size = new System.Drawing.Size(361, 229);
			this.txt_OrderMessages.TabIndex = 0;
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
			// txt_InventoryMessages
			// 
			this.txt_InventoryMessages.Location = new System.Drawing.Point(451, 53);
			this.txt_InventoryMessages.Multiline = true;
			this.txt_InventoryMessages.Name = "txt_InventoryMessages";
			this.txt_InventoryMessages.Size = new System.Drawing.Size(361, 229);
			this.txt_InventoryMessages.TabIndex = 0;
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(835, 450);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txt_InventoryMessages);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txt_OrderMessages);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "消息消费者";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txt_OrderMessages;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txt_InventoryMessages;
		private System.Windows.Forms.Label label2;
	}
}

