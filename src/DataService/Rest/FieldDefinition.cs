#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	public class FieldDefinition
	{
		#region constructors

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