using UnityEngine;
using System.Collections;

public class JumpSkillScript : SkillScript
{
  public float jumpSpeedBase = 4.0f;
  public float jumpSpeedMod = 1f;

  public override void PerformSkill()
  {
    if (player.onGround)
    {
      player.rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeedBase * jumpSpeedMod);
      player.onGround = false;
    }
  }
}
