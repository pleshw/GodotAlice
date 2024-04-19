using System;
using System.Linq;
using Entity.Commands;
using Godot;
using Scene;

namespace Entity;

public abstract partial class Entity : Node2D, IEntityBaseNode
{
	public Guid Id = Guid.NewGuid();

	public bool LockGameState = false;

	private EntityGameState _gameState = EntityGameState.IDLE;

	public EntityGameState GameState
	{
		get
		{
			return _gameState;
		}

		set
		{
			if (LockGameState)
			{
				return;
			}

			EntityGameState prev = _gameState;
			_gameState = value;
			GameStateChangeEvent(prev, value);
		}
	}

	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public CharacterBody2D CollisionBody { get; set; }

	public CollisionShape2D[] CollisionShapes { get; set; }

	public EntityDefaultAttributes Attributes = new();

	public EntityStats Stats { get; set; }

	public EntityDirectionState directionState = new();

	public EntityCombatController CombatController;
	public EntityMovementController MovementController;

	public EntityEquipmentSlots equipmentSlots;

	public MovementCommandKeybindMap movementKeyBinds;
	public UICommandKeybindMap uiKeyBinds;

	public abstract EntityInventoryBase BaseInventory { get; set; }

	private MainScene _mainScene;
	public MainScene MainScene
	{
		get
		{
			_mainScene ??= GetTree().Root.GetNode<MainScene>("MainScene");
			return _mainScene;
		}
	}

	private Camera2D _globalCamera;
	public Camera2D GlobalCamera
	{
		get
		{
			_globalCamera ??= MainScene.GetNode<Camera2D>("GlobalCamera");
			return _globalCamera;
		}
	}


	public bool ReadyToSpawn { get; set; } = false;

	public bool Spawned { get; set; } = false;

	public float DashSpeedModifier { get; set; } = 8;
	public float DashDistance { get { return MovementController.StepSize * 8f; } }


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
		CombatController = new EntityCombatController(this);
		movementKeyBinds = new MovementCommandKeybindMap(this);
		uiKeyBinds = new UICommandKeybindMap(this);
		equipmentSlots = new EntityEquipmentSlots(this);
		Stats = new(this);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		CollisionShapes = CollisionBody.GetChildren().Select(c => c as CollisionShape2D).ToArray();

		OnMovedEvent += (Vector2 from, Vector2 to) =>
		{
			GameState = EntityGameState.MOVING;
		};

		OnStoppedEvent += () =>
		{
			GameState = EntityGameState.IDLE;
		};

		OnEntityDashedEvent += (Vector2 from, Vector2 to) =>
		{
			GameState = EntityGameState.DASHING;
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
