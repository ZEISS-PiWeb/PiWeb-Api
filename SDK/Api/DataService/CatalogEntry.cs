#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;
	using System.Linq;

	using Common.Data;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This element describes a single entry of a catalogue. Each entry has a key, which has to be
	/// unique within the catalogue in which it is used, and zero or more attributes.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.CatalogueEntryConverter ) )]
	public class CatalogEntry : IAttributeItem
	{
		#region members

		private Attribute[] _Attributes = new Attribute[ 0 ];

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the attributes that belong to this catalog entry.
		/// </summary>
		public Attribute[] Attributes
		{
			get { return _Attributes; }
			set
			{
				value = value ?? new Attribute[0];

				_Attributes = value.All( attr => attr.IsNull() ) ? new Attribute[0] : value;
				_Attributes = _Attributes.OrderBy( a => a.Key ).ToArray();
			}
		}

		/// <summary>
		/// Gets or sets the key of this catalog entry.
		/// </summary>
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
