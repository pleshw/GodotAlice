using Godot;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class Enemy(Vector2 initialPosition) : EntityAnimated(initialPosition)
{
  private readonly Vector2 initialPosition = initialPosition;
  public override EntityInventoryBase BaseInventory { get; set; }


  public Enemy() : this(Vector2.Zero)
  {
  }
  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    base._Process(delta);
  }

  public override void _PhysicsProcess(double delta)
  {
    base._PhysicsProcess(delta);
  }
}
