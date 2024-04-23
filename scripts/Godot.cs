using GameManager;
using Godot;

namespace Extras;

public static class GodotFolderPath
{
  public static readonly StringName Resources;
  public static readonly StringName Scenes;
  public static readonly StringName Stages;
  public static readonly StringName Assets;
  public static readonly StringName Sounds;
  public static readonly StringName SceneMenus;
  public static readonly StringName Prefabs;
  public static readonly StringName Entities;
  public static readonly StringName UIPrefabs;
  public static readonly StringName MenuPrefabs;
  public static readonly StringName CursorsPrefabs;
  public static readonly StringName MainCharacters;

  static GodotFolderPath()
  {
    Resources = "res://";
    Assets = Resources + "assets/";
    Scenes = Resources + "scenes/";
    SceneMenus = Scenes + "menus/";
    Stages = Scenes + "stages/";
    Prefabs = Resources + "prefabs/";
    Sounds = Prefabs + "sounds/";
    Entities = Prefabs + "entities/";
    UIPrefabs = Prefabs + "ui/";
    MenuPrefabs = UIPrefabs + "menu/";
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
    public static readonly StringName BackSceneButton;

    public static readonly StringName OverCanvasLayer;

    static Menus()
    {
      OverCanvasLayer = "over_canvas_layer.tscn";
      BackSceneButton = "back_scene_button.tscn";
      MainMenu = "main_menu.tscn";
      SingleCharacterMenu = "select_character_menu.tscn";
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
    public static readonly StringName BackSceneButton;
    public static readonly StringName OverCanvasLayer;

    static Menus()
    {
      OverCanvasLayer = GodotFolderPath.MenuPrefabs + GodotFileName.Menus.OverCanvasLayer;
      BackSceneButton = GodotFolderPath.MenuPrefabs + GodotFileName.Menus.BackSceneButton;
      MainMenu = GodotFolderPath.Stages + GodotFileName.Menus.MainMenu;
      SingleCharacterMenu = GodotFolderPath.SceneMenus + GodotFileName.Menus.SingleCharacterMenu;
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


public static partial class GodotFileName
{
  public static class Sounds
  {
    public static readonly StringName MenuHoverAction;
    public static readonly StringName MenuMajorAction;
    static Sounds()
    {
      MenuHoverAction = "menu_hover_action.tscn";
      MenuMajorAction = "menu_confirm_action.tscn";
    }
  }
}

public static partial class GodotFilePath
{
  public static class Sounds
  {
    public static readonly StringName MenuHoverAction;
    public static readonly StringName MenuConfirmAction;
    static Sounds()
    {
      MenuHoverAction = GodotFolderPath.Sounds + GodotFileName.Sounds.MenuHoverAction;
      MenuConfirmAction = GodotFolderPath.Sounds + GodotFileName.Sounds.MenuMajorAction;
    }
  }
}