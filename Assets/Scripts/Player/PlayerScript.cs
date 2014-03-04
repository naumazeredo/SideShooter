using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
  // Move
  bool lookingRight = true;
  Vector2 lookingDir = Vector2.right;
  public float speed = 2.5f;

  // Jump
  public float jumpSpeed = 5.0f;
  bool onGround = false;
  public float jumpMaxTime = 0.3f;
  public float jumpCurTime = 0.0f;
  public bool jumpCountingTime = false; 

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
  }

  void FixedUpdate()
  {
    InputMovement();

    // if falling, verify if lost ground contact
    if (rigidbody2D.velocity.y < 0 && onGround)
    {
      RaycastHit2D hit1 = Physics2D.Raycast(colDL.position, -Vector2.up, 0.05f, 1 << 8);
      RaycastHit2D hit2 = Physics2D.Raycast(colDM.position, -Vector2.up, 0.05f, 1 << 8);
      RaycastHit2D hit3 = Physics2D.Raycast(colDR.position, -Vector2.up, 0.05f, 1 << 8);
      if (!(hit1 || hit2 || hit3))
      {
        onGround = false;
      }
    }
  }

  // Collision
  void OnCollisionEnter2D(Collision2D coll)
  {
    // Detect ground collision
    RaycastHit2D hit1 = Physics2D.Raycast(colDL.position, -Vector2.up, 0.05f, 1 << 8);
    RaycastHit2D hit2 = Physics2D.Raycast(colDM.position, -Vector2.up, 0.05f, 1 << 8);
    RaycastHit2D hit3 = Physics2D.Raycast(colDR.position, -Vector2.up, 0.05f, 1 << 8);
    if (hit1 || hit2 || hit3)
    {
      onGround = true;
      jumpCountingTime = false;
    }

    // Detect head collision
    if (!onGround && jumpCountingTime)
    {
      hit1 = Physics2D.Raycast(colUL.position, Vector2.up, 0.05f, 1 << 8);
      hit2 = Physics2D.Raycast(colUM.position, Vector2.up, 0.05f, 1 << 8);
      hit3 = Physics2D.Raycast(colUR.position, Vector2.up, 0.05f, 1 << 8);
      if (hit1 || hit2 || hit3)
      {
        jumpCountingTime = false;
        jumpCurTime = jumpMaxTime;
      }
    }
  }

  int GetDirection()
  {
    return (lookingRight ? 1 : -1);
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
    if (Input.GetKey(KeyCode.D) && CanMoveForward(lookingDir))
    {
      lookingRight = true;
      rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }
    else if (Input.GetKey(KeyCode.A) && CanMoveForward(lookingDir))
    {
      lookingRight = false;
      rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
    }
    else
    {
      // if not pressing left nor right, stop horizontal movement
      rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
    }

    // Update direction
    if (lookingRight && transform.localScale.x != 1f)
    {
      transform.localScale = new Vector3(1f, transform.localScale.y, 1f);
      lookingDir = Vector2.right;
    }
    else if (!lookingRight && transform.localScale.x != -1f)
    {
      transform.localScale = new Vector3(-1f, transform.localScale.y, 1f);
      lookingDir = -Vector2.right;
    }

    // Jump
    if (jumpCountingTime)
    {
      jumpCurTime += Time.deltaTime;
    }

    if (Input.GetKey(KeyCode.Space))
    {
      // Initial jump
      if (onGround)
      {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
        onGround = false;
        jumpCurTime = 0f;
        jumpCountingTime = true;
      }
      // Accumulation, so player can jump higher
      else if (jumpCurTime < jumpMaxTime)
      {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
      }
    }
  }
}
