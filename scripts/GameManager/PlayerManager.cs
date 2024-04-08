using Entity;
using Godot;

namespace GameManagers;

public partial class PlayerManager() : EntityManager<Player>("res://prefabs/entities/player.tscn")
{

  public Player playerInstance;

  protected Vector2 playerSpawnPoint = new()
  {
    X = 0,
    Y = 0
  };

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    base._Ready();

    TrySpawnAtPosition(playerSpawnPoint, out playerInstance, 1);
  }
}
