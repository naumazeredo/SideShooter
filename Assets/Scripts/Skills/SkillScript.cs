using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerScript))]
public abstract class SkillScript : MonoBehaviour
{
  protected PlayerScript player;
  protected bool performingSkill = false;

  protected virtual void Start()
  {
    player = GetComponent<PlayerScript>();
  }

  void FixedUpdate()
  {
    if (performingSkill) StepSkill();
  }

  // Inherited members
  public abstract void PerformSkill();
  protected virtual void StepSkill() {}

  protected virtual void StopSkill()
  {
    performingSkill = false;
  }
}
