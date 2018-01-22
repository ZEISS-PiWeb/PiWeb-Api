#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	/// <summary>
	/// Class that represents a parameter value (name and value) of an uri.
	/// </summary>
	public class ParameterDefinition
	{
		#region constructor

		/// <summary>
		/// Constructor.
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
		public string Name { get; private set; }

		/// <summary>
		/// Returns the value of the parameter.
		/// </summary>
		public string Value { get; private set; }

		#endregion

		#region methods

		/// <summary>
		/// Factory method to create a new <see cref="ParameterDefinition"/>.
		/// </summary>
		public static ParameterDefinition Create( string name, string value )
		{
			return new ParameterDefinition( name, value );
		}

		/// <summary>
		/// Overridden <see cref="object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return string.Format( "{0}={1}", Name, Value );
		}

		#endregion
	}
}