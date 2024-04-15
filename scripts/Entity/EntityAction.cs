namespace Entity;

public interface IEntityAction
{
  public bool CancelOnNextIteration { get; set; }
  public bool IsPerformingAction { get; set; }
}