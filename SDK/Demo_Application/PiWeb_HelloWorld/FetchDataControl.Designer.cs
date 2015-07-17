namespace PiWeb_HelloWorld
{
	partial class FetchDataControl
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ColumnHeader columnHeader10;
			System.Windows.Forms.ColumnHeader columnHeader11;
			System.Windows.Forms.ColumnHeader columnHeader13;
			System.Windows.Forms.ColumnHeader columnHeader12;
			System.Windows.Forms.ColumnHeader columnHeader5;
			System.Windows.Forms.ColumnHeader columnHeader7;
			System.Windows.Forms.ColumnHeader columnHeader9;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.ColumnHeader _CharPathColHeader;
			System.Windows.Forms.ColumnHeader _ValueColHeader;
			System.Windows.Forms.ColumnHeader columnHeader6;
			System.Windows.Forms.ColumnHeader columnHeader1;
			System.Windows.Forms.ColumnHeader columnHeader8;
			System.Windows.Forms.ColumnHeader columnHeader2;
			System.Windows.Forms.ColumnHeader columnHeader3;
			System.Windows.Forms.ColumnHeader columnHeader4;
			this._RawDataListView = new System.Windows.Forms.ListView();
			this._AdditionalDataRadioButton = new System.Windows.Forms.RadioButton();
			this._MeasurementValuesRadioButton = new System.Windows.Forms.RadioButton();
			this._MeasurementsRadioButton = new System.Windows.Forms.RadioButton();
			this._CharacteristicsRadioButton = new System.Windows.Forms.RadioButton();
			this._PartsListView = new System.Windows.Forms.ListView();
			this._MeasurementValuesListView = new System.Windows.Forms.ListView();
			this._MeasurementsListView = new System.Windows.Forms.ListView();
			this._CharacteristicsListView = new System.Windows.Forms.ListView();
			this._NoMeasurementSelectedLabel = new System.Windows.Forms.Label();
			columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			label2 = new System.Windows.Forms.Label();
			_CharPathColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			_ValueColHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// columnHeader10
			// 
			columnHeader10.Text = "Filename";
			columnHeader10.Width = 251;
			// 
			// columnHeader11
			// 
			columnHeader11.Text = "Mime Type";
			columnHeader11.Width = 175;
			// 
			// columnHeader13
			// 
			columnHeader13.Text = "Created";
			columnHeader13.Width = 192;
			// 
			// columnHeader12
			// 
			columnHeader12.Text = "Size (Bytes)";
			columnHeader12.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			columnHeader12.Width = 115;
			// 
			// columnHeader5
			// 
			columnHeader5.Text = "Path";
			columnHeader5.Width = 373;
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
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(6, 2);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(33, 15);
			label2.TabIndex = 7;
			label2.Text = "Parts";
			// 
			// _CharPathColHeader
			// 
			_CharPathColHeader.Text = "Characteristic";
			_CharPathColHeader.Width = 501;
			// 
			// _ValueColHeader
			// 
			_ValueColHeader.Text = "Value";
			_ValueColHeader.Width = 234;
			// 
			// columnHeader6
			// 
			columnHeader6.Text = "Date";
			columnHeader6.Width = 192;
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
			// columnHeader2
			// 
			columnHeader2.Text = "Path";
			columnHeader2.Width = 368;
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
			this._RawDataListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._RawDataListView.FullRowSelect = true;
			this._RawDataListView.Location = new System.Drawing.Point(9, 212);
			this._RawDataListView.Margin = new System.Windows.Forms.Padding(4);
			this._RawDataListView.Name = "_RawDataListView";
			this._RawDataListView.Size = new System.Drawing.Size(740, 372);
			this._RawDataListView.TabIndex = 5;
			this._RawDataListView.UseCompatibleStateImageBehavior = false;
			this._RawDataListView.View = System.Windows.Forms.View.Details;
			this._RawDataListView.Visible = false;
			// 
			// _AdditionalDataRadioButton
			// 
			this._AdditionalDataRadioButton.AutoSize = true;
			this._AdditionalDataRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._AdditionalDataRadioButton.Location = new System.Drawing.Point(462, 188);
			this._AdditionalDataRadioButton.Name = "_AdditionalDataRadioButton";
			this._AdditionalDataRadioButton.Size = new System.Drawing.Size(139, 19);
			this._AdditionalDataRadioButton.TabIndex = 12;
			this._AdditionalDataRadioButton.Text = "Additional Data (Part)";
			this._AdditionalDataRadioButton.UseVisualStyleBackColor = true;
			this._AdditionalDataRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _MeasurementValuesRadioButton
			// 
			this._MeasurementValuesRadioButton.AutoSize = true;
			this._MeasurementValuesRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._MeasurementValuesRadioButton.Location = new System.Drawing.Point(291, 188);
			this._MeasurementValuesRadioButton.Name = "_MeasurementValuesRadioButton";
			this._MeasurementValuesRadioButton.Size = new System.Drawing.Size(135, 19);
			this._MeasurementValuesRadioButton.TabIndex = 13;
			this._MeasurementValuesRadioButton.Text = "Measurement Values";
			this._MeasurementValuesRadioButton.UseVisualStyleBackColor = true;
			this._MeasurementValuesRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _MeasurementsRadioButton
			// 
			this._MeasurementsRadioButton.AutoSize = true;
			this._MeasurementsRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._MeasurementsRadioButton.Location = new System.Drawing.Point(150, 188);
			this._MeasurementsRadioButton.Name = "_MeasurementsRadioButton";
			this._MeasurementsRadioButton.Size = new System.Drawing.Size(103, 19);
			this._MeasurementsRadioButton.TabIndex = 14;
			this._MeasurementsRadioButton.Text = "Measurements";
			this._MeasurementsRadioButton.UseVisualStyleBackColor = true;
			this._MeasurementsRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _CharacteristicsRadioButton
			// 
			this._CharacteristicsRadioButton.AutoSize = true;
			this._CharacteristicsRadioButton.Checked = true;
			this._CharacteristicsRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._CharacteristicsRadioButton.Location = new System.Drawing.Point(9, 188);
			this._CharacteristicsRadioButton.Name = "_CharacteristicsRadioButton";
			this._CharacteristicsRadioButton.Size = new System.Drawing.Size(102, 19);
			this._CharacteristicsRadioButton.TabIndex = 15;
			this._CharacteristicsRadioButton.TabStop = true;
			this._CharacteristicsRadioButton.Text = "Characteristics";
			this._CharacteristicsRadioButton.UseVisualStyleBackColor = true;
			this._CharacteristicsRadioButton.Click += new System.EventHandler(this.RadioButton_Click);
			// 
			// _PartsListView
			// 
			this._PartsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._PartsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader5,
            columnHeader7,
            columnHeader9});
			this._PartsListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._PartsListView.FullRowSelect = true;
			this._PartsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._PartsListView.Location = new System.Drawing.Point(9, 19);
			this._PartsListView.Margin = new System.Windows.Forms.Padding(4);
			this._PartsListView.MultiSelect = false;
			this._PartsListView.Name = "_PartsListView";
			this._PartsListView.ShowItemToolTips = true;
			this._PartsListView.Size = new System.Drawing.Size(740, 144);
			this._PartsListView.TabIndex = 8;
			this._PartsListView.UseCompatibleStateImageBehavior = false;
			this._PartsListView.View = System.Windows.Forms.View.Details;
			this._PartsListView.SelectedIndexChanged += new System.EventHandler(this.PartsListView_SelectedIndexChanged);
			// 
			// _MeasurementValuesListView
			// 
			this._MeasurementValuesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._MeasurementValuesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            _CharPathColHeader,
            _ValueColHeader});
			this._MeasurementValuesListView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._MeasurementValuesListView.FullRowSelect = true;
			this._MeasurementValuesListView.Location = new System.Drawing.Point(9, 212);
			this._MeasurementValuesListView.Margin = new System.Windows.Forms.Padding(4);
			this._MeasurementValuesListView.MultiSelect = false;
			this._MeasurementValuesListView.Name = "_MeasurementValuesListView";
			this._MeasurementValuesListView.Size = new System.Drawing.Size(740, 372);
			this._MeasurementValuesListView.TabIndex = 9;
			this._MeasurementValuesListView.UseCompatibleStateImageBehavior = false;
			this._MeasurementValuesListView.View = System.Windows.Forms.View.Details;
			this._MeasurementValuesListView.Visible = false;
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
			this._MeasurementsListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._MeasurementsListView.FullRowSelect = true;
			this._MeasurementsListView.Location = new System.Drawing.Point(9, 212);
			this._MeasurementsListView.Margin = new System.Windows.Forms.Padding(4);
			this._MeasurementsListView.MultiSelect = false;
			this._MeasurementsListView.Name = "_MeasurementsListView";
			this._MeasurementsListView.ShowItemToolTips = true;
			this._MeasurementsListView.Size = new System.Drawing.Size(740, 372);
			this._MeasurementsListView.TabIndex = 10;
			this._MeasurementsListView.UseCompatibleStateImageBehavior = false;
			this._MeasurementsListView.View = System.Windows.Forms.View.Details;
			this._MeasurementsListView.Visible = false;
			this._MeasurementsListView.SelectedIndexChanged += new System.EventHandler(this.MeasurementsListView_SelectedIndexChanged);
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
			this._CharacteristicsListView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._CharacteristicsListView.FullRowSelect = true;
			this._CharacteristicsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this._CharacteristicsListView.Location = new System.Drawing.Point(9, 212);
			this._CharacteristicsListView.Margin = new System.Windows.Forms.Padding(4);
			this._CharacteristicsListView.MultiSelect = false;
			this._CharacteristicsListView.Name = "_CharacteristicsListView";
			this._CharacteristicsListView.ShowItemToolTips = true;
			this._CharacteristicsListView.Size = new System.Drawing.Size(740, 372);
			this._CharacteristicsListView.TabIndex = 11;
			this._CharacteristicsListView.UseCompatibleStateImageBehavior = false;
			this._CharacteristicsListView.View = System.Windows.Forms.View.Details;
			// 
			// _NoMeasurementSelectedLabel
			// 
			this._NoMeasurementSelectedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._NoMeasurementSelectedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._NoMeasurementSelectedLabel.ForeColor = System.Drawing.SystemColors.GrayText;
			this._NoMeasurementSelectedLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._NoMeasurementSelectedLabel.Location = new System.Drawing.Point(205, 366);
			this._NoMeasurementSelectedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this._NoMeasurementSelectedLabel.Name = "_NoMeasurementSelectedLabel";
			this._NoMeasurementSelectedLabel.Size = new System.Drawing.Size(347, 40);
			this._NoMeasurementSelectedLabel.TabIndex = 6;
			this._NoMeasurementSelectedLabel.Text = "Please select a measurement to view the measurement values.";
			this._NoMeasurementSelectedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this._NoMeasurementSelectedLabel.Visible = false;
			// 
			// FetchDataControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this._MeasurementsRadioButton);
			this.Controls.Add(this._MeasurementValuesRadioButton);
			this.Controls.Add(this._AdditionalDataRadioButton);
			this.Controls.Add(this._CharacteristicsRadioButton);
			this.Controls.Add(this._PartsListView);
			this.Controls.Add(this._NoMeasurementSelectedLabel);
			this.Controls.Add(label2);
			this.Controls.Add(this._MeasurementsListView);
			this.Controls.Add(this._RawDataListView);
			this.Controls.Add(this._MeasurementValuesListView);
			this.Controls.Add(this._CharacteristicsListView);
			this.Name = "FetchDataControl";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.Size = new System.Drawing.Size(755, 590);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView _RawDataListView;
		private System.Windows.Forms.Label _NoMeasurementSelectedLabel;
		private System.Windows.Forms.RadioButton _AdditionalDataRadioButton;
		private System.Windows.Forms.RadioButton _MeasurementValuesRadioButton;
		private System.Windows.Forms.RadioButton _MeasurementsRadioButton;
		private System.Windows.Forms.RadioButton _CharacteristicsRadioButton;
		private System.Windows.Forms.ListView _PartsListView;
		private System.Windows.Forms.ListView _MeasurementValuesListView;
		private System.Windows.Forms.ListView _MeasurementsListView;
		private System.Windows.Forms.ListView _CharacteristicsListView;
	}
}
