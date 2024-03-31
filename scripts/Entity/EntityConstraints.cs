namespace Entity;

public record struct EntityDefaults()
{
  public const int MovementSpeed = 6;
  public const int MovementRunModifier = 2;
  public const MOVEMENT_STATE MovementState = MOVEMENT_STATE.IDLE;
}

public enum MOVEMENT_STATE
{
  IDLE,
  WALKING,
  RUNNING,
  STOPPING,
  CRAWLING,
  FALLING,
  GRABBING
}
