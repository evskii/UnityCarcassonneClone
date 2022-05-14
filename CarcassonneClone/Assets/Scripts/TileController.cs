using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Start() {
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("TileSprites/" + SpriteName());
        gameObject.name = "Tile_" + SpriteName();
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
        Debug.Log(allSprites.Length);
        List<string> spriteNames = new List<string>();
        foreach (var sprite in allSprites) {
            if (sprite.name.Contains(spriteName)) {
                spriteNames.Add(sprite.name);
                Debug.Log(sprite.name);
            }
        }
        if (spriteNames.Count == 0) {
            Debug.LogError("Tile: " + spriteName + " is not real! Returning big city boy.");
            return "CCCC0";
        }
        return spriteNames[Random.Range(0, spriteNames.Count)];
    }
}
