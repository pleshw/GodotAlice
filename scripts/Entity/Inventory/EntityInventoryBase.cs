using Godot;
using Animation;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Entity;

public record struct ItemSlot
{
  public required int Amount { get; set; }

  public required EntityInventoryItem Content { get; set; }
  public required int Index;
}


/// <summary>
/// Items by slot
/// </summary>
/// <param name="owner">The Entity that owns that inventory</param>
/// <param name="maxSlots">The max amount of slots</param>
/// <param name="maxSlotSize">The amount of items per slot</param>
public partial class EntityInventoryBase(Entity owner, int maxSlots = 10, int maxSlotSize = 64) : SortedDictionary<int, ItemSlot>
{
  public Entity Owner = owner;

  public int FirstEmptySlot
  {
    get
    {
      return Keys.Min();
    }
  }

  public int this[EntityInventoryItem item]
  {
    get
    {
      return (from i in this where i.Value.Content == item select i.Key).DefaultIfEmpty(-1).First();
    }
  }

  /// <summary>
  /// Add a item in the first slot that contains it if the slot is not full or creates a new slot if it is
  /// </summary>
  /// <param name="item"></param>
  public void Add(EntityInventoryItem item)
  {
    if (this[item] == -1 && Count < maxSlots)
    {
      Add(FirstEmptySlot, new() { Amount = 1, Content = item, Index = FirstEmptySlot });
      return;
    }

    if (TryGetValue(this[item], out ItemSlot slot) && slot.Amount < maxSlotSize)
    {
      slot.Amount++;
    }
  }

  public static void SwapSlots(ItemSlot a, ItemSlot b)
  {
    ItemSlot tmp = new() { Amount = a.Amount, Content = a.Content, Index = a.Index };
    a.Content = b.Content;
    b.Content = tmp.Content;
  }
}
