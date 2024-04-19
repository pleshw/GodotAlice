using Godot;
using System.Collections.Generic;
using System;
using GameManagers;

namespace Entity;

public partial class Player(Vector2 initialPosition) : EntityAnimated(initialPosition)
{
  private readonly Vector2 initialPosition = initialPosition;
  public override EntityInventoryBase BaseInventory { get; set; }

  public PlayerInventory Inventory
  {
    get
    {
      return BaseInventory as PlayerInventory;
    }
  }

  public Player() : this(Vector2.Zero)
  {
    BaseInventory = new PlayerInventory(this);
  }

  public override void _Ready()
  {
    base._Ready();

    ReadyToSpawn = true;

    AddToGroup("Players");

    // Camera.MakeCurrent();
    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);

    MainScene.InputManager.OnKeyAction += (Key keyPressed, bool isRepeating, TimeSpan heldTime) =>
    {
      movementKeyBinds.Execute(keyPressed, isRepeating);
      uiKeyBinds.Execute(keyPressed, isRepeating);
    };

    MainScene.InputManager.OnMouseAction += (MouseButton button, bool isRepeating, TimeSpan heldTime) =>
    {
      if (button == MouseButton.Left)
      {
        var outcome = CombatController.ExecuteAttack(null, new EntityActionInfo
        {
          Attacker = this,
          WeaponExtraDamage = 50,
          RangeInCells = 900,
          IsMelee = true,
          DamageProportion = new DamageTypeProportion(1f, 0f),
          DamageElementalProperty = DamageElementalProperty.NEUTRAL
        });
        var mousePosition = GetGlobalMousePosition();
        directionState.FacingDirectionVector = GlobalPosition.DirectionTo(mousePosition);
        if (outcome == AttackOutcome.MISS)
        {
          PlayAttackAnimation();
        }
      }
    };
  }
}
