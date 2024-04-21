using System.Collections.Generic;
using Entity;
using Extras;
using Godot;
using Scene;

namespace GameManager;

public partial class PlayerManager() : EntityManager<Player>(GodotFolderPath.MainCharacters, "warrior_player.tscn", "pawn_player.tscn", "archer_player.tscn")
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

  public Player InstantiatePlayerByName(StringName entityFileName, StageLoader stageLoader)
  {
    TryInstantiateAtPosition(entityFileName, stageLoader, SpawnPoint, out playerInstance);

    PlayerCamera = playerInstance.Camera;
    AllPlayers.Add(playerInstance);
    playerInstance.DisplayName = "porra games";
    return playerInstance;
  }
}
