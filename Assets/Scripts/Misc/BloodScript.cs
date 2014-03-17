using UnityEngine;
using System.Collections;

public class BloodScript : MonoBehaviour
{
  ParticleSystem particle;

  void Awake()
  {
    particle = GetComponent<ParticleSystem>();
  }

  void Update()
  {
    if (!particle.IsAlive())
      GetComponent<SelfPoolScript>().PoolObject();
  }
}
