using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RebindMenu : MonoBehaviour
{
  bool showingMenu = false;

  [SerializeField]
  string menuButton = "Menu";

  RebindData rebindData;
  Dictionary<string, RebindKey> rebindKeys;

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

      foreach (string key in rebindKeys.Keys)
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label(key);

        string keyName;
        if (rebindKeys[key].type == RebindKey.Type.Button) keyName = rebindKeys[key].keyCode.ToString();
        else keyName = rebindKeys[key].axisName + (rebindKeys[key].axisPositive ? "+" : "-");

        if (GUILayout.Button(keyName))
        {
          rebindData.BindKey(key);
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
