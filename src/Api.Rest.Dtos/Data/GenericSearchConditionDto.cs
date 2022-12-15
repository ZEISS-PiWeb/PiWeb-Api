﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System.Collections.Generic;
	using System.Text.Json.Serialization;
	using Newtonsoft.Json;

	#endregion

	#region usings

	#endregion

	/// <summary>
	/// Klasse zum Parsen einer Filterzeichenkette für Messdaten.
	/// </summary>
	public class GenericSearchConditionDto
	{
		#region constants

		internal const string ConditionFieldName = "condition";
		internal const string ConditionsFieldName = "conditions";
		internal const string AttributeFieldName = "attribute";
		internal const string FieldNameFieldName = "fieldName";

		#endregion
	}

	public class GenericSearchNotDto : GenericSearchConditionDto
	{
		#region properties

		[JsonProperty( ConditionFieldName )]
		[JsonPropertyName( ConditionFieldName )]
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
		public GenericSearchAndDto( IReadOnlyCollection<GenericSearchConditionDto> filter )
		{
			Conditions = filter;
		}

		#endregion

		#region properties

		[JsonProperty( ConditionsFieldName )]
		[JsonPropertyName( ConditionsFieldName )]
		public IReadOnlyCollection<GenericSearchConditionDto> Conditions { get; set; }

		#endregion
	}

	public class GenericSearchValueConditionDto : GenericSearchConditionDto
	{
		#region properties

		[JsonProperty( "operation" )]
		[JsonPropertyName( "operation" )]
		public OperationDto Operation { get; set; }

		[JsonProperty( "value" )]
		[JsonPropertyName( "value" )]
		public string Value { get; set; }

		#endregion
	}

	public class GenericSearchAttributeConditionDto : GenericSearchValueConditionDto
	{
		#region properties

		[JsonProperty( AttributeFieldName )]
		[JsonPropertyName( AttributeFieldName )]
		public ushort Attribute { get; set; }

		#endregion
	}

	public class GenericSearchFieldConditionDto : GenericSearchValueConditionDto
	{
		#region properties

		[JsonProperty( FieldNameFieldName )]
		[JsonPropertyName( FieldNameFieldName )]
		public string FieldName { get; set; }

		#endregion
	}
}