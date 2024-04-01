using Godot;
using Entity;

namespace Entity;

public partial class Player(Vector2 initialPosition) : Entity(initialPosition)
{
  public Player() : this(Vector2.Zero)
  {
  }

  public override void _Ready()
  {
    movementKeyBind.BindDefaults();
    TeleportToNearestCell(new PlayerMovementInput
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
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    foreach (var key in keysPressed)
    {
      movementKeyBind.Execute(key);
    }

    DefaultMovementProcess(delta, out _);
  }
}
