using Entity;
using Extras;
using Godot;

namespace GameManagers;

public partial class PlayerManager() : EntityManager<Player>("res://prefabs/entities/", "pawn.tscn")
{
  public Camera2D GlobalCamera;

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
    GlobalCamera = MainScene.GetNode<Camera2D>("GlobalCamera");
    CallDeferred(nameof(InstantiatePlayer));
  }

  public void InstantiatePlayer()
  {
    Entity.Entity.GlobalCamera = GlobalCamera;

    TryInstantiateAtPosition(playerSpawnPoint, out playerInstance);
    playerInstance.movementKeyBinds.BindDefaults();
    playerInstance.uiKeyBinds.BindDefaults();
  }
}
