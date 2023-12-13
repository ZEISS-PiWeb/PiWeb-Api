#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Text;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	internal static class RequestUriHelper
	{
		#region methods

		internal static Uri MakeRequestUri( [NotNull] Uri serviceLocation, string requestPath, [CanBeNull] IEnumerable<ParameterDefinition> parameterDefinitions )
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