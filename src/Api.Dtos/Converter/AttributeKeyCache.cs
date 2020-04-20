#region copyright
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
// /* Carl Zeiss IMT (IZM Dresden)                    */
// /* Softwaresystem PiWeb                            */
// /* (c) Carl Zeiss 2016                             */
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Converter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	#endregion

	public class AttributeKeyCache
	{
		#region members
		
		[ThreadStatic]
		private static AttributeKeyCache _Cache;

		private readonly Dictionary<string, ushort> _StringToKey = new Dictionary<string, ushort>();
		private readonly Dictionary<ushort, string> _KeyToString = new Dictionary<ushort, string>();

		#endregion

		#region properties

		public static AttributeKeyCache Cache => _Cache ?? ( _Cache = new AttributeKeyCache() );

		#endregion

		#region methods

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public ushort StringToKey( string key )
		{
			ushort result;
			if( _StringToKey.TryGetValue( key, out result ) )
				return result;

			return _StringToKey[ key ] = ushort.Parse( key, System.Globalization.CultureInfo.InvariantCulture );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		public string KeyToString( ushort key )
		{
			string result;
			if( _KeyToString.TryGetValue( key, out result ) )
				return result;

			return _KeyToString[ key ] = key.ToString( System.Globalization.NumberFormatInfo.InvariantInfo );
		}

		#endregion
	}
}