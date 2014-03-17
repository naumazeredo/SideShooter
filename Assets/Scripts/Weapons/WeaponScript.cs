using UnityEngine;
using System.Collections;

public abstract class WeaponScript : MonoBehaviour
{
  //protected PlayerScript player;
  //protected PlayerMovementScript playerMove;
  public PlayerScript player;
  public PlayerMovementScript playerMove;

  protected bool beingHeld = false;
  protected bool attacking = false;

  public int damageMin, damageVariation;
  //public int damageMod;
  public float attackRate = 1f;   // Attacks per second

  public void PickUpWeapon(PlayerScript p)
  {
    player = p;
    playerMove = player.GetComponent<PlayerMovementScript>();
    beingHeld = true;
    attacking = false;
  }

  public void DropWeapon()
  {
    player = null;
    playerMove = null;
    beingHeld = false;
    attacking = false;
  }

  public virtual void Attack()
  {
    if (player == null || !beingHeld) return;
    attacking = true;
  }

  protected abstract void HitUnit(UnitScript unit, Vector2 hitPos, int penetration);
}
