using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private bool followMouse = false; 

    private Vector3 startPos; 
    private Vector3 pickupPos;

    private TileController tileController;

    private List<Vector3> allUsedPositions = new List<Vector3>();
    private bool positionUsed = false;

    private void Awake() {
        InitTile();

        var allTiles = FindObjectsOfType<TileController>();
        foreach (var tile in allTiles) {
            if (tile != this) {
                allUsedPositions.Add(tile.transform.position);
            }
        }
    }

    public void InitTile() {
        startPos = transform.position;
        tileController = GetComponent<TileController>();
        StartCoroutine(Jocks());
    }

    private void OnMouseDown() {
        OverrideMouseDown();
    }
    
    public void OverrideMouseDown() {
        Debug.Log("MouseDown");
        if (!tileController.tilePlaced) { 
            followMouse = true;
            tileController.ToggleConnectors(true);
            pickupPos = transform.position;
        }
    }


    public void OverrideMouseUp() {
        Debug.Log("MOUSEUP");
        if (positionUsed) {
            transform.position = pickupPos;
            return;
        }
        followMouse = false;
        tileController.ToggleConnectors(false);

        //Placement stuff happens here
        if (tileController.canPlace) { 
            tileController.tilePlaced = true;
            enabled = false;
            FindStart();
        } else {
            transform.position = pickupPos;
        }
    }

    private void Update() {
        if (followMouse) {
            
            positionUsed = allUsedPositions.Contains(PositionAsGrid(transform.position));
            
            CalculateScores();
            if (esScore + vaScore == 4 && vaScore > 0) {
                tileController.canPlace = true;
            } else {
                tileController.canPlace = false;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast (ray, out hit)) {
                 transform.position = PositionAsGrid(new Vector3(hit.point.x, startPos.y, hit.point.z)); 
            }

            if (Input.GetMouseButtonUp(0)) {
                OverrideMouseUp();
            }

            if (Input.GetMouseButtonDown(1)) {
                transform.Rotate(0, 90, 0);
            }
        }
    }
    
    private int esScore = 0;
    private int vaScore = 0;

    private void CalculateScores() {
        //Loop through chillers and add to scores
        esScore = 0;
        vaScore = 0;
        foreach (var connector in GetComponentsInChildren<TileConnector>()) {
            esScore += connector.emptySpace ? 1 : 0;
            vaScore += connector.valid ? 1 : 0;
        }
    }
        

    //This converts our position to whole numbers
    private Vector3 PositionAsGrid(Vector3 rawPos) {
        return new Vector3(Mathf.RoundToInt(rawPos.x), rawPos.y, Mathf.RoundToInt(rawPos.z));
    }

    private Transform roadStart = null; //Just used for drawing the road start for now
    
    private void FindStart() {
        //Get all connectors of this tile that are marked as valid (they connected to another tile of their type)
        var connectorsToCheck = GetComponentsInChildren<TileConnector>().Where(connector => connector.valid).ToList();
        
        TileConnector start = null;
        foreach (var connector in connectorsToCheck) {
            if (connector.tileType == TileController.TileType.Road) { //Only checking roads for now
                if (connector.closed) { //First check if this connector is the start
                    start = connector;
                    break;
                }
                //Create loop based variables
                bool startFound = false;
                TileConnector checking = connector;
                //Loop until we find the start (if there is no start we break from the loop)
                while (!startFound) {
                    if (checking.connectedTo == null) { //Checks if we are connected to a tile or not
                        startFound = true;
                    }
                    if (checking.closed) { //Checks if the tile we are currently checking is start
                        startFound = true;
                        start = checking;
                    }
                    if (checking.connectedTo.closed) { //Checks if the tile we are connected to is our start
                        startFound = true;
                        start = checking.connectedTo;
                    } else { //Runs if we haven't found a start
                        //If we get here then this means that this tile runs through to another edge, so we find that edge by getting in
                        //touch with their parents
                        TileController parent = checking.connectedTo.GetComponentInParent<TileController>();
                        var children = parent.GetComponentsInChildren<TileConnector>();
                        foreach (var child in children) {
                            if (child.tileType == checking.connectedTo.tileType && child != checking.connectedTo) {
                                checking = child;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (start != null) {
            roadStart = start.transform;
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        if (roadStart != null) {
            Gizmos.DrawSphere(roadStart.position, .25f);
        }
        
    }

    /// <summary>
    /// High level code here that debugs out if our jocks are jocks
    /// </summary>
    public IEnumerator Jocks() {
        yield return new WaitForSeconds(1);
        Debug.Log("<color=red>JOCKS</color>");
    }
}
