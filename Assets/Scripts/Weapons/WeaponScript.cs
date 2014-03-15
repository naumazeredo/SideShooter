using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
  protected PlayerScript player;

  protected bool beingHeld = false;
  protected bool attacking = false;

  public int damageMin, damageVariation;
  public int damageMod;

  public void PickUpWeapon(PlayerScript p)
  {
    player = p;
    beingHeld = true;
  }

  public void DropWeapon()
  {
    player = null;
    beingHeld = false;
  }
}
