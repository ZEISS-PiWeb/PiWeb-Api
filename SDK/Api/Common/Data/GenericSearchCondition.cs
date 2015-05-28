#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data
{
	#region usings

	using DataService;

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
		{ }

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

	public class GenericSearchAttributeCondition : GenericSearchCondition
	{
		#region properties

		public ushort Attribute { get; set; }

		public Operation Operation { get; set; }
		public string Value { get; set; }

		#endregion
	}
}