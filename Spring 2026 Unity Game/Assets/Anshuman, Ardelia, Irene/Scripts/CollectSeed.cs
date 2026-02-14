using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CollectSeed : MonoBehaviour {
	[SerializeField] private string name;
	[SerializeField] private int maxAmt;
	[SerializeField] private InputReader inputReader;
	[SerializeField] private GameObject popupPrompt;
	[SerializeField] private GameObject seedsManager;

	private bool isPlayerNearby = false;
	private Dictionary<string, int> seeds;

	private void Awake() {
		popupPrompt.SetActive(false);
		seeds = Seeds.seeds;
	}

	private void OnEnable() {
		inputReader.InteractEvent += collectInteraction;
	}

	private void OnDisable() {
		inputReader.InteractEvent -= collectInteraction;
	}

	private void collectInteraction() {
		if (isPlayerNearby) {
			if (seeds.ContainsKey(name)) {
				if (seeds[name] < maxAmt) {
					seeds[name]++;
					Debug.Log("<color=green> COLLECTED ANOTHER " + name + " SEED.</color>");
				} else {
					Debug.Log("<color=red>MAX " + name + " SEED AMOUNT REACHED</color>.");
				}
			} else {
				seeds.Add(name, 1);
				Debug.Log("<color=green>COLLECTED " + name + " SEED.</color>");
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
