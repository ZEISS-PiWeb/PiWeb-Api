#region copyright

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
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// This class represents a single measurement value that belongs to one characteristic and one measurement.
	/// </summary>
	[JsonConverter( typeof( DataValueConverter ) )]
	public struct DataValueDto : IAttributeItemDto, IEquatable<DataValueDto>
	{
		#region members

		private IReadOnlyList<AttributeDto> _Attributes;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValueDto"/> class.
		/// </summary>
		public DataValueDto( double? measuredValue )
		{
			_Attributes = measuredValue.HasValue
				? new[] { new AttributeDto( WellKnownKeys.Value.MeasuredValue, measuredValue.Value ) }
				: Array.Empty<AttributeDto>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValueDto"/> class.
		/// </summary>
		public DataValueDto( [NotNull] IReadOnlyList<AttributeDto> attributes )
		{
			_Attributes = attributes ?? throw new ArgumentNullException( nameof( attributes ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// Convinience property for accessing the measurement value (K1).
		/// </summary>
		[JsonIgnore]
		public double? MeasuredValue => this.GetDoubleAttributeValue( WellKnownKeys.Value.MeasuredValue );

		#endregion

		#region interface IAttributeItemDto

		/// <inheritdoc />
		[JsonPropertyName( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public IReadOnlyList<AttributeDto> Attributes
		{
			get => _Attributes ?? Array.Empty<AttributeDto>();
			set => _Attributes = value;
		}

		#endregion

		#region methods

		/// <inheritdoc />
		public bool Equals( DataValueDto other )
		{
			if( Attributes.Count != other.Attributes.Count )
				return false;

			if( Attributes.Count == 0 )
				return true;

			if( Attributes.Count == 1 )
				return Attributes[ 0 ].Equals( other.Attributes[ 0 ] );

			// ReSharper disable once LoopCanBeConvertedToQuery
			// ReSharper disable once ForCanBeConvertedToForeach
			for( var i = 0; i < Attributes.Count; i++ )
			{
				var attribute = other.GetAttribute( Attributes[ i ].Key );
				if( attribute is null || !Attributes[ i ].Equals( attribute.Value ) )
					return false;
			}

			return true;
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return obj is DataValueDto other && Equals( other );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Attributes.Count switch
			{
				0 => 0,
				1 => Attributes[ 0 ].GetHashCode(),
				_ => HashCode.Combine( MeasuredValue, Attributes.Count )
			};
		}

		#endregion
	}
}