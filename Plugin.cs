/*
 *	Developed By: Alzar
 *	Name: Easy Loadout
 *	Dependent: Rage Plugin Hook & LSPDFR
 *	Released On: GitHub & LSPDFR
 */

namespace EasyLoadout {
	using System.Windows.Forms;
	using System.Collections.Generic;
	using Rage;
	using EasyLoadout.Core.Utils;
	using Rage.Native;
	using RAGENativeUI;
	using RAGENativeUI.Elements;
	using LSPD_First_Response.Mod.API;

	public static class Plugin {
		private static MenuPool pMenuPool;
		private static UIMenu pLoadoutMenu;
		private static UIMenuItem pGiveLoadout;

		internal static void SetupPlugin() {
			Logger.DebugLog("Core Plugin Function Started");

			Core.Utils.Core.Setup();

			//Initial menu setup
			pMenuPool = new MenuPool();
			pLoadoutMenu = new UIMenu("Easy Loadout", "Choose your active loadout");
			pMenuPool.Add(pLoadoutMenu);

			//This is where the user-defined loadout count happens at and is initially loaded
			//We're running through all of the configs and adding them to the loadouts list aswell as adding a menu item for them
			for (int i = 0; i < Global.Application.LoadoutCount; i++) {
				pLoadoutMenu.AddItem(Core.Utils.Core.pLoadouts[i]);
			}

			pLoadoutMenu.AddItem(pGiveLoadout = new UIMenuItem("Give Loadout"));
			pLoadoutMenu.RefreshIndex();
			pLoadoutMenu.OnItemSelect += OnItemSelect;
			pLoadoutMenu.OnCheckboxChange += OnCheckboxChange;
		}



		//Game loop
		internal static void PluginLoop()
		{
			Logger.DebugLog("UI", "Starting plugin loop.");
			while (true)
			{
				GameFiber.Yield();
				//Checking if keybinds for opening menu is pressed. Currently it doesn't check if any other menu is open, so it can overlap with other RageNativeUI menus.
				if (Game.IsKeyDownRightNow(Global.Controls.OpenMenuModifier) && Game.IsKeyDown(Global.Controls.OpenMenu) || Global.Controls.OpenMenuModifier == Keys.None && Game.IsKeyDown(Global.Controls.OpenMenu))
				{
					Logger.DebugLog("UI", "Menu button pressed, toggling menu status");
					pLoadoutMenu.Visible = !pLoadoutMenu.Visible;
				}

				if (Game.IsKeyDownRightNow(Global.Controls.GiveLoadoutModifier) && Game.IsKeyDown(Global.Controls.GiveLoadout) || Global.Controls.GiveLoadoutModifier == Keys.None && Game.IsKeyDown(Global.Controls.GiveLoadout))
				{
					Logger.DebugLog("UI", "Quick Give Bind Pressed, Giving Loadout");
					LoadoutHandler.GiveLoadout();
				}

				pMenuPool.ProcessMenus();
			}
		}

		public static void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool isChecked) {
			//Ensuring UI that had the update is ours..
			if (sender == pLoadoutMenu) {
				//Then we're checking the checkboxes were updated...
				for (int i = 0; i < Global.Application.LoadoutCount; i++) {
					if (checkbox == Core.Utils.Core.pLoadouts[i]) {
						Core.Utils.Core.UpdateActiveLoadout(i);
					}
				}
			}
			else
				return;
		}

		public static void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index) {
			if (sender == pLoadoutMenu) {
				if(selectedItem == pGiveLoadout) {
					Logger.DebugLog("UI", "Give Loadout Menu Option Selected, Giving Loadout.");
					LoadoutHandler.GiveLoadout();
				}
			}
			else {
				return;
			}
		}
	}
}
