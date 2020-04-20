#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	/// <summary>
	/// Holds information about the attributes which should be returned on a search operation.
	/// </summary>
	public class AttributeSelector
	{
		#region constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributeSelector()
		{
			AllAttributes = AllAttributeSelection.True;
		}

		/// <summary>
		/// Constructor. Initializes a new attribute selector for a specific <see cref="AllAttributeSelection"/>.
		/// </summary>
		public AttributeSelector( AllAttributeSelection allAttributes )
		{
			AllAttributes = allAttributes;
		}

		/// <summary>
		/// Constructor. Initializes a new attribute selector for a specific set of attributes <code>attributes</code>.
		/// </summary>
		public AttributeSelector( ushort[] attributes )
		{
			AllAttributes = AllAttributeSelection.False;
			Attributes = attributes;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the list of attributes that should be fetched.
		/// </summary>
		public ushort[] Attributes { get; set; }

		/// <summary>
		/// Gets or sets a value that determines whether to fetch all, no or just the query efficient attributes.
		/// </summary>
		public AllAttributeSelection? AllAttributes { get; set; }

		#endregion
	}
}