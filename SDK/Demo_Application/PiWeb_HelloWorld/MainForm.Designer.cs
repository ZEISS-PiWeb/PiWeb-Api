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
			System.Windows.Forms.ColumnHeader columnHeader2;
			System.Windows.Forms.ColumnHeader columnHeader3;
			System.Windows.Forms.ColumnHeader columnHeader4;
			System.Windows.Forms.ColumnHeader columnHeader5;
			System.Windows.Forms.ColumnHeader columnHeader7;
			System.Windows.Forms.ColumnHeader columnHeader9;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.ColumnHeader _CharPathColHeader;
			System.Windows.Forms.ColumnHeader _ValueColHeader;
			System.Windows.Forms.ColumnHeader columnHeader6;
			System.Windows.Forms.ColumnHeader columnHeader1;
			System.Windows.Forms.ColumnHeader columnHeader8;
			System.Windows.Forms.ColumnHeader columnHeader10;
			System.Windows.Forms.ColumnHeader columnHeader11;
			System.Windows.Forms.ColumnHeader columnHeader12;
			System.Windows.Forms.ColumnHeader columnHeader13;
			this._ConnectionUrlTextBox = new System.Windows.Forms.TextBox();
			this._ConnectButton = new System.Windows.Forms.Button();
			this._CharacteristicsListView = new System.Windows.Forms.ListView();
			this._PartsListView = new System.Windows.Forms.ListView();
			this._RawDataListView = new System.Windows.Forms.ListView();
			this._MeasurementValuesListView = new System.Windows.Forms.ListView();
			this._MeasurementsListView = new System.Windows.Forms.ListView();
			this._TabControl = new System.Windows.Forms.TabControl();
			this._ActionsTabPage = new System.Windows.Forms.TabPage();
			this._NoMeasurementSelectedLabel = new System.Windows.Forms.Label();
			this._AdditionalDataRadioButton = new System.Windows.Forms.RadioButton();
			this._MeasurementValuesRadioButton = new System.Windows.Forms.RadioButton();
			this._MeasurementsRadioButton = new System.Windows.Forms.RadioButton();
			this._CharacteristicsRadioButton = new System.Windows.Forms.RadioButton();
			this._LogTabPage = new System.Windows.Forms.TabPage();
			this._ClearLogMessagesButton = new System.Windows.Forms.Button();
			this._LogMessagesTextBox = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			label2 = new System.Windows.Forms.Label();
			_CharPathColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			_ValueColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._TabControl.SuspendLayout();
			this._ActionsTabPage.SuspendLayout();
			this._LogTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// _ConnectionUrlTextBox
			// 
			this._ConnectionUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._ConnectionUrlTextBox.Location = new System.Drawing.Point(122, 13);
			this._ConnectionUrlTextBox.Margin = new System.Windows.Forms.Padding(4);
			this._ConnectionUrlTextBox.Name = "_ConnectionUrlTextBox";
			this._ConnectionUrlTextBox.Size = new System.Drawing.Size(512, 23);
			this._ConnectionUrlTextBox.TabIndex = 8;
			this._ConnectionUrlTextBox.Text = "http://127.0.0.1:8080";
			// 
			// _ConnectButton
			// 
			this._ConnectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._ConnectButton.Location = new System.Drawing.Point(642, 13);
			this._ConnectButton.Margin = new System.Windows.Forms.Padding(4);
			this._ConnectButton.Name = "_ConnectButton";
			this._ConnectButton.Size = new System.Drawing.Size(154, 23);
			this._ConnectButton.TabIndex = 7;
			this._ConnectButton.Text = "Connect";
			this._ConnectButton.UseVisualStyleBackColor = true;
			this._ConnectButton.Click += new System.EventHandler(this.ConnectButtonClick);
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
			// _CharacteristicsListView
			// 
			this._CharacteristicsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._CharacteristicsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader2,
            columnHeader3,
            columnHeader4});
			this._CharacteristicsListView.FullRowSelect = true;
			this._CharacteristicsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._CharacteristicsListView.Location = new System.Drawing.Point(8, 247);
			this._CharacteristicsListView.Margin = new System.Windows.Forms.Padding(4);
			this._CharacteristicsListView.MultiSelect = false;
			this._CharacteristicsListView.Name = "_CharacteristicsListView";
			this._CharacteristicsListView.ShowItemToolTips = true;
			this._CharacteristicsListView.Size = new System.Drawing.Size(762, 386);
			this._CharacteristicsListView.TabIndex = 3;
			this._CharacteristicsListView.UseCompatibleStateImageBehavior = false;
			this._CharacteristicsListView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "Path";
			columnHeader2.Width = 390;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "Change Date";
			columnHeader3.Width = 186;
			// 
			// columnHeader4
			// 
			columnHeader4.Text = "Attributes";
			columnHeader4.Width = 180;
			// 
			// _PartsListView
			// 
			this._PartsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._PartsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader5,
            columnHeader7,
            columnHeader9});
			this._PartsListView.FullRowSelect = true;
			this._PartsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._PartsListView.Location = new System.Drawing.Point(8, 37);
			this._PartsListView.Margin = new System.Windows.Forms.Padding(4);
			this._PartsListView.MultiSelect = false;
			this._PartsListView.Name = "_PartsListView";
			this._PartsListView.ShowItemToolTips = true;
			this._PartsListView.Size = new System.Drawing.Size(762, 150);
			this._PartsListView.TabIndex = 3;
			this._PartsListView.UseCompatibleStateImageBehavior = false;
			this._PartsListView.View = System.Windows.Forms.View.Details;
			this._PartsListView.SelectedIndexChanged += new System.EventHandler(this.PartsListView_SelectedIndexChanged);
			// 
			// columnHeader5
			// 
			columnHeader5.Text = "Path";
			columnHeader5.Width = 467;
			// 
			// columnHeader7
			// 
			columnHeader7.Text = "Change Date";
			columnHeader7.Width = 140;
			// 
			// columnHeader9
			// 
			columnHeader9.Text = "Attributes";
			columnHeader9.Width = 148;
			// 
			// _RawDataListView
			// 
			this._RawDataListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._RawDataListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader10,
            columnHeader11,
            columnHeader13,
            columnHeader12});
			this._RawDataListView.FullRowSelect = true;
			this._RawDataListView.Location = new System.Drawing.Point(8, 247);
			this._RawDataListView.Margin = new System.Windows.Forms.Padding(4);
			this._RawDataListView.Name = "_RawDataListView";
			this._RawDataListView.Size = new System.Drawing.Size(762, 386);
			this._RawDataListView.TabIndex = 0;
			this._RawDataListView.UseCompatibleStateImageBehavior = false;
			this._RawDataListView.View = System.Windows.Forms.View.Details;
			this._RawDataListView.Visible = false;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(5, 18);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(33, 15);
			label2.TabIndex = 1;
			label2.Text = "Parts";
			// 
			// _MeasurementValuesListView
			// 
			this._MeasurementValuesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._MeasurementValuesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _CharPathColHeader,
            _ValueColHeader});
			this._MeasurementValuesListView.FullRowSelect = true;
			this._MeasurementValuesListView.Location = new System.Drawing.Point(8, 247);
			this._MeasurementValuesListView.Margin = new System.Windows.Forms.Padding(4);
			this._MeasurementValuesListView.MultiSelect = false;
			this._MeasurementValuesListView.Name = "_MeasurementValuesListView";
			this._MeasurementValuesListView.Size = new System.Drawing.Size(762, 386);
			this._MeasurementValuesListView.TabIndex = 3;
			this._MeasurementValuesListView.UseCompatibleStateImageBehavior = false;
			this._MeasurementValuesListView.View = System.Windows.Forms.View.Details;
			this._MeasurementValuesListView.Visible = false;
			// 
			// _CharPathColHeader
			// 
			_CharPathColHeader.Text = "Characteristic";
			_CharPathColHeader.Width = 523;
			// 
			// _ValueColHeader
			// 
			_ValueColHeader.Text = "Value";
			_ValueColHeader.Width = 234;
			// 
			// _MeasurementsListView
			// 
			this._MeasurementsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._MeasurementsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader6,
            columnHeader1,
            columnHeader8});
			this._MeasurementsListView.FullRowSelect = true;
			this._MeasurementsListView.Location = new System.Drawing.Point(8, 247);
			this._MeasurementsListView.Margin = new System.Windows.Forms.Padding(4);
			this._MeasurementsListView.MultiSelect = false;
			this._MeasurementsListView.Name = "_MeasurementsListView";
			this._MeasurementsListView.ShowItemToolTips = true;
			this._MeasurementsListView.Size = new System.Drawing.Size(762, 386);
			this._MeasurementsListView.TabIndex = 3;
			this._MeasurementsListView.UseCompatibleStateImageBehavior = false;
			this._MeasurementsListView.View = System.Windows.Forms.View.Details;
			this._MeasurementsListView.Visible = false;
			this._MeasurementsListView.SelectedIndexChanged += new System.EventHandler(this.MeasurementsListView_SelectedIndexChanged);
			// 
			// columnHeader6
			// 
			columnHeader6.Text = "Date";
			columnHeader6.Width = 210;
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "Last Modified";
			columnHeader1.Width = 230;
			// 
			// columnHeader8
			// 
			columnHeader8.Text = "Attributes";
			columnHeader8.Width = 312;
			// 
			// _TabControl
			// 
			this._TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._TabControl.Controls.Add(this._ActionsTabPage);
			this._TabControl.Controls.Add(this._LogTabPage);
			this._TabControl.Location = new System.Drawing.Point(14, 55);
			this._TabControl.Margin = new System.Windows.Forms.Padding(4);
			this._TabControl.Name = "_TabControl";
			this._TabControl.SelectedIndex = 0;
			this._TabControl.Size = new System.Drawing.Size(786, 665);
			this._TabControl.TabIndex = 12;
			// 
			// _ActionsTabPage
			// 
			this._ActionsTabPage.Controls.Add(this._RawDataListView);
			this._ActionsTabPage.Controls.Add(this._NoMeasurementSelectedLabel);
			this._ActionsTabPage.Controls.Add(this._AdditionalDataRadioButton);
			this._ActionsTabPage.Controls.Add(this._MeasurementValuesRadioButton);
			this._ActionsTabPage.Controls.Add(this._MeasurementsRadioButton);
			this._ActionsTabPage.Controls.Add(this._CharacteristicsRadioButton);
			this._ActionsTabPage.Controls.Add(this._PartsListView);
			this._ActionsTabPage.Controls.Add(label2);
			this._ActionsTabPage.Controls.Add(this._MeasurementValuesListView);
			this._ActionsTabPage.Controls.Add(this._MeasurementsListView);
			this._ActionsTabPage.Controls.Add(this._CharacteristicsListView);
			this._ActionsTabPage.Location = new System.Drawing.Point(4, 24);
			this._ActionsTabPage.Margin = new System.Windows.Forms.Padding(4);
			this._ActionsTabPage.Name = "_ActionsTabPage";
			this._ActionsTabPage.Padding = new System.Windows.Forms.Padding(4);
			this._ActionsTabPage.Size = new System.Drawing.Size(778, 637);
			this._ActionsTabPage.TabIndex = 0;
			this._ActionsTabPage.Text = "Fetching data";
			this._ActionsTabPage.UseVisualStyleBackColor = true;
			// 
			// _NoMeasurementSelectedLabel
			// 
			this._NoMeasurementSelectedLabel.AutoSize = true;
			this._NoMeasurementSelectedLabel.ForeColor = System.Drawing.SystemColors.GrayText;
			this._NoMeasurementSelectedLabel.Location = new System.Drawing.Point(186, 436);
			this._NoMeasurementSelectedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this._NoMeasurementSelectedLabel.Name = "_NoMeasurementSelectedLabel";
			this._NoMeasurementSelectedLabel.Size = new System.Drawing.Size(331, 15);
			this._NoMeasurementSelectedLabel.TabIndex = 1;
			this._NoMeasurementSelectedLabel.Text = "Please select a measurement to view the measurement values";
			this._NoMeasurementSelectedLabel.Visible = false;
			// 
			// _AdditionalDataRadioButton
			// 
			this._AdditionalDataRadioButton.AutoSize = true;
			this._AdditionalDataRadioButton.Location = new System.Drawing.Point(429, 221);
			this._AdditionalDataRadioButton.Name = "_AdditionalDataRadioButton";
			this._AdditionalDataRadioButton.Size = new System.Drawing.Size(139, 19);
			this._AdditionalDataRadioButton.TabIndex = 4;
			this._AdditionalDataRadioButton.Text = "Additional Data (Part)";
			this._AdditionalDataRadioButton.UseVisualStyleBackColor = true;
			this._AdditionalDataRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _MeasurementValuesRadioButton
			// 
			this._MeasurementValuesRadioButton.AutoSize = true;
			this._MeasurementValuesRadioButton.Location = new System.Drawing.Point(267, 221);
			this._MeasurementValuesRadioButton.Name = "_MeasurementValuesRadioButton";
			this._MeasurementValuesRadioButton.Size = new System.Drawing.Size(135, 19);
			this._MeasurementValuesRadioButton.TabIndex = 4;
			this._MeasurementValuesRadioButton.Text = "Measurement Values";
			this._MeasurementValuesRadioButton.UseVisualStyleBackColor = true;
			this._MeasurementValuesRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _MeasurementsRadioButton
			// 
			this._MeasurementsRadioButton.AutoSize = true;
			this._MeasurementsRadioButton.Location = new System.Drawing.Point(137, 221);
			this._MeasurementsRadioButton.Name = "_MeasurementsRadioButton";
			this._MeasurementsRadioButton.Size = new System.Drawing.Size(103, 19);
			this._MeasurementsRadioButton.TabIndex = 4;
			this._MeasurementsRadioButton.Text = "Measurements";
			this._MeasurementsRadioButton.UseVisualStyleBackColor = true;
			this._MeasurementsRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _CharacteristicsRadioButton
			// 
			this._CharacteristicsRadioButton.AutoSize = true;
			this._CharacteristicsRadioButton.Checked = true;
			this._CharacteristicsRadioButton.Location = new System.Drawing.Point(8, 221);
			this._CharacteristicsRadioButton.Name = "_CharacteristicsRadioButton";
			this._CharacteristicsRadioButton.Size = new System.Drawing.Size(102, 19);
			this._CharacteristicsRadioButton.TabIndex = 4;
			this._CharacteristicsRadioButton.TabStop = true;
			this._CharacteristicsRadioButton.Text = "Characteristics";
			this._CharacteristicsRadioButton.UseVisualStyleBackColor = true;
			this._CharacteristicsRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _LogTabPage
			// 
			this._LogTabPage.Controls.Add(this._ClearLogMessagesButton);
			this._LogTabPage.Controls.Add(this._LogMessagesTextBox);
			this._LogTabPage.Location = new System.Drawing.Point(4, 24);
			this._LogTabPage.Margin = new System.Windows.Forms.Padding(4);
			this._LogTabPage.Name = "_LogTabPage";
			this._LogTabPage.Padding = new System.Windows.Forms.Padding(4);
			this._LogTabPage.Size = new System.Drawing.Size(778, 637);
			this._LogTabPage.TabIndex = 1;
			this._LogTabPage.Text = "Log messages";
			this._LogTabPage.UseVisualStyleBackColor = true;
			// 
			// _ClearLogMessagesButton
			// 
			this._ClearLogMessagesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._ClearLogMessagesButton.Location = new System.Drawing.Point(613, 603);
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
			this._LogMessagesTextBox.Size = new System.Drawing.Size(763, 588);
			this._LogMessagesTextBox.TabIndex = 0;
			this._LogMessagesTextBox.WordWrap = false;
			// 
			// columnHeader10
			// 
			columnHeader10.Text = "Filename";
			columnHeader10.Width = 258;
			// 
			// columnHeader11
			// 
			columnHeader11.Text = "Mime Type";
			columnHeader11.Width = 175;
			// 
			// columnHeader12
			// 
			columnHeader12.Text = "Size (Bytes)";
			columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			columnHeader12.Width = 115;
			// 
			// columnHeader13
			// 
			columnHeader13.Text = "Created";
			columnHeader13.Width = 192;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(814, 733);
			this.Controls.Add(this._TabControl);
			this.Controls.Add(label1);
			this.Controls.Add(this._ConnectionUrlTextBox);
			this.Controls.Add(this._ConnectButton);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = " PiWeb API Sample application";
			this._TabControl.ResumeLayout(false);
			this._ActionsTabPage.ResumeLayout(false);
			this._ActionsTabPage.PerformLayout();
			this._LogTabPage.ResumeLayout(false);
			this._LogTabPage.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _ConnectionUrlTextBox;
		private System.Windows.Forms.Button _ConnectButton;
		private System.Windows.Forms.ListView _MeasurementValuesListView;
		private System.Windows.Forms.ListView _MeasurementsListView;
		private System.Windows.Forms.ListView _CharacteristicsListView;
		private System.Windows.Forms.ListView _PartsListView;
		private System.Windows.Forms.ListView _RawDataListView;
		private System.Windows.Forms.TabControl _TabControl;
		private System.Windows.Forms.TabPage _ActionsTabPage;
		private System.Windows.Forms.TabPage _LogTabPage;
		private System.Windows.Forms.TextBox _LogMessagesTextBox;
		private System.Windows.Forms.Button _ClearLogMessagesButton;
		private System.Windows.Forms.RadioButton _CharacteristicsRadioButton;
		private System.Windows.Forms.RadioButton _MeasurementValuesRadioButton;
		private System.Windows.Forms.RadioButton _MeasurementsRadioButton;
		private System.Windows.Forms.RadioButton _AdditionalDataRadioButton;
		private System.Windows.Forms.Label _NoMeasurementSelectedLabel;
	}
}

