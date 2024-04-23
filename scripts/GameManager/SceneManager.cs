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

  public AudioManager AudioManager
  {
    get
    {
      return GetNode<AudioManager>("/root/AudioManager");
    }
  }

  public CanvasLayer OverCanvasLayer;

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

    OverCanvasLayer = LoadScene<CanvasLayer>(GodotFilePath.Menus.OverCanvasLayer, "OverCanvasLayer");
    BackSceneButton = CreateInstance<Button>(GodotFilePath.Menus.BackSceneButton, "BackSceneButton");

    BackSceneButton.Pressed += () =>
    {
      Back();
      AudioManager["MenuButtonConfirm"].Play();
    };

    OverCanvasLayer.AddChild(BackSceneButton);
    AddScenesToRootDeferred([OverCanvasLayer]);

    OnSceneStackChange += () =>
    {
      if (CanGoBack)
      {
        BackSceneButton.Show();
      }
      else
      {
        BackSceneButton.Hide();
      }
    };
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
    HideScenes();
    sceneInstance.Show();
    sceneInstance.Visible = true;

    if (CurrentScene is not null)
    {
      SceneStack.Push(CurrentScene);
    }

    CurrentScene = sceneInstance;
    SceneStackChangeEvent();
  }

  private void SetSceneWithoutModifyingStack(CanvasItem sceneInstance)
  {
    HideScenes();
    sceneInstance.Show();
    sceneInstance.Visible = true;
    CurrentScene = sceneInstance;
  }

  public bool CanGoBack
  {
    get
    {
      return SceneStack.Count > 0;
    }
  }

  public void Back()
  {
    if (!CanGoBack)
    {
      return;
    }

    SetSceneWithoutModifyingStack(SceneStack.Pop());

    SceneStackChangeEvent();
  }

  public event Action OnSceneStackChange;
  public void SceneStackChangeEvent()
  {
    OnSceneStackChange?.Invoke();
  }
}