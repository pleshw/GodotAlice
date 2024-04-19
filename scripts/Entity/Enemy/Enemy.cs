using Godot;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class Enemy(Vector2 initialPosition) : EntityAnimated(initialPosition)
{
  public int ShootCount = 0;

  [Export]
  public Node2D Bullet;

  public Area2D BulletArea
  {
    get
    {
      return Bullet.GetNode<Area2D>("Area2D");
    }
  }

  public CollisionShape2D BulletCollider
  {
    get
    {
      return BulletArea.GetNode<CollisionShape2D>("CollisionShape2D");
    }
  }

  public AnimatedSprite2D BulletSprite
  {
    get
    {
      return Bullet.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
  }

  public AudioStreamPlayer2D BulletSound
  {
    get
    {
      return Bullet.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    }
  }

  public bool IsShooting { get; set; } = false;
  public Vector2 BulletTarget { get; set; } = Vector2.Zero;

  private readonly Vector2 initialPosition = initialPosition;
  public override EntityInventoryBase BaseInventory { get; set; }


  public PlayerInventory Inventory
  {
    get
    {
      return BaseInventory as PlayerInventory;
    }
  }

  public Enemy() : this(Vector2.Zero)
  {
    BaseInventory = new PlayerInventory(this);
  }

  private readonly Dictionary<KeyList, DateTime> keysPressed = [];
  private readonly Dictionary<KeyList, TimeSpan> keysHeldDuration = [];
  private readonly Dictionary<KeyList, bool> keysCommandExecuted = [];


  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventKey inputEventKey)
    {
      KeyPressEvent(inputEventKey);
    }

    if (@event is InputEventMouseButton inputEventClick)
    {
      if (inputEventClick.Pressed)
      {
        var clickEventType = inputEventClick.ButtonIndex;
        if (clickEventType == MouseButton.Left)
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
      }
    }
  }

  public void KeyPressEvent(InputEventKey inputEventKey)
  {
    KeyList key = new(inputEventKey.Device, inputEventKey.Keycode);
    if (inputEventKey.Pressed)
    {
      if (!keysPressed.ContainsKey(key))
      {
        keysPressed.Add(key, DateTime.Now);
        keysCommandExecuted[key] = false;
      }
    }
    else
    {
      if (keysPressed.TryGetValue(key, out DateTime value))
      {
        TimeSpan heldDuration = DateTime.Now - value;
        keysHeldDuration[key] = heldDuration;
        keysPressed.Remove(key);
        keysCommandExecuted.Remove(key);
        // GD.Print("Key ", key.KeyCode, " held for ", heldDuration.TotalSeconds, " seconds.");
      }
    }
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    base._Process(delta);

    foreach (var keyAndTime in keysPressed)
    {
      bool isKeyRepeating = keysCommandExecuted[keyAndTime.Key];
      Key keyPressed = keyAndTime.Key.KeyCode;
      TimeSpan timeHeld = DateTime.Now - keyAndTime.Value;

      movementKeyBinds.Execute(keyPressed, isKeyRepeating);
      uiKeyBinds.Execute(keyPressed, isKeyRepeating);

      keysCommandExecuted[keyAndTime.Key] = true;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    base._PhysicsProcess(delta);
    HandleShooting(delta);
  }

  public void Shoot(Vector2 target)
  {
    Bullet.Visible = true;
    BulletTarget = target;
    Bullet.Position = Vector2.Zero;
    BulletSound.Play();
  }

  public void HandleShooting(double delta)
  {
    if (BulletTarget == Vector2.Zero)
    {
      return;
    }

    bool isLeft = directionState.FacingSide == DIRECTIONS.LEFT;
    BulletSprite.FlipH = isLeft;

    Bullet.Position += Bullet.Position with { X = (float)(isLeft ? -1 : 1 * 1000 * delta) };

    if (Bullet.Position.X >= BulletTarget.X)
    {
      Bullet.Visible = false;
      BulletTarget = Vector2.Zero;
      ++ShootCount;

      var fire = MainScene.GetNode<AnimatedSprite2D>("Fire");
      if (ShootCount >= 5)
      {
        if (!fire.Visible)
        {
          fire.Play("default");
          fire.Visible = true;
          return;
        }
      }

      if (ShootCount > 12)
      {
        fire.Position = fire.Position with { Y = fire.Position.Y - (fire.Position.Y * .1f) };
        fire.Scale *= 1.05f;
      }
    }
  }
}
