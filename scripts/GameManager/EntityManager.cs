using GameManager;
using Godot;
using Scene;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class EntityManager<EntityType>(string resourcePath, params StringName[] scenesToPreload) : GameResourceManager<EntityType>(resourcePath, scenesToPreload) where EntityType : Entity, new()
{
	public static readonly List<EntityType> AllEntities = [];

	protected readonly EntityType _entityReference = new();
	protected readonly Stack<Entity> _entityInstances = [];
	protected readonly Dictionary<Guid, Entity> _entitiesById = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	public EntityType GetEntityInstanceByName(string entityFileName, StageLoader stageLoader)
	{
		EntityType entityPrefab = CreateInstance(entityFileName, entityFileName + AllEntities.Count);

		AllEntities.Add(entityPrefab);
		_entityInstances.Push(entityPrefab);
		_entitiesById.Add(entityPrefab.Id, entityPrefab);
		stageLoader.InputManager.ListenTo(entityPrefab);

		return entityPrefab;
	}
}
