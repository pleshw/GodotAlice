using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Dexterity : IEntityAttribute
  {
    public string Abbreviation { get; set; } = "dex";
    public string Name { get; set; } = "Dexterity";
    public Entity Owner { get; set; } = null;

    public void ModifyUsingBaseAttributes(int points)
    {

    }

    public void ModifyUsingExternalAttributes(int points)
    {
    }

    public void ModifyUsingGeneralAttributes(int points)
    {
      Owner.Stats.MinDamageRanged += points;
      Owner.Stats.HitChance += points;

      int everyFive = points / 5;
      Owner.Stats.MinDamageMelee += everyFive;
      Owner.Stats.MagicDamage += everyFive;
      Owner.Stats.MagicResistance += everyFive;
    }
  }
}