
using Godot;
using Extras;
using System.Collections.Generic;

namespace GameManager;

public partial class AudioManager() : GameResourceManager<AudioStreamPlayer2D>()
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

  public override void _Ready()
  {
    base._Ready();
    Preload(MenuActionFilePaths);

    CreateInstance<AudioStreamPlayer2D>(GodotFilePath.Sounds.MenuHoverAction, "MenuButtonHover");
    CreateInstance<AudioStreamPlayer2D>(GodotFilePath.Sounds.MenuConfirmAction, "MenuButtonConfirm");

    AddScenesToRootDeferred();
  }
}