using System;
using System.Collections.Generic;
using Godot;

namespace GameManager;

public partial class GameResourceManager<T> : Node where T : Node
{
  public static readonly Dictionary<StringName, PackedScene> Prefabs = [];

  private ResourcePreloader _preloader;
  public ResourcePreloader Preloader
  {
    get
    {
      _preloader ??= new();

      return _preloader;
    }
  }

  public StringName FolderPath;

  public GameResourceManager(StringName folderPath, params StringName[] scenesToPreload)
  {
    FolderPath = folderPath;
    foreach (StringName sceneToPreload in scenesToPreload)
    {
      StringName resourcePath = GetResourcePath(sceneToPreload);
      PackedScene preloadResource = ResourceLoader.Load(resourcePath) as PackedScene;
      Preloader.AddResource(resourcePath, preloadResource);
    }
  }

  public StringName GetResourcePath(StringName sceneFileName)
  {
    return $"{FolderPath}{sceneFileName}";
  }

  public T CreateInstance(StringName sceneName, StringName nodeName)
  {
    T result;
    StringName resourcePath = GetResourcePath(sceneName);

    var preload = Preloader.GetResource(resourcePath) as PackedScene;
    if (preload is not null)
    {
      result = preload.Instantiate() as T;
      result.Name = nodeName;
      return result;
    }

    /// Importing scene and saving the resource for later use
    PackedScene sceneImported = ResourceLoader.Load(resourcePath) as PackedScene;
    Preloader.AddResource(resourcePath, sceneImported);
    Prefabs.Add(resourcePath, sceneImported);
    result = sceneImported.Instantiate() as T;
    result.Name = nodeName;
    return result;
  }

  public ConvertedType CreateInstance<ConvertedType>(StringName sceneName, StringName nodeName) where ConvertedType : GodotObject
  {
    return CreateInstance(sceneName, nodeName) as ConvertedType;
  }

  ~GameResourceManager()
  {
    foreach (var item in Prefabs)
    {
      item.Value.Free();
    }
  }
}