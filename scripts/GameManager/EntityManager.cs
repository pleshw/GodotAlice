using GameManager;
using Godot;
using Scene;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class EntityManager<EntityType>(string resourcePath, params StringName[] scenesToPreload) : GameNodeManagerBase<EntityType>(resourcePath, scenesToPreload) where EntityType : Entity, new()
{
	public static readonly List<EntityType> AllEntities = [];

	protected readonly EntityType _entityReference = new();
	protected readonly Stack<Entity> _entityInstances = [];
	protected readonly Dictionary<Guid, Entity> _entitiesById = [];



	public InputManager InputManager
	{
		get
		{
			return GetNode<InputManager>("/root/InputManager");
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	public EntityType GetEntityInstance(string entityScenePath)
	{
		EntityType entityPrefab = CreateInstance(entityScenePath, entityScenePath + AllEntities.Count);

		AllEntities.Add(entityPrefab);
		_entityInstances.Push(entityPrefab);
		_entitiesById.Add(entityPrefab.Id, entityPrefab);
		InputManager.ListenTo(entityPrefab);

		return entityPrefab;
	}
}
