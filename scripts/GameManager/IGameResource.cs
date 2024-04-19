using Godot;

namespace GameManager;

public interface IGameResource
{
  /// <summary>
  /// The that will be displayed in game
  /// </summary>
  public StringName ItemName { get; set; }

  /// <summary>
  /// The name of the .tres file at the path. e.g: equipment resource BareHands will be searched by EquipmentResourceManager at
  /// $"res://resources/equipment/BareHands.tres";
  /// </summary>
  public StringName ItemId { get; set; }

  public string ItemDescription { get; set; }

  public SpriteFrames Sprite { get; set; }
}