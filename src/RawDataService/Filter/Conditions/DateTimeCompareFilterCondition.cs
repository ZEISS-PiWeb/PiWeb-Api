#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Conditions
{
	#region usings

	using System;
	using System.Xml;
	using Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree;

	#endregion

	public class DateTimeCompareFilterCondition : FilterCondition
	{
		#region members

		private readonly DateTimeAttributes _Attribute;
		private readonly CompareOperation _Operation;
		private readonly DateTime? _Value;

		#endregion

		#region constructors

		public DateTimeCompareFilterCondition( DateTimeAttributes attribute, CompareOperation operation, DateTime? value )
		{
			_Attribute = attribute;
			_Operation = operation;
			_Value = value;
		}

		#endregion

		#region methods

		public override IFilterTree BuildFilterTree()
		{
			string valueString = null;
			if( _Value.HasValue )
				valueString = XmlConvert.ToString( _Value.Value, XmlDateTimeSerializationMode.RoundtripKind );

			var attributeName = FilterHelper.GetAttributeName( _Attribute );
			var operatorTokenType = FilterHelper.GetOperatorTokenType( _Operation );

			var valueTree = FilterTree.MakeValue( valueString );
			return FilterHelper.MakeComparison( operatorTokenType, attributeName, valueTree );
		}

		#endregion
	}
}
