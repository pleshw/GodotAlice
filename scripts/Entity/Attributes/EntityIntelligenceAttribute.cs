using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Intelligence : IEntityAttribute
  {
    public string Abbreviation { get; set; } = "int";
    public string Name { get; set; } = "Intelligence";
    public Entity Owner { get; set; } = null;

    public void ModifyUsingBaseAttributes(int points)
    {
    }

    public void ModifyUsingExternalAttributes(int points)
    {
    }

    public void ModifyUsingGeneralAttributes(int points)
    {
      Owner.Stats.MagicResistance += points;

      Owner.Stats.MagicDamage += (int)Math.Round(points * 1.5f);

      Owner.Stats.MaxMana += (int)Math.Round(points * Owner.Stats.MaxMana * 0.01f);

      Owner.Stats.BaseManaRecoveryRate += points / 6;
    }
  }
}