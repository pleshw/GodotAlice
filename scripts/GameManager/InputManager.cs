
using System;
using System.Collections.Generic;
using Entity;
using Godot;

namespace GameManagers;

public partial class InputManager : Node2D
{
  private readonly Dictionary<KeyList, DateTime> keysPressed = [];
  private readonly Dictionary<KeyList, TimeSpan> keysHeldDuration = [];
  private readonly Dictionary<KeyList, bool> keysCommandExecuted = [];

  private readonly Dictionary<MouseInputList, DateTime> mouseButtonPressed = [];
  private readonly Dictionary<MouseInputList, TimeSpan> mouseButtonHeldDuration = [];
  private readonly Dictionary<MouseInputList, bool> mouseButtonCommandExecuted = [];

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

    foreach (var buttonAndTime in mouseButtonPressed)
    {
      bool isRepeating = mouseButtonCommandExecuted[buttonAndTime.Key];
      MouseButton button = buttonAndTime.Key.Button;
      TimeSpan heldTime = DateTime.Now - buttonAndTime.Value;

      MouseActionEvent(button, isRepeating, heldTime);
      switch (button)
      {
        case MouseButton.Right:
          RightClickActionEvent(isRepeating, heldTime);
          break;
        case MouseButton.Left:
          LeftClickActionEvent(isRepeating, heldTime);
          break;
      }

      mouseButtonCommandExecuted[buttonAndTime.Key] = true;
    }
  }

  /// <summary>
  /// Stores the events and if it is the first time being pressed, if it is released.
  /// </summary>
  /// <param name="inputEventClick"></param>
  private void InputMouseHandler(InputEventMouseButton inputEventClick)
  {
    MouseInputList button = new(inputEventClick.ButtonIndex);
    if (inputEventClick.Pressed)
    {
      if (!mouseButtonPressed.ContainsKey(button))
      {
        mouseButtonPressed.Add(button, DateTime.Now);
        mouseButtonCommandExecuted[button] = false;
        MouseButtonDownEvent(inputEventClick.ButtonIndex);
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
      if (mouseButtonPressed.TryGetValue(button, out DateTime value))
      {
        TimeSpan heldDuration = DateTime.Now - value;
        mouseButtonHeldDuration[button] = heldDuration;
        mouseButtonPressed.Remove(button);
        mouseButtonCommandExecuted.Remove(button);
        MouseButtonUpEvent(inputEventClick.ButtonIndex, heldDuration);
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

}
