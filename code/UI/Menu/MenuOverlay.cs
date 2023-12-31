using Sandbox;
using Sandbox.UI;
using TSPS2.UI;

namespace TSPS2.Menu;

public class MenuOverlay : Panel
{
	/// <summary>
	/// Current active menu overlay.
	/// </summary>
	public static MenuOverlay CurrentMenu { get; set; }

	public static bool IsActive => CurrentMenu != null;

	public static T Open<T>() where T : MenuOverlay, new()
	{
		var overlay = new T();
		return Open(overlay) as T;
	}

	public static MenuOverlay Open(MenuOverlay overlay)
	{
		CloseActive();

		MainMenu.Current?.FindRootPanel()?.AddChild(overlay);
		CurrentMenu = overlay;
		return CurrentMenu;
	}

	public static void CloseActive()
	{
		CurrentMenu?.Close();
	}

	public void Close()
	{
		if (CurrentMenu == this)
			CurrentMenu = null;

		Delete();
	}
}