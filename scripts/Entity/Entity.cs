using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Entity.Commands;
using Godot;

namespace Entity;

public abstract partial class Entity : Node2D, IEntityBaseNode
{
	public static Camera2D GlobalCamera { get; set; } = null;

	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public CharacterBody2D CollisionBody { get; set; }

	public CollisionShape2D[] CollisionShapes { get; set; }

	[Export]
	public Control EntityUI { get; set; }

	public EntityStats Stats { get; set; } = new();

	public EntityDirectionState directionState = new();

	public EntityMovementController MovementController;

	public Guid Id = Guid.NewGuid();
	protected Dictionary<StringName, AnimatedSprite2D> _animationsByName = [];
	public Dictionary<StringName, AnimationData> Animations { get; set; } = [];

	public EntityIdleAnimator idleAnimator;
	public EntityMovementAnimator movementAnimator;
	public EntityAttackAnimator attackAnimator;

	public EntityEquipmentSlots equipmentSlots;

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

	public float DashSpeedModifier { get; set; } = 8;
	public float DashDistance { get { return MovementController.StepSize * 5f; } }


	public Node2D AnimationsNode
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

	public Node2D AttackAnimationsNode
	{
		get
		{
			return AnimationsNode.GetNode<Node2D>("Attack");
		}
	}

	public Dictionary<StringName, AnimatedSprite2D> MovementSpritesByName
	{
		get
		{
			return GetMovementSpritesByName(MovementAnimationsNode);
		}
	}

	public Dictionary<StringName, AnimatedSprite2D> IdleSpritesByName
	{
		get
		{
			return GetMovementSpritesByName(IdleAnimationsNode);
		}
	}

	public Dictionary<StringName, List<AnimatedSprite2D>> AttackSpritesByName
	{
		get
		{
			return GetAttackSpritesByName(AttackAnimationsNode);
		}
	}

	public int Level { get; set; } = 1;

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
		equipmentSlots = new EntityEquipmentSlots(this);
		movementAnimator = new EntityMovementAnimator(this);
		idleAnimator = new EntityIdleAnimator(this);
		attackAnimator = new EntityAttackAnimator(this);
	}



	public static Dictionary<StringName, AnimatedSprite2D> GetMovementSpritesByName(Node2D node)
	{
		return node.GetChildren()
				.Select(c => c as AnimatedSprite2D)
				.ToDictionary(sprite => sprite.Name, sprite => sprite);
	}

	public static Dictionary<StringName, List<AnimatedSprite2D>> GetAttackSpritesByName(Node2D node)
	{
		return node.GetChildren()
				.OfType<Node2D>()
				.Select(weapon => new
				{
					WeaponName = weapon.Name,
					Sprites = weapon.GetChildren().OfType<AnimatedSprite2D>().ToList()
				})
				.Where(weapon => weapon.Sprites.Count != 0)
				.ToDictionary(weapon => weapon.WeaponName, weapon => weapon.Sprites);
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

		AddAnimationSprites(IdleSpritesByName);
		AddAnimationSprites(MovementSpritesByName);

		idleAnimator.OnReady();
		movementAnimator.OnReady();
		attackAnimator.OnReady();

		idleAnimator.Play();
		CollisionShapes = CollisionBody.GetChildren().Select(c => c as CollisionShape2D).ToArray();

		OnTryEquipSuccessEvent += (EntityEquipmentBase equippedItem, EntityEquipmentSlotType position) =>
		{

		};

		UIMenu.Visible = false;

		equipmentSlots.ForEveryItemSlot((EntityEquipmentSlot itemSlot) =>
		{
			CallDeferred(nameof(SetEquippedItem), itemSlot);
		});

		AddToGroup("Entities");
	}

	public void SetEquippedItem(EntityEquipmentSlot equipmentSlot)
	{
		equipmentSlot.Name = equipmentSlot.SlotType.ToString();
		GetNode<Node>("EquippedItems").AddChild(equipmentSlot);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		MovementController.MovementProcess(delta, out bool _);
	}
}
