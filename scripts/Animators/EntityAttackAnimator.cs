using System.Collections.Generic;
using System.Linq;
using Animation;
using Godot;

namespace Entity;

public partial class EntityAttackAnimator(Entity entity) : EntityActionAnimator(entity)
{
  public override void OnReady()
  {
    ConfirmAnimations();
    HideAllAnimations();
  }


  public override void Play()
  {
    if (AnimationSprites == null)
    {
      return;
    }
  }

  public override void HideAllAnimations()
  {
    foreach (var anim in _entity.AnimationsByName)
    {
      anim.Value.Visible = false;
    }
  }
}
