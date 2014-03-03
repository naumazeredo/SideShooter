using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
  // Move
  bool lookingRight = true;
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
    colDL = transform.FindChild("collision").FindChild("dl").transform;
    colDM = transform.FindChild("collision").FindChild("dm").transform;
    colDR = transform.FindChild("collision").FindChild("dr").transform;
    colML = transform.FindChild("collision").FindChild("ml").transform;
    colMR = transform.FindChild("collision").FindChild("mr").transform;
    colUL = transform.FindChild("collision").FindChild("ul").transform;
    colUM = transform.FindChild("collision").FindChild("um").transform;
    colUR = transform.FindChild("collision").FindChild("ur").transform;
  }

  // Update is called once per frame
  void Update()
  {
    // Basic movement
    if (Input.GetKey(KeyCode.D) && CanMove(true))
    {
      lookingRight = true;
      rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
    }
    else if (Input.GetKey(KeyCode.A) && CanMove(false))
    {
      lookingRight = false;
      rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
    }
    else
    {
      // if not pressing left nor right, stop horizontal movement
      rigidbody2D.velocity = new Vector2(0f, rigidbody2D.velocity.y);
    }

    if (jumpCountingTime)
    {
      jumpCurTime += Time.deltaTime;
    }
  }

  void FixedUpdate()
  {
    // Jump
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
      else if (jumpCurTime <= jumpMaxTime)
      {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
      }
    }
    
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
  }

  int GetDirection()
  {
    return (lookingRight ? 1 : -1);
  }

  bool CanMove(bool right)
  {
    RaycastHit2D hit1, hit2, hit3;
    if (right)
    {
      hit1 = Physics2D.Raycast(colDR.position, Vector2.right, 0.05f, 1 << 8);
      hit2 = Physics2D.Raycast(colMR.position, Vector2.right, 0.05f, 1 << 8);
      hit3 = Physics2D.Raycast(colUR.position, Vector2.right, 0.05f, 1 << 8);
      if (hit1 || hit2 || hit3) return false;
      else return true;
    }
    else
    {
      hit1 = Physics2D.Raycast(colDL.position, -Vector2.right, 0.05f, 1 << 8);
      hit2 = Physics2D.Raycast(colML.position, -Vector2.right, 0.05f, 1 << 8);
      hit3 = Physics2D.Raycast(colUL.position, -Vector2.right, 0.05f, 1 << 8);
      if (hit1 || hit2 || hit3) return false;
      else return true;
    }
  }
}
