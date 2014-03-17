using UnityEngine;
using System.Collections;

public class DashSkillScript : TimedSkillScript
{
  public float dashSpeed = 5f;

  public override void PerformSkill()
  {
    performingSkill = true;
    player.canMove = false;
    player.jetFreeze = true;
    player.jetActive = false;
    PerformDash();
  }

  protected override void StepSkill()
  {
    base.StepSkill();

    // Player can grab the rope mid-dash
    if (player.onRope)
      StopSkill();
    else
      PerformDash();
  }

  protected override void StopSkill()
  {
    base.StopSkill();
    player.rigidbody2D.velocity = new Vector2(0f, 0f);
    player.canMove = true;
    player.jetFreeze = false;
  }

  void PerformDash()
  {
    player.rigidbody2D.velocity = player.GetDirection() * dashSpeed;
  }
}
