
using System;
using System.Collections.Generic;
using Entity;
using Godot;

namespace GameManager;

public partial class InputManager : Node2D
{
  public event Action<Key> OnKeyDown;
  public void KeyDownEvent(Key key)
  {
    OnKeyDown?.Invoke(key);
  }

  public event Action<Key, TimeSpan> OnKeyUp;
  public void KeyUpEvent(Key key, TimeSpan heldTime)
  {
    OnKeyUp?.Invoke(key, heldTime);
  }

  public event Action<MouseInputAction> OnMouseButtonDown;
  public void MouseButtonDownEvent(MouseInputAction mouseButton)
  {
    OnMouseButtonDown?.Invoke(mouseButton);
  }

  public event Action<MouseInputAction, Vector2, TimeSpan, HashSet<Node2D>> OnMouseButtonUp;
  public void MouseButtonUpEvent(MouseInputAction mouseButton, Vector2 finalPosition, TimeSpan heldTime, HashSet<Node2D> hoveringEnd)
  {
    OnMouseButtonUp?.Invoke(mouseButton, finalPosition, heldTime, hoveringEnd);
  }

  public event Action<Key, bool, TimeSpan> OnKeyAction;
  public void KeyActionEvent(Key key, bool isRepeating, TimeSpan heldTime)
  {
    OnKeyAction?.Invoke(key, isRepeating, heldTime);
  }

  public event Action<MouseInputAction, bool, TimeSpan, HashSet<Node2D>> OnMouseAction;
  public void MouseActionEvent(MouseInputAction button, bool isRepeating, TimeSpan heldTime, HashSet<Node2D> hovering)
  {
    OnMouseAction?.Invoke(button, isRepeating, heldTime, hovering);
  }

  public event Action OnLeftClick;
  public void LeftClickEvent()
  {
    OnLeftClick?.Invoke();
  }

  public event Action OnRightClick;
  public void RightClickEvent()
  {
    OnRightClick?.Invoke();
  }

  public event Action<MouseInputAction, bool, TimeSpan, HashSet<Node2D>> OnLeftClickAction;
  public void LeftClickActionEvent(MouseInputAction inputInfo, bool isRepeating, TimeSpan heldTime, HashSet<Node2D> hovering)
  {
    OnLeftClickAction?.Invoke(inputInfo, isRepeating, heldTime, hovering);
  }

  public event Action<MouseInputAction, bool, TimeSpan, HashSet<Node2D>> OnRightClickAction;
  public void RightClickActionEvent(MouseInputAction inputInfo, bool isRepeating, TimeSpan heldTime, HashSet<Node2D> hovering)
  {
    OnRightClickAction?.Invoke(inputInfo, isRepeating, heldTime, hovering);
  }
}