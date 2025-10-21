using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using WordVersionControl.Models;

namespace WordVersionControl
{
	public partial class Ribbon1
	{
		private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
		{
		}


		#region button_events
		private void BtnSaveCommit_Click(object sender, RibbonControlEventArgs e)
		{
			if (!Globals.ThisAddIn.enabled) return;
			var doc = Globals.ThisAddIn.Application.ActiveDocument;
			if (doc == null) { MessageBox.Show("未打开任何文档"); return; }
			var git_ops = Globals.ThisAddIn.git_ops;

			// save and copy to stage directory
			if (!doc.Saved) doc.Save();
			var doc_path = doc.FullName;
			var staged_doc_path = Path.Combine(git_ops.repository_path, GitOps.STAGED_FILE_NAME);
			doc.SaveAs2(staged_doc_path); // here doc change to staged_doc_path, we need to change it back later

			// remove all revision
			var staged_doc = doc.Application.Documents.Open(staged_doc_path, Visible: false);
			staged_doc.RejectAllRevisions();
			staged_doc.Close(SaveChanges: true);

			// change doc to origin
			doc = Globals.ThisAddIn.Application.Documents.Open(doc_path);

			// preview changes
			if (git_ops.commits.Count > 0)
			{
				var git_head_doc_name = "HEAD.docx";
				var head_output_path = git_ops.Export(git_ops.commits[0], git_head_doc_name);
				var app = new Word.Application();
				app.Visible = true;

				var old_doc = app.Documents.Open(head_output_path, ReadOnly: true, Visible: false);
				var new_doc = app.Documents.Open(staged_doc_path, ReadOnly: true, Visible: false);

				var compare_doc = app.CompareDocuments(
					old_doc, new_doc,
					Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationNew
				);

				old_doc.Close();
				new_doc.Close();
			}

			// save file name
			using (var filename_file = System.IO.File.CreateText(Path.Combine(git_ops.repository_path, GitOps.FILENAME_FILE_NAME)))
			{
				filename_file.Write(doc.Name);
			}

			// write message & commit
			using (var form = new CommitMessageForm())
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					string message = form.CommitMessage;
					// commit to git
					git_ops.Commit(message);
				}
			}

			// update commit list
			git_ops.UpdateCommits();

			MessageBox.Show("提交操作已完成");
		}

		private void BtnShowLog_Click(object sender, RibbonControlEventArgs e)
		{
			if (!Globals.ThisAddIn.enabled) return;
			SwitchRepoPane();
			var sidebar = Globals.ThisAddIn.sidebar_control;
			sidebar.open_type = RepoSidebar.OpenType.SHOW_HISTORY;
		}

		private void BtnCompare_Click(object sender, RibbonControlEventArgs e)
		{
			if (!Globals.ThisAddIn.enabled) return;
			SwitchRepoPane();
			var sidebar = Globals.ThisAddIn.sidebar_control;
			sidebar.open_type = RepoSidebar.OpenType.COMPARE_TWO;
		}

		private void BtnAutoSave_Click(object sender, RibbonControlEventArgs e)
		{
			if (!Globals.ThisAddIn.enabled) return;
			var button = (RibbonToggleButton)sender;
			bool enabled = button.Checked;
			Globals.ThisAddIn.autosave_service.Switch(enabled);
		}

		#endregion

		private void SwitchRepoPane()
		{
			
			var git_ops = Globals.ThisAddIn.git_ops;
			var pane = Globals.ThisAddIn.repo_pane;
			pane.Visible = !pane.Visible;

			if (pane.Visible)
			{
				// 示例数据（你可以改成从 Git 获取）
				var commits = git_ops.commits;
				Globals.ThisAddIn.sidebar_control.SetCommits(commits);
			}
		}


	}
}
