using System.Collections.Generic;
using Godot;

namespace Entity;


public record struct EntityMovementInput
{
  public required Vector2 Position;
  public required bool IsRunning;
  public required bool ForceStateChange;
  public required EntityGameState GameState;
}

public static class EntityMovementConstraints
{
  public readonly static Dictionary<MOVEMENT_STATE, int> MovementStatesPriorities = new()
  {
    {MOVEMENT_STATE.IDLE, 1},
    {MOVEMENT_STATE.WALKING, 2},
    {MOVEMENT_STATE.RUNNING, 3},
    {MOVEMENT_STATE.CRAWLING, 3},
    {MOVEMENT_STATE.GRABBING, 3},
    {MOVEMENT_STATE.DASHING, 4},
    {MOVEMENT_STATE.FALLING, 4},
    {MOVEMENT_STATE.STOPPING, 5},
  };
}