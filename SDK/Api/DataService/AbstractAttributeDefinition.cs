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
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This is the abstract base element which is used to define the possible attributes that an
	/// entity (like Part, Characteristic etc.) may have.
	/// Every attribute definition consists of a key, which is used to identify the attribute
	/// (e.g. <code>1002</code> or <code>2120</code> and a description.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.AttributeDefinitionConverter ) )]
	public abstract class AbstractAttributeDefinition
	{
		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		protected AbstractAttributeDefinition()
		{ }

		/// <summary>
		/// Constructor. Initializes a new definition using the specified key and value.
		/// </summary>
		/// <param name="key">The unique key for this attribute</param>
		/// <param name="description">The attribute description</param>
		/// <param name="queryEfficient"><code>true</code> if this attribute is efficient for filtering operations</param>
		protected AbstractAttributeDefinition( ushort key, string description, bool queryEfficient = false )
		{
			Key = key;
			QueryEfficient = queryEfficient;
			Description = description;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the key for this attribute definition.
		/// </summary>
		public ushort Key { get; set; }

		/// <summary>
		/// Gets or sets the name of this attribute definition.
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Determines whether this attribute is efficient for filtering operations.
		/// </summary>
		/// <remarks>
		/// This flag is currently unused. This may be used in futere web service versions.
		/// </remarks>
		public bool QueryEfficient { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Overridden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return Description;
		}

		#endregion
	}
}