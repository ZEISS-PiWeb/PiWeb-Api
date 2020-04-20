#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Formatter
{
	#region usings

	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree;

	#endregion

	public interface IFilterTreeFormatter
	{
		#region methods

		string FormatString( [NotNull] IFilterTree tree );

		#endregion
	}
}
