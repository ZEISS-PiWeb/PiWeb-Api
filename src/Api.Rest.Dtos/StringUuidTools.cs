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

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos.RawData;

	#endregion

	/// <summary>
	/// Helper class to convert a pair of measurementUuid and characteristicUuid to a string in form measurementUuid|characteristicUuid and vice versa.
	/// </summary>
	public static class StringUuidTools
	{
		#region methods

		/// <summary> Splits a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		/// <exception cref="InvalidOperationException">The syntax of <paramref name="uuidPair"/> is invalid.</exception>
		public static ValueRawDataIdentifierDto SplitStringUuidPair( string uuidPair )
		{
			if( !TrySplitStringUuidPair( uuidPair, out var result ) )
			{
				throw new InvalidOperationException(
					$"Cannot parse {uuidPair} as pair of UUIDs." +
					"Expected a pair of UUIDs in this format: MeasurementUuid|CharacteristicUuid." );
			}

			return result;
		}

		public static bool TrySplitStringUuidPair( string uuidPair, out ValueRawDataIdentifierDto result )
		{
			uuidPair = uuidPair.Replace( "{", string.Empty ).Replace( "}", string.Empty );

			result = null;

			var index = uuidPair.IndexOf( '|' );
			if( index == -1 )
				return false;

			var measurementUuidString = uuidPair.Substring( 0, index );

			if( !Guid.TryParse( measurementUuidString, out var measurementUuid ) )
				return false;

			var characteristicGuidString = uuidPair.Substring( index + 1 );

			if( !Guid.TryParse( characteristicGuidString, out var characteristicUuid ) )
				return false;

			result = new ValueRawDataIdentifierDto( measurementUuid, characteristicUuid );
			return true;
		}

		/// <summary> Creates a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		public static string CreateStringUuidPair( Guid measGuid, Guid charGuid )
		{
			return string.Concat( measGuid, '|', charGuid );
		}

		/// <summary> Creates a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		public static string CreateStringUuidPair( ValueRawDataIdentifierDto guidPair )
		{
			return string.Concat( guidPair.MeasurementUuid, '|', guidPair.CharacteristicUuid );
		}

		/// <summary> Checks if a given string is a unique UUID pair (in form measurementUuid|characteristicUuid) </summary>
		public static bool IsStringUuidPair( string uuidPair )
		{
			return TrySplitStringUuidPair( uuidPair, out _ );
		}

		/// <exception cref="ArgumentOutOfRangeException">The syntax of any uuid in <paramref name="uuids"/> is invalid.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="uuids"/> is <see langword="null" />.</exception>
		public static void CheckUuids( RawDataEntityDto entity, [NotNull] IEnumerable<string> uuids )
		{
			if( uuids == null )
				throw new ArgumentNullException( nameof( uuids ) );

			foreach( var uuid in uuids )
				CheckUuid( entity, uuid );
		}

		/// <exception cref="ArgumentOutOfRangeException">The syntax of <paramref name="uuid"/> is invalid.</exception>
		public static void CheckUuid( RawDataEntityDto entity, string uuid )
		{
			var uuidMustBeComposed = entity == RawDataEntityDto.Value;

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
		public static IReadOnlyList<Guid> StringUuidListToGuidList( [NotNull] IEnumerable<string> uuids )
		{
			if( uuids == null )
				throw new ArgumentNullException( nameof( uuids ) );

			return uuids.Select( StringUuidToGuid ).ToList();
		}

		/// <exception cref="ArgumentOutOfRangeException">The syntax of <paramref name="uuid"/> is invalid.</exception>
		public static Guid StringUuidToGuid( string uuid )
		{
			if( !Guid.TryParse( uuid, out var guid ) )
			{
				throw new ArgumentException( nameof( uuid ), $"'{uuid}' is not a valid Uuid." );
			}

			return guid;
		}

		#endregion
	}
}