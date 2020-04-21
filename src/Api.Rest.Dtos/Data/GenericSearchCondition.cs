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
	public class GenericSearchCondition
	{}

	public class GenericSearchNot : GenericSearchCondition
	{
		#region properties

		public GenericSearchCondition Condition { get; set; }

		#endregion
	}

	public class GenericSearchAnd : GenericSearchCondition
	{
		#region constructors

		/// <summary>
		/// Konstruktor
		/// </summary>
		public GenericSearchAnd()
		{}

		/// <summary>
		/// Konstruktor
		/// </summary>
		public GenericSearchAnd( GenericSearchCondition[] filter )
		{
			Conditions = filter;
		}

		#endregion

		#region properties

		public GenericSearchCondition[] Conditions { get; set; }

		#endregion
	}

	public class GenericSearchValueCondition : GenericSearchCondition
	{
		#region properties

		public Operation Operation { get; set; }
		public string Value { get; set; }

		#endregion
	}

	public class GenericSearchAttributeCondition : GenericSearchValueCondition
	{
		#region properties

		public ushort Attribute { get; set; }

		#endregion
	}

	public class GenericSearchFieldCondition : GenericSearchValueCondition
	{
		#region properties

		public string FieldName { get; set; }

		#endregion
	}
}