using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private Vector2 mousePosition;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Input")]
    [SerializeField] private float distance = 100f;

    [Header("Script")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private ToggleObstacleScript toggleObstacleScript;
    [SerializeField] private GridSystemScript gridSystemScript;
    [SerializeField] private GridSizeScriptableObject gridTileObj;

    private void Update()
    {
        CheckKey();
    }
    private void CheckKey()
    {
        if(Input.GetMouseButtonDown(0) && gridSystemScript._selectGrid != null && playerScript._turnFinish)
        {
            playerScript.SendMessage("SetTargetIndex", gridSystemScript._selectGrid.GetComponent<GridScript>()._id);
        }
        if(Input.GetKeyDown(KeyCode.Q) && playerScript._turnFinish)
        {
            EditMode();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            IsomatricMode();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !playerScript._turnFinish)
        {
            CancelTurn();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            IncreaseCamera();
        }
        else if(Input.GetKeyDown(KeyCode.T))
        {
            DecreaseCamera();
        }
        else if(Input.GetKeyDown(KeyCode.Z) && playerScript._turnFinish)
        {
            ChangeGrid(false);
        }
        else if(Input.GetKeyDown(KeyCode.X) && playerScript._turnFinish)
        {
            ChangeGrid(true);
        }
    }

    public void EditMode()
    {
        cameraManager.SendMessage("ChangeCamera", false);
        toggleObstacleScript.SendMessage("ActivateToggle");
    }

    public void IsomatricMode()
    {
        cameraManager.SendMessage("ChangeCamera", true);
        toggleObstacleScript.SendMessage("DisableToggle");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void CancelTurn()
    {
        playerScript.SendMessage("Cancel");
    }
    public void IncreaseCamera()
    {
        cameraManager.SendMessage("ChangeCameraViewSize", true);
    }
    public void DecreaseCamera()
    {
        cameraManager.SendMessage("ChangeCameraViewSize", false);
    }

    public void ChangeGrid(bool isIncrease)
    {
        if(isIncrease)
        {
            gridTileObj.gridTile = new Vector2(gridTileObj.gridTile.x + 1f, gridTileObj.gridTile.y + 1f);
        }
        else if(gridTileObj.gridTile.x > 2 && gridTileObj.gridTile.y > 2)
        {
            gridTileObj.gridTile = new Vector2(gridTileObj.gridTile.x - 1f, gridTileObj.gridTile.y - 1f);
        }

        SceneManager.LoadScene(0);
    }

    protected internal Transform GetCurrentGrid(LayerMask gridLayer)
    {
        if(_camera.gameObject.active == true && playerScript._turnFinish)
        {
            mousePosition = Input.mousePosition;

            Vector3 worldMousePosition = _camera.ScreenToWorldPoint(mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(worldMousePosition, _camera.transform.forward, out hit, distance, gridLayer))
            {
                return hit.transform;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
