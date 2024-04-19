using Entity;
using Extras;
using Godot;

namespace GameManagers;

public partial class PlayerManager() : EntityManager<Player>("res://prefabs/entities/", "pawn.tscn")
{
  public Camera2D GlobalCamera;
  public Camera2D PlayerCamera;

  public Player playerInstance;

  protected Vector2 SpawnPoint = new()
  {
    X = 0,
    Y = 0
  };

  public void InstantiatePlayer()
  {
    Entity.Entity.GlobalCamera = GlobalCamera;

    TryInstantiateAtPosition(SpawnPoint, out playerInstance);
    playerInstance.movementKeyBinds.BindDefaults();
    playerInstance.uiKeyBinds.BindDefaults();

    playerInstance.Ready += () =>
    {
      PlayerCamera = playerInstance.Camera;
    };
  }
}
