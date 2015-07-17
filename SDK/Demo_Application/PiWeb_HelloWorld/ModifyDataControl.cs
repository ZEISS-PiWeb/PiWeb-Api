#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace PiWeb_HelloWorld
{
	using System;
	using System.IO;
	using System.Xml;
	using Common.Data;
	using Properties;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;

	using DataService;
	using RawDataService;
	
	public partial class ModifyDataControl : UserControl
	{
		#region members

		private RawDataServiceRestClient _RestRawDataServiceClient;
		private DataServiceRestClient _RestDataServiceClient;
		private CatalogCollection _Catalogs;
		private Configuration _Configuration;

		private InspectionPlanPart _CurrentPart;
		private InspectionPlanCharacteristic[] _CurrentCharacteristics;

		#endregion

		#region constructors

		public ModifyDataControl()
		{
			InitializeComponent();
		}

		#endregion

		#region properties

		/// <summary>
		/// Event is raised when a message should be logged.
		/// </summary>
		public event MainForm.LogHandler LogMessage;

		#endregion
		
		#region methods

		public async void InitializeControl( Uri serverUri, Configuration configuration, CatalogCollection catalogs )
		{
			_Configuration = configuration;
			_Catalogs = catalogs;
			_RestDataServiceClient = new DataServiceRestClient( serverUri );
			_RestRawDataServiceClient = new RawDataServiceRestClient( serverUri );

			var partPath = PathHelper.String2PartPathInformation( _PartTextBox.Text );
			var parts = await _RestDataServiceClient.GetParts( partPath );
			var partsArray = parts as InspectionPlanPart[] ?? parts.ToArray();
			if( partsArray.Any() )
				_CurrentPart = partsArray.First();

			var chars = await _RestDataServiceClient.GetCharacteristics( partPath );
			var charsArray = chars as InspectionPlanCharacteristic[] ?? chars.ToArray();
			if( charsArray.Any() )
				_CurrentCharacteristics = charsArray;

			FillRawDataListView();
		}

		/// <summary>
		/// This methods creates a new part as well as three characteristics below.
		/// </summary>
		private async void AddPartButtonClick( object sender, EventArgs e )
		{
			LogMessage( "Creating inspection plan items" );

			var sw = System.Diagnostics.Stopwatch.StartNew();
			
			var partPath = PathHelper.String2PartPathInformation( _PartTextBox.Text );
			var char1Path = GetCharPath( _Char1TextBox );
			var char2Path = GetCharPath( _Char2TextBox );
			var char3Path = GetCharPath( _Char3TextBox );
			try
			{
				var part = new InspectionPlanPart { Path = partPath, Uuid = Guid.NewGuid() };
				var char1 = new InspectionPlanCharacteristic { Path = char1Path, Uuid = Guid.NewGuid() };
				var char2 = new InspectionPlanCharacteristic { Path = char2Path, Uuid = Guid.NewGuid() };
				var char3 = new InspectionPlanCharacteristic { Path = char3Path, Uuid = Guid.NewGuid() };
				var characteristics = new[] { char1, char2, char3 };
				
				await _RestDataServiceClient.CreateParts( new[] { part } );
				await _RestDataServiceClient.CreateCharacteristics( characteristics );
				
				_CurrentPart = part;
				_CurrentCharacteristics = characteristics;

				_RawDataListView.Items.Clear();

				sw.Stop();
				LogMessage( "Succesfully created inspection plan items in {0} ms\r\n", sw.ElapsedMilliseconds );
			}
			catch( Exception ex )
			{
				LogMessage( String.Format( "Error creating part '{0}': '{1}'.\r\n", partPath, ex.Message ) );
				_CurrentPart = null;
				_CurrentCharacteristics = null;
			}
		}

		/// <summary>
		/// This methods deletes the current part. Please note that deleting a part means the part itself as well 
		/// as all its child parts and child characteristics will be deleted. Leaving the part path or uuid blank 
		/// results in deleting everything below the root part.
		/// </summary>
		private async void DeletePartButton_Click( object sender, EventArgs e )
		{
			var result = DialogResult.OK;
			if( String.IsNullOrEmpty( _PartTextBox.Text ) )
				result = MessageBox.Show( "Leaving the part blank means 'Delete everything below the root part'\r\n Proceed?", "Proceed", MessageBoxButtons.OKCancel );

			if( result == DialogResult.OK )
			{
				var partPath = PathHelper.String2PartPathInformation( _PartTextBox.Text );
				try
				{
					LogMessage( "Deleting the part {0} and all characteristics beneath.", partPath );

					var sw = System.Diagnostics.Stopwatch.StartNew();

					await _RestDataServiceClient.DeleteParts( partPath );
					_RawDataListView.Items.Clear();

					sw.Stop();
					LogMessage( "Successfully deleted the part {0} and all characteristics beneath in {1} ms.\r\n", partPath, sw.ElapsedMilliseconds );
				}
				catch( Exception ex )
				{
					LogMessage( String.Format( "Error deleting part '{0}': '{1}'.\r\n", partPath, ex.Message ) );
				}
			}
		}

		/// <summary>
		/// This method deletes a certain characteristic. Please note that there are two possibilities: 
		/// Delete a characteristic by its uuid or by its path.
		/// </summary>
		private async void DeleteChar1Button_Click( object sender, EventArgs e )
		{
			PathInformation charPath = null;
			Guid? charUuid = null;
			if( sender == _DeleteChar1Button )
				charPath = GetCharPath( _Char1TextBox );
			if( sender == _DeleteChar2Button )
				charUuid = _CurrentCharacteristics[ 1 ].Uuid;
			if( sender == _DeleteChar3Button )
				charPath = GetCharPath( _Char3TextBox );
			
			if(charPath != null)
				await _RestDataServiceClient.DeleteCharacteristics( charPath );
			else if( charUuid.HasValue )
				await _RestDataServiceClient.DeleteCharacteristics( new[] { ( Guid )charUuid } );
		}
		
		/// <summary>
		/// This method creates a new measurement including two attributes: inspector and time. The inspector is set to a random inspector taken 
		/// from the particular catalog, time is set to the current time. Please note that datetime values need to be 
		/// Xml converted in order to be handled correctly. Measurement's time can also be set via the <see cref="DataMeasurement.Time"/> property. 
		/// As it internally sets the particular attribute be patient: Setting the <see cref="DataMeasurement.Time"/> property before setting the 
		/// <see cref="SimpleMeasurement.Attributes"/> property will cause the <see cref="DataMeasurement.Time"/> property to be overwritten/deleted.
		/// </summary>
		private async void CreateMeasurementsButton_Click( object sender, EventArgs e )
		{
			double val1, val2, val3;
			
			if( !Double.TryParse( _Value1TextBox.Text, out val1 ) || !Double.TryParse( _Value2TextBox.Text, out val2 ) ||
				!Double.TryParse( _Value3TextBox.Text, out val3 ) || _CurrentPart == null|| _CurrentCharacteristics == null || _CurrentCharacteristics.Length != 3 )
			{
				LogMessage( "Creating measurement failed due to badly formatted values!\r\n" );
				return;
			}

			try
			{
				LogMessage( "Creating a measurement and three values" );

				var sw = System.Diagnostics.Stopwatch.StartNew();

				var attributes = new List<DataService.Attribute>();
				attributes.Add( new DataService.Attribute( WellKnownKeys.Measurement.Time, XmlConvert.ToString( DateTime.Now, XmlDateTimeSerializationMode.RoundtripKind ) ) );
				
				var inspectorDef = _Configuration.GetDefinition( Entity.Measurement, WellKnownKeys.Measurement.InspectorName ) as CatalogAttributeDefinition;
				var rdm = new Random();
				if( inspectorDef != null )
				{
					var catalogEntry = _Catalogs[ inspectorDef.Catalog, rdm.Next( 0, _Catalogs[ inspectorDef.Catalog ].CatalogEntries.Length ).ToString() ];
					attributes.Add( new DataService.Attribute( WellKnownKeys.Measurement.InspectorName, catalogEntry.Key ) );
				}

				var measurement = new DataMeasurement
				{
					Uuid = Guid.NewGuid(),
					Attributes = attributes.ToArray(),
					Characteristics = new[]
					{
						new DataCharacteristic
						{
							Uuid = _CurrentCharacteristics[0].Uuid, 
							Timestamp = DateTime.Now, 
							Value = new DataValue( val1 )
						},
						new DataCharacteristic
						{
							Uuid = _CurrentCharacteristics[1].Uuid, 
							Timestamp = DateTime.Now, 
							Value = new DataValue( val2 )
						},
						new DataCharacteristic
						{
							Uuid = _CurrentCharacteristics[2].Uuid, 
							Timestamp = DateTime.Now, 
							Value = new DataValue( val3 )
						}
					},
					PartUuid = _CurrentPart.Uuid
				};
				await _RestDataServiceClient.CreateMeasurementValues( new[] { measurement } );

				sw.Stop();
				LogMessage( "Successfully create a measurmeent and three values in {0} ms.\r\n", sw.ElapsedMilliseconds );
			}
			catch( Exception ex )
			{
				LogMessage( String.Format( "Error creating measurement: '{0}'.\r\n", ex.Message ) );
			}
		}

		/// <summary>
		/// Addes the selected file as additoinal data to the certain part
		/// <remarks>Setting the <see cref="RawDataInformation.Key"/> to -1 leads the server to auto select the key.</remarks>
		/// </summary>
		private async void AddAdditionalDataButton_Click( object sender, EventArgs e )
		{
			using( var dlg = new OpenFileDialog() )
			{
				DialogResult result = dlg.ShowDialog();
				if( result == DialogResult.OK )
				{
					try
					{
						LogMessage( "Uploading addtional data: {0}", dlg.SafeFileName );

						var sw = System.Diagnostics.Stopwatch.StartNew();

						var data = File.ReadAllBytes( dlg.FileName );
						var md5 = System.Security.Cryptography.MD5.Create();
						await _RestRawDataServiceClient.CreateRawData( 
							new RawDataInformation
							{
								Key = -1,
								FileName = dlg.SafeFileName, 
								Target = RawDataTargetEntity.CreateForPart( _CurrentPart.Uuid ),
								MD5 = new Guid( md5.ComputeHash( data ) )
							}, 
							data );
						FillRawDataListView();

						sw.Stop();
						LogMessage( "Successfully uploaded additional data: {0} in {1} ms\r\n", dlg.SafeFileName, sw.ElapsedMilliseconds );
					}
					catch( Exception ex )
					{
						LogMessage( String.Format( "Error uploading additional data: '{0}'.\r\n", ex.Message ) );
					}
				}
			}
		}

		/// <summary>
		/// This method deletes either the selected additional data by their keys or all additional data
		/// </summary>
		private void DeleteAdditionalDataButton_Click( object sender, EventArgs e )
		{
			try
			{
				LogMessage( "Deleting addtional data" );

				var sw = System.Diagnostics.Stopwatch.StartNew();

				if( sender == _DelAllAdditionalDataButton )
					_RestRawDataServiceClient.DeleteAllRawDataForPart( _CurrentPart.Uuid );
				else if( sender == _DelSelectedAdditionalDataButton && _RawDataListView.SelectedItems.Count > 0 )
				{
					foreach( var item in _RawDataListView.SelectedItems )
					{
						var key = int.Parse( ( ( ListViewItem )item ).Tag.ToString() );
						_RestRawDataServiceClient.DeleteRawDataForPart( _CurrentPart.Uuid, key );
						_RawDataListView.Items.Remove( ( ListViewItem )item );
					}
				}

				sw.Stop();
				LogMessage( "Successfully deleted additional data in {0} ms\r\n", sw.ElapsedMilliseconds );
			}
			catch( Exception ex )
			{
				LogMessage( String.Format( "Error deleting additional data: '{0}'.\r\n", ex.Message ) );
			}
		}

		#endregion

		#region helper methods

		/// <summary>
		/// This method creates the full inspection plan path by concatenating the part's and the characteristic's path
		/// </summary>
		private PathInformation GetCharPath( TextBox charBox )
		{
			return PathHelper.String2PathInformation( 
				String.Concat( _PartTextBox.Text.Trim( '/' ), PathHelper.DelimiterString, charBox.Text.Trim( '/' ) ), "PC" );
		}

		/// <summary>
		/// This method fills the raw data list view. Please note the attempt to fetch the thumbnail. If not thumbnail exists
		/// a default thumbnail is shown. On fetching thumbnails as well as on fetching the rawdata itself client side caching is active. 
		/// </summary>
		private async void FillRawDataListView()
		{
			if( _CurrentPart != null )
			{
				_RawDataThumbnailImageList.Images.Clear();
				_RawDataThumbnailImageList.Images.Add( Resources.document );
				_RawDataListView.Items.Clear();
				var rawDataList = await _RestRawDataServiceClient.ListRawDataForParts( new[] { _CurrentPart.Uuid } );
				int i = 1;
				foreach( var rawDataInfo in rawDataList )
				{
					var subItems = new[] { rawDataInfo.FileName, rawDataInfo.Target.Uuid, rawDataInfo.Key.ToString() };
					var item = new ListViewItem( subItems ) { Tag = rawDataInfo.Key };
					
					var thumbnail = await _RestRawDataServiceClient.GetRawDataThumbnail( rawDataInfo );
					if( thumbnail != null )
					{
						var img = Image.FromStream( new MemoryStream( thumbnail ) );
						_RawDataThumbnailImageList.Images.Add( i.ToString(), img );
						item.ImageIndex = i;
						i++;
					}
					else
					{
						item.ImageIndex = 0;
					}
					_RawDataListView.Items.Add( item );
					_RawDataListView.Invalidate();
					_RawDataListView.Refresh();
					
				}
			}
		}

		#endregion
	}
}
