
using Godot;
using Extras;
using System.Collections.Generic;

namespace GameManager;

public partial class AudioManager : GameResourceManager<AudioStreamPlayer2D>
{
  public SceneManager SceneManager
  {
    get
    {
      return GetNode<SceneManager>("/root/SceneManager");
    }
  }

  private static StringName[] MenuActionFilePaths
  {
    get
    {
      return [
        GodotFilePath.Sounds.MenuConfirmAction,
        GodotFilePath.Sounds.MenuHoverAction,
      ];
    }
  }

  public AudioManager() : base()
  {
    Preload(MenuActionFilePaths);
  }

  public override void _Ready()
  {
    base._Ready();

    CreateInstance<AudioStreamPlayer2D>(GodotFilePath.Sounds.MenuHoverAction, "MenuButtonHover");
    CreateInstance<AudioStreamPlayer2D>(GodotFilePath.Sounds.MenuConfirmAction, "MenuButtonConfirm");

    AddScenesToRootDeferred();
  }
}