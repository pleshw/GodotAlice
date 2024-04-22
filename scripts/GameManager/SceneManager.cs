using Extras;
using System.Linq;
using Godot;
using Scene;

namespace GameManager;

public partial class SceneManager : GameResourceManager<CanvasItem>
{
  public readonly StageLoader StageLoader;

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

  public void SetScene(CanvasItem sceneInstance)
  {
    SetScene(sceneInstance.Name);
  }
}