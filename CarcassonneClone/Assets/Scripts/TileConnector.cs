using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConnector : MonoBehaviour
{
	public TileController.TileType tileType;
	private TileController tileController;
	public TileController connectedTo;
	public bool isOn;

	private void Awake() {
		tileController = GetComponentInParent<TileController>();
	}

	private void OnTriggerEnter(Collider other) {
		if (isOn) {
			if (other.TryGetComponent(out TileConnector connector)) {
				if (connector.tileType == tileType) {
					tileController.canPlace = true;
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (isOn) {
			tileController.canPlace = false;
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
