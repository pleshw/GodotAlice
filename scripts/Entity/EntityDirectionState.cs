using Godot;

namespace Entity;

public class EntityDirectionState
{
  public DIRECTIONS LastCommandDirection { get; set; } = DIRECTIONS.RIGHT;

  public DIRECTIONS LastFacedDirection { get; set; } = DIRECTIONS.RIGHT;

  public DIRECTIONS FacingSide
  {
    get
    {
      if (FacingDirectionVector.X == float.PositiveInfinity)
      {
        return LastFacedDirection;
      }

      if (FacingDirectionVector.X > 0)
      {
        return LastFacedDirection = DIRECTIONS.RIGHT;
      }
      else if (FacingDirectionVector.X < 0)
      {
        return LastFacedDirection = DIRECTIONS.LEFT;
      }
      else
      {
        return LastFacedDirection;
      }
    }
  }

  public Vector2 FacingDirectionVector { get; set; } = new Vector2 { X = 1, Y = 0 };
}