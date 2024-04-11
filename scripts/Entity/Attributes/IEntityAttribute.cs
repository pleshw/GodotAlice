using Godot;

namespace Entity;

public interface IEntityAttribute
{
  public string Abbreviation { get; set; }
  public string Name { get; set; }
  public Entity Owner { get; set; }

  /// <summary>
  /// All the changes that are unique to base attributes.
  /// </summary>
  /// <param name="points">Number of points that are invested by the player from this source</param>
  public void ModifyUsingBaseAttributes(int points);

  /// <summary>
  /// All the changes that are for external attributes such as provided by weapons.
  /// </summary>
  /// <param name="points">Number of points that are invested by the player from this source</param>
  public void ModifyUsingExternalAttributes(int points);

  /// <summary>
  /// All the changes that are both for base and external attributes.
  /// </summary>
  /// <param name="points">Number of points that are invested by the player from both sources</param>
  public void ModifyUsingGeneralAttributes(int points);
}