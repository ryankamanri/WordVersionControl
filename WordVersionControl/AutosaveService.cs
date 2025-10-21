using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Office.Interop.Word;

namespace WordVersionControl
{
	public class AutosaveService
	{
		private bool enabled = false;
		private Timer autosave_timer;
		private Document document;
		public AutosaveService(Document document, double interval = 5000) 
		{
			autosave_timer = new Timer();
			autosave_timer.Interval = interval; // 每 5 秒检测一次
			autosave_timer.Elapsed += Autosave_timer_Elapsed;

			this.document = document;

			if (enabled) autosave_timer.Start();
		}

		private void Autosave_timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var doc = this.document;
			if (doc == null || doc.Saved) return;

			try
			{
				doc.Save();
				//Utils.ShowBalloonTip("已自动保存", "已自动保存");
			}
			catch (Exception ex)
			{
				Utils.ShowBalloonTip(ex.Message, "自动保存失败");
			}
		}

		public void Switch(bool open)
		{
			if (open && !enabled)
			{
				autosave_timer.Start();
				enabled = true;
			}
			
			if (!open && enabled)
			{
				autosave_timer.Stop();
				enabled = false;
			}
		}

	}
}
