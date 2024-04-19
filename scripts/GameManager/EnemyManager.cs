using Entity;
using Godot;
using Extras;
using System.Linq;
using System.Collections.Generic;

namespace GameManagers;

public partial class EnemyManager() : EntityManager<Enemy>("res://prefabs/entities/", "archer.tscn")
{
  public List<Enemy> enemies = [];

  public void InstantiateEnemies()
  {
    for (int i = 0; i < 10; ++i)
    {
      int distance = 30 * i;
      TryInstantiateAtPosition(Utils.RandomVector(100, 100, 170 + distance, 170 + distance), out Enemy enemyInstance);
      enemies.Add(enemyInstance);
    }
  }
}
