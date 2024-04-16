using Godot;

namespace Entity;

public abstract class EntityAttributeBase : IEntityAttribute
{
  public int Points = 0;
  public abstract string Name { get; }
  public abstract string Abbreviation { get; }
}