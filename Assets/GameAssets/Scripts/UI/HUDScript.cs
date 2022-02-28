using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDScript : MonoBehaviour
{
	public TMP_Text coins;
	public TMP_Text health;

	public string coinsFormat = "";
	public string healthFormat = "";

	private GameMasterScript gm;
	private AmongUs player;


	protected virtual void OnCoinsUpdated()
	{
		coins.text = gm.coins.ToString(coinsFormat);
	}

	protected virtual void OnHealthUpdated()
	{
		health.text = player.health.currentHp.ToString(healthFormat);
	}

	public virtual void Refresh()
	{
		OnCoinsUpdated();
		OnHealthUpdated();
	}

	private void Start()
	{
		gm = GameMasterScript.instance;
		player = FindObjectOfType<AmongUs>();

		//Connect this to other events
		gm.OnCoinsUpdated.AddListener(OnCoinsUpdated);
		gm.OnHealthUpdated.AddListener(OnHealthUpdated);
		player.health.OnChange.AddListener(OnHealthUpdated);

		Refresh();
	}
}
