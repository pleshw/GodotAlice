using System;
using System.Collections.Generic;
using Godot;

namespace GameManager;

public partial class GameResourceManagerBase<T> : Node where T : Resource
{
  public readonly Dictionary<StringName, T> Resources = [];

  public T this[StringName sceneName]
  {
    get
    {
      if (Resources.TryGetValue(sceneName, out T value))
      {
        return value;
      }
      else
      {
        throw new KeyNotFoundException($"Scene '{sceneName}' not found.");
      }
    }
  }

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

  public GameResourceManagerBase()
  {
    FolderPath = null;
  }


  public GameResourceManagerBase(StringName folderPath, params StringName[] scenesToPreload)
  {
    FolderPath = folderPath;
    foreach (StringName sceneToPreload in scenesToPreload)
    {
      StringName resourcePath = GetResourcePath(sceneToPreload);
      PackedScene preloadResource = ResourceLoader.Load(resourcePath) as PackedScene;
      Preloader.AddResource(resourcePath, preloadResource);
    }
  }
  /// <summary>
  /// If the class have no path folder this function will return the input
  /// </summary>
  /// <param name="sceneFileName"></param>
  /// <returns></returns>
  public StringName GetResourcePath(StringName sceneFileName)
  {
    if (FolderPath == null)
    {
      return sceneFileName;
    }

    return $"{FolderPath}{sceneFileName}";
  }

  public T CreateInstance(StringName sceneName, StringName nodeName)
  {
    T result;
    StringName resourcePath = GetResourcePath(sceneName);

    /// Importing scene and saving the resource for later use
    result = GD.Load<T>(resourcePath);
    Preloader.AddResource(resourcePath, result);

    result.ResourceName = nodeName;

    Resources.Add(nodeName, result);

    return result;
  }

  public Node LoadScene(StringName resourcePath, StringName resourceName)
  {
    PackedScene sceneImported = ResourceLoader.Load(resourcePath) as PackedScene;

    Node result = sceneImported.Instantiate();
    result.Name = resourceName;
    return result;
  }

  public ConvertedType LoadScene<ConvertedType>(StringName resourcePath, StringName resourceName) where ConvertedType : Node
  {
    PackedScene sceneImported = ResourceLoader.Load(resourcePath) as PackedScene;

    Node result = sceneImported.Instantiate();
    result.Name = resourceName;
    return result as ConvertedType;
  }

  public ConvertedType CreateInstance<ConvertedType>(StringName sceneName, StringName nodeName) where ConvertedType : Node
  {
    return CreateInstance(sceneName, nodeName) as ConvertedType;
  }

  public ConvertedType GetResource<ConvertedType>(StringName sceneName) where ConvertedType : Node
  {
    return this[sceneName] as ConvertedType;
  }
}