using System.Collections.Generic;
using Animation;
using Godot;

namespace Entity;

public interface IEntityCombatStats
{
  public int MinDamageMelee { get; }
  public int MinDamageRanged { get; }
  public int CriticalChance { get; }
  public int DodgePoints { get; }
}