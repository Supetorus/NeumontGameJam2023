using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[SerializeField] private List<Weapon> weapons;

	[SerializeField] private List<Button> weaponOptions;

	private void OnEnable()
	{
		int mask = 0;
		int index;

		foreach(Button i in weaponOptions)
		{
			do
			{
				index = Random.Range(0, weapons.Count);
			} while ((mask & (1 << index)) != 0);

			mask |= 1 << index;

			Weapon weapon = weapons[index];

			i.onClick.RemoveAllListeners();
			i.onClick.AddListener(() => { ChooseWeapon(weapon); });
			i.GetComponent<Image>().sprite = weapon.WeaponSprite;
		}
	}

	public void ChooseWeapon(Weapon weapon)
	{
		GameManager.Instance.Player.GetComponent<Character>().SetWeapon(weapon);

		GameManager.Instance.CloseShop();
	}
}