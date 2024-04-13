using System;
using System.Collections.Generic;
using GameManagers;
using Godot;

namespace Entity;

public partial class EquipmentManager : GameResourceManager<EntityEquipmentBase>
{

  private static EquipmentManager _instance;
  public static EquipmentManager Instance
  {
    get
    {
      _instance ??= new EquipmentManager();
      return _instance;
    }
  }

  private EquipmentManager() : base("res://prefabs/items/equipment/")
  {
  }
}