using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerScript))]
public abstract class SkillScript : MonoBehaviour
{
  protected PlayerMovementScript player;
  protected bool performingSkill = false;

  protected virtual void Awake()
  {
    player = GetComponent<PlayerMovementScript>();
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
