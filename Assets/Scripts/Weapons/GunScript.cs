using UnityEngine;
using System.Collections;

public abstract class GunScript : WeaponScript
{
  protected Transform origin;
  public int penetration = 1;

  void Awake()
  {
    origin = transform.FindChild("origin");
  }
}
