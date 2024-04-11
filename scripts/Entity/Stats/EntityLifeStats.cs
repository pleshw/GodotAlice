using Godot;

namespace Entity;

public partial class EntityStats : IEntityLifeStats
{
  public int MaxHealth { get; set; } = 1;
  public int CurrentHealth { get; set; } = 1;
  public int MaxMana { get; set; } = 0;
  public int CurrentMana { get; set; } = 0;
  public bool IsInvulnerable { get; set; } = true;
  public int BaseHealthRecoveryRate { get; set; } = 0;
  public int BaseManaRecoveryRate { get; set; } = 0;
}