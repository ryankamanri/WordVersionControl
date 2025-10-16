namespace WordVersionControl
{
	partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Ribbon1()
			: base(Globals.Factory.GetRibbonFactory())
		{
			InitializeComponent();
		}

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.tab1 = this.Factory.CreateRibbonTab();
			this.group1 = this.Factory.CreateRibbonGroup();
			this.btnSaveCommit = this.Factory.CreateRibbonButton();
			this.btnShowLog = this.Factory.CreateRibbonButton();
			this.btnCompare = this.Factory.CreateRibbonButton();
			this.tab1.SuspendLayout();
			this.group1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tab1
			// 
			this.tab1.Groups.Add(this.group1);
			this.tab1.Label = "版本控制";
			this.tab1.Name = "tab1";
			// 
			// group1
			// 
			this.group1.Items.Add(this.btnSaveCommit);
			this.group1.Items.Add(this.btnShowLog);
			this.group1.Items.Add(this.btnCompare);
			this.group1.Label = "操作";
			this.group1.Name = "group1";
			// 
			// btnSaveCommit
			// 
			this.btnSaveCommit.Label = "保存并提交";
			this.btnSaveCommit.Name = "btnSaveCommit";
			this.btnSaveCommit.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSaveCommit_Click);
			// 
			// btnShowLog
			// 
			this.btnShowLog.Label = "查看历史";
			this.btnShowLog.Name = "btnShowLog";
			this.btnShowLog.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnShowLog_Click);
			// 
			// btnCompare
			// 
			this.btnCompare.Label = "比较最近两次";
			this.btnCompare.Name = "btnCompare";
			this.btnCompare.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnCompare_Click);
			// 
			// Ribbon1
			// 
			this.Name = "Ribbon1";
			this.RibbonType = "Microsoft.Word.Document";
			this.Tabs.Add(this.tab1);
			this.tab1.ResumeLayout(false);
			this.tab1.PerformLayout();
			this.group1.ResumeLayout(false);
			this.group1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
		private Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btnSaveCommit;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btnShowLog;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btnCompare;
	}

	partial class ThisRibbonCollection
	{
		internal Ribbon1 Ribbon1
		{
			get { return this.GetRibbon<Ribbon1>(); }
		}
	}
}
