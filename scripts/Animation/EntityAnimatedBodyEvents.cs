using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity;

public partial class EntityAnimatedBody : Node2D
{
  public event Action OnReadyEvent;
  public void ReadyEvent()
  {
    OnReadyEvent?.Invoke();
  }
}