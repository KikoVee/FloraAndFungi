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

	Color color;
	public bool walkable;
	public HexGridChunk chunk;

	[SerializeField] private HexCell[] neighbors;
	
	public HexCell GetNeighbor (HexDirection direction)
	{
		return neighbors [(int) direction];
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
}