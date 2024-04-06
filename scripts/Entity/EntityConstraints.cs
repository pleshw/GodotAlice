using System;
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