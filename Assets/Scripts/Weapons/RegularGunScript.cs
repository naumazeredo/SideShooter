using UnityEngine;
using System.Collections;

// Regular gun is a non-projectile gun
//public abstract class RegularGunScript : GunScript
public class RegularGunScript : GunScript
{
  public override void Attack()
  {
    base.Attack();

    RaycastHit2D[] hits = Physics2D.RaycastAll(VectorExt.xy(origin.position), playerMove.GetDirection(), 10, (1 << 8) + (1 << 9));
    if (hits.Length > 0)
    {
      int penetrationCount = 0;
      foreach (RaycastHit2D hit in hits)
      {
        if (hit.collider.tag == "Enemy")
        {
          // Hit Unit
          UnitScript unit = hit.transform.GetComponent<UnitScript>();
          HitUnit(unit, hit.point, penetrationCount);

          penetrationCount++;
          if (penetrationCount >= penetration)
            break;
        }
      }
    }
  }

  protected override void HitUnit(UnitScript unit, Vector2 hitPos, int penetration = 0)
  {
    // Create effects
    GameObject newblood = ObjectPoolScript.instance.GetObjectForType("blood", false);
    newblood.transform.position = VectorExt.V2toV3(hitPos);
    newblood.transform.localRotation = Quaternion.AngleAxis(-playerMove.GetDirection().x * 90, Vector3.up);

    // Damage unit
    int damage = damageMin + Random.Range(0, damageVariation);
    damage /= (penetration + 1);
    unit.DamageUnit(damage);
    unit.KnockbackUnit(playerMove.GetDirection() * knockbackForce);

    // Log
    Debug.Log("damaged " + damage.ToString() + " on unit " + unit.name);
  }
}
