using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;
	public Transform mushroomPrefab;
	private NutrientManager _nutrientManager;


	void Awake () {
		SelectColor(0);
	}

	private void Start()
	{
		_nutrientManager = NutrientManager.currentNutrientManager;
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
			if (_nutrientManager.TrySpendSugarAmount(_nutrientManager.giveNutrientCost))
			{
				GameManager.currentManager.GiveTreesNutrients();
				_nutrientManager.SpendSugar(_nutrientManager.giveNutrientCost);
			}
			else
			{
				Debug.Log("not enough sugar");
			}
		}
	}


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