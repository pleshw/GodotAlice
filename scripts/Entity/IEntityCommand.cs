namespace Entity;

public interface IEntityCommand
{
  public void Execute(bool repeating);
}