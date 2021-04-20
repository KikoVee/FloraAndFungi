using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
	public RectTransform uiRect;

	public Color Color
	{
		get { return color; }
		set
		{
			if (color == value)
			{
				return;
			}

			color = value;
			Refresh();
		}
	}
	public Vector3 Position
	{
		get { return transform.localPosition; }
	}
	Color color;
	public Color startColor;
	public HexGridChunk chunk;

	public enum cellType {empty, tree, fungi};

	public cellType myType;
	
	[SerializeField] private HexCell[] neighbors;

	private void Start()
	{
		color = startColor;
	}

	public HexCell GetNeighbor (HexDirection direction)
	{
		return neighbors [(int) direction];
	}

	public HexCell[] GetNeighbors()
	{
		HexCell[] cellNeighbors = neighbors;
		return cellNeighbors;
	}

	public void SetNeighbor(HexDirection direction, HexCell cell)
	{
		neighbors[(int) direction] = cell;
		cell.neighbors[(int) direction.Opposite()] = this;
	}
	

	void Refresh()
	{
		if (chunk)
		{
			chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++)
			{
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk)
				{
					neighbor.chunk.Refresh();
				}
			}
		}
	}

	public void SetType(int type)
	{
		if (type == 0)
		{
			myType = cellType.empty;
		}

		if (type == 1)
		{
			myType = cellType.tree;
		}

		if (type == 2)
		{
			myType = cellType.fungi;
		}
		
		//Debug.Log("cell type is" + myType);
	}

	

}