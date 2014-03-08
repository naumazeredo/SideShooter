SideShooter (temporary name)
===========

*Style*: 2D Rogue-like semiRPG Sci-fi

Design
------
- All players start equal. No classes, races or unique weapons (maybe races or classes, later)
- Players have a weapon equipped, a weapon on his back, a jetpack, three skills and two techniques (maybe equipments too, later)
- _Skills_ are simple actions (e.g.: dash, hight jump, taunt, retreat)
- _Techniques_ are unique abilities that perform some kind of special effect, sometimes requiring the player to be equipped with some type of weapon/equipment

- Players can move right and left, shoot/attack and use a jetpack
- Weapons can be close-combat or ranged
- Ranged weapons can have projectiles or not (small bullet guns will have no projectiles. Rocket-propellers and bows/crossbows must have)

- Players start with a set of basic _Skills_ (Dash, Retreat, Taunt, Roll) and a set of basic _Techniques_ (Rapid-fire, Explosive-shot, Pierce-shot)
- Players learn new _skills_ and _techniques_ by satisfying certain conditions
- _Skills_ are learned only by a combination and use of other _skills_
- _Techniques_ require special conditions and combinations to be learned. Generally, it requires a type of weapon to be equipped and some skills to be used (e.g.: Omnislash will be learned if the player dashes through a serie of enemies attacking with a sword-type weapon). It can also require some environment characteristic in conjuction with other specific events/actions.

- Players can carry two weapons, one equipped and the other carried in he's back (with certain _techniques_ he can carry some more (e.g.: Dual Wielding Sword let the player equip a pair of swords and carry a pair. ).

####Jetpack and Jump
- Jump modifies the player's velocity instantly, giving a initial impulse
- Jetpack _must_ not give a great acceleration (it can cancels the gravity, maybe a little bit more)

####Skills
#####Basic Skills
- Dash: player thrusts forward a small distance, but he can be hit during it. Can be used mid-air. Can attack with melee weapons durint it
- Retreat: player thrusts backwards a small distance, but he can be hit during it. (used mid-air?)
- Taunt: player provokes all AI-controlled enemies in a small/medium range, forcing all enemies provoked to go attack him
- Sprint: player runs! (for a small time)
- Jump: yeah, player have to skill jump to jump...
- Crouch: player crouchs, reducing the collision box and enabling the player to shoot lower
- Defend: player enters a defensive state, blocking damage for a small time

#####Non-basic Skills
- Counter-attack (Defend): melee attacks after defend
- Roll (Defend + Dash): player rolls on ground, prevent all damage and avoiding all projectiles during the roll. Cannot be used mid-air


####Techniques
#####Format: name \[requirements\]\[damage\]\[cd\]: description
#####Basic Techniques
- Rapid-fire \[ranged bullet? weapons\]\[100%\]\[5s\]: player shoots 4 shots rapidly
- Explosive-shot \[ranged bullet? weapons\]\[120%\]\[8s\]: player shoots a explosive shot, causing damage in the area it exploded
- Pierce-shot \[ranged bullet? weapons\]\[100%\]\[5s\]: player shoots a piercing shot that goes through enemies, hitting more than one enemy

#####Sword Techniques
- Omnislash \[cutting close-combat weapons\]\[120%\]\[12s\]: player dashes through enemies, attacking them and avoiding any physical damage while dashing. Attacks 5 times maximum. Can hit the same enemy multiple times
- Dual Wielding \[sword-type weapons\]\[passive\]: lets player equip/carry two swords

#####Any Type
- Berserker \[any weapons / gloves?\]\[100%\]\[12s\]: player enters in a frenzy, attacking faster but receiving more damage
- Refraction (DotA): player avoids 7 instances of damage and gain bonus damage in 7 instances too

#####Passive Techniques
- Strygwyr's Blood (DotA): player gets faster and stronger for every weak enemy nearby

#####Naum's ideas
- Santoryu \[Roronoa Zoro\]: lets you equip three swords and carry other three. In reference to One Piece's Roronoa Zoro

####Guns

#####
- Mini-gun
- Rocket Propeller Guns: player shoots a rocket that goes straight forward. Than him enters a recharge period to be able to shoot again

#####Special
- Pewgun: Cazuza recorded "pew"
- Sniper's Rifle: weapon with a Risk of Rain counter after shot
- Spy's Watch (TF2 Spy's Watch): player turns invisible
- Buster Sword (FFVII Cloud's Sword)
- Pickaxe (Minecraft)
- Crying Tears (Binding of Isaac): player shoots tears

###To discuss
- Game Objective
- Game Environment (also what are the enemies)
- Boss prizes (items!)
- Swapping weapons during skirmish/campaign

####New Ideas
- Sidekickers(?): Small aliens that can help the players

####To Implement
- (High-end) Smoke particles when player dash/retreat touching the ground
- Weapons: Base Weapon Class (weapon weight, damage), Derived Gun Class (fire rate, bullet damage), Derived Projectile Gun Class (projectile, gun recoil, recharge time)
- Action Log: to learning mechanism
    Log every player weapon: saves it timestamp when initialized and saves its duration
- Skill/Technique/Action Hierarchy: to learning mechanism
    Tree per skill/technique/action
    Every branch saves a time in the past and another action player must have done to perform a new skill/technique

    Example:

    Jump Skill
    |
    |
    Crouch Skill -> still active => Perform "High Jump"

