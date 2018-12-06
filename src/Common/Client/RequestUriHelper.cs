#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Text;

	#endregion

	internal static class RequestUriHelper
	{
		#region methods

		internal static Uri MakeRequestUri( Uri serviceLocation, string requestPath, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			var uriBuilder = new UriBuilder( serviceLocation );
			uriBuilder.Path += requestPath;

			if( parameterDefinitions != null )
			{
				var queryString = new StringBuilder();
				foreach( var parameterDefinition in parameterDefinitions )
				{
					if( queryString.Length > 0 )
						queryString.Append( "&" );

					queryString.Append( parameterDefinition.Name ).Append( "=" ).Append( Uri.EscapeDataString( parameterDefinition.Value ) );
				}
				uriBuilder.Query = queryString.ToString();
			}

			return uriBuilder.Uri;
		}

		#endregion
	}
}
