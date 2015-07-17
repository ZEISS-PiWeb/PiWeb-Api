namespace PiWeb_HelloWorld
{
	partial class MainForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code

		private void InitializeComponent()
		{
			System.Windows.Forms.Label label1;
			this._ConnectionUrlTextBox = new System.Windows.Forms.TextBox();
			this._ConnectButton = new System.Windows.Forms.Button();
			this._TabControl = new System.Windows.Forms.TabControl();
			this._ActionsTabPage = new System.Windows.Forms.TabPage();
			this._FetchDataControl = new PiWeb_HelloWorld.FetchDataControl();
			this._ModifyTabPage = new System.Windows.Forms.TabPage();
			this._ModifyDataControl = new PiWeb_HelloWorld.ModifyDataControl();
			this._LogTabPage = new System.Windows.Forms.TabPage();
			this._ClearLogMessagesButton = new System.Windows.Forms.Button();
			this._LogMessagesTextBox = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			this._TabControl.SuspendLayout();
			this._ActionsTabPage.SuspendLayout();
			this._ModifyTabPage.SuspendLayout();
			this._LogTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(40, 16);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(76, 15);
			label1.TabIndex = 10;
			label1.Text = "Endpoint Uri:";
			// 
			// _ConnectionUrlTextBox
			// 
			this._ConnectionUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._ConnectionUrlTextBox.Location = new System.Drawing.Point(122, 13);
			this._ConnectionUrlTextBox.Margin = new System.Windows.Forms.Padding(4);
			this._ConnectionUrlTextBox.Name = "_ConnectionUrlTextBox";
			this._ConnectionUrlTextBox.Size = new System.Drawing.Size(656, 23);
			this._ConnectionUrlTextBox.TabIndex = 8;
			this._ConnectionUrlTextBox.Text = "http://127.0.0.1:8080";
			// 
			// _ConnectButton
			// 
			this._ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._ConnectButton.Location = new System.Drawing.Point(786, 13);
			this._ConnectButton.Margin = new System.Windows.Forms.Padding(4);
			this._ConnectButton.Name = "_ConnectButton";
			this._ConnectButton.Size = new System.Drawing.Size(154, 23);
			this._ConnectButton.TabIndex = 7;
			this._ConnectButton.Text = "Connect";
			this._ConnectButton.UseVisualStyleBackColor = true;
			this._ConnectButton.Click += new System.EventHandler(this.ConnectButtonClick);
			// 
			// _TabControl
			// 
			this._TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._TabControl.Controls.Add(this._ActionsTabPage);
			this._TabControl.Controls.Add(this._ModifyTabPage);
			this._TabControl.Controls.Add(this._LogTabPage);
			this._TabControl.Location = new System.Drawing.Point(14, 55);
			this._TabControl.Margin = new System.Windows.Forms.Padding(4);
			this._TabControl.Name = "_TabControl";
			this._TabControl.SelectedIndex = 0;
			this._TabControl.Size = new System.Drawing.Size(930, 733);
			this._TabControl.TabIndex = 12;
			// 
			// _ActionsTabPage
			// 
			this._ActionsTabPage.Controls.Add(this._FetchDataControl);
			this._ActionsTabPage.Location = new System.Drawing.Point(4, 24);
			this._ActionsTabPage.Margin = new System.Windows.Forms.Padding(4);
			this._ActionsTabPage.Name = "_ActionsTabPage";
			this._ActionsTabPage.Padding = new System.Windows.Forms.Padding(16);
			this._ActionsTabPage.Size = new System.Drawing.Size(922, 705);
			this._ActionsTabPage.TabIndex = 0;
			this._ActionsTabPage.Text = "Fetching data";
			this._ActionsTabPage.UseVisualStyleBackColor = true;
			// 
			// _FetchDataControl
			// 
			this._FetchDataControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._FetchDataControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._FetchDataControl.Location = new System.Drawing.Point(19, 16);
			this._FetchDataControl.Name = "_FetchDataControl";
			this._FetchDataControl.Padding = new System.Windows.Forms.Padding(2);
			this._FetchDataControl.Size = new System.Drawing.Size(884, 638);
			this._FetchDataControl.TabIndex = 0;
			this._FetchDataControl.LogMessage += new PiWeb_HelloWorld.MainForm.LogHandler(this.LogMessage);
			// 
			// _ModifyTabPage
			// 
			this._ModifyTabPage.Controls.Add(this._ModifyDataControl);
			this._ModifyTabPage.Location = new System.Drawing.Point(4, 24);
			this._ModifyTabPage.Name = "_ModifyTabPage";
			this._ModifyTabPage.Padding = new System.Windows.Forms.Padding(3);
			this._ModifyTabPage.Size = new System.Drawing.Size(922, 705);
			this._ModifyTabPage.TabIndex = 2;
			this._ModifyTabPage.Text = "Modifying data";
			this._ModifyTabPage.UseVisualStyleBackColor = true;
			// 
			// _ModifyDataControl
			// 
			this._ModifyDataControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._ModifyDataControl.Location = new System.Drawing.Point(6, 6);
			this._ModifyDataControl.Name = "_ModifyDataControl";
			this._ModifyDataControl.Size = new System.Drawing.Size(910, 500);
			this._ModifyDataControl.TabIndex = 0;
			this._ModifyDataControl.LogMessage += new PiWeb_HelloWorld.MainForm.LogHandler(this.LogMessage);
			// 
			// _LogTabPage
			// 
			this._LogTabPage.Controls.Add(this._ClearLogMessagesButton);
			this._LogTabPage.Controls.Add(this._LogMessagesTextBox);
			this._LogTabPage.Location = new System.Drawing.Point(4, 24);
			this._LogTabPage.Margin = new System.Windows.Forms.Padding(4);
			this._LogTabPage.Name = "_LogTabPage";
			this._LogTabPage.Padding = new System.Windows.Forms.Padding(4);
			this._LogTabPage.Size = new System.Drawing.Size(922, 705);
			this._LogTabPage.TabIndex = 1;
			this._LogTabPage.Text = "Log messages";
			this._LogTabPage.UseVisualStyleBackColor = true;
			// 
			// _ClearLogMessagesButton
			// 
			this._ClearLogMessagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._ClearLogMessagesButton.Location = new System.Drawing.Point(752, 671);
			this._ClearLogMessagesButton.Margin = new System.Windows.Forms.Padding(4);
			this._ClearLogMessagesButton.Name = "_ClearLogMessagesButton";
			this._ClearLogMessagesButton.Size = new System.Drawing.Size(157, 26);
			this._ClearLogMessagesButton.TabIndex = 1;
			this._ClearLogMessagesButton.Text = "Clear log messages";
			this._ClearLogMessagesButton.UseVisualStyleBackColor = true;
			this._ClearLogMessagesButton.Click += new System.EventHandler(this.ClearLogMessagesButton_Click);
			// 
			// _LogMessagesTextBox
			// 
			this._LogMessagesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._LogMessagesTextBox.Location = new System.Drawing.Point(7, 7);
			this._LogMessagesTextBox.Margin = new System.Windows.Forms.Padding(4);
			this._LogMessagesTextBox.Multiline = true;
			this._LogMessagesTextBox.Name = "_LogMessagesTextBox";
			this._LogMessagesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._LogMessagesTextBox.Size = new System.Drawing.Size(902, 656);
			this._LogMessagesTextBox.TabIndex = 0;
			this._LogMessagesTextBox.WordWrap = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(958, 801);
			this.Controls.Add(this._TabControl);
			this.Controls.Add(label1);
			this.Controls.Add(this._ConnectionUrlTextBox);
			this.Controls.Add(this._ConnectButton);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(974, 840);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = " PiWeb API Sample application";
			this._TabControl.ResumeLayout(false);
			this._ActionsTabPage.ResumeLayout(false);
			this._ModifyTabPage.ResumeLayout(false);
			this._LogTabPage.ResumeLayout(false);
			this._LogTabPage.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _ConnectionUrlTextBox;
		private System.Windows.Forms.Button _ConnectButton;
		private System.Windows.Forms.TabControl _TabControl;
		private System.Windows.Forms.TabPage _ActionsTabPage;
		private System.Windows.Forms.TabPage _LogTabPage;
		private System.Windows.Forms.TextBox _LogMessagesTextBox;
		private System.Windows.Forms.Button _ClearLogMessagesButton;
		private FetchDataControl _FetchDataControl;
		private System.Windows.Forms.TabPage _ModifyTabPage;
		private ModifyDataControl _ModifyDataControl;
	}
}

