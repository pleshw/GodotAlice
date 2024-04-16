using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Agility : EntityAttributeBase
  {
    public override string Abbreviation { get { return "agi"; } }
    public override string Name { get { return "Agility"; } }
  }
}