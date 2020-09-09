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
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// Holds information of a <see cref="CatalogDto"/>'s entry
	/// </summary>
	public class CatalogEntryDto : IAttributeItemDto
	{
		#region members

		private AttributeDto[] _Attributes = new AttributeDto[ 0 ];

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogEntryDto"/> class.
		/// </summary>
		public CatalogEntryDto()
		{
			Key = -1;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the unique key of this catalog entry.
		/// </summary>
		[JsonProperty( "key" )]
		public short Key { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return ToString( CultureInfo.CurrentUICulture );
		}

		/// <summary>
		/// Returns a string representation of this entry using the format provider
		/// <code>provider</code>.
		/// </summary>
		public string ToString( IFormatProvider provider )
		{
			if( Attributes == null )
				return "";

			if( Attributes.Length == 1 )
				return Convert.ToString( GetTypedAttributeValue( Attributes[ 0 ] ), provider );

			var allSameEntries = true;

			var sb = new StringBuilder();
			foreach( var att in Attributes )
			{
				if( sb.Length > 0 )
					sb.Append( " - " );

				if( att.Value.Trim().Length > 0 )
					sb.Append( Convert.ToString( GetTypedAttributeValue( att ), provider ) );

				allSameEntries &= att.Value == _Attributes[ 0 ].Value;
			}

			if( allSameEntries && _Attributes != null && _Attributes.Length > 0 )
				return Convert.ToString( GetTypedAttributeValue( Attributes[ 0 ] ), provider );

			return sb.ToString();
		}

		private object GetTypedAttributeValue( AttributeDto attribute )
		{
			if( attribute.RawValue != null )
				return ( attribute.RawValue is DateTime time ) ? time.ToLocalTime() : attribute.RawValue;

			return attribute.Value;
		}

		#endregion

		#region interface IAttributeItemDto

		/// <summary>
		/// Gets or sets the attributes that belong to this catalog entry.
		/// </summary>
		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ) )]
		public AttributeDto[] Attributes
		{
			[NotNull] get => _Attributes;
			set
			{
				value = value ?? new AttributeDto[ 0 ];

				_Attributes = value.All( attr => attr.IsNull() ) ? new AttributeDto[ 0 ] : value;
				_Attributes = _Attributes.OrderBy( a => a.Key ).ToArray();
			}
		}

		#endregion
	}
}