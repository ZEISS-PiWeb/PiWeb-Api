#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos
{
	/// <summary>
	/// Class that represents a parameter value (name and value) of an uri.
	/// </summary>
	public class ParameterDefinition
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterDefinition"/> class.
		/// </summary>
		private ParameterDefinition( string name, string value )
		{
			Name = name;
			Value = value;
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the name of the parameter.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Returns the value of the parameter.
		/// </summary>
		public string Value { get; }

		#endregion

		#region methods

		/// <summary>
		/// Factory method to create a new <see cref="ParameterDefinition"/>.
		/// </summary>
		public static ParameterDefinition Create( string name, string value )
		{
			return new ParameterDefinition( name, value );
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{Name}={Value}";
		}

		#endregion
	}
}