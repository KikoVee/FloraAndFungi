using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private GameManager _gameManager;
    private HexMapEditor _hexEditor;
    private NutrientManager _nutrientManager;

    private void Start()
    {
        _gameManager = GameManager.currentManager;
        _hexEditor = _gameManager._hexMapEditor;
        _nutrientManager = NutrientManager.currentNutrientManager;
    }

    private void Update()
    {
        if (_gameManager.turnEndSequence != true)
        {
            if (
                Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) 
            {
                HandleInput();
                
            }
        }    
    }
    
    public void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            {

                if (hit.collider.GetComponent<HexMesh>())
                {
                    _hexEditor.HitCell(hit);
                   

                }

                if (hit.collider.GetComponent<Collectables>())
                {
                    Destroy(hit.transform.gameObject);
                    _nutrientManager.AddSugar(1);

                }

                if (hit.collider.GetComponent<TreeBehaviour>())
                {
                    hit.transform.gameObject.GetComponent<TreeBehaviour>().CollectSugar();
                }
            }
        }
    }
    
}
