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


  public Enemy() : this(Vector2.Zero)
  {
  }

  public override void _Ready()
  {
    base._Ready();

    movementKeyBind.BindArrows();
    MovementController.TeleportToNearestCell(new EntityMovementInput
    {
      Position = initialPosition,
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.IDLE,
    });
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey inputEventKey)
    {
      if (inputEventKey.Pressed)
      {
        keysPressed.Add(inputEventKey.Keycode);
      }
      else
      {
        keysPressed.Remove(inputEventKey.Keycode);
      }
    }

    if (@event is InputEventMouseButton inputEventClick)
    {
      if (inputEventClick.Pressed)
      {
        var clickEventType = inputEventClick.ButtonIndex;
        if (clickEventType == MouseButton.Left)
        {
          // Camera.MakeCurrent();
        }
      }
    }
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    base._Process(delta);
  }
}
