using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class TileController : MonoBehaviour
{
    public enum TileType{
        Grass,
        City,
        Road
    }

    private TileConnector[] tileConnectors;

    public bool starterTile = false;
    [HideInInspector] public bool canPlace = false;
    [HideInInspector] public bool tilePlaced = false;

    private SpriteRenderer renderer;

    private void Awake() {
        tileConnectors = GetComponentsInChildren<TileConnector>();
        ToggleConnectors(false);
        tilePlaced = starterTile;

        if (starterTile) {
            GenerateTile("CRGR0");
        }
    }

    public void ToggleConnectors(bool isOn) {
        foreach (var connector in tileConnectors) {
            connector.isOn = isOn;
        }
    }

    public string SpriteName() {
        //Create the string based on tile types
        string spriteName = "";
        foreach (var con in tileConnectors) {
            if (con.tileType == TileType.City) {
                spriteName += "C";
            }else if (con.tileType == TileType.Grass) {
                spriteName += "G";
            }else if (con.tileType == TileType.Road) {
                spriteName += "R";
            }
        }

        //Get the string number
        var allSprites = Resources.LoadAll("TileSprites", typeof(Sprite));
        List<string> spriteNames = new List<string>();
        foreach (var sprite in allSprites) {
            if (sprite.name.Contains(spriteName)) {
                spriteNames.Add(sprite.name);
            }
        }
        if (spriteNames.Count == 0) {
            Debug.LogError("Tile: " + spriteName + " is not real! Returning big city boy.");
            return "CCCC0";
        }
        return spriteNames[Random.Range(0, spriteNames.Count)];
    }

    public void GenerateTile(string tileId) {
        //Get the string
        for (int i = 0; i < tileConnectors.Length; i++) {
            switch (tileId[i]) {
                case 'G':
                    tileConnectors[i].tileType = TileType.Grass;
                    break;
                case 'C':
                    tileConnectors[i].tileType = TileType.City;
                    break;
                case 'R':
                    tileConnectors[i].tileType = TileType.Road;
                    break;
            }
        }
        
        renderer = GetComponentInChildren<SpriteRenderer>();
        var spriteName = tileId.Length == 4 ? SpriteName() : tileId;
        renderer.sprite = Resources.Load<Sprite>("TileSprites/" + spriteName);
        gameObject.name = "Tile_" + SpriteName();
    }
}
