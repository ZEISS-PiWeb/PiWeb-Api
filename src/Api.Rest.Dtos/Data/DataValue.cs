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

	using System.Globalization;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// This class represents a single measurement value that belongs to one characteristic and one measurement.
	/// </summary>
	public class DataValue : IAttributeItem
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValue"/> class.
		/// </summary>
		public DataValue()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValue"/> class.
		/// </summary>
		public DataValue( double? measuredValue )
		{
			if( measuredValue.HasValue )
				Attributes = new[] { new Attribute( WellKnownKeys.Value.MeasuredValue, measuredValue.Value ) };
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataValue"/> class.
		/// </summary>
		public DataValue( Attribute[] attributes )
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

		#region interface IAttributeItem

		/// <summary>
		/// Gets or sets the attributes that belong to the measurement value. By default, every measurement 
		/// value has the attribute with key <code>1</code> which is the measurent value as a double value.
		/// </summary>
		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public Attribute[] Attributes { get; set; }

		#endregion
	}
}