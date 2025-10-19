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
using System.Windows.Forms.Integration; // ElementHost

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

		public CustomTaskPane repo_pane;
		public RepoSidebarHost sidebar_control;

		public GitOps git_ops { get; private set; }
        public AutosaveService autosave_service { get; private set; }
		private void OnDocumentOpen(Document doc)
		{
			var wvc_path = Utils.EnsureWVCDirectory();
			var repo_path = GitOps.EnsureGitRepo(wvc_path, doc);
            this.git_ops = new GitOps(repo_path);

            this.autosave_service = new AutosaveService(doc);

			// sidebar init
			sidebar_control = new RepoSidebarHost();

			repo_pane = this.CustomTaskPanes.Add(sidebar_control, "版本控制");
			repo_pane.Visible = false; // 默认隐藏
			repo_pane.Width = 300;
		}

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
