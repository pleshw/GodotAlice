using System.Collections.Generic;
using Entity;
using Extras;
using Godot;

namespace GameManager;

public partial class PlayerManager() : EntityManager<Player>(GodotFolderPath.Entities, "default_player.tscn")
{
  public Camera2D GlobalCamera;
  public Camera2D PlayerCamera;

  public static readonly List<Player> AllPlayers = [];

  public Player playerInstance;

  protected Vector2 SpawnPoint = new()
  {
    X = 0,
    Y = 0
  };

  public void InstantiatePlayer()
  {
    TryInstantiateAtPosition(SpawnPoint, out playerInstance);
    playerInstance.movementKeyBinds.BindDefaults();
    playerInstance.uiKeyBinds.BindDefaults();

    PlayerCamera = playerInstance.Camera;
    AllPlayers.Add(playerInstance);
    playerInstance.DisplayName = "porra games";
  }
}
