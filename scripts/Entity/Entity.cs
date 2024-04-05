using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Entity.Commands.Movement;
using Godot;

namespace Entity;

public abstract partial class Entity : Node2D, IEntityBaseNode
{

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
	public Camera2D Camera { get; set; }
	protected Dictionary<StringName, AnimatedSprite2D> _animationsByName = [];
	public Dictionary<StringName, AnimationData> Animations { get; set; } = [];
	public CharacterBody2D CollisionBody { get; set; }
	public CollisionShape2D[] CollisionShapes { get; set; }
	public abstract StringName ResourceName { get; set; }

	public EntityIdleAnimator idleAnimator;
	public EntityMovementAnimator movementAnimator;
	public MovementCommandKeybind movementKeyBind;
	public HashSet<Key> keysPressed = [];

	public Dictionary<StringName, AnimatedSprite2D> AnimationsByName
	{
		get
		{
			return _animationsByName;
		}
	}

	public bool ReadyToSpawn { get; set; } = false;

	public bool Spawned { get; set; } = false;

	public float DashSpeedModifier { get; set; } = 6;
	public float DashDistance { get { return MovementController.StepSize * 10f; } }

	public Entity()
	{
		MovementController = new EntityMovementController(this, Vector2.Zero, 32);
		movementKeyBind = new MovementCommandKeybind(this);
		movementAnimator = new EntityMovementAnimator(this);
		idleAnimator = new EntityIdleAnimator(this);
	}

	public Entity(Vector2 initialPosition)
	{
		MovementController = new EntityMovementController(this, initialPosition, 32);
		movementKeyBind = new MovementCommandKeybind(this);
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

		Camera = GetNode<Camera2D>("Camera");

		AddAnimationSprites(IdleAnimationsSpritesByName);
		AddAnimationSprites(MovementAnimations);

		idleAnimator.OnReady();
		movementAnimator.OnReady();

		idleAnimator.Play();

		CollisionBody = GetNode<CharacterBody2D>("CollisionBody");
		CollisionShapes = CollisionBody.GetChildren().Select(c => c as CollisionShape2D).ToArray();
	}


	public override void _Input(InputEvent @event)
	{
		keysPressed.Clear();
		if (@event is InputEventKey inputEventKey)
		{
			if (inputEventKey.Pressed)
			{
				keysPressed.Add(inputEventKey.Keycode);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		foreach (var key in keysPressed)
		{
			movementKeyBind.Execute(key);
		}
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
