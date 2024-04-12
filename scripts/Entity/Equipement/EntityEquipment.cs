using System.Collections.Generic;
using Godot;

namespace Entity;

public static class EntityEquipment
{
  public static readonly HashSet<EntityEquipmentBase> Prefabs = [];

  public static StringName GetResourcePath(EntityEquipmentBase resourceToLoad)
  {
    return $"res://resources/equipment/{resourceToLoad.ItemId}.tres";
  }

  public static EntityEquipmentBase Instantiate(EntityEquipmentBase resourceToInstantiate)
  {
    if (Prefabs.TryGetValue(resourceToInstantiate, out EntityEquipmentBase res))
    {
      return ResourceLoader.Load<EntityEquipmentBase>(res.GetInstanceId().ToString());
    }

    var instance = ResourceLoader.Load<EntityEquipmentBase>(GetResourcePath(resourceToInstantiate)).Duplicate() as EntityEquipmentBase;

    Prefabs.Add(instance);

    ResourceSaver.Save(instance, instance.GetInstanceId().ToString());

    return instance;
  }
}