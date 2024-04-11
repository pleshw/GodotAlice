using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Strength : IEntityAttribute
  {
    public string Abbreviation { get; set; } = "str";
    public string Name { get; set; } = "Strength";
    public Entity Owner { get; set; } = null;

    public void ModifyUsingBaseAttributes(int points)
    {
    }

    public void ModifyUsingExternalAttributes(int points)
    {
    }

    public void ModifyUsingGeneralAttributes(int points)
    {
      Owner.Stats.MinDamageMelee += points;

      Owner.Stats.MinDamageRanged += points / 5;
    }
  }
}