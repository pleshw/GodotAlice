using System;
using System.Collections.Generic;
using Godot;

namespace Entity;

public record struct EntityDefaults()
{
  public const int MovementSpeed = 10;
  public const int MovementRunModifier = 2;
  public const MOVEMENT_STATE MovementState = MOVEMENT_STATE.IDLE;
}

public enum MOVEMENT_STATE
{
  IDLE,
  WALKING,
  DASHING,
  RUNNING,
  STOPPING,
  CRAWLING,
  FALLING,
  GRABBING
}

public enum DIRECTIONS
{
  TOP,
  RIGHT,
  BOTTOM,
  LEFT
}

public enum SIDES
{
  RIGHT,
  LEFT
}

// KeyList struct to store both device and keycode
public struct KeyList(int device, Key keyCode)
{
  public int Device = device;
  public Key KeyCode = keyCode;

  // Override GetHashCode and Equals to allow using KeyList as a dictionary key
  public override readonly int GetHashCode()
  {
    return HashCode.Combine(Device, KeyCode);
  }

  public override readonly bool Equals(object obj)
  {
    if (obj is not KeyList)
    {
      return false;
    }

    KeyList other = (KeyList)obj;
    return Device == other.Device && KeyCode == other.KeyCode;
  }

  public static bool operator ==(KeyList left, KeyList right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(KeyList left, KeyList right)
  {
    return !(left == right);
  }
}


public struct MouseInputAction(InputEventMouseButton inputEvent, Vector2 position, HashSet<Node2D> hovering) : IEquatable<MouseInputAction>
{
  public InputEventMouseButton Event = inputEvent;
  public Vector2 StartPosition = position;
  public HashSet<Node2D> StartHovering = hovering;

  public override readonly int GetHashCode()
  {
    return Event.ButtonIndex.GetHashCode();
  }

  public bool IsLeftClick
  {
    get
    {
      return Event.ButtonIndex == MouseButton.Left;
    }
  }


  public bool IsRightClick
  {
    get
    {
      return Event.ButtonIndex == MouseButton.Right;
    }
  }

  public override readonly bool Equals(object obj)
  {
    if (obj is not MouseInputAction)
    {
      return false;
    }

    MouseInputAction other = (MouseInputAction)obj;
    return Event.ButtonIndex == other.Event.ButtonIndex;
  }

  public readonly bool Equals(MouseInputAction other)
  {
    return Event.ButtonIndex == other.Event.ButtonIndex;
  }

  public static bool operator ==(MouseInputAction left, MouseInputAction right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(MouseInputAction left, MouseInputAction right)
  {
    return !(left == right);
  }
}