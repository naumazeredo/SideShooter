using UnityEngine;
using System.Collections;

public abstract class TimedSkillScript : SkillScript
{
  protected float currentTime = 0f;
  public float skillMaxTime = 1f;

  protected override void StepSkill()
  {
    currentTime += Time.deltaTime;
    if (currentTime >= skillMaxTime) StopSkill();
  }

  protected override void StopSkill()
  {
    base.StopSkill();
    currentTime = 0f;
  }
}
