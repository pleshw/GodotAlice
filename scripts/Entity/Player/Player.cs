using Godot;
using Animation;
using System.Collections.Generic;
using System;

namespace Entity;

public partial class Player(Vector2 initialPosition) : AnimatedEntity(initialPosition)
{

  [Export]
  private StringName _resourceName = "res://prefabs/entities/player.tscn";
  private readonly Vector2 initialPosition = initialPosition;

  public override StringName ResourceName
  {
    get => _resourceName;
    set { }
  }

  public override EntityInventoryBase Inventory { get; set; }

  public Player() : this(Vector2.Zero)
  {
    Inventory = new(this);
  }

  public override void _Ready()
  {
    base._Ready();

    movementKeyBinds.BindDefaults();
    uiKeyBinds.BindDefaults();

    ReadyToSpawn = true;

    MovementController.TeleportToNearestCell(new EntityMovementInput
    {
      Position = initialPosition,
      IsRunning = false,
      ForceMovementState = true,
      MovementState = MOVEMENT_STATE.IDLE,
    });

    // DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
  }

  private readonly Dictionary<KeyList, DateTime> keysPressed = [];
  private readonly Dictionary<KeyList, TimeSpan> keysHeldDuration = [];
  private readonly Dictionary<KeyList, bool> keysCommandExecuted = [];


  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey inputEventKey)
    {
      KeyList key = new(inputEventKey.Device, inputEventKey.Keycode);
      if (inputEventKey.Pressed)
      {
        if (!keysPressed.ContainsKey(key))
        {
          keysPressed.Add(key, DateTime.Now);
          keysCommandExecuted[key] = false;
        }
      }
      else
      {
        if (keysPressed.TryGetValue(key, out DateTime value))
        {
          TimeSpan heldDuration = DateTime.Now - value;
          keysHeldDuration[key] = heldDuration;
          keysPressed.Remove(key);
          keysCommandExecuted.Remove(key);
          // GD.Print("Key ", key.KeyCode, " held for ", heldDuration.TotalSeconds, " seconds.");
        }
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

    foreach (var keyAndTime in keysPressed)
    {
      bool isKeyRepeating = keysCommandExecuted[keyAndTime.Key];
      Key keyPressed = keyAndTime.Key.KeyCode;
      TimeSpan timeHeld = DateTime.Now - keyAndTime.Value;

      movementKeyBinds.Execute(keyPressed, isKeyRepeating);
      uiKeyBinds.Execute(keyPressed, isKeyRepeating);

      keysCommandExecuted[keyAndTime.Key] = true;
    }
  }
}
