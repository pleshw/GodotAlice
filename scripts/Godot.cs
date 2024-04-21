using GameManager;
using Godot;

namespace Extras;

public static class GodotFolderPath
{
  public static readonly StringName Resources;
  public static readonly StringName Scenes;
  public static readonly StringName Stages;
  public static readonly StringName SceneMenus;
  public static readonly StringName Prefabs;
  public static readonly StringName Entities;
  public static readonly StringName UIPrefabs;
  public static readonly StringName CursorsPrefabs;
  public static readonly StringName MainCharacters;

  static GodotFolderPath()
  {
    Resources = "res://";
    Scenes = Resources + "scenes/";
    Stages = Scenes + "stages/";
    SceneMenus = Scenes + "menus/";
    Prefabs = Resources + "prefabs/";
    Entities = Prefabs + "entities/";
    UIPrefabs = Prefabs + "ui/";
    CursorsPrefabs = UIPrefabs + "cursors/";
    MainCharacters = Entities + "main_characters/";
  }
}


public static partial class GodotFileName
{
  public static class MainCharacters
  {
    public static readonly StringName Archer;
    public static readonly StringName Pawn;
    public static readonly StringName Warrior;

    static MainCharacters()
    {
      Archer = "archer_player.tscn";
      Pawn = "pawn_player.tscn";
      Warrior = "warrior_player.tscn";
    }
  }
}

public static partial class GodotFileName
{
  public static class UI
  {
    public static readonly StringName DefaultCursor;

    static UI()
    {
      DefaultCursor = "default_cursor.tscn";
    }
  }
}

public static partial class GodotFileName
{
  public static class Menus
  {
    public static readonly StringName MainMenu;
    public static readonly StringName SingleCharacterMenu;
    public static readonly StringName CoopCharacterMenu;
    public static readonly StringName MultiplayerConnectionMenu;

    static Menus()
    {
      MainMenu = "main_menu.tscn";
      SingleCharacterMenu = "select_character_menu.tscn";
      CoopCharacterMenu = "coop_character_menu.tscn";
      MultiplayerConnectionMenu = "coop_network_options.tscn";
    }
  }
}

public static partial class GodotFilePath
{
  public static class UI
  {
    public static readonly StringName DefaultCursor;

    static UI()
    {
      DefaultCursor = GodotFolderPath.CursorsPrefabs + GodotFileName.UI.DefaultCursor;
    }
  }
}

public static partial class GodotFilePath
{
  public static class Menus
  {
    public static readonly StringName MainMenu;
    public static readonly StringName SingleCharacterMenu;
    public static readonly StringName CoopCharacterMenu;
    public static readonly StringName MultiplayerConnectionMenu;

    static Menus()
    {
      MainMenu = GodotFolderPath.Stages + GodotFileName.Menus.MainMenu;
      SingleCharacterMenu = GodotFolderPath.SceneMenus + GodotFileName.Menus.SingleCharacterMenu;
      CoopCharacterMenu = GodotFolderPath.SceneMenus + GodotFileName.Menus.CoopCharacterMenu;
      MultiplayerConnectionMenu = GodotFolderPath.SceneMenus + GodotFileName.Menus.MultiplayerConnectionMenu;
    }
  }
}

public static partial class GodotFilePath
{
  public static class Players
  {
    public static readonly StringName Archer;
    public static readonly StringName Pawn;
    public static readonly StringName Warrior;

    static Players()
    {
      Archer = GodotFolderPath.MainCharacters + GodotFileName.MainCharacters.Archer;
      Pawn = GodotFolderPath.MainCharacters + GodotFileName.MainCharacters.Pawn;
      Warrior = GodotFolderPath.MainCharacters + GodotFileName.MainCharacters.Warrior;
    }
  }
}