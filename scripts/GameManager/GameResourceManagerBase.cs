using System;
using System.Collections.Generic;
using Godot;

namespace GameManagers;

public partial class GameResourceManager<T>(StringName resourcePath) : Node2D where T : Node2D
{
  public StringName resourcePath = resourcePath;

  [Export]
  public PackedScene[] PrefabList = [];

  public readonly HashSet<PackedScene> Prefabs = [];

  public override void _Ready()
  {
    base._Ready();
    foreach (PackedScene prefab in PrefabList)
    {
      PackedScene sceneImported = ResourceLoader.Load(prefab.ResourcePath) as PackedScene;
      Prefabs.Add(sceneImported);
    }
  }

  public StringName GetResourcePath(StringName sceneFileName)
  {
    return $"{resourcePath}{sceneFileName}.tscn";
  }

  public T CreateInstance(StringName sceneFileName, StringName nodeName)
  {
    /// Importing scene and saving the resource for later use
    PackedScene sceneImported = ResourceLoader.Load(GetResourcePath(sceneFileName)) as PackedScene;
    Prefabs.Add(sceneImported);
    ResourceSaver.Save(sceneImported, sceneImported.GetInstanceId().ToString());
    T instance = sceneImported.Instantiate() as T;
    instance.Name = nodeName;
    return instance;
  }

  ~GameResourceManager()
  {
    foreach (var item in Prefabs)
    {
      item.Free();
    }
  }
}