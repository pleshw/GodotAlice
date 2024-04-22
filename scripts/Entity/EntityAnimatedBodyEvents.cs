using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity;

public partial class EntityAnimatedBody : Node2D
{
  public event Action OnReady;
  public void ReadyEvent()
  {
    OnReady?.Invoke();
  }

  public event Action OnFreeze;
  public void FreezeEvent()
  {
    OnFreeze?.Invoke();
  }
}