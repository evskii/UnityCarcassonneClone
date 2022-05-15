using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;


public class DrawPile : MonoBehaviour
{
	public GameObject tileToSpawn;

	private List<String> allTileIds = new List<string>();
	private List<String> shuffledTiles = new List<string>();

	private void Awake() {
		InitAllTiles();
	}

	public void InitAllTiles() {
		//Load our base tiles and their quantities from a csv
		var dataFile = Resources.Load<TextAsset>("DataFiles/TileSpawns");
		foreach (var line in dataFile.text.Split('\n')) {
			var data = line.Split(',');
			if (String.IsNullOrEmpty(line)) { continue; }
			for (int i = 0; i < Int32.Parse(data[1]); i++) {
				allTileIds.Add(data[0]);
			}
		}
		
		//Shuffle
		var unusedTiles = allTileIds;
		while (unusedTiles.Count > 0) {
			var randomInt = UnityEngine.Random.Range(0, unusedTiles.Count);
			shuffledTiles.Add(unusedTiles[randomInt]);
			unusedTiles.Remove(unusedTiles[randomInt]);
		}
	}
	
	public void Draw() {
		if (shuffledTiles.Count > 0) {
			var newTile = Instantiate(tileToSpawn, transform.position, Quaternion.identity);
            newTile.GetComponent<TileController>().GenerateTile(shuffledTiles[0]);
            newTile.GetComponent<TileMovement>().OverrideMouseDown();
            shuffledTiles.Remove(shuffledTiles[0]);
		} else {
			Debug.Log("<color=magenta>All tiles used!</color>");
		}
		
	}

	private void OnMouseDown() {
		Draw();
	}
}
