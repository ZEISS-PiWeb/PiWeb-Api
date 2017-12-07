#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data
{
	#region usings

    using System;

    #endregion

    /// <summary> 
    /// Helper class to convert a pair of measurementUuid and characteristicUuid to a string in form measurementUuid|characteristicUuid and vice versa. 
    /// </summary>
	public static class StringUuidTools
	{
		/// <summary> Splits a string containig a measurementUuid and a characteristicUuid in form measurementUuid|characteristicUuid. </summary>
		public static ValueRawDataIdentifier SplitStringUuidPair( string uuidPair )
		{
		    uuidPair = uuidPair.Replace( "{", String.Empty ).Replace( "}", String.Empty );

			var index = uuidPair.IndexOf( '|' );

            Guid measGuid, charGuid;
            if( index == -1 || 
                !Guid.TryParse( uuidPair.Substring( 0, index ), out measGuid ) ||
                !Guid.TryParse( uuidPair.Substring( index + 1 ), out charGuid ))
		    {
                throw new InvalidOperationException( "RawDataEntity.Value expects two uuids in this format: \"{Measurement Uuid}|{Characteristic Uuid}\"." );
		    }

			return ValueRawDataIdentifier.Create( measGuid, charGuid);
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
        public static bool IsStringUuidPair( string pair )
        {
            try
            {
                SplitStringUuidPair( pair );
                return true;
            }
            catch
            {
                return false;
            }
        }
	}

	/// <summary>
	/// References an identifier for a measured values raw data. Consists of the measurement uuid and the characteristic uuid. 
	/// </summary>
	public class ValueRawDataIdentifier
	{
		#region constructor

		public ValueRawDataIdentifier( Guid measurementUuid, Guid characteristicUuid )
		{
			MeasurementUuid = measurementUuid;
			CharacteristicUuid = characteristicUuid;
		}

		#endregion

		#region properties

		public Guid MeasurementUuid { get; private set; }
		public Guid CharacteristicUuid { get; private set; }

		#endregion

		#region methods

		public static ValueRawDataIdentifier Create( Guid measurementUuid, Guid characteristicUuid )
		{
			return new ValueRawDataIdentifier( measurementUuid, characteristicUuid );
		}

		#endregion
	}
}