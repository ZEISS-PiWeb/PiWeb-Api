#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Net;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Credential that contains username / Password information.
	/// </summary>
	public sealed class UsernamePasswordCredential : ICredential, IEquatable<UsernamePasswordCredential>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="UsernamePasswordCredential"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="username"/> or <paramref name="password"/> are <see langword="null"/> or empty.</exception>
		public UsernamePasswordCredential( [NotNull] string username, [NotNull] string password )
		{
			Username = username ?? throw new ArgumentNullException( nameof( username ) );
			Password = password ?? throw new ArgumentNullException( nameof( password ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// Return the username.
		/// </summary>
		[NotNull]
		public string Username { get; }

		/// <summary>
		/// Returns the password.
		/// </summary>
		[NotNull]
		public string Password { get; }

		#endregion

		#region methods

		public static UsernamePasswordCredential CreateFromNetworkCredential( NetworkCredential credential )
		{
			return credential == null ? null : new UsernamePasswordCredential( credential.UserName, credential.Password );
		}

		public NetworkCredential ToNetworkCredential()
		{
			return new NetworkCredential( Username, Password );
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return Equals( obj as UsernamePasswordCredential );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return ( Username.GetHashCode() * 397 ) ^ Password.GetHashCode();
			}
		}

		#endregion

		#region interface ICredential

		/// <inheritdoc />
		public string DisplayId => Username;

		/// <inheritdoc />
		public bool Equals( ICredential other )
		{
			return Equals( other as UsernamePasswordCredential );
		}

		#endregion

		#region interface IEquatable<UsernamePasswordCredential>

		/// <inheritdoc />
		public bool Equals( UsernamePasswordCredential other )
		{
			if( ReferenceEquals( this, other ) ) return true;

			return other != null
					&& string.Equals( Username, other.Username )
					&& string.Equals( Password, other.Password );
		}

		#endregion
	}
}