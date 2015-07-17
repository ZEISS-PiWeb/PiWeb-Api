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

		private Configuration _Configuration;
		private CatalogCollection _Catalogs;
		
		#endregion

		#region constructor

		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region properties

		public delegate void LogHandler( string message, params object[] args );

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
				var serviceInformatrion = await _RestDataServiceClient.GetServiceInformation();
				sw.Stop();

				LogMessage( "Successfully fetched service information from data service in {0} ms: '{1}'.\r\n", sw.ElapsedMilliseconds, serviceInformatrion );
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
				var rdServiceInformation = await _RestRawDataServiceClient.GetServiceInformation();
				sw.Stop();

				LogMessage( "Successfully fetched service information from data service in {0} ms: '{1}'.\r\n", sw.ElapsedMilliseconds, rdServiceInformation );
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

			_FetchDataControl.InitializeControl( _ServerUri, _Configuration, _Catalogs );
			_ModifyDataControl.InitializeControl( _ServerUri, _Configuration, _Catalogs );
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
