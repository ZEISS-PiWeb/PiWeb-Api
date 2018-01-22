#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	/// <summary>
	/// 
	/// </summary>
	public class RawDataQuery
	{
		#region constructors

		public RawDataQuery()
		{
		}

		public RawDataQuery(RawDataTargetEntity[] entities )
		{
			Entities = entities;
		}

		#endregion

		#region properties
		
		public RawDataTargetEntity[] Entities { get; set; } = { };

		#endregion
	}
}