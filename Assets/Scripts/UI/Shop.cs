using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
	[SerializeField] private List<Weapon> weapons;

	[SerializeField] private List<Button> weaponOptions;

	[SerializeField] private GameObject weaponsPanel;

	[SerializeField] private Button buttonPrefab;

	private void OnEnable()
	{

		if (GameManager.Instance == null)
			return;
		if (GameManager.Instance.Player == null)
			return;

		Character player = GameManager.Instance.Player?.GetComponent<Character>();

		if (player == null)
			return;

		List<int> upgrads = player.GetWeapon().Upgrades;

		if (weaponOptions != null)
		{
			foreach (Button i in weaponOptions)
				Destroy(i.gameObject);
			weaponOptions.Clear();
		}

		if (upgrads != null && upgrads.Count != 0)
		{
			foreach (int i in upgrads)
			{
				Button b = Instantiate(buttonPrefab, weaponsPanel.transform);
				weaponOptions.Add(b);

				Weapon weapon = weapons[i];

				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(() => { ChooseWeapon(weapon); });
				b.GetComponent<Image>().sprite = weapon.WeaponSprite;
			}
		}
		else
		{
			int mask = 0;
			int index;

			for(int i = 0; i < 3; i++)
			{
				Button b = Instantiate(buttonPrefab, weaponsPanel.transform);
				weaponOptions.Add(b);

				do
				{
					index = Random.Range(0, weapons.Count);
				} while ((mask & (1 << index)) != 0);

				mask |= 1 << index;

				Weapon weapon = weapons[index];

				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(() => { ChooseWeapon(weapon); });
				b.GetComponent<Image>().sprite = weapon.WeaponSprite;
			}
		}
	}

	public void ChooseWeapon(Weapon weapon)
	{
		GameManager.Instance.Player.GetComponent<Character>().SetWeapon(weapon);

		GameManager.Instance.CloseShop();
	}

	public void Heal()
	{
		GameManager.Instance.Player.GetComponent<Character>().Heal();

		GameManager.Instance.CloseShop();
	}
}