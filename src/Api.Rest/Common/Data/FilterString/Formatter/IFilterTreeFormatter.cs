#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Formatter
{
	#region usings

	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public interface IFilterTreeFormatter
	{
		#region methods

		string FormatString( [NotNull] IFilterTree tree );

		#endregion
	}
}