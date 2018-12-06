#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region using

	using System;
	using Newtonsoft.Json;
	using Zeiss.IMT.PiWeb.Api.Common.Data;

	#endregion

	/// <summary>
	/// Specifies a concrete entity for a raw data object. If raw data is attached to an entity 
	/// of type <see cref="RawDataEntity.Value"/>, the attribute <code>Uuid</code> contains a 
	/// compound key in the following format: <code>{MeasurementUuid}|{CharacteristicUuid}</code>.
	/// </summary>
	public class RawDataTargetEntity
	{
		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public RawDataTargetEntity()
		{}

		/// <summary>
		/// Creates a new <see cref="RawDataTargetEntity"/> based on the specified <code>uuid</code> and <code>entity</code>.
		/// </summary>
		/// <param name="entity">The entity to which this raw data object belongs to.</param>
		/// <param name="uuid">The uuid of the entity.</param>
		private RawDataTargetEntity( RawDataEntity entity, Guid uuid ) : this( entity, uuid.ToString() ) 
		{}

		/// <summary>
		/// Creates a new <see cref="RawDataTargetEntity"/> based on the specified <code>uuid</code> and <code>entity</code>.
		/// </summary>
		/// <param name="entity">The entity to which this raw data object belongs to.</param>
		/// <param name="uuid">The uuid of the entity.</param>
		public RawDataTargetEntity( RawDataEntity entity, string uuid )
		{
			Entity = entity;
			Uuid = uuid;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the entity to which this raw data object belongs to.
		/// </summary>
		[JsonProperty( "entity" )]
		public RawDataEntity Entity { get; set; }

		/// <summary>
		/// Gets or sets the uuid of the entity.
		/// </summary>
		[JsonProperty( "uuid" )]
		public string Uuid { get; set; }

		#endregion

        #region methods

        /// <summary>Factory method to create a new <see cref="RawDataTargetEntity"/>-object for a <see cref="RawDataEntity.Part"/>.</summary>
        /// <param name="uuid">The uuid of a part.</param>
        public static RawDataTargetEntity CreateForPart( Guid uuid )
        {
            return new RawDataTargetEntity( RawDataEntity.Part, uuid );
        }

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntity"/>-object for a <see cref="RawDataEntity.Characteristic"/>.</summary>
		/// <param name="uuid">The uuid of a characteristic.</param>
		public static RawDataTargetEntity CreateForCharacteristic( Guid uuid )
        {
            return new RawDataTargetEntity( RawDataEntity.Characteristic, uuid );
        }

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntity"/>-object for a <see cref="RawDataEntity.Measurement"/>.</summary>
		/// <param name="uuid">The uuid of a measurement.</param>
        public static RawDataTargetEntity CreateForMeasurement( Guid uuid )
        {
            return new RawDataTargetEntity( RawDataEntity.Measurement, uuid );
        }

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntity"/>-object for a <see cref="RawDataEntity.Value"/>.</summary>
		/// <remarks>
		/// Please note that the characteristic should belong to the part to which the specified measurement belongs to.
		/// </remarks>
		/// <param name="characteristicUuid">The uuid of a characteristic.</param>
		/// <param name="measurementUuid">The uuid of a measurement.</param>
        public static RawDataTargetEntity CreateForValue( Guid measurementUuid, Guid characteristicUuid )
        {
            return new RawDataTargetEntity( RawDataEntity.Value, StringUuidTools.CreateStringUuidPair(measurementUuid, characteristicUuid) );
        }

		/// <summary>
		/// Overriden <see cref="Object.ToString"/>-method.
		/// </summary>
		public override string ToString()
		{
			return string.Format( "{0} ({1})", Entity, Uuid );
		}

		#endregion
	}
}