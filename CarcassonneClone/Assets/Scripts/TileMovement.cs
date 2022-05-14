using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private bool followMouse = false; 

    private Vector3 startPos; 
    private Vector3 pickupPos;

    private TileController tileController;
    

    private void Start() {
        startPos = transform.position;
        tileController = GetComponent<TileController>();
    }

    private void OnMouseDown() {
        if (!tileController.tilePlaced) { 
            followMouse = true;
            tileController.ToggleConnectors(true);
            pickupPos = transform.position;
        }
    }

    private void OnMouseUp() {
        followMouse = false;
        tileController.ToggleConnectors(false);

        if (tileController.canPlace) { 
            tileController.tilePlaced = true;
            enabled = false;
        } else {
            transform.position = pickupPos;
        }
    }

    private void Update() {
        if (followMouse) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast (ray, out hit)) {
                 transform.position = PositionAsGrid(new Vector3(hit.point.x, startPos.y, hit.point.z)); 
            }
        }
    }

    //This converts our position to whole numbers
    private Vector3 PositionAsGrid(Vector3 rawPos) {
        return new Vector3(Mathf.RoundToInt(rawPos.x), rawPos.y, Mathf.RoundToInt(rawPos.z));
    }
}
