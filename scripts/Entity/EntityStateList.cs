using System;
using System.Collections.Generic;
using Godot;

namespace Entity;


public class EntityStateList<T>(Entity entityReference, Comparer<T> comparer) : SortedSet<T>(comparer)
{
  private readonly Entity entityReference = entityReference;

  public new void Add(T item)
  {
    base.Add(item);
    entityReference.StateHasChangedEvent();
  }

  public new void Remove(T item)
  {
    base.Remove(item);
    entityReference.StateHasChangedEvent();
  }
}