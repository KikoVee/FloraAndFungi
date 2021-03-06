using UnityEngine;

public class CameraController : MonoBehaviour
{
   
    
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    private Vector3 cameraPos;
    private float cameraRot;
    public float panlimitX;
    public float panlimitZ;
    public float rotlimitMin;
    public float rotlimitMax;
    public float scrollSpeed = 20f;
    public float minY;
    public float maxY;
    [SerializeField] private GameObject rotationObject;
    private Vector3 mouseOrigin;
    

    private void Start()
    {
        cameraPos = transform.position;
        cameraRot = transform.rotation.y;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float rotationY = transform.rotation.y;


        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <=  panBorderThickness)
        {
           
            pos.x -= panSpeed * Time.deltaTime;
        }

        /*if (Input.GetKey("q") || Input.GetKey("e"))
        {
            if (Input.GetKey("q"))
            {
                rotationY -= panSpeed * Time.deltaTime;
                rotationObject.transform.Rotate(0, rotationY, 0);
                rotationY = Mathf.Clamp(rotationY, (cameraRot - rotlimitMin), (cameraRot + rotlimitMax));

            }

            if (Input.GetKey("e"))
            {
                rotationY += panSpeed * Time.deltaTime;
                rotationObject.transform.Rotate(0, rotationY, 0);
                rotationY = Mathf.Clamp(rotationY, (cameraRot - rotlimitMin), (cameraRot + rotlimitMax));

            }
        }*/
        


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y += scroll * scrollSpeed * 100f * Time.deltaTime;
        
        pos.x = Mathf.Clamp(pos.x, (cameraPos.x - panlimitX), (cameraPos.x + panlimitX));
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, (cameraPos.z - panlimitZ), (cameraPos.z + panlimitZ));
        //rotationY = Mathf.Clamp(rotationY, (cameraRot - rotlimitMin), (cameraRot + rotlimitMax));

        transform.position = pos;
    }
}
