using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Weapon
{
	public float SweepingAngle;
	public float Damage;
	public float Knockback;
	public float AttackSpeed;
	public float AttackRange;

	public Sprite WeaponSprite;
	public Sprite SweepSprite;

	public AudioClip SwingSound;

	public List<int> Upgrades;

}
