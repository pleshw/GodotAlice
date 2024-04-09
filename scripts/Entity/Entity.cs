using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Entity.Commands.Movement;
using Entity.Commands.UI;
using Godot;

namespace Entity;

public abstract partial class Entity : Node2D, IEntityBaseNode
{
	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public CharacterBody2D CollisionBody { get; set; }

	public CollisionShape2D[] CollisionShapes { get; set; }

	[Export]
	public Control EntityUI { get; set; }

	[Export]
	public Control InventoryWindow { get; set; }

	public float FacingDirectionAngle
	{
		get
		{
			float angle = Mathf.Atan2(FacingDirectionVector.Y, FacingDirectionVector.X);
			float degrees = Mathf.RadToDeg(angle);

			if (degrees < 0)
			{
				degrees += 360;
			}

			return degrees;
		}
	}

	public DIRECTIONS LastCommandDirection { get; set; } = DIRECTIONS.RIGHT;

	public DIRECTIONS LastFacedDirection { get; set; } = DIRECTIONS.RIGHT;

	public DIRECTIONS FacingSide
	{
		get
		{
			if (FacingDirectionVector.X == float.PositiveInfinity)
			{
				return LastFacedDirection;
			}

			if (FacingDirectionVector.X > 0)
			{
				return LastFacedDirection = DIRECTIONS.RIGHT;
			}
			else if (FacingDirectionVector.X < 0)
			{
				return LastFacedDirection = DIRECTIONS.LEFT;
			}
			else
			{
				return LastFacedDirection;
			}
		}
	}

	public DIRECTIONS LastFacingDirection { get; } = DIRECTIONS.BOTTOM;

	public Vector2 FacingDirectionVector { get; set; } = new Vector2 { X = 1, Y = 0 };

	public EntityMovementController MovementController;

	public Guid Id = Guid.NewGuid();
	protected Dictionary<StringName, AnimatedSprite2D> _animationsByName = [];
	public Dictionary<StringName, AnimationData> Animations { get; set; } = [];

	public EntityIdleAnimator idleAnimator;
	public EntityMovementAnimator movementAnimator;
	public MovementCommandKeybindMap movementKeyBinds;
	public UICommandKeybindMap uiKeyBinds;

	public abstract EntityInventoryBase BaseInventory { get; set; }

	public Dictionary<StringName, AnimatedSprite2D> AnimationsByName
	{
		get
		{
			return _animationsByName;
		}
	}

	public bool ReadyToSpawn { get; set; } = false;

	public bool Spawned { get; set; } = false;

	public float DashSpeedModifier { get; set; } = 4;
	public float DashDistance { get { return MovementController.StepSize * 5f; } }

	public Entity()
	{
		Setup();
	}

	public Entity(Vector2 initialPosition)
	{
		Setup(initialPosition, 32);
	}

	public void Setup(Vector2 initialPosition = default, int gridCellWidth = 32)
	{
		MovementController = new EntityMovementController(this, initialPosition, gridCellWidth);
		movementKeyBinds = new MovementCommandKeybindMap(this);
		uiKeyBinds = new UICommandKeybindMap(this);
		movementAnimator = new EntityMovementAnimator(this);
		idleAnimator = new EntityIdleAnimator(this);
	}


	public Node AnimationsNode
	{
		get
		{
			return GetNode<Node2D>("Animations");
		}
	}

	public Node2D IdleAnimationsNode
	{
		get
		{
			return AnimationsNode.GetNode<Node2D>("Idle");
		}
	}

	public Node2D MovementAnimationsNode
	{
		get
		{
			return AnimationsNode.GetNode<Node2D>("Movement");
		}
	}

	public Dictionary<StringName, AnimatedSprite2D> MovementAnimations
	{
		get
		{
			return GetDictAnimationSpritesByName(MovementAnimationsNode);
		}
	}

	public Dictionary<StringName, AnimatedSprite2D> IdleAnimationsSpritesByName
	{
		get
		{
			return GetDictAnimationSpritesByName(IdleAnimationsNode);
		}
	}

	public static Dictionary<StringName, AnimatedSprite2D> GetDictAnimationSpritesByName(Node2D node)
	{
		return node.GetChildren()
				.Select(c => c as AnimatedSprite2D)
				.ToDictionary(sprite => sprite.Name, sprite => sprite);
	}

	private void AddAnimationSprites(Dictionary<StringName, AnimatedSprite2D> dictAnimations)
	{
		foreach (var kvp in dictAnimations)
		{
			_animationsByName[kvp.Key] = kvp.Value;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		AddAnimationSprites(IdleAnimationsSpritesByName);
		AddAnimationSprites(MovementAnimations);

		idleAnimator.OnReady();
		movementAnimator.OnReady();

		idleAnimator.Play();
		CollisionShapes = CollisionBody.GetChildren().Select(c => c as CollisionShape2D).ToArray();

		InventoryWindow.Visible = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		MovementController.MovementProcess(delta, out bool _);
	}


	[Signal]
	public delegate void StateChangedEventHandler();

	[Signal]
	public delegate void EntityStoppedEventHandler();

	[Signal]
	public delegate void EntityMovedEventHandler(Vector2 from, Vector2 to);

	[Signal]
	public delegate void MovementStateUpdatedEventHandler();

	[Signal]
	public delegate void MovementInputTriggeredEventHandler();
}
