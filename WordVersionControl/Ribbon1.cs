using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace WordVersionControl
{
	public partial class Ribbon1
	{
		private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
		{
			
		}

		private void BtnSaveCommit_Click(object sender, RibbonControlEventArgs e)
		{
			var doc = Globals.ThisAddIn.Application.ActiveDocument;
			if (doc == null) { MessageBox.Show("未打开任何文档"); return; }

			string docPath = doc.FullName;
			string dir = Path.GetDirectoryName(docPath);
			doc.Save();

			EnsureGitRepo(dir);
			RunGit(dir, $"add \"{Path.GetFileName(docPath)}\"");
			RunGit(dir, $"commit -m \"Auto save {DateTime.Now:yyyy-MM-dd HH:mm:ss}\"");
			MessageBox.Show("已提交到 Git");
		}

		private void BtnShowLog_Click(object sender, RibbonControlEventArgs e)
		{
			var doc = Globals.ThisAddIn.Application.ActiveDocument;
			if (doc == null) { MessageBox.Show("未打开任何文档"); return; }
			string dir = Path.GetDirectoryName(doc.FullName);

			string output = RunGit(dir, "log --oneline -n 10");
			MessageBox.Show(output, "最近版本历史");
		}

		private void BtnCompare_Click(object sender, RibbonControlEventArgs e)
		{
			var doc = Globals.ThisAddIn.Application.ActiveDocument;
			if (doc == null) { MessageBox.Show("未打开任何文档"); return; }
			string dir = Path.GetDirectoryName(doc.FullName);
			string fileName = Path.GetFileName(doc.FullName);

			// 提取最近两次提交的版本
			string oldPath = Path.Combine(dir, "_old.docx");
			string newPath = Path.Combine(dir, "_new.docx");
			ExportGitVersion(dir, $"HEAD~1:\"{fileName}\"", oldPath);
			ExportGitVersion(dir, $"HEAD:\"{fileName}\"", newPath);

			// 调用 Word 的比较功能
			var app = Globals.ThisAddIn.Application;
			app.CompareDocuments(
				app.Documents.Open(oldPath, ReadOnly: true),
				app.Documents.Open(newPath, ReadOnly: true),
				Microsoft.Office.Interop.Word.WdCompareDestination.wdCompareDestinationNew
			);
		}

		private void EnsureGitRepo(string dir)
		{
			if (!Directory.Exists(Path.Combine(dir, ".git")))
				RunGit(dir, "init");
		}

		private string RunGit(string dir, string args)
		{
			var psi = new ProcessStartInfo("git", args)
			{
				WorkingDirectory = dir,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				StandardOutputEncoding = Encoding.UTF8,
				StandardErrorEncoding = Encoding.UTF8
			};
			using (var proc = Process.Start(psi))
			{
				string output = proc.StandardOutput.ReadToEnd() + proc.StandardError.ReadToEnd();
				proc.WaitForExit();
				return output;
			}
		}

		private void ExportGitVersion(string dir, string gitSpec, string outputPath)
		{
			var psi = new ProcessStartInfo("git", $"show {gitSpec}")
			{
				WorkingDirectory = dir,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				StandardOutputEncoding = Encoding.UTF8
			};

			using (var proc = Process.Start(psi))
			{
				using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
				{
					proc.StandardOutput.BaseStream.CopyTo(fs);
				}
				proc.WaitForExit();

				if (proc.ExitCode != 0)
				{
					string err = proc.StandardError.ReadToEnd();
					throw new Exception($"Git show 失败: {err}");
				}
			}
		}
	}
}
