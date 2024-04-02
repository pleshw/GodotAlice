using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Commands.Movement;
using Godot;

namespace Entity;

public abstract partial class Entity : EntityMovement, IEntityBaseNode
{
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

	public Entity() : base(Vector2.Zero)
	{
		movementKeyBind = new MovementCommandKeybind(this);
		movementAnimator = new EntityMovementAnimator(this);
		idleAnimator = new EntityIdleAnimator(this);
	}

	public Entity(Vector2 initialPosition) : base(initialPosition)
	{
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
		movementAnimator.PlayNext = idleAnimator.IdleAnimationData;

		idleAnimator.PlayIdle();

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
		foreach (var key in keysPressed)
		{
			movementKeyBind.Execute(key);
		}

		DefaultMovementProcess(delta, out _);
	}
}
