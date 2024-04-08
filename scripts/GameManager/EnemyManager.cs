using Entity;
using Godot;

namespace GameManagers;

public partial class EnemyManager() : EntityManager<Enemy>("")
{
  protected Vector2 playerSpawnPoint = new()
  {
    X = 0,
    Y = 0
  };

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    base._Ready();
  }
}
