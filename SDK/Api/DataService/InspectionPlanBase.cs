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
	using System.Diagnostics;
	using System.ComponentModel;
	
	using Common.Data;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This is the base class for inspection plan entities (i.e. parts and characteristics). Each inspection plan entity is identified by an <see cref="Uuid"/> 
	/// (constants, even when renamed) and a unique <see cref="Path"/>.
	/// </summary>
	[DebuggerDisplay( "{Path}" )]
	public abstract class InspectionPlanBase : IAttributeItem
	{
		#region contants

		private static readonly Attribute[] EmptyAttributeList = new Attribute[0];

		#endregion

		#region members

		private Attribute[] _Attributes = EmptyAttributeList;
		
		[JsonProperty( PropertyName="path" )]
		private PathInformation _Path;

		#endregion

		#region properties

		/// <summary>
		/// Indexer for accessing the attribute value with the specified <code>key</code>.
		/// </summary>
		public string this[ ushort key ]
		{
			get { return this.GetAttributeValue( key ); }
			set { this.SetAttribute( key, value ); }
		}

		/// <summary>
		/// Gets or sets the attributes of this inspection plan entity.
		/// </summary>
		public Attribute[] Attributes
		{
			get { return _Attributes; }
			set { _Attributes = value ?? EmptyAttributeList; }
		}

		/// <summary>
		/// Gets or sets the uuid of this inspection plan entity. The uuid is always constant, even if 
		/// this entity is renamed.
		/// </summary>
		[JsonProperty( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the comment of this inspection plan entity.
		/// </summary>
		[JsonProperty( "comment" )]
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the path of this inspection plan entity.
		/// </summary>
		[JsonIgnore ]
		public PathInformation Path
		{
			get { return _Path ?? PathInformation.Root; }
			set { _Path = value; }
		}

		/// <summary>
		/// This is the version number of this entity. The version number is using a global versioning scheme accross all 
		/// version changes of the whole inspection plan. This means, that single instances of a part or characteristic can 
		/// have non consecutive version numbers.
		/// </summary>
		[JsonProperty( "version" )]
		[DefaultValue( 0 )]
		public uint Version { get; set; }

		/// <summary>
		/// Contains the date and time of the last change applied to this instance.
		/// </summary>
		[JsonProperty( "timestamp" )]
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// Gets or sets a flag if this item represents the current version. If this flag is 
		/// <code>false</code> a newer version of this entity exists and this entity is part
		/// of the inspection plan history.
		/// </summary>
		[DefaultValue( true )]
		public bool Current { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Overriden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return Version + " - " + Path;
		}

		#endregion
	}
}