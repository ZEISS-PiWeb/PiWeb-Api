#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	#region using

	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Dtos.Converter;

	#endregion

	/// <summary>
	/// This class represents a single measurement value that belongs to one characteristic and one measurement.
	/// </summary>
	public class DataValue : IAttributeItem
	{
		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public DataValue()
		{ }

		/// <summary>
		/// Constructor.
		/// </summary>
		public DataValue( double? measuredValue )
		{
			if( measuredValue.HasValue )
				Attributes = new [] { new Attribute( WellKnownKeys.Value.MeasuredValue, measuredValue.Value ) };
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public DataValue( Attribute[] attributes )
		{
			Attributes = attributes;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the attributes that belong to the measurement value. By default, every measurement 
		/// value has the attribute with key <code>1</code> which is the measurent value as a double value.
		/// </summary>
		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public Attribute[] Attributes { get; set; }

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
						return double.Parse( att.Value, System.Globalization.CultureInfo.InvariantCulture );
				}
				return null;
			}
		}

		#endregion
	}
}
