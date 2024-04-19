using GameManagers;
using Godot;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class EntityManager<EntityType>(string resourcePath, string resourceName) : GameResourceManager<EntityType>(resourcePath) where EntityType : Entity, new()
{
	public Node2D MainScene { get; set; } = null;

	protected StringName ResourceName = resourceName;

	public static readonly List<EntityType> AllEntities = [];

	protected readonly EntityType _entityReference = new();
	protected readonly Stack<Entity> _entityInstances = [];
	protected readonly Dictionary<Guid, Entity> _entitiesById = [];


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		MainScene = GetTree().Root.GetNode<Node2D>("MainScene");
	}

	public bool TryInstantiateAtPosition(Vector2 position, out EntityType entityInstance, int maxAmountOfInstances = -1)
	{
		if (maxAmountOfInstances > 0 && _entityInstances.Count >= maxAmountOfInstances)
		{
			entityInstance = null;
			return false;
		}

		entityInstance = GetEntityInstance();

		AddEntityNodeToScene(entityInstance);

		entityInstance.MovementController.TeleportToNearestCell(new EntityMovementInput
		{
			Position = position,
			IsRunning = false,
			ForceStateChange = true,
			GameState = EntityGameState.IDLE
		});

		return true;
	}

	private void AddEntityNodeToScene(Entity entityPrefab)
	{
		MainScene.AddChild(entityPrefab);
		entityPrefab.Spawned = true;
	}

	private EntityType GetEntityInstance()
	{
		EntityType entityPrefab = CreateInstance(resourceName, "test" + AllEntities.Count);

		AllEntities.Add(entityPrefab);
		_entityInstances.Push(entityPrefab);
		_entitiesById.Add(entityPrefab.Id, entityPrefab);

		return entityPrefab;
	}
}
