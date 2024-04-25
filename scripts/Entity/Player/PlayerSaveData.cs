
using System.Collections.Generic;

namespace Entity;

public class PlayerSaveData()
{
  public required string Name { get; set; }
  public required Dictionary<string, int> AttributePointsByName { get; set; }
  public required Dictionary<string, string> SpriteFramesByPosition { get; set; }

  public required string Location { get; set; }
}