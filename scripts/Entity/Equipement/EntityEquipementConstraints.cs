using System;

namespace Entity;

[Flags]
public enum EntityEquipmentSlotType
{
  NONE = 1 << 0,

  ANY = 1 << 1,

  LEFT_HAND = 1 << 2,
  RIGHT_HAND = 1 << 3,

  HELMET = 1 << 4,
  ARMOR = 1 << 5,
  NECK = 1 << 6,
  BACK = 1 << 7,
  LEGS = 1 << 8,
  BOOTS = 1 << 9,

  ACCESSORY_LEFT = 1 << 10,
  ACCESSORY_RIGHT = 1 << 11,
}
