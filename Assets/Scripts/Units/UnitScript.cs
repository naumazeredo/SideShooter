using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour
{
  public int totalHealth = 100;
  public int currentHealth = 100;
  public bool alive = true;

  public void DamageUnit(int amount)
  {
    currentHealth -= amount;
    if (currentHealth <= 0)
    {
      currentHealth = 0;
      alive = false;
    }
  }

  public void HealUnit(int amount)
  {
    currentHealth += amount;
    if (currentHealth > totalHealth)
      currentHealth = totalHealth;
  }

  public void RessurectUnit()
  {
    alive = true;
  }
}
