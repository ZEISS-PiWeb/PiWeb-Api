#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Specifies a concrete entity for a raw data object. If raw data is attached to an entity
	/// of type <see cref="RawDataEntityDto.Value"/>, the attribute <code>Uuid</code> contains a
	/// compound key in the following format: <code>{MeasurementUuid}|{CharacteristicUuid}</code>.
	/// </summary>
	public class RawDataTargetEntityDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataTargetEntityDto"/> class.
		/// </summary>
		public RawDataTargetEntityDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataTargetEntityDto"/> class based on the specified <code>uuid</code> and <code>entity</code>.
		/// </summary>
		/// <param name="entity">The entity to which this raw data object belongs to.</param>
		/// <param name="uuid">The uuid of the entity.</param>
		private RawDataTargetEntityDto( RawDataEntityDto entity, Guid uuid ) : this( entity, uuid.ToString() )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataTargetEntityDto"/> class based on the specified <code>uuid</code> and <code>entity</code>.
		/// </summary>
		/// <param name="entity">The entity to which this raw data object belongs to.</param>
		/// <param name="uuid">The uuid of the entity.</param>
		public RawDataTargetEntityDto( RawDataEntityDto entity, string uuid )
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
		public RawDataEntityDto Entity { get; set; }

		/// <summary>
		/// Gets or sets the uuid of the entity.
		/// </summary>
		[JsonProperty( "uuid" )]
		public string Uuid { get; set; }

		#endregion

		#region methods

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntityDto"/>-object for a <see cref="RawDataEntityDto.Part"/>.</summary>
		/// <param name="uuid">The uuid of a part.</param>
		public static RawDataTargetEntityDto CreateForPart( Guid uuid )
		{
			return new RawDataTargetEntityDto( RawDataEntityDto.Part, uuid );
		}

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntityDto"/>-object for a <see cref="RawDataEntityDto.Characteristic"/>.</summary>
		/// <param name="uuid">The uuid of a characteristic.</param>
		public static RawDataTargetEntityDto CreateForCharacteristic( Guid uuid )
		{
			return new RawDataTargetEntityDto( RawDataEntityDto.Characteristic, uuid );
		}

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntityDto"/>-object for a <see cref="RawDataEntityDto.Measurement"/>.</summary>
		/// <param name="uuid">The uuid of a measurement.</param>
		public static RawDataTargetEntityDto CreateForMeasurement( Guid uuid )
		{
			return new RawDataTargetEntityDto( RawDataEntityDto.Measurement, uuid );
		}

		/// <summary>Factory method to create a new <see cref="RawDataTargetEntityDto"/>-object for a <see cref="RawDataEntityDto.Value"/>.</summary>
		/// <remarks>
		/// Please note that the characteristic should belong to the part to which the specified measurement belongs to.
		/// </remarks>
		/// <param name="characteristicUuid">The uuid of a characteristic.</param>
		/// <param name="measurementUuid">The uuid of a measurement.</param>
		public static RawDataTargetEntityDto CreateForValue( Guid measurementUuid, Guid characteristicUuid )
		{
			return new RawDataTargetEntityDto( RawDataEntityDto.Value, StringUuidTools.CreateStringUuidPair( measurementUuid, characteristicUuid ) );
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{Entity} ({Uuid})";
		}

		#endregion
	}
}