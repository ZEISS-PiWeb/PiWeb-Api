#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts;

/// <summary>
/// Represents the different result refresh policies available for some rest client requests.
/// </summary>
public enum RefreshPolicy
{
	/// <summary>
	/// If a previous result exists, it will be returned again avoiding any rest communication. Otherwise, a new result will be fetched
	/// requiring at least one rest request.
	/// </summary>
	UseLatestResult,

	/// <summary>
	/// A new result will be fetched even when a previous result already exists.
	/// </summary>
	RefreshAlways
}