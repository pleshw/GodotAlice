using System;
using System.Collections.Generic;
using Godot;

namespace GameManagers;

public class GameResourceManager<T>(StringName resourcePath) where T : Node2D
{
  public StringName resourcePath = resourcePath;
  public readonly Dictionary<string, PackedScene> Prefabs = [];

  public StringName GetResourcePath(StringName sceneFileName)
  {
    return $"{resourcePath}{sceneFileName}.tres";
  }

  public T CreateInstance(StringName sceneFileName)
  {
    if (Prefabs.TryGetValue(sceneFileName, out PackedScene scene))
    {
      return scene.Instantiate() as T;
    }

    /// Importing scene and saving the resource for later use
    PackedScene sceneImported = ResourceLoader.Load(GetResourcePath(sceneFileName)) as PackedScene;
    Prefabs.Add(sceneFileName, sceneImported);
    ResourceSaver.Save(sceneImported, sceneImported.GetInstanceId().ToString());

    return sceneImported.Instantiate() as T;
  }

  ~GameResourceManager()
  {
    foreach (var item in Prefabs)
    {
      item.Value.Free();
    }
  }
}