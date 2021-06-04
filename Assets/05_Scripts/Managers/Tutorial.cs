using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class Tutorial : MonoBehaviour
{
    [SerializeField] public GameObject[] tutorialText;
    [SerializeField] public int collectText;
    [SerializeField] public int upgradeText;

    private GameObject text;
    private Text oldText;
    private int textNumber = 0;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject upgradeButton;

    [SerializeField] private GameObject mushroom;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private float blendShape = 100f;
    private bool growing = false;
    public int growSpeed = 9;

    [SerializeField] private GameObject tree;
    [SerializeField] private ParticleSystem spores;


        
    // Start is called before the first frame update
    void Start()
    {
        text = tutorialText[0];
        text.SetActive(true);
        continueButton.SetActive(true);
        playButton.SetActive(false);
        _skinnedMeshRenderer = mushroom.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blendShape > 0 && growing)
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(0, blendShape);
            blendShape -= growSpeed * Time.deltaTime;
        }
        
        if (
            Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) 
        {
            RayCastInput();
                
        }
        
    }

    public void TutorialText()
    {
        if (textNumber < tutorialText.Length - 1)
        {
            GameObject oldText = text;
            textNumber += 1;
            text = tutorialText[textNumber];
            text.SetActive(true); 
            oldText.SetActive(false);
            
        }
        else
        {
            continueButton.SetActive(false);
            playButton.SetActive(true);
        }
        
        if (textNumber == collectText)
        {
            tree.SetActive(true);    
        }

        if (textNumber == upgradeText)
        {
            upgradeButton.SetActive(true);
            continueButton.SetActive(false);
        }
        
    }

    

    private void RayCastInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            {

                if (hit.collider.name.Equals("FungiObject-Menu"))
                {
                    growing = true;

                }

                if (hit.collider.name.Equals("TreeObject-MainMenu-02"))
                {
                    tree.GetComponent<Outline>().enabled = false;
                    
                }
            }

        }
    }

    public void Upgrade()
    {
        spores. Play();
        tree.GetComponent<TreeMainMenu>().MainMenuEffect();
        continueButton.SetActive(true);
        upgradeButton.SetActive(false);
    }
    
    public void Play()
    {
        FindObjectOfType<GameSceneManager>().LoadGame();
    }
    
    public void PlayClick()
    {
        FindObjectOfType<AudioManager>().Play("Button");
  
    }
    
}
