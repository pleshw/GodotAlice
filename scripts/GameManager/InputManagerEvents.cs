
using System;
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

  public event Action<MouseButton> OnMouseButtonDown;
  public void MouseButtonDownEvent(MouseButton mouseButton)
  {
    OnMouseButtonDown?.Invoke(mouseButton);
  }

  public event Action<MouseButton, TimeSpan> OnMouseButtonUp;
  public void MouseButtonUpEvent(MouseButton mouseButton, TimeSpan heldTime)
  {
    OnMouseButtonUp?.Invoke(mouseButton, heldTime);
  }

  public event Action<Key, bool, TimeSpan> OnKeyAction;
  public void KeyActionEvent(Key key, bool isRepeating, TimeSpan heldTime)
  {
    OnKeyAction?.Invoke(key, isRepeating, heldTime);
  }

  public event Action<MouseButton, bool, TimeSpan> OnMouseAction;
  public void MouseActionEvent(MouseButton button, bool isRepeating, TimeSpan heldTime)
  {
    OnMouseAction?.Invoke(button, isRepeating, heldTime);
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

  public event Action<bool, TimeSpan> OnLeftClickAction;
  public void LeftClickActionEvent(bool isRepeating, TimeSpan heldTime)
  {
    OnLeftClickAction?.Invoke(isRepeating, heldTime);
  }

  public event Action<bool, TimeSpan> OnRightClickAction;
  public void RightClickActionEvent(bool isRepeating, TimeSpan heldTime)
  {
    OnRightClickAction?.Invoke(isRepeating, heldTime);
  }
}