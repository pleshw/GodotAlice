using System;
using System.Collections.Generic;
using Godot;

namespace GameManagers;

public partial class GameResourceManager<T>(StringName resourcePath) : Node where T : Node
{
  public StringName ResourcePath = resourcePath;

  [Export]
  public PackedScene[] PrefabList = [];

  public readonly Dictionary<StringName, PackedScene> Prefabs = [];

  public override void _Ready()
  {
    base._Ready();
    foreach (PackedScene prefab in PrefabList)
    {
      PackedScene sceneImported = ResourceLoader.Load(prefab.ResourcePath) as PackedScene;
      Prefabs.Add(prefab.ResourcePath, sceneImported);
    }
  }

  public StringName GetResourcePath(StringName sceneFileName)
  {
    return $"{ResourcePath}{sceneFileName}";
  }

  public T CreateInstance(StringName sceneName, StringName nodeName)
  {
    T result;
    StringName resourcePath = GetResourcePath(sceneName);
    if (Prefabs.TryGetValue(resourcePath, out PackedScene packedScene))
    {
      result = packedScene.Instantiate() as T;
      result.Name = nodeName;
      return result;
    }

    /// Importing scene and saving the resource for later use
    PackedScene sceneImported = ResourceLoader.Load(resourcePath) as PackedScene;
    ResourceSaver.Save(sceneImported);
    Prefabs.Add(resourcePath, sceneImported);
    result = sceneImported.Instantiate() as T;
    result.Name = nodeName;
    return result;
  }

  ~GameResourceManager()
  {
    foreach (var item in Prefabs)
    {
      item.Value.Free();
    }
  }
}