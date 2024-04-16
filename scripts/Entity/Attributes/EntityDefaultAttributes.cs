namespace Entity;

public record struct EntityDefaultAttributes()
{
  public EntityAttributeBase Agility = new EntityAttributes.Agility();
  public EntityAttributeBase Dexterity = new EntityAttributes.Dexterity();
  public EntityAttributeBase Intelligence = new EntityAttributes.Intelligence();
  public EntityAttributeBase Luck = new EntityAttributes.Luck();
  public EntityAttributeBase Strength = new EntityAttributes.Strength();
  public EntityAttributeBase Vitality = new EntityAttributes.Vitality();
}