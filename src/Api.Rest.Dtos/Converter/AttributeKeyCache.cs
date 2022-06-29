#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region usings

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Runtime.CompilerServices;

	#endregion

	internal static class AttributeKeyCache
	{
		#region members

		private static readonly ConcurrentDictionary<ushort, string> Cache = new ConcurrentDictionary<ushort, string>();

		#endregion

		#region methods

		public static string StringForKey( ushort key )
		{
			if( Cache.TryGetValue( key, out var result ) )
				return result;

			var value = key.ToString( NumberFormatInfo.InvariantInfo );
			Cache.TryAdd( key, value );

			return value;
		}

		#endregion
	}
}