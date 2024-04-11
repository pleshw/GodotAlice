using System.Collections.Generic;
using Animation;
using Godot;

namespace Entity;

public interface IEntityCombatStats
{
  public int MinDamage { get; set; }
  public int MaxDamage { get; set; }
  public int CriticalChance { get; set; }
  public int DodgeChance { get; set; }
}