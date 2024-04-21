using GameManager;
using Godot;
using Scene;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class EntityManager<EntityType>(string resourcePath, params StringName[] scenesToPreload) : GameResourceManager<EntityType>(resourcePath, scenesToPreload) where EntityType : Entity, new()
{
	private StageLoader _mainScene;
	public StageLoader MainScene
	{
		get
		{
			_mainScene ??= GetTree().Root.GetNode<StageLoader>("MainScene");
			return _mainScene;
		}
	}

	public static readonly List<EntityType> AllEntities = [];

	protected readonly EntityType _entityReference = new();
	protected readonly Stack<Entity> _entityInstances = [];
	protected readonly Dictionary<Guid, Entity> _entitiesById = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	public bool TryInstantiateAtPosition(string entityFileName, StageLoader stageLoader, Vector2 position, out EntityType entityInstance, int maxAmountOfInstances = -1)
	{
		if (maxAmountOfInstances > 0 && _entityInstances.Count >= maxAmountOfInstances)
		{
			entityInstance = null;
			return false;
		}

		entityInstance = GetEntityInstanceByName(entityFileName, stageLoader);

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

	private EntityType GetEntityInstanceByName(string entityFileName, StageLoader stageLoader)
	{
		EntityType entityPrefab = CreateInstance(entityFileName, entityFileName + AllEntities.Count);

		AllEntities.Add(entityPrefab);
		_entityInstances.Push(entityPrefab);
		_entitiesById.Add(entityPrefab.Id, entityPrefab);
		stageLoader.InputManager.ListenTo(entityPrefab);

		return entityPrefab;
	}
}
