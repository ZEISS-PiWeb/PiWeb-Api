#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data
{
	/// <summary>
	/// Interface für Objekte, die eine Liste von Attributen zur Verfügung stellen.
	/// </summary>
	public interface IAttributeItem
	{
		/// <summary>
		/// Gibt alle Attribute als Feld zurück.
		/// </summary>
		DataService.Attribute[] Attributes { get; set; }
	}
}