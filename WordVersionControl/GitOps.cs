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
		public string repository_path { get; private set; }
		public string tmp_path => Path.Combine(repository_path, ".tmp");
		public const string STAGED_FILE_NAME = "__staged_version__.docx";
		public const string FILENAME_FILE_NAME = "filename";
		public List<Commit> commits { get; private set; }
		public GitOps(string repository_path) 
		{
			this.repository_path = repository_path;

			this.commits = new List<Commit>();
			UpdateCommits();
		}

		public void UpdateCommits()
		{
			foreach (var commit in UpdatedCommits())
			{
				commits.Add(commit);
			}
		}

		private IEnumerable<Commit> UpdatedCommits()
		{
			// check whether the repository is empty
			if(RunGit(repository_path, "show-ref", false).Length == 0)
			{
				yield break;
			}
			//
			string git_format_output_str = $"log --format=\"%H|%an|%cI\"";
            foreach (var format_str in RunGit(repository_path, git_format_output_str).Split('\n'))
            {
				if (format_str.Length == 0) continue; 
				var format_list = format_str.Split('|');
				var message = RunGit(repository_path, $"show --format=\"%B\" {format_list[0]} -s");

				yield return new Commit(
					format_list[0], // commit_id
					format_list[1], // author
					DateTime.Parse(format_list[2]), // committer date, strict ISO 8601 format
					message
				);
            }
		}

		public string Export(Commit commit, string filename)
		{
			var output_path = Path.Combine(tmp_path, filename);
			try
			{
				ExportGitVersion(repository_path, $"{commit.commit_id}:\"{STAGED_FILE_NAME}\"", output_path);
			} catch (Exception e) 
			{ 
				MessageBox.Show(e.StackTrace, e.Message);
			}
			
			return output_path;
		}

		public void Commit(string message)
		{
			RunGit(repository_path, $"add {GitOps.STAGED_FILE_NAME} {GitOps.FILENAME_FILE_NAME}");
			RunGit(repository_path, $"commit -m \"{message.Replace("\"", "\\\"")}\"");
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
				var tmp_path = Path.Combine(repo_path, ".tmp");
				Directory.CreateDirectory(tmp_path);
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

		private static string RunGit(string dir, string args, bool show_message = true)
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
					if (show_message) MessageBox.Show(output + "\n" + error, "Git Error");
					return "";
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
