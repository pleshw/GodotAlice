using System.Collections.Generic;
using Godot;

namespace Entity;

public interface IEntityCombatStats
{
  public int MinDamageMelee { get; }
  public int MinDamageRanged { get; }
  public int CriticalChance { get; }
  public int DodgePoints { get; }
}