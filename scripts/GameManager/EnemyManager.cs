using Entity;
using Godot;
using Extras;
using System.Linq;
using System.Collections.Generic;

namespace GameManager;

public partial class EnemyManager() : EntityManager<Enemy>(GodotFolderPath.Entities, "archer.tscn")
{
  public List<Enemy> enemies = [];

}
