using Godot;
using Entity;
using System;

namespace Entity;

public partial class Enemy(Vector2 initialPosition) : Entity
{
  protected readonly Vector2 _spawnPoint;

  public Vector2 SpawnPoint
  {
    get
    {
      return _spawnPoint;
    }
  }

  public override EntityInventoryBase BaseInventory { get; set; }

  public Enemy() : this(Vector2.Zero)
  {
    BaseInventory = new(this);
  }

  public override void _Ready()
  {
    base._Ready();

    movementKeyBinds.BindArrows();
    MovementController.TeleportToNearestCell(new EntityMovementInput
    {
      Position = initialPosition,
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.IDLE,
    });
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    base._Process(delta);
  }
}
