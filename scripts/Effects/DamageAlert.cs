
using System;
using Godot;
using Extras;

namespace Effect;

public partial class DamageAlert : Node2D
{

  private Tween DefaultSpawnTween;
  private Tween DefaultDespawnTween;

  private Label _value;
  public Label Value
  {
    get
    {
      _value ??= GetNode<Label>("Value");
      return _value;
    }
  }

  public void Spawn(string text)
  {
    Value.Text = text;
    DefaultSpawnAnimation();
  }

  public void Despawn()
  {
    DefaultDespawnAnimation();
  }

  private void DefaultSpawnAnimation()
  {
    DefaultSpawnTween?.Kill();
    DefaultSpawnTween = GetTree().Root.CreateTween();

    Value.Position = Vector2.Zero;
    Value.Visible = true;
    Visible = true;

    Value.SetIndexed("modulate:a", 0.0f);
    DefaultSpawnTween.TweenProperty(Value, "modulate:a", 1, 0.2f).SetTrans(Tween.TransitionType.Linear);
    DefaultSpawnTween.TweenProperty(Value, "position", Utils.GetRandomVector(-150, 150, -50, -100), .2f).SetTrans(Tween.TransitionType.Linear);
  }

  private void DefaultDespawnAnimation()
  {
  }
}