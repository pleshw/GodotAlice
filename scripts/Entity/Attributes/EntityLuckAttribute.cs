using System;
using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Luck : IEntityAttribute
  {
    public string Abbreviation { get; set; } = "luk";
    public string Name { get; set; } = "Luck";
    public Entity Owner { get; set; } = null;

    public void ModifyUsingBaseAttributes(int points)
    {

    }

    public void ModifyUsingExternalAttributes(int points)
    {
    }

    public void ModifyUsingGeneralAttributes(int points)
    {
      int everyThree = points / 3;
      Owner.Stats.CriticalChance += everyThree;
      Owner.Stats.MinDamageMelee += everyThree;
      Owner.Stats.MinDamageRanged += everyThree;
      Owner.Stats.MagicDamage += everyThree;
      Owner.Stats.HitChance += everyThree;

      int everyFive = points / 5;
      Owner.Stats.DodgeChance += everyFive;

      Owner.Stats.FleeChance += points / 10;
    }
  }
}