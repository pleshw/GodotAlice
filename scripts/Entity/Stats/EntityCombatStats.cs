using Godot;

namespace Entity;

public partial class EntityStats : IEntityCombatStats
{
  public int MinDamage { get; set; } = 1;
  public int MaxDamage { get; set; } = 1;
  public int CriticalChance { get; set; } = 0;
  public int DodgeChance { get; set; } = 0;
}