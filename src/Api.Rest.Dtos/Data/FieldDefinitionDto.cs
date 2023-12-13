#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	public class FieldDefinitionDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDefinitionDto"/> class.
		/// </summary>
		public FieldDefinitionDto( string fieldName, FieldTypeDto fieldType )
		{
			FieldName = fieldName;
			FieldType = fieldType;
		}

		#endregion

		#region properties

		public FieldTypeDto FieldType { get; set; }

		public string FieldName { get; set; }

		#endregion
	}
}