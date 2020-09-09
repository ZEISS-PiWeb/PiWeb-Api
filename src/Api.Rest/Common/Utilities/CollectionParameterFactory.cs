#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Zeiss.PiWeb.Api.Rest.Common.Data;

	#endregion

	/// <summary>
	/// Helper Factory to create typed CollectionParameterDefinitions.
	/// </summary>
	internal static class CollectionParameterFactory
	{
		#region methods

		public static CollectionParameterDefinition<Guid> Create( string name, IEnumerable<Guid> values )
		{
			var conversion = new Func<IEnumerable<Guid>, string>(
				guids => RestClientHelper.ConvertGuidListToString( guids.ToArray() ) );

			return new CollectionParameterDefinition<Guid>( name, values, conversion );
		}

		public static CollectionParameterDefinition<string> Create( string name, IEnumerable<string> values )
		{
			return new CollectionParameterDefinition<string>( name, values, RestClientHelper.ToListString );
		}

		#endregion
	}
}