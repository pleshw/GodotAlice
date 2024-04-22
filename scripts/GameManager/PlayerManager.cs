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

  public StringName PlayerStage;

  public Player MainPlayerInstance;

  public Player InstantiatePlayerByName(StringName entityFileName, StageLoader stageLoader)
  {
    Player playerInstance = GetEntityInstanceByName(entityFileName, stageLoader);

    PlayerCamera = playerInstance.Camera;
    if (AllPlayers.Count == 0)
    {
      MainPlayerInstance = playerInstance;
    }
    AllPlayers.Add(playerInstance);
    playerInstance.DisplayName = "porra games";
    return playerInstance;
  }
}
