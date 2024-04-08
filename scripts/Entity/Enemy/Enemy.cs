using Godot;
using Entity;
using System;

namespace Entity;

public partial class Enemy(Vector2 initialPosition) : Entity
{

  private StringName _resourceName = "res://prefabs/player.tscn";

  public override StringName ResourceName
  {
    get => _resourceName;
    set { }
  }

  protected readonly Vector2 _spawnPoint;

  public Vector2 SpawnPoint
  {
    get
    {
      return _spawnPoint;
    }
  }

  public override EntityInventory Inventory { get; set; }

  public Enemy() : this(Vector2.Zero)
  {
    Inventory = new(this);
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
