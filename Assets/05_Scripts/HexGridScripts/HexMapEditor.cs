using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

	public HexGrid hexGrid;
	public GameObject groundVisual;

	private Color activeColor;
	private NutrientManager _nutrientManager;
	private GameManager _gameManager;
	private bool fungiNeighbor;


	void Awake () {
		SelectColor(0);
	}

	private void Start()
	{
		_nutrientManager = NutrientManager.currentNutrientManager;
		_gameManager = GameManager.currentManager;
		fungiNeighbor = true;
	}


	

	public void HitCell(RaycastHit hit)
	{
		EditCell(hexGrid.GetCell(hit.point));
	}

	void EditCell(HexCell cell)
	{
		
		if (cell.myType == HexCell.cellType.empty)
		{
			CheckCellNeightbors(cell);

			if (_nutrientManager.TrySpendSugarAmount(_nutrientManager.expansionCost) && fungiNeighbor == true)
			{
				cell.Color = colors[Random.Range(0,2)];
				cell.SetType(2);
				AddFeature(cell.Position);
				_nutrientManager.SpendSugar(_nutrientManager.expansionCost);
				fungiNeighbor = false;
				ColorNeighbors(cell);
				CheckForTrees(cell);
				//hexGrid.Refresh();
			}
		}	
	}

	void ColorNeighbors(HexCell cell)
	{
		HexCell[] cells = cell.GetNeighbors();
		
		foreach (HexCell _cell in cells)
		{
			if (_cell.myType == HexCell.cellType.empty)
			{
				_cell.Color = colors[Random.Range(3,5)];
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

	void CheckForTrees(HexCell cell)
	{
		HexCell[] cells = cell.GetNeighbors();
		
		foreach (HexCell _cell in cells)
		{
			if (_cell.myType == HexCell.cellType.tree)
			{
				Transform tree = _cell.GetComponent<Transform>().transform;
				if (!_gameManager.touchedTrees.Contains(tree))
				{
					_gameManager.touchedTrees.Add(tree);
					Debug.Log(GameManager.currentManager.touchedTrees.Count);

				}
			}
				
		}
	}

	void AddFeature(Vector3 position)
	{
		Transform mushroomPrefab;
		mushroomPrefab = GameManager.currentManager.fungiPrefab;
		Transform instance = Instantiate(mushroomPrefab);
		instance.localPosition = position; 
		instance.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0,360),0));
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
	}
}