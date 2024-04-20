using Entity;
using Godot;
using Extras;
using System.Linq;
using System.Collections.Generic;

namespace GameManager;

public partial class EnemyManager() : EntityManager<Enemy>(GodotFolderPath.Entities, "archer.tscn")
{
  public List<Enemy> enemies = [];

  public void InstantiateEnemies()
  {
    for (int i = 0; i < 10; ++i)
    {
      int distance = 30 * i;
      TryInstantiateAtPosition(Utils.GetRandomVector(100, 100, 170 + distance, 170 + distance), out Enemy enemyInstance);
      enemies.Add(enemyInstance);
    }
  }
}
