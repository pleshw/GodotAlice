using Entity;
using Godot;
using Extras;
using System.Linq;

namespace GameManagers;

public partial class EnemyManager() : EntityManager<Enemy>("res://prefabs/entities/", "archer.tscn")
{
  public Enemy[] enemies;

  public override void _Ready()
  {
    base._Ready();

    CallDeferred(nameof(InstantiateEnemies));
  }

  public void InstantiateEnemies()
  {
    for (int i = 0; i < 10; ++i)
    {
      int distance = 30 * i;
      TryInstantiateAtPosition(Utils.RandomVector(100, 100, 170 + distance, 170 + distance), out Enemy enemyInstance);
    }
  }
}
