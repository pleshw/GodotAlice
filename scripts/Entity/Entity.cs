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

	public Guid Id = Guid.NewGuid();

	public GameStates GameStates { get; set; } = new();

	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public CharacterBody2D CollisionBody { get; set; }

	public CollisionShape2D[] CollisionShapes { get; set; }

	public EntityStats Stats { get; set; } = new();

	public EntityDirectionState directionState = new();

	public EntityMovementController MovementController;

	public EntityEquipmentSlots equipmentSlots;

	public MovementCommandKeybindMap movementKeyBinds;
	public UICommandKeybindMap uiKeyBinds;

	public abstract EntityInventoryBase BaseInventory { get; set; }

	public Node2D MainScene
	{
		get
		{
			return GetTree().Root.GetNode<Node2D>("MainScene");
		}
	}

	public bool ReadyToSpawn { get; set; } = false;

	public bool Spawned { get; set; } = false;

	public float DashSpeedModifier { get; set; } = 8;
	public float DashDistance { get { return MovementController.StepSize * 5f; } }


	public int Level { get; set; } = 1;

	public Entity()
	{
		Setup();
	}

	public Entity(Vector2 initialPosition)
	{
		Setup(initialPosition, 32);
	}

	public virtual void Setup(Vector2 initialPosition = default, int gridCellWidth = 32)
	{
		MovementController = new EntityMovementController(this, initialPosition, gridCellWidth);
		movementKeyBinds = new MovementCommandKeybindMap(this);
		uiKeyBinds = new UICommandKeybindMap(this);
		equipmentSlots = new EntityEquipmentSlots(this);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

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
