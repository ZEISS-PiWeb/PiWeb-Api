#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Contracts
{
	#region usings

	using System;
	using System.Text;

	#endregion

	public class OperationNotSupportedOnServerException : RestClientException
	{
		#region constructors

		public OperationNotSupportedOnServerException( string message, Version requiredVersion, Version currentVersion ) 
			: base( MakeMessage( message, requiredVersion, currentVersion ) )
		{
			RequiredVersion = requiredVersion;
			CurrentVersion = currentVersion;
		}

		private static string MakeMessage( string message, Version requiredVersion, Version currentVersion )
		{
			var builder = new StringBuilder();
			builder.Append( message ).AppendLine();
			builder.Append( "Required server interface: " ).Append( requiredVersion ).AppendLine();
			builder.Append( "Current server interface: " ).Append( currentVersion );

			return builder.ToString();
		}

		#endregion

		#region properties

		public Version RequiredVersion { get; private set; }
		public Version CurrentVersion { get; private set; }

		#endregion
	}
}
