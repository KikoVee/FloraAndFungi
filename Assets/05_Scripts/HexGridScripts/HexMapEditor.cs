using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;

	private Color activeColor;
	private NutrientManager _nutrientManager;
	private bool fungiNeighbor;


	void Awake () {
		SelectColor(0);
	}

	private void Start()
	{
		_nutrientManager = NutrientManager.currentNutrientManager;
		fungiNeighbor = true;
	}


	void Update () {
		if (
			Input.GetMouseButtonDown(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}

		/*if (Input.GetKeyDown(KeyCode.Space))
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
		}*/
	}


	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			{
				EditCell(hexGrid.GetCell(hit.point));
			}
		}
	}

	void EditCell(HexCell cell)
	{
		
		if (cell.myType == HexCell.cellType.empty)
		{
			CheckCellNeightbors(cell);

			if (_nutrientManager.TrySpendSugarAmount(_nutrientManager.expansionCost) && fungiNeighbor == true)
			{
				cell.Color = activeColor;
				cell.SetType(2);
				AddFeature(cell.Position);
				_nutrientManager.SpendSugar(_nutrientManager.expansionCost);
				fungiNeighbor = false;
				//hexGrid.Refresh();
			}
		}	
	}

	void CheckCellNeightbors(HexCell cell)
	{
		HexCell[] cells = cell.GetNeighbors();
		
		foreach (HexCell _cell in cells)
		{
			if (_cell.myType == HexCell.cellType.fungi)
			{
				fungiNeighbor = true;
				break;

			}
				
		}

	}

	void AddFeature(Vector3 position)
	{
		Transform mushroomPrefab;
		mushroomPrefab = GameManager.currentManager.fungiPrefab;
		Transform instance = Instantiate(mushroomPrefab);
		instance.localPosition = position; 
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
}