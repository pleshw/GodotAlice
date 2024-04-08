using Godot;
using Animation;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Entity;


/// <summary>
/// Items by slot
/// </summary>
/// <param name="owner">The Entity that owns that inventory</param>
/// <param name="maxSlots">The max amount of slots</param>
/// <param name="maxSlotSize">The amount of items per slot</param>
public partial class PlayerInventory(Entity owner, int maxSlots = 10, int maxSlotSize = 64) : EntityInventoryBase(owner, maxSlots, maxSlotSize)
{
  
}