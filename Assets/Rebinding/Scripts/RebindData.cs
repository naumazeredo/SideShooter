using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RebindData : MonoBehaviour
{
  [SerializeField]
  List<RebindKey> defaultKeys;

  Dictionary<string, RebindKey> rebindKeys;
  List<RebindKey> savedKeys;

  List<KeyCode> keys;
  List<string> axes;
  public float axesDeadZone = 0.2f;

  bool binding = false;
  string bindingName = "";

  // Time to avoid repeated input
  float timeCur= 0f;
  bool timePassed = false;
  [SerializeField]
  float timeToWait = 0.2f;

  bool init = false;

#if UNITY_STANDALONE
  // Option to save keys on registry or on file in game folder
  [SerializeField]
  bool saveOnRegistry = true;

  [SerializeField]
  string saveFile = "config.txt";
#endif

  // Get Rebind Manager
  static public RebindData GetRebindManager()
  {
    return GameObject.Find("Rebind Manager").GetComponent<RebindData>();
  }

  void Init()
  {
    // Set default keys
    savedKeys = LoadKeys();
    rebindKeys = CopyKeysToDict(savedKeys);
    init = true;
  }

  // Start key list
  void Start()
  {
    if (!init) Init();

    // List all Keys
    KeyCode[] nk = (KeyCode[])Enum.GetValues(typeof(KeyCode));
    keys = new List<KeyCode>(nk);

    // Remove general joystick keys
    keys.Remove(KeyCode.JoystickButton0);
    keys.Remove(KeyCode.JoystickButton1);
    keys.Remove(KeyCode.JoystickButton2);
    keys.Remove(KeyCode.JoystickButton3);
    keys.Remove(KeyCode.JoystickButton4);
    keys.Remove(KeyCode.JoystickButton5);
    keys.Remove(KeyCode.JoystickButton6);
    keys.Remove(KeyCode.JoystickButton7);
    keys.Remove(KeyCode.JoystickButton8);
    keys.Remove(KeyCode.JoystickButton9);
    keys.Remove(KeyCode.JoystickButton10);
    keys.Remove(KeyCode.JoystickButton11);
    keys.Remove(KeyCode.JoystickButton12);
    keys.Remove(KeyCode.JoystickButton13);
    keys.Remove(KeyCode.JoystickButton14);
    keys.Remove(KeyCode.JoystickButton15);
    keys.Remove(KeyCode.JoystickButton16);
    keys.Remove(KeyCode.JoystickButton18);
    keys.Remove(KeyCode.JoystickButton19);

    // List all Axes
    axes = new List<string>();
    for (int joy = 1; joy <= 4; ++joy)
    {
      for (int axis = 1; axis <= 7; ++axis)
      {
        axes.Add("J" + joy + "A" + axis);
      }
    }
  }

  // Since user must do an input to bind be successful
  // the program must repeat the binding until user inputs something
  bool TryToBindKey(string name)
  {
    RebindKey newkey = new RebindKey();
    newkey.name = name;
    bool keyAssigned = false;

    // Keyboard/Mouse
    foreach (KeyCode key in keys)
    {
      if (Input.GetKeyDown(key))
      {
        newkey.type = RebindKey.Type.Button;
        newkey.keyCode = key;
        keyAssigned = true;
        break;
      }
    }

    // Axes (Joystick axis!)
    if (!keyAssigned)
    {
      foreach (string joy in axes)
      {
        float value = Input.GetAxis(joy);
        if (Mathf.Abs(value) > 0.5f)
        {
          newkey.type = RebindKey.Type.Axis;
          newkey.axisName = joy;
          newkey.axisPositive = (value > 0);
          keyAssigned = true;
          break;
        }
      }
    }

    // Assign new key to rebindKey with same name
    if (keyAssigned)
    {
      rebindKeys[name] = newkey;
    }

    return keyAssigned;
  }

  public void BindKey(string name)
  {
    binding = true;
    bindingName = name;
    timeCur = 0f;
    timePassed = false;
  }

  public bool IsBinding()
  {
    return binding;
  }

  // Get Key
  public bool GetKey(string name)
  {
    // Avoids actions when binding
    if (binding) 
      return false;

    RebindKey key = new RebindKey();
    //key = FindKey(name);
    key = rebindKeys[name];

    if (key.type == RebindKey.Type.Button)
    {
      return Input.GetKey(key.keyCode);
    }
    else
    {
      return GetAxis(key);
    }
  }

  // Get Key Down
  public bool GetKeyDown(string name)
  {
    // Avoids actions when binding
    if (binding)
      return false;

    RebindKey key = new RebindKey();
    //key = FindKey(name);
    key = rebindKeys[name];

    if (key.type == RebindKey.Type.Button)
    {
      return Input.GetKeyDown(key.keyCode);
    }
    else
    {
      if (key.keyDown && !key.keyDownOld) return true;
      else return false;
    }
  }

  // Get Key Up
  public bool GetKeyUp(string name)
  {
    // Avoids actions when binding
    if (binding) return false;

    RebindKey key = new RebindKey();
    //key = FindKey(name);
    key = rebindKeys[name];

    if (key.type == RebindKey.Type.Button)
    {
      return Input.GetKeyUp(key.keyCode);
    }
    else
    {
      if (!key.keyDown && key.keyDownOld) return true;
      else return false;
    }
  }

  bool GetAxis(RebindKey key)
  {
    float value = Input.GetAxis(key.axisName);

    if (Mathf.Abs(value) <= axesDeadZone) return false;
    else if (key.axisPositive == (value > axesDeadZone)) return true; //XNOR
    else return false;
  }

  // Copy of list by value!
  List<RebindKey> CopyKeys(List<RebindKey> source)
  {
    List<RebindKey> copy = new List<RebindKey>();

    for (int i = 0; i < source.Count; ++i)
      copy.Add(source[i]);

    return copy;
  }

  // Copy of list by value!
  Dictionary<string, RebindKey> CopyKeysToDict(List<RebindKey> source)
  {
    Dictionary<string, RebindKey> copy = new Dictionary<string, RebindKey>();

    for (int i = 0; i < source.Count; ++i)
      copy.Add(source[i].name, source[i]);

    return copy;
  }

  /*
  public List<RebindKey> GetCurrentKeys()
  {
    if (!init) Init();
    return new List<RebindKey>(rebindKeys.Values);
  }
  */
  public Dictionary<string, RebindKey> GetCurrentKeys()
  {
    if (!init) Init();
    return rebindKeys;
  }

  public void RestoreDefaultKeys()
  {
    rebindKeys = CopyKeysToDict(defaultKeys);
  }

  public void SaveKeys()
  {
    string prefsToSave= "";

    string keyNames = "";
    string keyType = "";
    string keyCode = "";
    string keyAxisName = "";
    string keyAxisPositive = "";

    foreach(RebindKey key in rebindKeys.Values)
    {
      keyNames += key.name + "*";
      keyType += key.type.ToString() + "*";
      keyCode += ((int)key.keyCode).ToString() + "*";
      keyAxisName += key.axisName + "*";
      keyAxisPositive += key.axisPositive.ToString() + "*";
    }

    prefsToSave = keyNames + "\n" + keyType + "\n" + keyCode + "\n" + keyAxisName + "\n" + keyAxisPositive;

#if UNITY_STANDALONE
    if (saveOnRegistry) PlayerPrefs.SetString("RebindKeys", prefsToSave);
    else File.WriteAllText(saveFile, prefsToSave);
#else
    PlayerPrefs.SetString("RebindKeys", prefsToSave);
#endif

  }

  List<RebindKey> LoadKeys()
  {
    string prefsLoaded = "";

#if UNITY_STANDALONE
    if (saveOnRegistry) prefsLoaded = PlayerPrefs.GetString("RebindKeys");
    else if (File.Exists(saveFile)) prefsLoaded = File.ReadAllText(saveFile);
#else
    prefsLoaded = PlayerPrefs.GetString("RebindKeys");
#endif

    // If no preferences to load, load defaults
    if (prefsLoaded == "") return CopyKeys(defaultKeys);

    string[] prefsSplit = prefsLoaded.Split("\n".ToCharArray());

    string[] keyName = prefsSplit[0].Split("*".ToCharArray());
    string[] keyType = prefsSplit[1].Split("*".ToCharArray());
    string[] keyCode = prefsSplit[2].Split("*".ToCharArray());
    string[] keyAxisName = prefsSplit[3].Split("*".ToCharArray());
    string[] keyAxisPositive = prefsSplit[4].Split("*".ToCharArray());

    List<RebindKey> loadedKeys = new List<RebindKey>();

    for (int i = 0; i < keyName.Length -1; ++i)
    {
      RebindKey.Type type = (RebindKey.Type)Enum.Parse(typeof(RebindKey.Type), keyType[i]);
      if (type == RebindKey.Type.Button) loadedKeys.Add(new RebindKey(keyName[i], (KeyCode)int.Parse(keyCode[i])));
      else loadedKeys.Add(new RebindKey(keyName[i], keyAxisName[i], bool.Parse(keyAxisPositive[i])));
    }

    return loadedKeys;
  }

  void Update()
  {
    if (binding)
    {
      // Time to avoid repeated clicks
      if (!timePassed)
      {
        timeCur += Time.deltaTime;
        if (timeCur >= timeToWait) timePassed = true;
      }
      else binding = !TryToBindKey(bindingName);
    }
    else
    {
      // Update state of every joy axis key
      // This is necessary to simulate GetKeyDown and GetKeyUp with axis
      foreach (string key in rebindKeys.Keys)
      {
        if (rebindKeys[key].type == RebindKey.Type.Axis)
        {
          rebindKeys[key].keyDownOld = rebindKeys[key].keyDown;
          rebindKeys[key].keyDown = GetAxis(rebindKeys[key]);
        }
      }
    }
  }
}
