using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CollectWater: MonoBehaviour {
	[SerializeField] private int maxAmt;
	[SerializeField] private InputReader inputReader;
	[SerializeField] private GameObject popupPrompt;
	[SerializeField] private GameObject seedsManager;

	private bool isPlayerNearby = false;

	private void Awake() {
		popupPrompt.SetActive(false);
	}

	private void OnEnable() {
		inputReader.InteractEvent += collectInteraction;
	}

	private void OnDisable() {
		inputReader.InteractEvent -= collectInteraction;
	}

	private void collectInteraction() {
		if (isPlayerNearby) {
			if (PlayerInv.water < maxAmt) {
				PlayerInv.water++;
				Debug.Log("<color=blue> COLLECTED ANOTHER WATER.</color>");
			} else {
				Debug.Log("<color=red>MAX WATER AMOUNT REACHED</color>.");
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			isPlayerNearby = true;
			popupPrompt.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			isPlayerNearby = false;
			popupPrompt.SetActive(false);
		}
	}
}
