using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Vitality : IEntityAttribute
  {
    public string Abbreviation { get; set; } = "vit";
    public string Name { get; set; } = "Vitality";
    public Entity Owner { get; set; } = null;

    public void ModifyUsingBaseAttributes(int points)
    {

    }

    public void ModifyUsingExternalAttributes(int points)
    {
    }

    public void ModifyUsingGeneralAttributes(int points)
    {
      Owner.Stats.MaxHealth += (int)Math.Round(points * Owner.Stats.MaxHealth * 0.01f);

      Owner.Stats.Defense += points / 2;

      int everyFive = points / 5;
      Owner.Stats.MagicResistance += everyFive;
      Owner.Stats.BaseHealthRecoveryRate += everyFive;
    }
  }
}