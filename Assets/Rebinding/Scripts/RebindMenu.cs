using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RebindMenu : MonoBehaviour
{
  bool showingMenu = false;

  [SerializeField]
  string menuButton = "Menu";

  RebindData rebindData;
  List<RebindKey> rebindKeys;

  void Start()
  {
    rebindData = RebindData.GetRebindManager();
    rebindKeys = rebindData.GetCurrentKeys();
  }

  void Update()
  {
    if (rebindData.GetKeyDown(menuButton)) showingMenu = !showingMenu;
  }

  void OnGUI()
  {
    if (showingMenu)
    {
      GUILayout.BeginVertical("box");

      GUILayout.Label("Keybindings");

      GUILayout.BeginHorizontal();
      GUILayout.Label("Key");
      GUILayout.Label("Code");
      GUILayout.EndHorizontal();

      for (int i = 0; i < rebindKeys.Count; ++i)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(rebindKeys[i].name);

        string keyName;
        if (rebindKeys[i].type == RebindKey.Type.Button) keyName = rebindKeys[i].keyCode.ToString();
        else keyName = rebindKeys[i].axisName + (rebindKeys[i].axisPositive ? "+" : "-");

        if (GUILayout.Button(keyName))
        {
          rebindData.BindKey(rebindKeys[i].name);
        }

        GUILayout.EndHorizontal();
      }

      GUILayout.BeginHorizontal();

      if (GUILayout.Button("Save Keys"))
      {
        rebindData.SaveKeys();
      }

      if (GUILayout.Button("Restore to Defaults"))
      {
        rebindData.RestoreDefaultKeys();
        rebindKeys = rebindData.GetCurrentKeys();
      }

      GUILayout.EndHorizontal();

      GUILayout.EndVertical();
    }
  }
}
