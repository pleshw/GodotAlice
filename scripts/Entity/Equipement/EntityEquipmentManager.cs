using System;
using System.Collections.Generic;
using GameManagers;
using Godot;

namespace Entity;

public class EntityEquipmentResourceManager : GameResourceManager<EntityEquipmentBase>
{

  private static EntityEquipmentResourceManager _instance;
  public static EntityEquipmentResourceManager Instance
  {
    get
    {
      _instance ??= new EntityEquipmentResourceManager();
      return _instance;
    }
  }

  private EntityEquipmentResourceManager() : base("res://prefabs/items/equipment/")
  {
  }
}