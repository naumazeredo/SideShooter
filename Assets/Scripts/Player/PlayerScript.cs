using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
  // Player specific
  int playerID = 1;

  // Rebinding
  RebindData keys;

  // Weapon
  [SerializeField]
  WeaponScript weapon;

   // Skills
  [SerializeField]
  int skillCount = 2;
  //[SerializeField]
  SkillScript[] skill;

  // Techniques
  

  // Use this for initialization
  void Start()
  {
    // Rebinding
    keys = RebindData.GetRebindManager();

    // Weapon
    weapon.PickUpWeapon(this);

    // Get Basic Skills
    skill = new SkillScript[skillCount];
    skill[0] = GetComponent<JumpSkillScript>();
    skill[1] = GetComponent<DashSkillScript>();
  }

  void Update()
  {
    InputWeapon();
    InputSkills();
  }

  private void InputWeapon()
  {
    if (keys.GetKeyDown("P" + playerID + "Attack"))
    {
      if (weapon)
        weapon.Attack();
    }
  }

  private void InputSkills()
  {
    // Skills
    // Initiate skills with input
    for (int i = 0; i < skillCount; ++i)
    {
      if (keys.GetKeyDown("P" + playerID + "Skill" + i))
      {
        if (skill[i])
          skill[i].PerformSkill();
      }
    }
  }

  public int getPlayerID()
  {
    return playerID;
  }
}
