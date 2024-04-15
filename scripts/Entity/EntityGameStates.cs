
using System;
using Godot;

namespace Entity;

[Flags]
public enum GameStates
{
  IDLE = 1 << 0,
  PERFORMING_ACTION = 1 << 1,
  INVULNERABLE = 1 << 2,
}
