using Extras;
using System.Linq;
using Godot;
using Scene;
using UI;
using System.Collections.Generic;
using System;

namespace GameManager;

public partial class SceneManager : GameResourceManager<CanvasItem>
{
  public readonly StageLoader StageLoader;

  /// <summary>
  /// Main Menu is the main scene
  /// </summary>
  public MainMenu MainMenu
  {
    get
    {
      return GetTree().Root.GetNode<MainMenu>("MainMenu");
    }
  }

  public Button BackSceneButton;

  public CanvasItem CurrentScene;

  public readonly Stack<CanvasItem> SceneStack = [];

  public SceneManager() : base()
  {
    StageLoader = new(this);
    Preload([GodotFilePath.Menus.BackSceneButton]);
  }

  public override void _Ready()
  {
    base._Ready();

    BackSceneButton = CreateInstance<Button>(GodotFilePath.Menus.BackSceneButton, "BackSceneButton");

    BackSceneButton.TopLevel = true;
    BackSceneButton.ZIndex = 100;
    BackSceneButton.Visible = true;

    AddScenesToRootDeferred();
  }

  public void HideScenes()
  {
    MainMenu.Hide();
    foreach (var item in Scenes)
    {
      item.Value.Hide();
    }
  }

  public void SetScene(CanvasItem sceneInstance)
  {
    SceneStack.Push(CurrentScene);
    SceneStackChangeEvent();
    HideScenes();
    sceneInstance.Show();
    sceneInstance.Visible = true;
    CurrentScene = sceneInstance;
  }

  public bool CanGoBack
  {
    get
    {
      return SceneStack.Count > 1;
    }
  }

  public void Back()
  {
    if (!CanGoBack)
    {
      return;
    }

    SetScene(SceneStack.Pop());
    SceneStackChangeEvent();
  }

  public event Action OnSceneStackChange;
  public void SceneStackChangeEvent()
  {
    OnSceneStackChange?.Invoke();
  }
}