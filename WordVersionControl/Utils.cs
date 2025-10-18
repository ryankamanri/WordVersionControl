using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace WordVersionControl
{
	internal class Utils
	{
		private const string XML_NAMESPACE = "urn:wvc-repo-signature";
		private const string ROOT_NAME = "RepoSignature";
		private const string NODE_NAME = "Id";

		public static void SetRepoSignature(Word.Document doc, string repoId)
		{
			// 删除旧的 XML Part（避免重复）
			foreach (Office.CustomXMLPart part in doc.CustomXMLParts)
			{
				if (part.DocumentElement != null && part.DocumentElement.NamespaceURI == XML_NAMESPACE)
				{
					part.Delete();
					break;
				}
			}

			// 生成新的 XML 内容
			string xmlContent = $@"
			<{ROOT_NAME} xmlns='{XML_NAMESPACE}'>
				<{NODE_NAME}>{System.Security.SecurityElement.Escape(repoId)}</{NODE_NAME}>
			</{ROOT_NAME}>";

			// 添加到文档
			doc.CustomXMLParts.Add(xmlContent);
		}

		public static string GetRepoSignature(Word.Document doc)
		{
			foreach (Office.CustomXMLPart part in doc.CustomXMLParts)
			{
				if (part.DocumentElement != null && part.DocumentElement.NamespaceURI == XML_NAMESPACE)
				{
					foreach (Office.CustomXMLNode child in part.DocumentElement.ChildNodes)
					{
						if (child.BaseName == NODE_NAME)
						{
							return child.Text;
						}
					}
				}
			}
			return null;
		}

		private static string WVC_DIRECTORY_NAME = ".wvc";
		public static string EnsureWVCDirectory()
		{
			string user_folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string wvc_folder = Path.Combine(user_folder, WVC_DIRECTORY_NAME);
			if (! Directory.Exists(wvc_folder))
			{
				Directory.CreateDirectory(wvc_folder);
			}
			return wvc_folder;
		}


		public static void ShowBalloonTip(string message, string title, int timeout = 3000)
		{
			NotifyIcon notifyIcon = new NotifyIcon
			{
				Icon = System.Drawing.SystemIcons.Information,
				Visible = true
			};
			notifyIcon.ShowBalloonTip(timeout, title, message, ToolTipIcon.Info);
			// 自动释放资源
			var timer = new Timer { Interval = timeout + 500 };
			timer.Tick += (s, e) =>
			{
				notifyIcon.Visible = false;
				notifyIcon.Dispose();
				timer.Stop();
				timer.Dispose();
			};
			timer.Start();
		}
	}
}
