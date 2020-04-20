#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;

	#endregion

    /// <summary> 
    /// Helper class to convert a pair of measurementUuid and characteristicUuid to a string in form measurementUuid|characteristicUuid and vice versa. 
    /// </summary>
	public static class StringUuidTools
	{
		/// <summary> Creates a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		public static string CreateStringUuidPair( Guid measGuid, Guid charGuid )
		{
			return string.Concat(measGuid, '|', charGuid);
		}

		/// <exception cref="ArgumentOutOfRangeException">The syntax of any uuid in <paramref name="uuids"/> is invalid.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="uuids"/> is <see langword="null" />.</exception>
		public static List<Guid> StringUuidListToGuidList( [NotNull] IEnumerable<string> uuids )
		{
			if( uuids == null )
				throw new ArgumentNullException( nameof( uuids ) );

			return uuids.Select( StringUuidToGuid ).ToList();
		}

		/// <exception cref="ArgumentOutOfRangeException">The syntax of <paramref name="uuid"/> is invalid.</exception>
		public static Guid StringUuidToGuid( string uuid )
		{
			Guid guid;
			if(!Guid.TryParse( uuid, out guid ) )
			{
				throw new ArgumentException(
					nameof( uuid ),
					$"'{uuid}' is not a valid Uuid." );
			}
			return guid;
		}
	}
}