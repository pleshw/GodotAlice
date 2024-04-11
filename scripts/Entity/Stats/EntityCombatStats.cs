using Godot;

namespace Entity;

public partial class EntityStats : IEntityCombatStats
{
  public int MinDamageMelee { get; set; } = 1;
  public int MinDamageRanged { get; set; } = 1;
  public int MagicDamage { get; set; } = 0;
  public int CriticalChance { get; set; } = 0;
  public int HitChance { get; set; } = 0;
  public int DodgeChance { get; set; } = 0;
  public int FleeChance { get; set; } = 0;
  public int Defense { get; set; } = 0;
  public int MagicResistance { get; set; } = 0;
}