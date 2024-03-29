﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree
{
	#region usings

	using System.Collections.Generic;

	#endregion

	public interface IFilterTree
	{
		#region properties

		Token Token { get; }
		int ChildCount { get; }
		int? Position { get; }
		int? Length { get; }

		#endregion

		#region methods

		IFilterTree GetChild( int index );
		IEnumerable<IFilterTree> GetChildren();

		#endregion
	}
}