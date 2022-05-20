using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConnector : MonoBehaviour
{
	public TileController.TileType tileType;
	private TileController tileController;
	public TileConnector connectedTo;
	[HideInInspector] public bool isOn;

	[HideInInspector] public bool valid = false;
	[HideInInspector] public bool emptySpace = true;

	public bool closed;
	
	private void Awake() {
		tileController = GetComponentInParent<TileController>();
	}

	private void OnTriggerEnter(Collider other) {
		if (isOn) {
			if (other.TryGetComponent(out TileConnector connector)) {
				if (connector.tileType == tileType) {
					connectedTo = connector;
					connector.connectedTo = this;
					valid = true;
				} else {
					valid = false;
				}
				emptySpace = false;
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (isOn) {
			tileController.canPlace = false;
			connectedTo = null;
			emptySpace = true;
			valid = false;
		}
	}

	private void OnDrawGizmos() {
		if (tileType == TileController.TileType.City) {
			Gizmos.color = Color.gray;
		}else if (tileType == TileController.TileType.Grass) {
			Gizmos.color = Color.green;
		} else {
			Gizmos.color = Color.white;
		}
		
		Gizmos.DrawSphere(transform.position, 0.05f);
	}
}
