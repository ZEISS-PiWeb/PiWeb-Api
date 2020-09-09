#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// This class represents a single part of an inspection plan path. A path part has a <see cref="Value"/> and can
	/// either specify a characteristic or a part (<see cref="Type"/>).
	/// Notice that comparision of path elements is case insensitiv per default.
	/// </summary>
	/// <remarks>This class is immutable!</remarks>
	public sealed class PathElementDto : IEquatable<PathElementDto>
	{
		#region members

		/// <summary>
		/// Constant value for an empty part.
		/// </summary>
		public static readonly PathElementDto EmptyPart = Part( "" );

		/// <summary>
		/// Constant value for an empty characteristic.
		/// </summary>
		public static readonly PathElementDto EmptyCharacteristic = Char( "" );

		private int _HashCode = -1;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PathElementDto"/> class.
		/// </summary>
		public PathElementDto( InspectionPlanEntityDto type = InspectionPlanEntityDto.Part, string value = "" )
		{
			Type = type;
			Value = value ?? string.Empty;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gibt den Type des Pfadabschnittes zurück.
		/// </summary>
		public InspectionPlanEntityDto Type { get; }

		/// <summary>
		/// Gibt den Namen zurück.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Gibt an, ob diese <see cref="PathElementDto"/>-Instanz leer ist.
		/// </summary>
		public bool IsEmpty => Value.Length == 0;

		#endregion

		#region methods

		/// <summary>
		/// Creates a new path element with type <see cref="InspectionPlanEntityDto.Part"/>.
		/// </summary>
		public static PathElementDto Part( string name )
		{
			return new PathElementDto( InspectionPlanEntityDto.Part, name );
		}

		/// <summary>
		/// Creates a new path element with type <see cref="InspectionPlanEntityDto.Characteristic"/>.
		/// </summary>
		public static PathElementDto Char( string name )
		{
			return new PathElementDto( InspectionPlanEntityDto.Characteristic, name );
		}

		/// <summary>
		/// Equality operator. Path element are compared case insensitive.
		/// </summary>
		public static bool operator ==( PathElementDto p1, PathElementDto p2 )
		{
			return Equals( p1, p2 );
		}

		/// <summary>
		/// Inequality operator. Path element are compared case insensitive.
		/// </summary>
		public static bool operator !=( PathElementDto p1, PathElementDto p2 )
		{
			return !Equals( p1, p2 );
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return Equals( obj as PathElementDto );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			if( _HashCode == -1 )
				_HashCode = StringComparer.OrdinalIgnoreCase.GetHashCode( Value );
			return _HashCode;
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return Value;
		}

		#endregion

		#region interface IEquatable<PathElementDto>

		/// <inheritdoc />
		public bool Equals( PathElementDto other )
		{
			return !ReferenceEquals( other, null ) &&
					Type == other.Type &&
					GetHashCode() == other.GetHashCode() &&
					string.Equals( Value, other.Value, StringComparison.OrdinalIgnoreCase );
		}

		#endregion
	}
}