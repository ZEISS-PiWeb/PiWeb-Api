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

	using System.Collections.Generic;
	using System.Globalization;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// This class represents a single measurement value that belongs to one characteristic and one measurement.
	/// </summary>
	public class DataValueDto : IAttributeItemDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValueDto"/> class.
		/// </summary>
		public DataValueDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValueDto"/> class.
		/// </summary>
		public DataValueDto( double? measuredValue )
		{
			if( measuredValue.HasValue )
				Attributes = new[] { new AttributeDto( WellKnownKeys.Value.MeasuredValue, measuredValue.Value ) };
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValueDto"/> class.
		/// </summary>
		public DataValueDto( IReadOnlyList<AttributeDto> attributes )
		{
			Attributes = attributes;
		}

		#endregion

		#region properties

		/// <summary>
		/// Convinience property for accessing the measurement value (K1).
		/// </summary>
		[JsonIgnore]
		public double? MeasuredValue
		{
			get
			{
				var att = this.GetAttribute( WellKnownKeys.Value.MeasuredValue );
				if( att != null )
				{
					if( att.RawValue != null )
						return (double)att.RawValue;
					if( !string.IsNullOrEmpty( att.Value ) )
						return double.Parse( att.Value, CultureInfo.InvariantCulture );
				}

				return null;
			}
		}

		#endregion

		#region interface IAttributeItemDto

		/// <inheritdoc />
		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public IReadOnlyList<AttributeDto> Attributes { get; set; }

		#endregion
	}
}