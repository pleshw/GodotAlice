using Godot;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class EntityManager<EntityType>(string resourceName) : Node where EntityType : Entity, new()
{
	private PackedScene EntityPrefab;

	protected StringName ResourceName = resourceName;

	protected readonly EntityType _entityReference = new();
	protected readonly Stack<Entity> _entityInstances = [];
	protected readonly Dictionary<Guid, Entity> _entitiesById = [];


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EntityPrefab = ResourceLoader.Load(ResourceName) as PackedScene;
	}

	public bool TrySpawnAtPosition(Vector2 position, out EntityType entityInstance, int maxAmountOfInstances = -1)
	{
		if (maxAmountOfInstances > 0 && _entityInstances.Count >= maxAmountOfInstances)
		{
			entityInstance = null;
			return false;
		}

		entityInstance = AddInstance();

		entityInstance.MovementController.initialPosition = position;

		CallDeferred(nameof(Spawn), entityInstance);

		return true;
	}

	private void Spawn(Entity entityPrefab)
	{
		AddChild(entityPrefab);
		entityPrefab.Spawned = true;
	}

	private EntityType AddInstance()
	{
		EntityType entityPrefab = EntityPrefab.Instantiate() as EntityType;

		_entityInstances.Push(entityPrefab);
		_entitiesById.Add(entityPrefab.Id, entityPrefab);

		return entityPrefab;
	}
}
