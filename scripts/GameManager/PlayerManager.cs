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


  /// <summary>
  /// 
  /// </summary>
  /// <param name="entitySceneName">The name of the player scene at  the main_characters folder </param>
  /// <param name="playerDisplayName"></param>
  /// <returns></returns>
  public Player GetPlayerInstance(StringName entitySceneName, string playerDisplayName = "porra games")
  {
    Player playerInstance = GetEntityInstance(entitySceneName);

    PlayerCamera = playerInstance.Camera;

    if (AllPlayers.Count == 0)
    {
      MainPlayerInstance = playerInstance;
    }

    AllPlayers.Add(playerInstance);
    playerInstance.DisplayName = playerDisplayName;
    return playerInstance;
  }
}
