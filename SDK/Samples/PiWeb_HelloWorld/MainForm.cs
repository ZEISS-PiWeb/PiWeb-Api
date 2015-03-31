#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace PiWeb_HelloWorld
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	
	using Common.Data;

	using DataService;
	using RawDataService;

	#endregion

	/// <summary>
	/// This sample application demonstrates a few basic tasks for querying data (parts, characteristics, measurements and values ) 
	/// from the data and raw data service.
	/// </summary>
	public partial class MainForm : Form
	{
		#region members

		private Uri _ServerUri;
		private DataServiceRestClient _RestDataServiceClient;
		private RawDataServiceRestClient _RestRawDataServiceClient;

		private DataService.ServiceInformation _DataService_ServiceInformation;
		private RawDataService.ServiceInformation _RawDataService_ServiceInformation;

		private Configuration _Configuration;
		private CatalogCollection _Catalogs;
		
		private InspectionPlanPart[] _Parts;
		private Dictionary<Guid,InspectionPlanCharacteristic> _Characteristics;
		private SimpleMeasurement[] _Measurements;
		private DataCharacteristic[] _Values;
		
		private RawDataInformation[] _RawDataInformation;

		#endregion

		#region constructor

		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region methods

		/// <summary>
		/// This methods fetches the service information from both the data service and the raw data service.
		/// The service information contains general information about the services like its version and the 
		/// servers feature set. Fetching the service information can also be used for connection check purposes
		/// since it's guaranteed that the check is fast and does not cause any noticeable server load.
		/// </summary>
		private async Task CheckConnection()
		{
			// Data Service
			try
			{
				LogMessage( "Fetching service information from data service." );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_DataService_ServiceInformation = await _RestDataServiceClient.GetServiceInformation();
				sw.Stop();

				LogMessage( "Successfully fetched service information from data service in {0} ms: '{1}'.\r\n", sw.ElapsedMilliseconds, _DataService_ServiceInformation );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching service information from data service: '{0}'.\r\n", ex.Message );
			}

			// Raw Data Service
			try
			{
				LogMessage( "Fetching service information from raw data service." );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_RawDataService_ServiceInformation = await _RestRawDataServiceClient.GetServiceInformation();
				sw.Stop();

				LogMessage( "Successfully fetched service information from data service in {0} ms: '{1}'.\r\n", sw.ElapsedMilliseconds, _RawDataService_ServiceInformation );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching service information from raw data service: '{0}'.\r\n", ex.Message );
			}
		}

		/// <summary>
		/// This method fetches the configuration from the data service. The configuration contains definitions about all
		/// attributes that might be present for part, characteristics, measurements, values and catalog. Each attribute
		/// definition defines a data type for the attribute value.
		/// </summary>
		private async Task FetchConfiguration()
		{
			try
			{
				LogMessage( "Fetching configuration from data service." );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_Configuration = await _RestDataServiceClient.GetConfiguration();
				sw.Stop();

				LogMessage( "Successfully fetched configuration with {1} attribute definitions from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, _Configuration.AllAttributes.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching configuration from data service: '{0}'.\r\n", ex.Message );
			}
		}

		/// <summary>
		/// This method fetches the list of catalogs from the data service. Attributes can point to a catalog entry (as described by the
		/// configuration) using the index of the catalog entry. This is the list of all catalogs including their entries. Please keep in
		/// mind that it might be possible that an attribute value points to a non existing catalog entry.
		/// </summary>
		private async Task FetchCatalogs()
		{
			try
			{
				LogMessage( "Fetching catalogs from data service." );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				var catalogs = await _RestDataServiceClient.GetCatalogs();
				_Catalogs = new CatalogCollection( catalogs );
				sw.Stop();

				LogMessage( "Successfully fetched {1} catalogs from data service in {0} ms: '{2}'.\r\n", sw.ElapsedMilliseconds, _Catalogs.Count, string.Join( ", ", _Catalogs.Select( c => c.Name ) ) );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching catalogs from data service: '{0}'.\r\n", ex.Message );
			}
		}

		/// <summary>
		/// This method fetches all parts from the data service. Please note that this behavior is not good practice. This is for 
		/// demonstration purposes only: To be able to select any part and fetch measurements, values and additional data. The 
		/// preferred way of fetching parts is to fetch them step by step as the user navigates deeper into the inspection plan tree.
		/// </summary>
		private async Task FetchAllParts()
		{
			_PartsListView.Items.Clear();
			_Parts = new InspectionPlanPart[0];

			try
			{
				LogMessage( "Fetching all parts from data service." );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_Parts = await _RestDataServiceClient.GetParts( null, new InspectionPlanFilterAttributes { Depth = ushort.MaxValue } );
				sw.Stop();

				LogMessage( "Successfully fetched {1} parts from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, _Parts.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching all parts from data service: '{0}'.\r\n", ex.Message );
			}

			_PartsListView.BeginUpdate();
			foreach( var part in _Parts.OrderBy( p => p.Path.ToString() ) )
			{
				_PartsListView.Items.Add( CreateListViewItem( part ) );
			}
			_PartsListView.EndUpdate();
		}

		/// <summary>
		/// This method fetches all characteristics and child characteristics of the selected part. Characteristics from child parts 
		/// are not fetched though. This behavior - fetching all characteristics for one part with one call - is the best practice 
		/// and the most efficient way of fetching characteristics.
		/// </summary>
		private async Task FetchAllCharacteristicsForPart( PathInformation partPath )
		{
			_CharacteristicsListView.Items.Clear();

			try
			{
				LogMessage( "Fetching all characteristics for part '{0}' from data service.", partPath );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				var characteristics = await _RestDataServiceClient.GetCharacteristics( partPath );
				_Characteristics = characteristics.ToDictionary( c => c.Uuid );

				sw.Stop();

				LogMessage( "Successfully fetched {1} characteristics from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, _Characteristics.Count );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching all characteristics for part '{0}' from data service: '{1}'.\r\n", partPath, ex.Message );
			}

			_CharacteristicsListView.BeginUpdate();
			foreach( var part in _Characteristics.Values.OrderBy( p => p.Path.ToString() ) )
			{
				_CharacteristicsListView.Items.Add( CreateListViewItem( part ) );
			}
			_CharacteristicsListView.EndUpdate();
		}

		/// <summary>
		/// This method fetches the most recent 100 measurements for the selected part. Please have a look at the other properties inside 
		/// the filter class to understand all possibilities of filtering.
		/// </summary>
		private async Task FetchMeasurements( PathInformation partPath )
		{
			_MeasurementsListView.Items.Clear();

			try
			{
				LogMessage( "Fetching all last 100 measurements for part '{0}' from data service.", partPath );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_Measurements = await _RestDataServiceClient.GetMeasurements( partPath, new MeasurementFilterAttributes { LimitResult = 100 } );
				sw.Stop();

				LogMessage( "Successfully fetched {1} measurements from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, _Measurements.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching measurements for part '{0}' from data service: '{1}'.\r\n", partPath, ex.Message );
			}

			_MeasurementsListView.BeginUpdate();
			foreach( var meas in _Measurements.OrderByDescending( p => p.Time ) )
			{
				_MeasurementsListView.Items.Add( CreateListViewItem( meas ) );
			}
			_MeasurementsListView.EndUpdate();
		}

		/// <summary>
		/// This method fetches all measurement values of the selected measurement. The number of characteristics can be restricted using the
		/// filter class. However, if all characteristics are fetched, fetching the whole measurement without a characteristic restriction
		/// is best practice and the most efficient way.
		/// </summary>
		private async Task FetchMeasurementValues( Guid measurementUuid )
		{
			_MeasurementValuesListView.Items.Clear();

			try
			{
				LogMessage( "Fetching all measurement values for measurement with uuid '{0}' from data service.", measurementUuid );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				var values = await _RestDataServiceClient.GetMeasurementValues( filter: new MeasurementValueFilterAttributes { MeasurementUuids = new[] { measurementUuid } } );
				_Values = values != null ? values[ 0 ].Characteristics : new DataCharacteristic[0];
				sw.Stop();

				LogMessage( "Successfully fetched {1} measurement values from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, _Values.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching measurement values for measurement with uuid '{0}' from data service: '{1}'.\r\n", measurementUuid, ex.Message );
			}

			_MeasurementValuesListView.BeginUpdate();
			foreach( var value in _Values.OrderBy( p => GetCharacteristicName( p.Uuid ) ) )
			{
				_MeasurementValuesListView.Items.Add( CreateListViewItem( value ) );
			}
			_MeasurementValuesListView.EndUpdate();
		}

		/// <summary>
		/// This method fetches additional data for the selected part. For demonstration purposes only the additional data for parts are fetched.
		/// However, additional data can be stored for parts, characteristics, measurements and also for values.
		/// When presenting a list of raw data entries please consider to fetch a thumbnail picture for each entry using the method 
		/// <see cref="RawDataServiceRestClient.GetRawDataThumbnail"/>.
		/// </summary>
		private async Task FetchAdditionalDataListForPart( Guid partUuid )
		{
			_RawDataListView.Items.Clear();

			try
			{
				LogMessage( "Fetching additional data for part with uuid '{0}' from raw data service.", partUuid );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_RawDataInformation = await _RestRawDataServiceClient.ListRawDataForParts( new[] { partUuid } );
				sw.Stop();

				LogMessage( "Successfully fetched {1} additional data from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, _RawDataInformation.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching additional data for part with uuid '{0}' from data service: '{1}'.\r\n", partUuid, ex.Message );
			}

			_RawDataListView.BeginUpdate();
			foreach( var rawDataInfo in _RawDataInformation.OrderBy( p => p.FileName.ToString() ) )
			{
				_RawDataListView.Items.Add( CreateListViewItem( rawDataInfo ) );
			}
			_RawDataListView.EndUpdate();
		}

		#endregion

		#region helper methods

		private async void ConnectButtonClick( object sender, EventArgs e )
		{
			_ServerUri = new Uri( _ConnectionUrlTextBox.Text );

			_RestDataServiceClient = new DataServiceRestClient( _ServerUri );
			_RestRawDataServiceClient = new RawDataServiceRestClient( _ServerUri );

			await CheckConnection();
			await FetchConfiguration();
			await FetchCatalogs();
			await FetchAllParts();
		}

		private async void PartsListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			if( _PartsListView.SelectedItems.Count != 1 )
				return;

			var part = (InspectionPlanPart)_PartsListView.SelectedItems[ 0 ].Tag;

			await FetchAllCharacteristicsForPart( part.Path );
			await FetchMeasurements( part.Path );
			await FetchAdditionalDataListForPart( part.Uuid );
		}

		private async void MeasurementsListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			_NoMeasurementSelectedLabel.Visible = _MeasurementsListView.SelectedItems.Count != 1;

			if( _MeasurementsListView.SelectedItems.Count != 1 )
				return;

			var measurement = (SimpleMeasurement)_MeasurementsListView.SelectedItems[ 0 ].Tag;
			await FetchMeasurementValues( measurement.Uuid );
		}

		private void RadioButton_Click( object sender, EventArgs e )
		{
			_MeasurementsListView.Visible = false;
			_MeasurementValuesListView.Visible = false;
			_CharacteristicsListView.Visible = false;
			_RawDataListView.Visible = false;
			_NoMeasurementSelectedLabel.Visible = false;

			if( sender == _MeasurementsRadioButton )
				_MeasurementsListView.Visible = true;
			
			if( sender == _CharacteristicsRadioButton )
				_CharacteristicsListView.Visible = true;
			
			if( sender == _AdditionalDataRadioButton )
				_RawDataListView.Visible = true;

			if( sender == _MeasurementValuesRadioButton )
			{
				_NoMeasurementSelectedLabel.Visible = _MeasurementsListView.SelectedItems.Count != 1;
				_MeasurementValuesListView.Visible = true;
			}
		}

		private string GetCharacteristicName( Guid characteristicUuid )
		{
			InspectionPlanCharacteristic characteristic;
			if( _Characteristics.TryGetValue( characteristicUuid, out characteristic ) )
				return characteristic.Path.Name;
			
			return "";
		}

		private ListViewItem CreateListViewItem( InspectionPlanBase plan )
		{
			var subItems = new[]
			{
				plan.Path.IsRoot ? "/" : plan.Path.ToString(),
				plan.Timestamp.ToString( CultureInfo.CurrentCulture ),
				string.Format( "{0} attributes", plan.Attributes.Length )
			};

			return new ListViewItem( subItems )
			{
				Tag = plan,
				ToolTipText = CreateAttributeTooltip( plan )
			};
		}

		private ListViewItem CreateListViewItem( SimpleMeasurement measurement )
		{
			var subItems = new[]
			{
				measurement.Time.ToString( CultureInfo.CurrentCulture ),
				measurement.LastModified.ToString( CultureInfo.CurrentCulture ),
				string.Format( "{0} attributes", measurement.Attributes.Length )
			};

			return new ListViewItem( subItems )
			{
				Tag = measurement,
				ToolTipText = CreateAttributeTooltip( measurement )
			};
		}

		private ListViewItem CreateListViewItem( DataCharacteristic value )
		{
			var subItems = new[]
			{
				GetCharacteristicName( value.Uuid ),
				value.Value != null && value.Value.MeasuredValue.HasValue ? value.Value.MeasuredValue.Value.ToString( CultureInfo.CurrentCulture ) : "-"
			};

			return new ListViewItem( subItems )
			{
				Tag = value,
				ToolTipText = CreateAttributeTooltip( value.Value )
			};
		}

		private ListViewItem CreateListViewItem( RawDataInformation info )
		{
			var subItems = new[]
			{
				info.FileName,
				info.MimeType,
				info.Created.ToString( CultureInfo.CurrentCulture ),
				info.Size.ToString( "N0" )
			};

			return new ListViewItem( subItems ) { Tag = info };
		}

		private string CreateAttributeTooltip( IAttributeItem item )
		{
			if( item == null )
				return null;

			var tooltip = new StringBuilder();
			foreach( var att in item.Attributes.OrderBy( a => a.Key ) )
			{
				tooltip.AppendFormat( "K{0} [{1}]: {2}\r\n", att.Key, _Configuration.GetName( att.Key ), _Configuration.GetFormattedValue( att.Key, att.Value, _Catalogs ) );
			}
			return tooltip.ToString();
		}

		private void LogMessage( string message, params object[] args )
		{
			_LogMessagesTextBox.AppendText( string.Format( message, args ) );
			_LogMessagesTextBox.AppendText( Environment.NewLine );
		}

		private void ClearLogMessagesButton_Click( object sender, EventArgs e )
		{
			_LogMessagesTextBox.Clear();
		}

		#endregion
	}
}
