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
			this.txt_Messages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txt_Messages
			// 
			this.txt_Messages.Location = new System.Drawing.Point(12, 12);
			this.txt_Messages.Multiline = true;
			this.txt_Messages.Name = "txt_Messages";
			this.txt_Messages.Size = new System.Drawing.Size(578, 229);
			this.txt_Messages.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(602, 450);
			this.Controls.Add(this.txt_Messages);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "消息消费者";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txt_Messages;
	}
}

