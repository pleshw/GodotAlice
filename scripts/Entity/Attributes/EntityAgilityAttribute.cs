using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Agility : IEntityAttribute
  {
    public string Abbreviation { get; set; } = "agi";
    public string Name { get; set; } = "Agility";
    public Entity Owner { get; set; } = null;

    public void ModifyUsingBaseAttributes(int points)
    {
    }

    public void ModifyUsingExternalAttributes(int points)
    {
    }

    public void ModifyUsingGeneralAttributes(int points)
    {
      Owner.Stats.DodgeChance += points;

      Owner.Stats.Defense += points / 5;
    }
  }
}