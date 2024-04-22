using Extras;
using System.Linq;
using Godot;
using Scene;
using UI;

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

  public SceneManager() : base()
  {
    StageLoader = new(this);
  }

  public void SetScene(StringName sceneInstanceName)
  {
    foreach (var item in Scenes)
    {
      if (item.Key == sceneInstanceName)
      {
        item.Value.Show();
      }
      else
      {
        item.Value.Hide();
      }
    }
  }

  public void HideScenes()
  {
    foreach (var item in Scenes)
    {
      item.Value.Hide();
    }
  }

  public void SetScene(CanvasItem sceneInstance)
  {
    HideScenes();
    sceneInstance.Show();
  }

  public void AddScenesToRootDeferred()
  {
    CallDeferred(nameof(AddScenesToRoot));
  }

  public void AddScenesToRoot()
  {
    foreach (var item in Scenes)
    {
      if (item.Value.GetParent().GetParent() != GetTree().Root)
      {
        GetTree().Root.AddChild(item.Value.GetParent());
      }
    }
  }
}