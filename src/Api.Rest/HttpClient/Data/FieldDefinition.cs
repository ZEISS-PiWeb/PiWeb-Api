#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Data
{
	public class FieldDefinition
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDefinition"/> class.
		/// </summary>
		public FieldDefinition( string fieldName, FieldType fieldType )
		{
			FieldName = fieldName;
			FieldType = fieldType;
		}

		#endregion

		#region properties

		public FieldType FieldType { get; set; }

		public string FieldName { get; set; }

		#endregion
	}
}