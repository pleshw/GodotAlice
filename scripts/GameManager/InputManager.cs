
using System;
using System.Collections.Generic;
using Entity;
using Extras;
using Godot;
using Scene;
using UI;

namespace GameManager;

public partial class InputManager() : GameResourceManager<GameCursor>(GodotFolderPath.CursorsPrefabs, GodotFileName.UI.DefaultCursor)
{
  public GameCursor Cursor;

  public SceneManager SceneManager
  {
    get
    {
      return GetNode<SceneManager>("/root/SceneManager");
    }
  }

  public readonly HashSet<Entity.Entity> ListenCollisionSet = [];
  public readonly HashSet<Node2D> Hovering = [];

  private readonly Dictionary<KeyList, DateTime> keysPressed = [];
  private readonly Dictionary<KeyList, TimeSpan> keysHeldDuration = [];
  private readonly Dictionary<KeyList, bool> keysCommandExecuted = [];

  private readonly Dictionary<MouseInputAction, DateTime> mouseButtonPressed = [];
  private readonly Dictionary<MouseInputAction, TimeSpan> mouseButtonHeldDuration = [];
  private readonly Dictionary<MouseInputAction, bool> mouseButtonCommandExecuted = [];

  public override void _Ready()
  {
    base._Ready();
    Cursor = CreateInstance<GameCursor>(GodotFileName.UI.DefaultCursor, "MainCursor");
    CallDeferred(nameof(SetupCursor));
  }

  public void SetupCursor()
  {
    Cursor.CollisionArea.AreaEntered += (Area2D area) =>
    {
      if (area.GetParent() is Entity.Entity entity)
      {
        Hovering.Add(entity);
        entity.MouseIn();
        // GD.Print("Hovered: ", entity.Name);
      }
    };

    Cursor.CollisionArea.AreaExited += (Area2D area) =>
    {
      if (area.GetParent() is Entity.Entity entity)
      {
        Hovering.Remove(entity);
        entity.MouseOut();
        // GD.Print("Hover Out: ", entity.Name);
      }
    };

    Input.MouseMode = Input.MouseModeEnum.Hidden;

    GetTree().Root.AddChild(Cursor);
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey inputEventKey)
    {
      InputKeyHandler(inputEventKey);
    }

    if (@event is InputEventMouseButton inputEventClick)
    {
      InputMouseHandler(inputEventClick);
    }
  }

  public override void _Process(double delta)
  {
    base._Process(delta);

    foreach (var keyAndTime in keysPressed)
    {
      bool isRepeating = keysCommandExecuted[keyAndTime.Key];
      Key keyPressed = keyAndTime.Key.KeyCode;
      TimeSpan timeHeld = DateTime.Now - keyAndTime.Value;

      KeyActionEvent(keyPressed, isRepeating, timeHeld);

      keysCommandExecuted[keyAndTime.Key] = true;
    }

    foreach (var mouseInputEvent in mouseButtonPressed)
    {
      bool isRepeating = mouseButtonCommandExecuted[mouseInputEvent.Key];
      TimeSpan heldTime = DateTime.Now - mouseInputEvent.Value;

      MouseActionEvent(mouseInputEvent.Key, isRepeating, heldTime, Hovering);
      switch (mouseInputEvent.Key.Event.ButtonIndex)
      {
        case MouseButton.Right:
          RightClickActionEvent(mouseInputEvent.Key, isRepeating, heldTime, Hovering);
          break;
        case MouseButton.Left:
          LeftClickActionEvent(mouseInputEvent.Key, isRepeating, heldTime, Hovering);
          break;
      }

      mouseButtonCommandExecuted[mouseInputEvent.Key] = true;
    }
  }

  /// <summary>
  /// Stores the events and if it is the first time being pressed, if it is released.
  /// </summary>
  /// <param name="inputEventClick"></param>
  private void InputMouseHandler(InputEventMouseButton inputEventClick)
  {
    MouseInputAction inputEvent = new(inputEventClick, inputEventClick.Position, Hovering);
    if (inputEventClick.Pressed)
    {
      if (!mouseButtonPressed.ContainsKey(inputEvent))
      {
        mouseButtonPressed.Add(inputEvent, DateTime.Now);
        mouseButtonCommandExecuted[inputEvent] = false;
        MouseButtonDownEvent(inputEvent);
        switch (inputEventClick.ButtonIndex)
        {
          case MouseButton.Right:
            RightClickEvent();
            break;
          case MouseButton.Left:
            LeftClickEvent();
            break;
        }
      }
    }
    else
    {
      if (mouseButtonPressed.TryGetValue(inputEvent, out DateTime value))
      {
        TimeSpan heldDuration = DateTime.Now - value;
        mouseButtonHeldDuration[inputEvent] = heldDuration;
        mouseButtonPressed.Remove(inputEvent);
        mouseButtonCommandExecuted.Remove(inputEvent);
        MouseButtonUpEvent(inputEvent, inputEventClick.Position, heldDuration, Hovering);
      }
    }
  }

  public void InputKeyHandler(InputEventKey inputEventKey)
  {
    KeyList key = new(inputEventKey.Device, inputEventKey.Keycode);
    if (inputEventKey.Pressed)
    {
      if (!keysPressed.ContainsKey(key))
      {
        keysPressed.Add(key, DateTime.Now);
        keysCommandExecuted[key] = false;
        KeyDownEvent(inputEventKey.Keycode);
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
        KeyUpEvent(inputEventKey.Keycode, heldDuration);
      }
    }
  }

  public void ListenTo(Entity.Entity node)
  {
    ListenCollisionSet.Add(node);
  }
}
