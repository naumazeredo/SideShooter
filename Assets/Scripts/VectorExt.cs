using UnityEngine;
using System.Collections;

public static class VectorExt
{
  // Vector3 extension 
  public static Vector2 xy(this Vector3 vec)
  {
    return new Vector2(vec.x, vec.y);
  }

  public static Vector2 xz(this Vector3 vec)
  {
    return new Vector2(vec.x, vec.z);
  }

  public static Vector2 yz(this Vector3 vec)
  {
    return new Vector2(vec.y, vec.z);
  }

  // Vector2 extension
  public static Vector2 invert(this Vector2 vec)
  {
    return new Vector2(vec.y, vec.x);
  }

  public static Vector3 V2toV3(this Vector2 vec)
  {
    return new Vector3(vec.x, vec.y);
  }
}
