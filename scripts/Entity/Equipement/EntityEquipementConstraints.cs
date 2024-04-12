using System;

namespace Entity;

[Flags]
public enum EntityEquipmentSlotType
{
  ANY,

  LEFT_HAND,
  RIGHT_HAND,

  HELMET,
  ARMOR,
  NECK,
  BACK,
  LEGS,
  BOOTS,

  ACCESSORY_LEFT,
  ACCESSORY_RIGHT
}
