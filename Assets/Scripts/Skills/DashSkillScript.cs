using UnityEngine;
using System.Collections;

public class DashSkillScript : TimedSkillScript
{
  public float dashSpeed = 5f;

  public override void PerformSkill()
  {
    performingSkill = true;
    player.canMove = false;
    player.jetCanActive = false;
    player.jetActive = false;
    PerformDash();
  }

  protected override void StepSkill()
  {
    base.StepSkill();
    PerformDash();
  }

  protected override void StopSkill()
  {
    base.StopSkill();
    player.rigidbody2D.velocity = new Vector2(0f, 0f);
    player.canMove = true;
  }

  void PerformDash()
  {
    player.rigidbody2D.velocity = player.GetDirection() * dashSpeed;
    player.jetCanActive = false;
  }
}
