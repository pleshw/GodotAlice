using Godot;

namespace Entity;

public partial class EntityStats : IEntityLifeStats
{
  public bool IsDead
  {
    get
    {
      return CurrentHealth <= 0;
    }
  }

  public int MaxHealth { get; }
  public int CurrentHealth { get; set; }
  public int MaxMana { get; }
  public int CurrentMana { get; set; }
  public int BaseHealthRecoveryRate { get; }
  public int BaseManaRecoveryRate { get; }
}