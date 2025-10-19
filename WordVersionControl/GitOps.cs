using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using WordVersionControl;
using WordVersionControl.Models;

namespace WordVersionControl
{
	public class GitOps
	{
		private string repository_path;
		private const string GIT_FORMAT_OUTPUT_SPLITER = "|||";
		public GitOps(string repository_path) 
		{
			this.repository_path = repository_path;
		}

		public IEnumerable<Commit> GetLog()
		{
			string git_format_output_str = $"log --format=\"%H|%an|%cI|%B\"";
			//RunGit(repository_path, );
			return default;
		}

		public void Export(Commit commit, string filename)
		{
			
		}

		public void Commit(Commit commit, string message)
		{
		}

		public static string EnsureGitRepo(string wvc_path, Document doc)
		{
			var repo_sign = Utils.GetRepoSignature(doc);
			string repo_path;
			if (repo_sign == null)
			{
				//// 未绑定，初始化新的仓库
				repo_sign = Guid.NewGuid().ToString("N");
				repo_path = Path.Combine(wvc_path, repo_sign);
				Directory.CreateDirectory(repo_path);
				RunGit(repo_path, "init");
				Utils.SetRepoSignature(doc, repo_sign);

				Utils.ShowBalloonTip(
					$"已为此文档创建 Git 仓库：\n{repo_sign}",
					"Git 初始化完成");
			}
			else
			{
				// 已存在签名
				repo_path = Path.Combine(wvc_path, repo_sign);
				if (repo_path == null || !Directory.Exists(repo_path))
				{
					Utils.ShowBalloonTip(
						"找不到对应的 Git 仓库，可能被移动或删除。",
						"Git 仓库丢失");
				} else
				{
					Utils.ShowBalloonTip(
					$"已找到此文档对应的 Git 仓库：\n{repo_sign}",
					"Git 初始化完成");
				}
			}

			return repo_path;
		}

		private static string RunGit(string dir, string args)
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
				proc.WaitForExit();
				string output = proc.StandardOutput.ReadToEnd();
				string error = proc.StandardError.ReadToEnd();
				if (proc.ExitCode != 0) 
				{
					MessageBox.Show(output + "\n" + error, "Git Error");
				}
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
