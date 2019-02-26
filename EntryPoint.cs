

/*
 *	Developed By: Alzar
 *	Name: Easy Loadout
 *	Dependent: Rage Plugin Hook & LSPDFR
 *	Released On: GitHub & LSPDFR
 */

[assembly: Rage.Attributes.Plugin("Easy Loadout", Author = "sr7066", Description = "Allows for customized loadouts to be given on-command thanks to an in-game menu as well as a quick-give bind.", SupportUrl = "https://github.com/Alzar/EasyLoadout/issues")]

namespace EasyLoadout {
	using Rage;
	using System.Windows.Forms;
	using EasyLoadout.Core.Utils;

	public class EntryPoint {

		public static void Main() {
			string CurrentVersion = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
			Updater.VersionCheck("UI", "https://raw.githubusercontent.com/Alzar/EasyLoadout/master/core-latest.txt", CurrentVersion);
			Plugin.SetupPlugin();
			GameFiber.StartNew(delegate { Plugin.PluginLoop(); });
		}
	}
}
