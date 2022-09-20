﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Data.SqlTypes;
	using System.Diagnostics;
	using System.Text.Json.Serialization;
	using System.Xml;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// This class represents the base class for measurements with its attributes. The concrete class for measurements that also contains the measurement values is <see cref="DataMeasurementDto"/>.
	/// A measurement is identified by an <see cref="Uuid"/>. A measurement always belongs to one and only one part.
	/// </summary>
	[DebuggerDisplay( "Measurement (Uuid={Uuid} Time={Time})" )]
	public class SimpleMeasurementDto : IAttributeItemDto
	{
		#region members

		private static readonly DateTime MinimumValidDatabaseDateTime = DateTime.SpecifyKind( SqlDateTime.MinValue.Value, DateTimeKind.Utc );

		private IReadOnlyList<AttributeDto> _Attributes = Array.Empty<AttributeDto>();
		private DateTime? _CachedTimeValue;
		private bool _HasCachedTime;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the uuid of this measurement.
		/// </summary>
		[JsonPropertyName( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the uuid the part this measurement belongs to.
		/// </summary>
		[JsonPropertyName( "partUuid" )]
		public Guid PartUuid { get; set; }

		/// <summary>
		/// Gets or sets the last modification timestamp of this measurement. The server will update this
		/// timestamp whenever an attribute of this measurement is changed or whenever measurement values
		/// of this measurement are updated, deleted and added.
		/// </summary>
		[JsonPropertyName( "lastModified" )]
		public DateTime LastModified { get; set; }

		/// <summary>
		/// Gets or sets the creation timestamp of this measurement.
		/// </summary>
		[JsonPropertyName( "created" )]
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets the status information for this measurement. This status information can be requested when
		/// performing a measurement search using one of the values from <see cref="MeasurementStatisticsDto"/>.
		/// </summary>
		[JsonPropertyName( "status" )]
		public IReadOnlyList<SimpleMeasurementStatusDto> Status { get; set; }

		/// <summary>
		/// Gets or sets the time of this measurement. If this measurement has no time attribute, then <see cref="MinimumValidDatabaseDateTime"/> will
		/// be returned.
		/// </summary>
		[JsonIgnore]
		public DateTime? Time
		{
			[DebuggerStepThrough]
			get
			{
				if( _HasCachedTime )
					return _CachedTimeValue;

				try
				{
					var attribute = this.GetAttribute( WellKnownKeys.Measurement.Time );
					if( attribute != null && !string.IsNullOrEmpty( attribute.Value.Value ) )
						_CachedTimeValue = XmlConvert.ToDateTime( attribute.Value.Value, XmlDateTimeSerializationMode.RoundtripKind );
				}
				catch
				{
					// ignored
				}

				_HasCachedTime = true;

				return _CachedTimeValue;
			}
			set
			{
				if( value == null )
				{
					this.RemoveAttribute( WellKnownKeys.Measurement.Time );
					_CachedTimeValue = null;
					_HasCachedTime = true;
				}
				else
				{
					this.SetAttribute( new AttributeDto( WellKnownKeys.Measurement.Time, XmlConvert.ToString( value.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );
					_CachedTimeValue = value;
					_HasCachedTime = true;
				}
			}
		}

		/// <summary>
		/// Returns the measurement time and in case of no time is specified, the minimum time allowed (<see cref="System.Data.SqlTypes.SqlDateTime.MinValue"/>).
		/// </summary>
		[JsonIgnore]
		public DateTime TimeOrMinDate => Time ?? MinimumValidDatabaseDateTime;

		/// <summary>
		/// Returns the measurement time and in case of no time is specified, the creation date of the measurement.
		/// </summary>
		[JsonIgnore]
		public DateTime TimeOrCreationDate => Time ?? Created;

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return $"'{Time}' [{Uuid}]";
		}

		#endregion

		#region interface IAttributeItemDto

		/// <inheritdoc />
		[JsonPropertyName( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public IReadOnlyList<AttributeDto> Attributes
		{
			[NotNull] get => _Attributes;
			set
			{
				_Attributes = value ?? Array.Empty<AttributeDto>();
				_CachedTimeValue = null;
				_HasCachedTime = false;
			}
		}

		#endregion
	}
}