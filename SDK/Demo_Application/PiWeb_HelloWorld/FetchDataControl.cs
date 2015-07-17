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
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Globalization;

	using Common.Data;
	using DataService;
	using RawDataService;

	#endregion

	public partial class FetchDataControl : UserControl
	{
		#region members

		private DataServiceRestClient _RestDataServiceClient;
		private RawDataServiceRestClient _RestRawDataServiceClient;

		private Configuration _Configuration;
		private CatalogCollection _Catalogs;

		Dictionary<Guid, InspectionPlanCharacteristic> _Characteristics = new Dictionary<Guid, InspectionPlanCharacteristic>();

		#endregion

		#region constructors

		public FetchDataControl()
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

			await FetchAllParts();
		}

		/// <summary>
		/// This method fetches all parts from the data service. Please note that this behavior is not good practice. This is for 
		/// demonstration purposes only: To be able to select any part and fetch measurements, values and additional data. The 
		/// preferred way of fetching parts is to fetch them step by step as the user navigates deeper into the inspection plan tree.
		/// </summary>
		private async Task FetchAllParts()
		{
			_PartsListView.Items.Clear();
			var parts = new InspectionPlanPart[ 0 ];

			try
			{
				LogMessage( "Fetching all parts from data service." );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				parts = ( await _RestDataServiceClient.GetParts( null, null, ushort.MaxValue ) ).ToArray();
				sw.Stop();

				LogMessage( "Successfully fetched {1} parts from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, parts.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching all parts from data service: '{0}'.\r\n", ex.Message );
			}

			_PartsListView.BeginUpdate();
			foreach( var part in parts.OrderBy( p => p.Path.ToString() ) )
			{
				_PartsListView.Items.Add( await CreateListViewItem( part ) );
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
				_CharacteristicsListView.Items.Add( await CreateListViewItem( part ) );
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
			SimpleMeasurement[] _Measurements = new SimpleMeasurement[0];

			try
			{
				LogMessage( "Fetching all last 100 measurements for part '{0}' from data service.", partPath );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				_Measurements = ( await _RestDataServiceClient.GetMeasurements( partPath, new MeasurementFilterAttributes { LimitResult = 100 } ) ).ToArray();
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
			DataCharacteristic[] _Values = new DataCharacteristic[0];
			try
			{
				LogMessage( "Fetching all measurement values for measurement with uuid '{0}' from data service.", measurementUuid );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				var values = ( await _RestDataServiceClient.GetMeasurementValues( filter: new MeasurementValueFilterAttributes { MeasurementUuids = new[] { measurementUuid } } ) ).ToArray();
				_Values = values != null ? values[ 0 ].Characteristics : new DataCharacteristic[ 0 ];
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
		private async Task<RawDataInformation[]> FetchAdditionalDataListForPart( Guid partUuid )
		{
			_RawDataListView.Items.Clear();
			var rawDataInformation = new RawDataInformation[0];
			try
			{
				LogMessage( "Fetching additional data for part with uuid '{0}' from raw data service.", partUuid );

				var sw = System.Diagnostics.Stopwatch.StartNew();
				rawDataInformation = await _RestRawDataServiceClient.ListRawDataForParts( new[] { partUuid } );
				
				sw.Stop();

				LogMessage( "Successfully fetched {1} additional data from data service in {0} ms.\r\n", sw.ElapsedMilliseconds, rawDataInformation.Length );
			}
			catch( Exception ex )
			{
				LogMessage( "Error fetching additional data for part with uuid '{0}' from data service: '{1}'.\r\n", partUuid, ex.Message );
			}

			_RawDataListView.BeginUpdate();
			foreach( var rawDataInfo in rawDataInformation.OrderBy( p => p.FileName.ToString() ) )
			{
				_RawDataListView.Items.Add( CreateListViewItem( rawDataInfo ) );
			}
			_RawDataListView.EndUpdate();
			return rawDataInformation;
		}


		#endregion


		private async void PartsListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			if( _PartsListView.SelectedItems.Count != 1 )
				return;

			var part = ( InspectionPlanPart )_PartsListView.SelectedItems[ 0 ].Tag;

			await FetchAllCharacteristicsForPart( part.Path );
			await FetchMeasurements( part.Path );
			await FetchAdditionalDataListForPart( part.Uuid );
		}

		private async void MeasurementsListView_SelectedIndexChanged( object sender, EventArgs e )
		{
			_NoMeasurementSelectedLabel.Visible = _MeasurementsListView.SelectedItems.Count != 1;

			if( _MeasurementsListView.SelectedItems.Count != 1 )
			{
				_MeasurementValuesListView.Items.Clear();
				return;
			}

			var measurement = ( SimpleMeasurement )_MeasurementsListView.SelectedItems[ 0 ].Tag;
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
				if( _MeasurementsListView.SelectedItems.Count != 1 )
					_MeasurementValuesListView.Items.Clear();
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

		private async Task<ListViewItem> CreateListViewItem( InspectionPlanBase plan )
		{
			var subItems = new[]
			{
				plan.Path.IsRoot ? "/" : plan.Path.ToString(),
				plan.Timestamp.ToLocalTime().ToString( CultureInfo.CurrentCulture ),
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
				measurement.Time.ToLocalTime().ToString( CultureInfo.CurrentCulture ),
				measurement.LastModified.ToLocalTime().ToString( CultureInfo.CurrentCulture ),
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
				info.Created.ToLocalTime().ToString( CultureInfo.CurrentCulture ),
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
	}
}
