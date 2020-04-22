#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
    /// <summary>
    /// Holds entities that should be kept while clearing a part
    /// </summary>
    public enum ClearPartKeepEntities
    {
        /// <summary>
        /// Do not delete sub parts below to be cleared part
        /// </summary>
        SubParts
    }
}