using System.Collections.Generic;
using Godot;

namespace Entity.Commands;

public abstract class EntityCommandKeybind(Entity entity) : Dictionary<Key, IEntityCommand>
{
  public Entity entity = entity;

  public virtual void BindKey(Key key, IEntityCommand command)
  {
    if (!TryAdd(key, command))
    {
      this[key] = command;
    }
  }

  public void Execute(Key key, bool repeating)
  {
    if (TryGetValue(key, out var command))
    {
      command.Execute(repeating);
    }
  }

  public abstract void BindDefaults();
}