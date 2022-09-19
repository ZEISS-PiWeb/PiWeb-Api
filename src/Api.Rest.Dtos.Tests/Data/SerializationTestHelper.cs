#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System.IO;

	#endregion

	public static class SerializationTestHelper
	{
		#region methods

		public static string ReadResourceString( string resourceName )
		{
			using var stream = typeof( InspectionPlanSerializationTests ).Assembly.GetManifestResourceStream( "Zeiss.PiWeb.Api.Rest.Dtos.Tests." + resourceName );
			using var reader = new StreamReader( stream );

			return reader.ReadToEnd();
		}

		#endregion
	}
}