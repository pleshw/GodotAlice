using Godot;

namespace Extras;

public static class GodotFolderPath
{
  public static readonly StringName Resources;
  public static readonly StringName Scenes;
  public static readonly StringName Places;
  public static readonly StringName SceneMenus;
  public static readonly StringName Prefabs;
  public static readonly StringName Entities;

  static GodotFolderPath()
  {
    Resources = "res://";
    Scenes = Resources + "scenes/";
    Places = Scenes + "places/";
    SceneMenus = Scenes + "menus/";
    Prefabs = Resources + "prefabs/";
    Entities = Prefabs + "entities/";
  }
}

public static class GodotFilePath
{
  public static readonly StringName MainMenu;
  public static readonly StringName CreateCharacterMenu;

  static GodotFilePath()
  {
    MainMenu = GodotFolderPath.Places + "main_menu.tscn";
    CreateCharacterMenu = GodotFolderPath.SceneMenus + "create_character_menu.tscn";
  }
}