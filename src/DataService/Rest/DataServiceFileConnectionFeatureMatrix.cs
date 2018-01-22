#region copyright
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
// /* Carl Zeiss IMT (IZM Dresden)                    */
// /* Softwaresystem PiWeb                            */
// /* (c) Carl Zeiss 2016                             */
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	#region usings

	using Zeiss.IMT.PiWeb.Api.Common.Data;

	#endregion

	/// <summary>
	/// Provides the minimum server version for several features for file connections.
	/// </summary>
	public class DataServiceFileConnectionFeatureMatrix : DataServiceFeatureMatrix
	{
		public DataServiceFileConnectionFeatureMatrix( InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange ){}

		// File connection does not support attribute check
		public override bool SupportsCheckAttributeUsage => false;
	}
}