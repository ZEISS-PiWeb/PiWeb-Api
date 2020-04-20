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

	using System;
	using System.Linq;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Dtos.Converter;

	#endregion

	/// <summary>
	/// Holds information of a <see cref="Catalog"/>'s entry
	/// </summary>
	public class CatalogEntry : IAttributeItem
	{
		#region members

		private Attribute[] _Attributes = new Attribute[ 0 ];

		#endregion

		public CatalogEntry()
		{
			Key = -1;
		}

		#region properties

		/// <summary>
		/// Gets or sets the attributes that belong to this catalog entry.
		/// </summary>
		[JsonProperty( "attributes" ), JsonConverter( typeof( AttributeArrayConverter ))]
		public Attribute[] Attributes
		{
			[NotNull]
			get { return _Attributes; }
			set
			{
				value = value ?? new Attribute[0];

				_Attributes = value.All( attr => attr.IsNull() ) ? new Attribute[0] : value;
				_Attributes = _Attributes.OrderBy( a => a.Key ).ToArray();
			}
		}

		/// <summary>
		/// Gets or sets the unique key of this catalog entry.
		/// </summary>
		[JsonProperty( "key" )]
		public short Key { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Overridden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return ToString( System.Globalization.CultureInfo.CurrentUICulture );
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

			var sb = new System.Text.StringBuilder();
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

		private object GetTypedAttributeValue( Attribute attribute )
		{
			if( attribute.RawValue != null )
				return ( attribute.RawValue is DateTime ) ? ( (DateTime)attribute.RawValue ).ToLocalTime() : attribute.RawValue;

			return attribute.Value;
		}

		#endregion
	}
}
