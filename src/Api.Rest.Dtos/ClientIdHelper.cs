#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos
{
	#region usings

	using System.IO;
	using System.Reflection;

	#endregion

	/// <summary>
	/// Hilfsklasse zur Ermittlung der ClientId die an die diversen Services gesendet wird.
	/// </summary>
	public static class ClientIdHelper
	{
		#region members

		/// <summary> Default-Wert für die Client-ID die den einzelnen Webservice-Methoden mitgegeben wird. </summary>
		public static readonly string ClientId = "PiWeb";

		/// <summary> Produkt-Name des Clients</summary>
		public static readonly string ClientProduct;

		/// <summary> Produkt-Version des Clients </summary>
		public static readonly string ClientVersion;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes the <see cref="ClientIdHelper"/> class.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Minor Code Smell", "S3963:\"static\" fields should be initialized inline", Justification = "<Pending>" )]
		static ClientIdHelper()
		{
			var entryAssembly = Assembly.GetEntryAssembly() ?? typeof( ClientIdHelper ).Assembly;
			try
			{
				var customAttributes = entryAssembly.GetCustomAttributes( typeof( AssemblyProductAttribute ), false );
				if( customAttributes.Length > 0 )
				{
					ClientId = ( (AssemblyProductAttribute)customAttributes[ 0 ] ).Product;
				}

				if( string.IsNullOrEmpty( ClientId ) )
				{
					ClientId = Path.GetFileNameWithoutExtension( entryAssembly.Location );
				}
			}
			catch
			{
				ClientId = "PiWeb";
			}

			ClientProduct = string.Concat( ClientId, "_", entryAssembly.GetName().Name ).Replace( " ", "_" );
			ClientVersion = entryAssembly.GetName().Version.ToString().Replace( " ", "_" );
			ClientId += " " + entryAssembly.GetName().Name + " " + entryAssembly.GetName().Version;
		}

		#endregion
	}
}