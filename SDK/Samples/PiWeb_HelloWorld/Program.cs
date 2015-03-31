namespace PiWeb_HelloWorld
{
	#region using

	using System;
	using System.Windows.Forms;

	#endregion

	static class Program
	{
		/// <summary>
		/// Main method for PiWeb API sample application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainForm() );
		}
	}
}
