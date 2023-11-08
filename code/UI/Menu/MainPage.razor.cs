using Sandbox;
using Sandbox.Localization;
using Sandbox.Menu;
using Sandbox.UI;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TSPS2.UI;

namespace TSPS2.Menu;

public partial class MainPage : Panel
{
    public MainPage()
    {
        BindClass("ingame", () => Game.InGame);
    }

    public void OnClickResumeGame()
    {
        Game.Menu.HideMenu();
    }

    public async Task OnClickBeginTheGame()
    {
        await Game.Menu.StartServerAsync(1, "The Stanley Parable", "softsplit.seriousroom");
    }

    public void OnClickCredits()
    {
        this.Navigate("/credits");
    }

    public void OnClickSettings()
    {
        this.Navigate("/settings");
    }

    public void OnClickQuit()
    {
        if (Game.InGame)
        {
            Game.Menu.LeaveServer("");
        }
        else
        {
            Game.Menu.Close();
        }
    }
}