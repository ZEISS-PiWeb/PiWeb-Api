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

	#endregion

	/// <summary>
	/// Klasse zum Parsen einer Filterzeichenkette für Messdaten.
	/// </summary>
	public class GenericSearchConditionDto
	{ }

	public class GenericSearchNotDto : GenericSearchConditionDto
	{
		#region properties

		public GenericSearchConditionDto Condition { get; set; }

		#endregion
	}

	public class GenericSearchAndDto : GenericSearchConditionDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericSearchAndDto"/> class.
		/// </summary>
		public GenericSearchAndDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericSearchAndDto"/> class.
		/// </summary>
		public GenericSearchAndDto( GenericSearchConditionDto[] filter )
		{
			Conditions = filter;
		}

		#endregion

		#region properties

		public GenericSearchConditionDto[] Conditions { get; set; }

		#endregion
	}

	public class GenericSearchValueConditionDto : GenericSearchConditionDto
	{
		#region properties

		public OperationDto Operation { get; set; }
		public string Value { get; set; }

		#endregion
	}

	public class GenericSearchAttributeConditionDto : GenericSearchValueConditionDto
	{
		#region properties

		public ushort Attribute { get; set; }

		#endregion
	}

	public class GenericSearchFieldConditionDto : GenericSearchValueConditionDto
	{
		#region properties

		public string FieldName { get; set; }

		#endregion
	}
}