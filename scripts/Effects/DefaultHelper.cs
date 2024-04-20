
using System;
using Godot;
using Extras;
using Entity;

namespace Effect;

public partial class DefaultHelper : Node2D
{
  public bool Centered { get; set; } = true;

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


  public string Text
  {
    get
    {
      return Value.Text;
    }
    set
    {
      Value.Text = value;
      Value.ResetSize();
      if (Centered)
      {
        Value.Position = Value.Position with { X = (-Value.Size.X + 10) / 2 };
      }
    }
  }


  public new bool Visible
  {
    get
    {
      return base.Visible;
    }

    set
    {
      base.Visible = value;
      Value.Visible = value;
    }
  }

  private EntityAnimated _parent;
  protected EntityAnimated Parent
  {
    get
    {
      _parent ??= GetParent<EntityAnimated>();
      return _parent;
    }
  }

  public void Spawn(string text)
  {
    Value.Text = text;
    Value.ZIndex = Parent.AnimatedBody.ZIndex - 1;
    DefaultSpawnAnimation();
  }

  public void Despawn()
  {
    DefaultDespawnAnimation();
  }

  public virtual void DefaultSpawnAnimation()
  {
    DefaultSpawnTween?.Kill();
    DefaultSpawnTween = GetTree().Root.CreateTween();

    Value.Scale *= 0.1f;

    Vector2 parentPosition = Parent.GlobalPosition;
    Value.GlobalPosition = new Vector2
    {
      X = parentPosition.X - (Value.Size.X * 0.1f / 2),
      Y = parentPosition.Y - (Value.Size.Y * 0.1f / 2),
    };

    Value.Visible = true;
    Visible = true;

    Value.SetIndexed("modulate:a", 0.0f);
    DefaultSpawnTween.TweenProperty(Value, "modulate:a", 1, .1f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Linear);

    DefaultSpawnTween.TweenProperty(Value, "position", Utils.GetRandomVector(-50, 50, -100, -150), .1f)
      .SetEase(Tween.EaseType.Out)
      .SetTrans(Tween.TransitionType.Linear);

    DefaultSpawnTween.Parallel().TweenProperty(Value, "scale", Vector2.One, .1f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Linear);

    DefaultSpawnTween.TweenCallback(Callable.From(DefaultDespawnAnimation));
  }

  public virtual void DefaultDespawnAnimation()
  {
    DefaultDespawnTween?.Kill();
    DefaultDespawnTween = GetTree().Root.CreateTween();

    Value.SetIndexed("modulate:a", 1.0f);

    DefaultDespawnTween.Parallel().TweenProperty(Value, "position", Value.Position with { Y = Value.Position.Y - 50 }, .5f)
      .SetEase(Tween.EaseType.In)
      .SetTrans(Tween.TransitionType.Linear);

    DefaultDespawnTween.Parallel().TweenProperty(Value, "modulate:a", 0, .5f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Linear);
  }
}