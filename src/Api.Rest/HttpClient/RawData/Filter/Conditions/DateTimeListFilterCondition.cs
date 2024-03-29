﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Conditions
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Xml;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class DateTimeListFilterCondition : FilterCondition
	{
		#region members

		private readonly DateTimeAttributes _Attribute;
		private readonly ListOperation _Operation;
		private readonly List<DateTime?> _Values;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DateTimeListFilterCondition"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public DateTimeListFilterCondition( DateTimeAttributes attribute, ListOperation operation, [NotNull] IEnumerable<DateTime?> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );
			_Attribute = attribute;
			_Operation = operation;
			_Values = new List<DateTime?>( values );
		}

		#endregion

		#region methods

		/// <inheritdoc />
		public override IFilterTree BuildFilterTree()
		{
			var valuesStrings = new List<string>();

			foreach( var dateTime in _Values )
			{
				string valueString = null;
				if( dateTime.HasValue )
					valueString = XmlConvert.ToString( dateTime.Value, XmlDateTimeSerializationMode.RoundtripKind );

				valuesStrings.Add( valueString );
			}

			var attributeName = FilterHelper.GetAttributeName( _Attribute );
			var operatorTokenType = FilterHelper.GetOperatorTokenType( _Operation );

			var valueTree = FilterTree.MakeValueList( valuesStrings.ToArray() );
			return FilterHelper.MakeComparison( operatorTokenType, attributeName, valueTree );
		}

		#endregion
	}
}