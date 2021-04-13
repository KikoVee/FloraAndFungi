﻿using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;


	void Awake () {
		SelectColor(0);
	}

	void Update () {
		if (
			Input.GetMouseButtonDown(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}
	}

	/*public void ChangeColor(HexCell currentCell)
	{
		Vector3 pos = currentCell.transform.position;
		hexGrid.ColorCell(pos, activeColor);

	}*/

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			//if (hit.collider.gameObject == hexGrid.gameObject)
			{
				EditCell(hexGrid.GetCell(hit.point));
			}
		}
	}

	void EditCell(HexCell cell)
	{
		cell.Color = activeColor;
		//hexGrid.Refresh();
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
}