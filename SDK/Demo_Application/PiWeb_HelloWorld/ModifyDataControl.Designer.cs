namespace PiWeb_HelloWorld
{
	partial class ModifyDataControl
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
			this.components = new System.ComponentModel.Container();
			this._PartTextBox = new System.Windows.Forms.TextBox();
			this._PartLabel = new System.Windows.Forms.Label();
			this._Char1TextBox = new System.Windows.Forms.TextBox();
			this.Characteristic1Label = new System.Windows.Forms.Label();
			this._Char2TextBox = new System.Windows.Forms.TextBox();
			this.Characteristic2Label = new System.Windows.Forms.Label();
			this._Char3TextBox = new System.Windows.Forms.TextBox();
			this.Characteristic3Label = new System.Windows.Forms.Label();
			this._Value1TextBox = new System.Windows.Forms.TextBox();
			this._Value2TextBox = new System.Windows.Forms.TextBox();
			this._Value3TextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this._DeletePartButton = new System.Windows.Forms.Button();
			this._CreateInspPlanButton = new System.Windows.Forms.Button();
			this._CreateMeasurementsButton = new System.Windows.Forms.Button();
			this._DeleteChar1Button = new System.Windows.Forms.Button();
			this._DeleteChar3Button = new System.Windows.Forms.Button();
			this._DeleteChar2Button = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._RawDataListView = new System.Windows.Forms.ListView();
			this._RawDataThumbnailImageList = new System.Windows.Forms.ImageList(this.components);
			this._DelSelectedAdditionalDataButton = new System.Windows.Forms.Button();
			this._DelAllAdditionalDataButton = new System.Windows.Forms.Button();
			this._AddAdditionalDataButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// _PartTextBox
			// 
			this._PartTextBox.Location = new System.Drawing.Point(101, 23);
			this._PartTextBox.Name = "_PartTextBox";
			this._PartTextBox.Size = new System.Drawing.Size(100, 23);
			this._PartTextBox.TabIndex = 0;
			this._PartTextBox.Text = "DemoPart1";
			// 
			// _PartLabel
			// 
			this._PartLabel.AutoSize = true;
			this._PartLabel.Location = new System.Drawing.Point(69, 27);
			this._PartLabel.Name = "_PartLabel";
			this._PartLabel.Size = new System.Drawing.Size(28, 15);
			this._PartLabel.TabIndex = 2;
			this._PartLabel.Text = "Part";
			// 
			// _Char1TextBox
			// 
			this._Char1TextBox.Location = new System.Drawing.Point(160, 49);
			this._Char1TextBox.Name = "_Char1TextBox";
			this._Char1TextBox.Size = new System.Drawing.Size(109, 23);
			this._Char1TextBox.TabIndex = 1;
			this._Char1TextBox.Text = "Char1";
			// 
			// Characteristic1Label
			// 
			this.Characteristic1Label.AutoSize = true;
			this.Characteristic1Label.Location = new System.Drawing.Point(69, 52);
			this.Characteristic1Label.Name = "Characteristic1Label";
			this.Characteristic1Label.Size = new System.Drawing.Size(85, 15);
			this.Characteristic1Label.TabIndex = 2;
			this.Characteristic1Label.Text = "Characteristic1";
			// 
			// _Char2TextBox
			// 
			this._Char2TextBox.Location = new System.Drawing.Point(160, 75);
			this._Char2TextBox.Name = "_Char2TextBox";
			this._Char2TextBox.Size = new System.Drawing.Size(109, 23);
			this._Char2TextBox.TabIndex = 2;
			this._Char2TextBox.Text = "Char2";
			// 
			// Characteristic2Label
			// 
			this.Characteristic2Label.AutoSize = true;
			this.Characteristic2Label.Location = new System.Drawing.Point(69, 78);
			this.Characteristic2Label.Name = "Characteristic2Label";
			this.Characteristic2Label.Size = new System.Drawing.Size(85, 15);
			this.Characteristic2Label.TabIndex = 2;
			this.Characteristic2Label.Text = "Characteristic2";
			// 
			// _Char3TextBox
			// 
			this._Char3TextBox.Location = new System.Drawing.Point(160, 101);
			this._Char3TextBox.Name = "_Char3TextBox";
			this._Char3TextBox.Size = new System.Drawing.Size(109, 23);
			this._Char3TextBox.TabIndex = 3;
			this._Char3TextBox.Text = "Char3";
			// 
			// Characteristic3Label
			// 
			this.Characteristic3Label.AutoSize = true;
			this.Characteristic3Label.Location = new System.Drawing.Point(69, 104);
			this.Characteristic3Label.Name = "Characteristic3Label";
			this.Characteristic3Label.Size = new System.Drawing.Size(85, 15);
			this.Characteristic3Label.TabIndex = 2;
			this.Characteristic3Label.Text = "Characteristic3";
			// 
			// _Value1TextBox
			// 
			this._Value1TextBox.Location = new System.Drawing.Point(434, 50);
			this._Value1TextBox.Name = "_Value1TextBox";
			this._Value1TextBox.Size = new System.Drawing.Size(106, 23);
			this._Value1TextBox.TabIndex = 5;
			this._Value1TextBox.Text = "0,25";
			this._Value1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// _Value2TextBox
			// 
			this._Value2TextBox.Location = new System.Drawing.Point(434, 76);
			this._Value2TextBox.Name = "_Value2TextBox";
			this._Value2TextBox.Size = new System.Drawing.Size(106, 23);
			this._Value2TextBox.TabIndex = 6;
			this._Value2TextBox.Text = "0,65";
			this._Value2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// _Value3TextBox
			// 
			this._Value3TextBox.Location = new System.Drawing.Point(434, 102);
			this._Value3TextBox.Name = "_Value3TextBox";
			this._Value3TextBox.Size = new System.Drawing.Size(106, 23);
			this._Value3TextBox.TabIndex = 7;
			this._Value3TextBox.Text = "1,45";
			this._Value3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(307, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(121, 15);
			this.label3.TabIndex = 9;
			this.label3.Text = "Measurement Value 3";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(307, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 15);
			this.label1.TabIndex = 9;
			this.label1.Text = "Measurement Value 2";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(307, 105);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(121, 15);
			this.label2.TabIndex = 9;
			this.label2.Text = "Measurement Value 1";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(25, 28);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(595, 225);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Inspection Plan and Measurements";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this._DeletePartButton);
			this.panel1.Controls.Add(this._Char3TextBox);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.Characteristic2Label);
			this.panel1.Controls.Add(this._CreateInspPlanButton);
			this.panel1.Controls.Add(this._Value3TextBox);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this._Value2TextBox);
			this.panel1.Controls.Add(this._CreateMeasurementsButton);
			this.panel1.Controls.Add(this.Characteristic3Label);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this._Char2TextBox);
			this.panel1.Controls.Add(this._PartTextBox);
			this.panel1.Controls.Add(this._DeleteChar1Button);
			this.panel1.Controls.Add(this._DeleteChar3Button);
			this.panel1.Controls.Add(this.Characteristic1Label);
			this.panel1.Controls.Add(this._PartLabel);
			this.panel1.Controls.Add(this._Value1TextBox);
			this.panel1.Controls.Add(this._DeleteChar2Button);
			this.panel1.Controls.Add(this._Char1TextBox);
			this.panel1.Location = new System.Drawing.Point(3, 19);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(589, 203);
			this.panel1.TabIndex = 0;
			// 
			// _DeletePartButton
			// 
			this._DeletePartButton.Image = global::PiWeb_HelloWorld.Properties.Resources.delete;
			this._DeletePartButton.Location = new System.Drawing.Point(39, 22);
			this._DeletePartButton.Name = "_DeletePartButton";
			this._DeletePartButton.Size = new System.Drawing.Size(24, 24);
			this._DeletePartButton.TabIndex = 9;
			this._DeletePartButton.UseVisualStyleBackColor = true;
			this._DeletePartButton.Click += new System.EventHandler(this.DeletePartButton_Click);
			// 
			// _CreateInspPlanButton
			// 
			this._CreateInspPlanButton.Image = global::PiWeb_HelloWorld.Properties.Resources.add_button_blue;
			this._CreateInspPlanButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._CreateInspPlanButton.Location = new System.Drawing.Point(39, 150);
			this._CreateInspPlanButton.Name = "_CreateInspPlanButton";
			this._CreateInspPlanButton.Size = new System.Drawing.Size(230, 30);
			this._CreateInspPlanButton.TabIndex = 4;
			this._CreateInspPlanButton.Text = "Create InspectionPlan Items";
			this._CreateInspPlanButton.UseVisualStyleBackColor = true;
			this._CreateInspPlanButton.Click += new System.EventHandler(this.AddPartButtonClick);
			// 
			// _CreateMeasurementsButton
			// 
			this._CreateMeasurementsButton.Image = global::PiWeb_HelloWorld.Properties.Resources.add_button_blue;
			this._CreateMeasurementsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._CreateMeasurementsButton.Location = new System.Drawing.Point(310, 150);
			this._CreateMeasurementsButton.Name = "_CreateMeasurementsButton";
			this._CreateMeasurementsButton.Size = new System.Drawing.Size(230, 30);
			this._CreateMeasurementsButton.TabIndex = 8;
			this._CreateMeasurementsButton.Text = "Create Measurement and Values";
			this._CreateMeasurementsButton.UseVisualStyleBackColor = true;
			this._CreateMeasurementsButton.Click += new System.EventHandler(this.CreateMeasurementsButton_Click);
			// 
			// _DeleteChar1Button
			// 
			this._DeleteChar1Button.Image = global::PiWeb_HelloWorld.Properties.Resources.delete;
			this._DeleteChar1Button.Location = new System.Drawing.Point(39, 48);
			this._DeleteChar1Button.Name = "_DeleteChar1Button";
			this._DeleteChar1Button.Size = new System.Drawing.Size(24, 24);
			this._DeleteChar1Button.TabIndex = 10;
			this._DeleteChar1Button.UseVisualStyleBackColor = true;
			this._DeleteChar1Button.Click += new System.EventHandler(this.DeleteChar1Button_Click);
			// 
			// _DeleteChar3Button
			// 
			this._DeleteChar3Button.Image = global::PiWeb_HelloWorld.Properties.Resources.delete;
			this._DeleteChar3Button.Location = new System.Drawing.Point(39, 100);
			this._DeleteChar3Button.Name = "_DeleteChar3Button";
			this._DeleteChar3Button.Size = new System.Drawing.Size(24, 24);
			this._DeleteChar3Button.TabIndex = 12;
			this._DeleteChar3Button.UseVisualStyleBackColor = true;
			// 
			// _DeleteChar2Button
			// 
			this._DeleteChar2Button.Image = global::PiWeb_HelloWorld.Properties.Resources.delete;
			this._DeleteChar2Button.Location = new System.Drawing.Point(39, 74);
			this._DeleteChar2Button.Name = "_DeleteChar2Button";
			this._DeleteChar2Button.Size = new System.Drawing.Size(24, 24);
			this._DeleteChar2Button.TabIndex = 11;
			this._DeleteChar2Button.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this._RawDataListView);
			this.groupBox2.Controls.Add(this._DelSelectedAdditionalDataButton);
			this.groupBox2.Controls.Add(this._DelAllAdditionalDataButton);
			this.groupBox2.Controls.Add(this._AddAdditionalDataButton);
			this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(25, 292);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(595, 163);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Additional Data";
			// 
			// _RawDataListView
			// 
			this._RawDataListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._RawDataListView.LargeImageList = this._RawDataThumbnailImageList;
			this._RawDataListView.Location = new System.Drawing.Point(196, 22);
			this._RawDataListView.Name = "_RawDataListView";
			this._RawDataListView.Size = new System.Drawing.Size(393, 133);
			this._RawDataListView.SmallImageList = this._RawDataThumbnailImageList;
			this._RawDataListView.TabIndex = 4;
			this._RawDataListView.TileSize = new System.Drawing.Size(184, 60);
			this._RawDataListView.UseCompatibleStateImageBehavior = false;
			// 
			// _RawDataThumbnailImageList
			// 
			this._RawDataThumbnailImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this._RawDataThumbnailImageList.ImageSize = new System.Drawing.Size(32, 32);
			this._RawDataThumbnailImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _DelSelectedAdditionalDataButton
			// 
			this._DelSelectedAdditionalDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._DelSelectedAdditionalDataButton.Image = global::PiWeb_HelloWorld.Properties.Resources.delete;
			this._DelSelectedAdditionalDataButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._DelSelectedAdditionalDataButton.Location = new System.Drawing.Point(45, 85);
			this._DelSelectedAdditionalDataButton.Name = "_DelSelectedAdditionalDataButton";
			this._DelSelectedAdditionalDataButton.Size = new System.Drawing.Size(145, 30);
			this._DelSelectedAdditionalDataButton.TabIndex = 14;
			this._DelSelectedAdditionalDataButton.Text = "Delete selected";
			this._DelSelectedAdditionalDataButton.UseVisualStyleBackColor = true;
			this._DelSelectedAdditionalDataButton.Click += new System.EventHandler(this.DeleteAdditionalDataButton_Click);
			// 
			// _DelAllAdditionalDataButton
			// 
			this._DelAllAdditionalDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._DelAllAdditionalDataButton.Image = global::PiWeb_HelloWorld.Properties.Resources.delete;
			this._DelAllAdditionalDataButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._DelAllAdditionalDataButton.Location = new System.Drawing.Point(45, 125);
			this._DelAllAdditionalDataButton.Name = "_DelAllAdditionalDataButton";
			this._DelAllAdditionalDataButton.Size = new System.Drawing.Size(145, 30);
			this._DelAllAdditionalDataButton.TabIndex = 15;
			this._DelAllAdditionalDataButton.Text = "Delete all";
			this._DelAllAdditionalDataButton.UseVisualStyleBackColor = true;
			this._DelAllAdditionalDataButton.Click += new System.EventHandler(this.DeleteAdditionalDataButton_Click);
			// 
			// _AddAdditionalDataButton
			// 
			this._AddAdditionalDataButton.Image = global::PiWeb_HelloWorld.Properties.Resources.add_button_blue;
			this._AddAdditionalDataButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._AddAdditionalDataButton.Location = new System.Drawing.Point(45, 22);
			this._AddAdditionalDataButton.Name = "_AddAdditionalDataButton";
			this._AddAdditionalDataButton.Size = new System.Drawing.Size(145, 30);
			this._AddAdditionalDataButton.TabIndex = 13;
			this._AddAdditionalDataButton.Text = "Add";
			this._AddAdditionalDataButton.UseVisualStyleBackColor = true;
			this._AddAdditionalDataButton.Click += new System.EventHandler(this.AddAdditionalDataButton_Click);
			// 
			// ModifyDataControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "ModifyDataControl";
			this.Size = new System.Drawing.Size(644, 500);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _CreateInspPlanButton;
		private System.Windows.Forms.TextBox _PartTextBox;
		private System.Windows.Forms.Label _PartLabel;
		private System.Windows.Forms.TextBox _Char1TextBox;
		private System.Windows.Forms.Label Characteristic1Label;
		private System.Windows.Forms.TextBox _Char2TextBox;
		private System.Windows.Forms.Label Characteristic2Label;
		private System.Windows.Forms.TextBox _Char3TextBox;
		private System.Windows.Forms.Label Characteristic3Label;
		private System.Windows.Forms.Button _DeleteChar1Button;
		private System.Windows.Forms.Button _DeleteChar2Button;
		private System.Windows.Forms.Button _DeleteChar3Button;
		private System.Windows.Forms.Button _DeletePartButton;
		private System.Windows.Forms.TextBox _Value1TextBox;
		private System.Windows.Forms.TextBox _Value2TextBox;
		private System.Windows.Forms.TextBox _Value3TextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button _CreateMeasurementsButton;
		private System.Windows.Forms.Button _AddAdditionalDataButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListView _RawDataListView;
		private System.Windows.Forms.Button _DelAllAdditionalDataButton;
		private System.Windows.Forms.Button _DelSelectedAdditionalDataButton;
		private System.Windows.Forms.ImageList _RawDataThumbnailImageList;
		private System.Windows.Forms.Panel panel1;
	}
}
