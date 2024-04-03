using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Commands.Movement;
using Godot;

namespace Entity;

public abstract partial class Entity : Node2D, IEntityBaseNode
{
	public DIRECTIONS facingDirection = DIRECTIONS.BOTTOM;
	public DIRECTIONS lastFacingDirection = DIRECTIONS.BOTTOM;

	public EntityMovementController MovementController;

	public Guid Id = Guid.NewGuid();
	public Camera2D Camera { get; set; }
	public Dictionary<StringName, AnimatedSprite2D> Animations { get; set; } = [];
	public CharacterBody2D CollisionBody { get; set; }
	public CollisionShape2D[] CollisionShapes { get; set; }
	public abstract StringName ResourceName { get; set; }

	public EntityIdleAnimator idleAnimator;
	public EntityMovementAnimator movementAnimator;
	public MovementCommandKeybind movementKeyBind;
	public HashSet<Key> keysPressed = [];

	public bool ReadyToSpawn { get; set; } = false;

	public bool Spawned { get; set; } = false;

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
			return GetDictAnimationsByNameFromNode(MovementAnimationsNode);
		}
	}

	public Dictionary<StringName, AnimatedSprite2D> IdleAnimations
	{
		get
		{
			return GetDictAnimationsByNameFromNode(IdleAnimationsNode);
		}
	}

	public static Dictionary<StringName, AnimatedSprite2D> GetDictAnimationsByNameFromNode(Node2D node)
	{
		return node.GetChildren()
				.Select(c => c as AnimatedSprite2D)
				.ToDictionary(sprite => sprite.Name, sprite => sprite);
	}

	public void AddAnimations(Dictionary<StringName, AnimatedSprite2D> dictAnimations)
	{
		foreach (var kvp in dictAnimations)
		{
			Animations[kvp.Key] = kvp.Value;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		Camera = GetNode<Camera2D>("Camera");

		AddAnimations(IdleAnimations);
		AddAnimations(MovementAnimations);

		idleAnimator.OnReady();
		movementAnimator.OnReady();

		idleAnimator.Play();

		CollisionBody = GetNode<CharacterBody2D>("CollisionBody");
		CollisionShapes = CollisionBody.GetChildren().Select(c => c as CollisionShape2D).ToArray();
	}


	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey inputEventKey)
		{
			if (inputEventKey.Pressed)
			{
				keysPressed.Add(inputEventKey.Keycode);
			}
			else
			{
				keysPressed.Remove(inputEventKey.Keycode);
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

		MovementController.DefaultMovementProcess(delta, out bool hasPlayerWalked);

		if (!hasPlayerWalked)
		{
			MovementController.MovementState = MOVEMENT_STATE.IDLE;
		}

		if (MovementController.MovementStateUpdated && facingDirection != lastFacingDirection)
		{
			MovementController.MovementState = MOVEMENT_STATE.WALKING;
		}
	}
}
