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
			this.operation_tab = this.Factory.CreateRibbonTab();
			this.group_history = this.Factory.CreateRibbonGroup();
			this.btn_show_history = this.Factory.CreateRibbonButton();
			this.group_compare = this.Factory.CreateRibbonGroup();
			this.btn_compare = this.Factory.CreateRibbonButton();
			this.group_commit = this.Factory.CreateRibbonGroup();
			this.btn_save_commit = this.Factory.CreateRibbonButton();
			this.group_autosave = this.Factory.CreateRibbonGroup();
			this.btn_autosave = this.Factory.CreateRibbonButton();
			this.operation_tab.SuspendLayout();
			this.group_history.SuspendLayout();
			this.group_compare.SuspendLayout();
			this.group_commit.SuspendLayout();
			this.group_autosave.SuspendLayout();
			this.SuspendLayout();
			// 
			// operation_tab
			// 
			this.operation_tab.Groups.Add(this.group_history);
			this.operation_tab.Groups.Add(this.group_compare);
			this.operation_tab.Groups.Add(this.group_commit);
			this.operation_tab.Groups.Add(this.group_autosave);
			this.operation_tab.Label = "版本控制";
			this.operation_tab.Name = "operation_tab";
			// 
			// group_history
			// 
			this.group_history.Items.Add(this.btn_show_history);
			this.group_history.Label = "历史记录";
			this.group_history.Name = "group_history";
			// 
			// btn_show_history
			// 
			this.btn_show_history.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.btn_show_history.Image = global::WordVersionControl.Properties.Resources.扁平化立体线性多色APP网页按钮导航分类_爱给网_aigei_com__3___1_;
			this.btn_show_history.Label = "查看历史";
			this.btn_show_history.Name = "btn_show_history";
			this.btn_show_history.ShowImage = true;
			this.btn_show_history.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnShowLog_Click);
			// 
			// group_compare
			// 
			this.group_compare.Items.Add(this.btn_compare);
			this.group_compare.Label = "比较";
			this.group_compare.Name = "group_compare";
			// 
			// btn_compare
			// 
			this.btn_compare.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.btn_compare.Image = global::WordVersionControl.Properties.Resources.扁平化立体线性多色APP网页按钮导航分类_爱给网_aigei_com__2___1_;
			this.btn_compare.Label = "文档比较";
			this.btn_compare.Name = "btn_compare";
			this.btn_compare.ShowImage = true;
			this.btn_compare.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnCompare_Click);
			// 
			// group_commit
			// 
			this.group_commit.Items.Add(this.btn_save_commit);
			this.group_commit.Label = "提交";
			this.group_commit.Name = "group_commit";
			// 
			// btn_save_commit
			// 
			this.btn_save_commit.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.btn_save_commit.Image = global::WordVersionControl.Properties.Resources.扁平化立体线性多色APP网页按钮导航分类_爱给网_aigei_com__4___1_;
			this.btn_save_commit.Label = "提交变更";
			this.btn_save_commit.Name = "btn_save_commit";
			this.btn_save_commit.ShowImage = true;
			this.btn_save_commit.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSaveCommit_Click);
			// 
			// group_autosave
			// 
			this.group_autosave.Items.Add(this.btn_autosave);
			this.group_autosave.Label = "自动保存";
			this.group_autosave.Name = "group_autosave";
			// 
			// btn_autosave
			// 
			this.btn_autosave.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.btn_autosave.Image = global::WordVersionControl.Properties.Resources.扁平化立体线性多色APP网页按钮导航分类_爱给网_aigei_com__5___1_;
			this.btn_autosave.Label = "自动保存";
			this.btn_autosave.Name = "btn_autosave";
			this.btn_autosave.ShowImage = true;
			// 
			// Ribbon1
			// 
			this.Name = "Ribbon1";
			this.RibbonType = "Microsoft.Word.Document";
			this.Tabs.Add(this.operation_tab);
			this.operation_tab.ResumeLayout(false);
			this.operation_tab.PerformLayout();
			this.group_history.ResumeLayout(false);
			this.group_history.PerformLayout();
			this.group_compare.ResumeLayout(false);
			this.group_compare.PerformLayout();
			this.group_commit.ResumeLayout(false);
			this.group_commit.PerformLayout();
			this.group_autosave.ResumeLayout(false);
			this.group_autosave.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Office.Tools.Ribbon.RibbonTab operation_tab;
		private Microsoft.Office.Tools.Ribbon.RibbonGroup group_history;
		private Microsoft.Office.Tools.Ribbon.RibbonGroup group_compare;
		private Microsoft.Office.Tools.Ribbon.RibbonGroup group_commit;
		private Microsoft.Office.Tools.Ribbon.RibbonGroup group_autosave;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btn_save_commit;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btn_show_history;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btn_compare;
		private Microsoft.Office.Tools.Ribbon.RibbonButton btn_autosave;
	}

	partial class ThisRibbonCollection
	{
		internal Ribbon1 Ribbon1
		{
			get { return this.GetRibbon<Ribbon1>(); }
		}
	}
}
