using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools;
using WordVersionControl;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Forms.Integration;
using System.IO; // ElementHost

namespace WordVersionControl
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Application.DocumentOpen += OnDocumentOpen;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

		public bool enabled { get; private set; }

		public CustomTaskPane repo_pane { get; private set; }
		public RepoSidebarHost sidebar_control { get; private set; }

		public GitOps git_ops { get; private set; }
        public AutosaveService autosave_service { get; private set; }
		private void OnDocumentOpen(Document doc)
		{
			var wvc_path = Utils.EnsureWVCDirectory();
			enabled = (!doc.FullName.StartsWith(Path.GetFullPath(wvc_path), System.StringComparison.OrdinalIgnoreCase) && doc.Name.EndsWith(".docx"));

			if (!enabled) 
			{
				if (!doc.Name.EndsWith(".docx"))
				{
					Utils.ShowBalloonTip("目前仅支持对.docx文件的版本控制", "版本控制未启用");
				}
				return;
			}

			var repo_path = GitOps.EnsureGitRepo(wvc_path, doc);
            this.git_ops = new GitOps(repo_path);

            this.autosave_service = new AutosaveService(doc);

			// sidebar init
			sidebar_control = new RepoSidebarHost();
			sidebar_control.OnEvent = OnSidebarEvents;

			repo_pane = this.CustomTaskPanes.Add(sidebar_control, "版本控制");
			repo_pane.Visible = false; // 默认隐藏
			repo_pane.Width = 300;

			// open revision
			doc.TrackRevisions = true;
		}

		#region sidebar_events
		private void OnSidebarEvents(RepoSidebar.OpenType open_type, List<Models.Commit> commit_pool)
		{
			if (open_type == RepoSidebar.OpenType.SHOW_HISTORY) 
			{
				MessageBox.Show($"将查看下列提交的历史记录:\n\n{commit_pool[0]}");
				var git_history_doc_name = "history.docx";
				var history_output_path = git_ops.Export(commit_pool[0], git_history_doc_name);
				var app = new Word.Application { Visible = true };
				var history_doc = app.Documents.Open(history_output_path, ReadOnly: true, Visible: true);
			}
			if (open_type == RepoSidebar.OpenType.COMPARE_TWO)
			{
				MessageBox.Show($"将比较下列提交的历史记录:\n\n{commit_pool[0]}\n\n和\n\n{commit_pool[1]}");
				var old_doc_path = git_ops.Export(commit_pool[0], "old.docx");
				var new_doc_path = git_ops.Export(commit_pool[1], "new.docx");
				var app = new Word.Application { Visible = true };
				var old_doc = app.Documents.Open(old_doc_path, ReadOnly: true, Visible: false);
				var new_doc = app.Documents.Open(new_doc_path, ReadOnly: true, Visible: false);

				var compare_doc = app.CompareDocuments(
					old_doc, new_doc,
					Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationNew
				);

				old_doc.Close();
				new_doc.Close();
			}
			
		}
		#endregion

		#region VSTO 生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
