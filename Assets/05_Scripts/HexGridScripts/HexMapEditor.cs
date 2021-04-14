using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;
	public Transform mushroomPrefab;


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

		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameManager.currentManager.GiveTreesNutrients();
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
		cell.SetType(2);
		AddFeature(cell.Position);
		//hexGrid.Refresh();
	}

	void AddFeature(Vector3 position)
	{
		Transform instance = Instantiate(mushroomPrefab);
		instance.localPosition = position; 
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
}