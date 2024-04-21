using Godot;
using System.Collections.Generic;
using System;
using Effect;
using Entity.Commands;

namespace Entity;

public partial class Player(Vector2 initialPosition) : EntityAnimated(initialPosition)
{
  private string _displayName = "";
  public string DisplayName
  {
    get
    {
      return _displayName;
    }
    set
    {
      _displayName = value;
      NameHelper.Text = _displayName;
    }
  }

  public UICommandKeybindMap uiKeyBinds;

  private readonly Vector2 initialPosition = initialPosition;
  public override EntityInventoryBase BaseInventory { get; set; }

  public PlayerInventory Inventory
  {
    get
    {
      return BaseInventory as PlayerInventory;
    }
  }

  public DefaultHelper DamageAlertHelper
  {
    get
    {
      return GetNode<DefaultHelper>("DamageAlert");
    }
  }

  public DefaultHelper NameHelper
  {
    get
    {
      return GetNode<DefaultHelper>("PlayerName");
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

  }

  public void PlayerEnteredInGame()
  {

    uiKeyBinds = new UICommandKeybindMap(this);
    MainScene.InputManager.OnKeyAction += (Key keyPressed, bool isRepeating, TimeSpan heldTime) =>
    {
      movementKeyBinds.Execute(keyPressed, isRepeating);
      uiKeyBinds.Execute(keyPressed, isRepeating);
    };

    OnAttackAnimationFrameChangeEvent += (int currentFrame, int animationFrameCount) =>
    {
      if (currentFrame == animationFrameCount / 2)
      {
        AudioStreamPlayer2D[] soundEffect = [GetNode<AudioStreamPlayer2D>("Ponhonho"), GetNode<AudioStreamPlayer2D>("Inhonho")];
        int randomSeed = (int)MainScene.Random.NextInt64(800, 1400);
        DamageAlertHelper.Spawn(randomSeed.ToString());
        AudioStreamPlayer2D playSound = soundEffect[randomSeed % 2];
        playSound.VolumeDb = randomSeed / 100;
        playSound.Play();
      }
    };

    OnMouseOver += () =>
    {
      (AnimatedBody.Material as ShaderMaterial).SetShaderParameter("color", new Vector4(255, 255, 255, 1));
      (AnimatedBody.Material as ShaderMaterial).SetShaderParameter("width", 4);
      (AnimatedBody.Material as ShaderMaterial).SetShaderParameter("add_margins", false);
      NameHelper.Visible = true;
      NameHelper.Value.Visible = true;
      GD.Print(DisplayName);
    };

    OnMouseOut += () =>
    {
      (AnimatedBody.Material as ShaderMaterial).SetShaderParameter("color", new Vector4(255, 255, 255, 0));
      (AnimatedBody.Material as ShaderMaterial).SetShaderParameter("width", 0);
      NameHelper.Visible = false;
    };

    MainScene.InputManager.OnMouseAction += (MouseInputAction input, bool isRepeating, TimeSpan heldTime, HashSet<Node2D> hovering) =>
    {
      if (input.IsLeftClick)
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
