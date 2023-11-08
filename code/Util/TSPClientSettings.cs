using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TSPS2;

internal class TSPClientSettings
{
	protected static List<string> GroupOrder = new List<string>()
	{
		GROUP_GENERAL_GAMEPLAY,
		GROUP_GENERAL_PRESENTATION,
		GROUP_GENERAL_ACCESSIBILITY,
        GROUP_AUDIO_VOLUMELEVELS,
        GROUP_CONTROLS_CONTROLSCHEME
	};

	public const string GROUP_GENERAL_GAMEPLAY = "#GameSettings.Group.General.Gameplay";
	public const string GROUP_GENERAL_PRESENTATION = "#GameSettings.Group.General.Presentation";
	public const string GROUP_GENERAL_ACCESSIBILITY = "#GameSettings.Group.General.Accessibility";
    public const string GROUP_AUDIO_VOLUMELEVELS = "#GameSettings.Group.Audio.VolumeLevels";
    public const string GROUP_CONTROLS_CONTROLSCHEME = "#GameSettings.Group.Controls.ControlScheme";
	const string COOKIE_NAME = "tsps2.clientsettings";

	[Display( Name = "#GameSettings.Setting.ControllerVibration", Description = "#GameSettings.Description.ControllerVibration", GroupName = GROUP_GENERAL_GAMEPLAY )]
	public bool ControllerVibration { get; set; } = true;

    [Display( Name = "#GameSettings.Setting.ShowSubtitles", Description = "#GameSettings.Description.ShowSubtitles", GroupName = GROUP_GENERAL_PRESENTATION )]
	public bool ShowSubtitles { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.SubtitleSize", Description = "#GameSettings.Description.SubtitleSize", GroupName = GROUP_GENERAL_PRESENTATION )]
	public string SubtitleSize { get; set; } = "Medium";

    [Display( Name = "#GameSettings.Setting.SubtitleBackgroundOpacity", Description = "#GameSettings.Description.SubtitleBackgroundOpacity", GroupName = GROUP_GENERAL_PRESENTATION )]
    [MinMax( 0, 100 )]
	public int SubtitleBackgroundOpacity { get; set; } = 80;

    [Display( Name = "#GameSettings.Setting.ShowReticle", Description = "#GameSettings.Description.ShowReticle", GroupName = GROUP_GENERAL_PRESENTATION )]
	public bool ShowReticle { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.ReticleType", Description = "#GameSettings.Description.ReticleType", GroupName = GROUP_GENERAL_PRESENTATION )]
	public string ReticleType { get; set; } = "Dot";

    [Display( Name = "#GameSettings.Setting.ReticleSize", Description = "#GameSettings.Description.ReticleSize", GroupName = GROUP_GENERAL_PRESENTATION )]
    [MinMax( 20, 540 )]
	public int ReticleSize { get; set; } = 40;

    [Display( Name = "#GameSettings.Setting.ReticleOpacity", Description = "#GameSettings.Description.ReticleOpacity", GroupName = GROUP_GENERAL_PRESENTATION )]
    [MinMax( 0, 100 )]
	public int ReticleOpacity { get; set; } = 100;

    [Display( Name = "#GameSettings.Setting.ShowTranslationLabels", Description = "#GameSettings.Description.ShowTranslationLabels", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool ShowTranslationLabels { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.TranslationLabelSize", Description = "#GameSettings.Description.TranslationLabelSize", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public string TranslationLabelSize { get; set; } = "Medium";

    [Display( Name = "#GameSettings.Setting.TranslationLabelBackgroundOpacity", Description = "#GameSettings.Description.TranslationLabelBackgroundOpacity", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
    [MinMax( 0, 100 )]
	public int TranslationLabelBackgroundOpacity { get; set; } = 80;

    [Display( Name = "#GameSettings.Setting.ShowColorLabels", Description = "#GameSettings.Description.ShowColorLabels", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool ShowColorLabels { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.ShowContentWarnings", Description = "#GameSettings.Description.ShowContentWarnings", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool ShowContentWarnings { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.ReduceMotion", Description = "#GameSettings.Description.ReduceMotion", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool ReduceMotion { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.LowDexterityMode", Description = "#GameSettings.Description.LowDexterityMode", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool LowDexterityMode { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.AutoWalk", Description = "#GameSettings.Description.AutoWalk", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool AutoWalk { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.ToggleAutoWalk", Description = "#GameSettings.Description.ToggleAutoWalk", GroupName = GROUP_GENERAL_ACCESSIBILITY )]
	public bool ToggleAutoWalk { get; set; } = false;

    [Display( Name = "#GameSettings.Setting.NarratorVolume", Description = "#GameSettings.Description.NarratorVolume", GroupName = GROUP_AUDIO_VOLUMELEVELS )]
    [MinMax( 0, 100 )]
	public int NarratorVolume { get; set; } = 100;

    [Display( Name = "#GameSettings.Setting.MusicVolume", Description = "#GameSettings.Description.MusicVolume", GroupName = GROUP_AUDIO_VOLUMELEVELS )]
    [MinMax( 0, 100 )]
	public int MusicVolume { get; set; } = 100;

    [Display( Name = "#GameSettings.Setting.SoundEffectsVolume", Description = "#GameSettings.Description.SoundEffectsVolume", GroupName = GROUP_AUDIO_VOLUMELEVELS )]
    [MinMax( 0, 100 )]
	public int SoundEffectsVolume { get; set; } = 100;

    [Display( Name = "#GameSettings.Setting.SimplifiedControls", Description = "#GameSettings.Description.SimplifiedControls", GroupName = GROUP_CONTROLS_CONTROLSCHEME )]
	public bool SimplifiedControls { get; set; } = false;

	public static int GetGroupOrder( string group )
	{
		int index = GroupOrder.FindIndex( x => x == group );
		if ( index < 0 ) index = int.MaxValue;
		return index;
	}

	private static TSPClientSettings current;
	public static TSPClientSettings Current
	{
		get
		{
			if ( current == null )
			{
				current = Cookie.Get<TSPClientSettings>( COOKIE_NAME, new() );
			}

			return current;
		}
	}

	public void Save() => Cookie.Set( COOKIE_NAME, this );

	public static void Reset()
	{
		current = new();
		current.Save();
	}
}