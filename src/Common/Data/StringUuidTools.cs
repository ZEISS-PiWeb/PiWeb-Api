#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.RawDataService.Rest;

	#endregion

    /// <summary> 
    /// Helper class to convert a pair of measurementUuid and characteristicUuid to a string in form measurementUuid|characteristicUuid and vice versa. 
    /// </summary>
	public static class StringUuidTools
	{
	    /// <summary> Splits a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
	    /// <exception cref="InvalidOperationException">The syntax of <paramref name="uuidPair"/> is invalid.</exception>
	    public static ValueRawDataIdentifier SplitStringUuidPair( string uuidPair )
		{
			ValueRawDataIdentifier result;
			if( !TrySplitStringUuidPair( uuidPair, out result ) )
			{
				throw new InvalidOperationException(
					$"Cannot parse {uuidPair} as pair of UUIDs." +
					"Expected a pair of UUIDs in this format: MeasurementUuid|CharacteristicUuid." );
			}

			return result;
		}

	    public static bool TrySplitStringUuidPair( string uuidPair, out ValueRawDataIdentifier result )
		{
			uuidPair = uuidPair.Replace( "{", string.Empty ).Replace( "}", string.Empty );

			result = null;
			
			var index = uuidPair.IndexOf( '|' );
			if( index == -1 )
				return false;

			var measurementUuidString = uuidPair.Substring( 0, index );
			Guid measurementUuid;
			if( !Guid.TryParse( measurementUuidString, out measurementUuid ) )
				return false;

			var characteristicGuidString = uuidPair.Substring( index + 1 );
			Guid characteristicUuid;
			if( !Guid.TryParse( characteristicGuidString, out characteristicUuid ) )
				return false;

			result = new ValueRawDataIdentifier( measurementUuid, characteristicUuid );
			return true;
		}

		/// <summary> Creates a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		public static string CreateStringUuidPair( Guid measGuid, Guid charGuid )
		{
			return string.Concat(measGuid, '|', charGuid);
		}

		/// <summary> Creates a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		public static string CreateStringUuidPair( ValueRawDataIdentifier guidPair )
		{
			return string.Concat( guidPair.MeasurementUuid, '|', guidPair.CharacteristicUuid );
		}

        /// <summary> Checks if a given string is a unique UUID pair (in form measurementUuid|characteristicUuid) </summary>
        public static bool IsStringUuidPair( string uuidPair )
        {
	        ValueRawDataIdentifier unused;
	        return TrySplitStringUuidPair( uuidPair, out unused );
        }

	    /// <exception cref="ArgumentOutOfRangeException">The syntax of any uuid in <paramref name="uuids"/> is invalid.</exception>
	    /// <exception cref="ArgumentNullException"><paramref name="uuids"/> is <see langword="null" />.</exception>
	    public static void CheckUuids( RawDataEntity entity, [NotNull] IEnumerable<string> uuids )
		{
			if( uuids == null )
				throw new ArgumentNullException( nameof( uuids ) );

			foreach( var uuid in uuids )
				CheckUuid( entity, uuid );
		}

		/// <exception cref="ArgumentOutOfRangeException">The syntax of <paramref name="uuid"/> is invalid.</exception>
		public static void CheckUuid( RawDataEntity entity, string uuid )
		{
			var uuidMustBeComposed = entity == RawDataEntity.Value;

			if( uuidMustBeComposed && !IsStringUuidPair( uuid ) )
			{
				throw new ArgumentOutOfRangeException(
					nameof( uuid ),
					$"'{uuid}' is not a valid measurement value identifier. " +
					"Measurement value identifiers are expected in this format: MeasurementUuid|CharacteristicUuid." );
			}

			if( !uuidMustBeComposed )
				StringUuidToGuid( uuid );
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