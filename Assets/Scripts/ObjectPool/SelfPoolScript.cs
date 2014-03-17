using UnityEngine;
using System.Collections;

public class SelfPoolScript : MonoBehaviour
{
  public void PoolObject()
  {
    ObjectPoolScript.instance.PoolObject(gameObject);
  }
}
