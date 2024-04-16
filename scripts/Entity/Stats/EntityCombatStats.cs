using Godot;

namespace Entity;

public partial class EntityStats(Entity entity) : IEntityCombatStats
{
  public Entity Entity = entity;
  public int MinDamageMelee { get; }
  public int MinDamageRanged { get; }
  public int MagicDamage { get; }
  public int CriticalChance { get; }

  public int DodgeBonus { get; set; } = 0;

  /// <summary>
  /// Dodge chance is 100 - (AttackerHitPoints - EntityDodgePoints)
  /// </summary>
  public int DodgePoints
  {
    get
    {
      return 100 + DodgeBonus + Entity.Level + Entity.Attributes.Agility.Points + (Entity.Attributes.Luck.Points / 5);
    }
  }
  public int HitPoints { get; }

  public int DefensePercentage { get; }
  public int FlatDefense { get; }

  public int FlatMagicResistance { get; }
}