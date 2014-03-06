using UnityEngine;
using System.Collections;

[System.Serializable]
public class RebindKey
{
  public enum Type
  {
    Button,
    Axis
  }

  public string name = "";
  public Type type;
  public KeyCode keyCode = KeyCode.A;
  public string axisName = "";
  public bool axisPositive = true;

  [System.NonSerialized]
  public bool keyDownOld = false;

  [System.NonSerialized]
  public bool keyDown = false;

  public RebindKey()
  { }

  // Button constructor
  public RebindKey(string nname, KeyCode ncode)
  {
    name = nname;
    type = Type.Button;
    keyCode = ncode;
  }

  // Axis constructor
  public RebindKey(string nname, string naname, bool napos)
  {
    name = nname;
    type = Type.Axis;
    axisName = naname;
    axisPositive = napos;
  }
}
