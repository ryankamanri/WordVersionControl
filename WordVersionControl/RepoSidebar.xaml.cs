using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordVersionControl
{
	/// <summary>
	/// Interaction logic for RepoSidebar.xaml
	/// </summary>
	public partial class RepoSidebar : System.Windows.Controls.UserControl
	{
		public RepoSidebar()
		{
			InitializeComponent();
		}

		public void SetCommits(IEnumerable<Models.Commit> commits)
		{
			CommitList.ItemsSource = commits;
		}

		private void Version_Click(object sender, MouseButtonEventArgs e)
		{
			if (sender is System.Windows.Controls.TextBlock tb)
			{
				string version = tb.Text;
				MessageBox.Show($"你点击了版本：{version}");
				// TODO: 调用你自己的逻辑，比如 RunGit("checkout", version)
			}
		}

		private void CommitList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (CommitList.SelectedItem is Models.Commit commit)
			{
				MessageBox.Show($"{commit.commit_id} selected!");
			}
		}
	}

	public partial class RepoSidebarHost : System.Windows.Forms.UserControl
	{
		private ElementHost host;

		public RepoSidebarHost()
		{
			InitializeComponent();
			InitializeElementHost();
		}

		private void InitializeComponent()
		{
			this.host = new ElementHost();
			this.SuspendLayout();
			// 
			// host
			// 
			this.host.Dock = DockStyle.Fill;
			this.host.Name = "elementHost";
			// 
			// RepoHostControl (this)
			// 
			this.Controls.Add(this.host);
			this.Name = "RepoHostControl";
			this.Size = new System.Drawing.Size(300, 600);
			this.ResumeLayout(false);
		}

		private void InitializeElementHost()
		{
			host.Child = new RepoSidebar();
		}

		// 可选：暴露方法给外部，便于在 ThisAddIn 中调用
		public void SetCommits(IEnumerable<Models.Commit> commits)
		{
			if (host?.Child is RepoSidebar rs)
			{
				rs.SetCommits(commits);
			}
		}
	}
}
