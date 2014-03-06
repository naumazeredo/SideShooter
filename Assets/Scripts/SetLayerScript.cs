using UnityEngine;
using System.Collections;

public class SetLayerScript : MonoBehaviour
{
  public string layer = "";

  // Use this for initialization
  void Start()
  {
    renderer.sortingLayerName = layer;
  }
}
