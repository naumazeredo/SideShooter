using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
  // Player specific
  public int playerID = 1;

  // Rebinding
  RebindData keys;
  /*
  [SerializeField]
  string keyRight,
    keyLeft,
    keyJet,
    keyFire,
    keySkill1,
    keySkill2,
    keySkill3,
    keySkill4;
  */

  // Move
  bool lookingRight = true;
  Vector2 lookingDir = Vector2.right;
  public float speedBase = 2.5f;
  public float speedMod = 1f;
  [System.NonSerialized] public bool canMove = true;
  [System.NonSerialized] public bool onGround = false;

  // Jetpack
  [System.NonSerialized] public bool jetActive = false;
  [System.NonSerialized] public bool jetCanActive = true;
  public float jetForceBase = 13f;
  public float jetForceMod = 1f;
  float jetMaxTime = 0.5f;
  float jetCurTime = 0f;
  ParticleSystem jetEmitter;

   // Skills
  //public SkillScript[] skill = new SkillScript[4];
  public SkillScript[] skill = new SkillScript[4];

  // Collision
  Transform colDL,
    colDM,
    colDR,
    colML,
    colMR,
    colUL,
    colUM,
    colUR;

  // Use this for initialization
  void Start()
  {
    // Rebinding
    keys = RebindData.GetRebindManager();

    // Jetpack
    jetEmitter = transform.FindChild("jetpack").GetComponent<ParticleSystem>();

    // Get Basic Skills
    skill[0] = GetComponent<JumpSkillScript>();
    skill[1] = GetComponent<DashSkillScript>();

    // Collision
    BoxCollider2D box = GetComponent<BoxCollider2D>();
    Vector2 center = box.center, size = box.size;
    colDL = transform.FindChild("collision").FindChild("dl").transform;
    colDM = transform.FindChild("collision").FindChild("dm").transform; 
    colDR = transform.FindChild("collision").FindChild("dr").transform;
    colML = transform.FindChild("collision").FindChild("ml").transform;
    colMR = transform.FindChild("collision").FindChild("mr").transform;
    colUL = transform.FindChild("collision").FindChild("ul").transform;
    colUM = transform.FindChild("collision").FindChild("um").transform;
    colUR = transform.FindChild("collision").FindChild("ur").transform;

    colDL.localPosition = center + new Vector2(-size.x / 2f, -size.y / 2f);
    colDM.localPosition = center + new Vector2(0f, -size.y / 2f);
    colDR.localPosition = center + new Vector2(size.x / 2f, -size.y / 2f);
    colML.localPosition = center + new Vector2(-size.x / 2f, 0f);
    colMR.localPosition = center + new Vector2(size.x / 2f, 0f);
    colUL.localPosition = center + new Vector2(-size.x / 2f, size.y / 2f);
    colUM.localPosition = center + new Vector2(0f, size.y / 2f);
    colUR.localPosition = center + new Vector2(size.x / 2f, size.y / 2f);
  }

  // Update is called once per frame
  void Update()
  {
    // Jetpack
    if (jetActive && !jetEmitter.enableEmission) jetEmitter.enableEmission = true;
    else if (!jetActive && jetEmitter.enableEmission) jetEmitter.enableEmission = false;
  }

  void FixedUpdate()
  {
    InputMovement();

    // if falling, verify if lost ground contact
    if (rigidbody2D.velocity.y < 0 && onGround) IsGrounded();
  }

  // Collision
  void OnCollisionEnter2D(Collision2D coll)
  {
    IsGrounded();
  }

  public Vector2 GetDirection()
  {
    return lookingDir;
  }

  bool CanMoveForward(Vector2 dir)
  {
    RaycastHit2D hit1, hit2, hit3;
    hit1 = Physics2D.Raycast(colDR.position, dir, 0.05f, 1 << 8);
    hit2 = Physics2D.Raycast(colMR.position, dir, 0.05f, 1 << 8);
    hit3 = Physics2D.Raycast(colUR.position, dir, 0.05f, 1 << 8);
    if (hit1 || hit2 || hit3) return false;
    else return true;
  }

  // Input Handler
  private void InputMovement()
  {
    // Basic movement
    if (canMove)
    {
      // Rebinding
      if (keys.GetKey("P" + playerID + "Right") && CanMoveForward(lookingDir))
      {
        lookingRight = true;
        rigidbody2D.velocity = new Vector2(speedBase * speedMod, rigidbody2D.velocity.y);
      }
      else if (keys.GetKey("P" + playerID + "Left") && CanMoveForward(lookingDir))
      {
        lookingRight = false;
        rigidbody2D.velocity = new Vector2(-speedBase * speedMod, rigidbody2D.velocity.y);
      }
      else
      {
        rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
      }
    }

    // Update direction
    if (lookingRight && transform.localScale.x != 1f)
    {
      transform.localScale = new Vector3(1f, transform.localScale.y, 1f);
      lookingDir = Vector2.right;

      // SHITTY FIX for jetpack
      jetEmitter.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else if (!lookingRight && transform.localScale.x != -1f)
    {
      transform.localScale = new Vector3(-1f, transform.localScale.y, 1f);
      lookingDir = -Vector2.right;

      // SHITTY FIX for jetpack
      jetEmitter.transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    // Skills
    // Initiate skills with input
    for (int i = 1; i <= 4; ++i)
    {
      if (keys.GetKeyDown("P" + playerID + "Skill" + i))
      {
        if (skill[i - 1]) skill[i - 1].PerformSkill();
      }
    }

    // Jetpack
    // Jetpack counts time when is activated
    // When it turns deactivated or the max time using it reaches
    // it begins to count down 'til it reaches zero
    // It can only be reactivated when the time reaches zero
    if (jetActive)
    {
      if (jetCurTime < jetMaxTime)
      {
        jetCurTime += Time.deltaTime;
      }
      else
      {
        jetCurTime = jetMaxTime;
        jetActive = false;
      }
    }
    else
    {
      if (jetCurTime > 0f)
      {
        jetCurTime -= Time.deltaTime;
      }
      else
      {
        jetCurTime = 0f;
        jetCanActive = true;
      }
    }

    if (keys.GetKeyDown("P" + playerID + "Jet") && !jetActive && jetCanActive)
    {
      jetActive = true;
      jetCanActive = false;
    }

    if (keys.GetKeyUp("P" + playerID + "Jet"))
    {
      jetActive = false;
    }

    if (keys.GetKey("P" + playerID + "Jet") && jetActive)
    {
      // Add force
      rigidbody2D.AddForce(Vector2.up * jetForceBase * jetForceMod);
      IsGrounded();
    }
  }

  private void IsGrounded()
  {
    RaycastHit2D hit1 = Physics2D.Raycast(colDL.position, -Vector2.up, 0.05f, 1 << 8);
    RaycastHit2D hit2 = Physics2D.Raycast(colDM.position, -Vector2.up, 0.05f, 1 << 8);
    RaycastHit2D hit3 = Physics2D.Raycast(colDR.position, -Vector2.up, 0.05f, 1 << 8);
    if (hit1 || hit2 || hit3) onGround = true;
    else onGround = false;
  }

  public void SetGravityInfluence(bool influenced)
  {
    if (influenced) rigidbody2D.gravityScale = 1f;
    else rigidbody2D.gravityScale = 0f;
  }
}
