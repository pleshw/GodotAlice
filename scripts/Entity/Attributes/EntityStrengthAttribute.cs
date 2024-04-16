using Godot;

namespace Entity;

public static partial class EntityAttributes
{
  public class Strength : EntityAttributeBase
  {
    public override string Abbreviation { get; } = "str";
    public override string Name { get; } = "Strength";
  }
}