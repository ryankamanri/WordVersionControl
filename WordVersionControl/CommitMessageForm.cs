using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordVersionControl
{
	public partial class CommitMessageForm : Form
	{
		public string CommitMessage { get; private set; }

		public CommitMessageForm()
		{
			InitializeComponent();
		}

		private void btn_ok_Click(object sender, EventArgs e)
		{
			CommitMessage = txtMessage.Text.Trim();
			if (string.IsNullOrEmpty(CommitMessage))
			{
				MessageBox.Show("请输入提交说明。");
				return;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btn_cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
