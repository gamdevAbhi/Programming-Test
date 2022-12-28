using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] private GameObject topViewCamera;
    [SerializeField] private GameObject IsomatricCamera;
    [SerializeField] private GameObject gridManager;

    [Header("Variable")]
    [SerializeField] private float orthographicIncrease = 0.5f;
    [SerializeField] private float topYIcrease = 1f;

    [Header("Script")]
    [SerializeField] private ToggleObstacleScript toggleObstacleScript;

    private void Start()
    {
        ChangeCamera(true);
    }

    private void ChangeCamera(bool isIsomatric)
    {
        if(isIsomatric == false)
        {
                topViewCamera.SetActive(true);
                IsomatricCamera.SetActive(false);
        }
        else
        {
            topViewCamera.SetActive(false);
            IsomatricCamera.SetActive(true);
        }
    }

    private void ChangeCameraViewSize(bool isIncrease)
    {
        if(topViewCamera.active == true)
        {
            if(isIncrease)
            {
                TopViewIncrease();
            }
            else
            {
                TopViewDecrease();
            }

            toggleObstacleScript.SendMessage("ChangePosition", gridManager.transform);
        }
        else
        {
            if(isIncrease)
            {
                IsometricIncrease();
            }
            else
            {
                IsometricDecrease();
            }
        }
    }

    private void IsometricIncrease()
    {
        IsomatricCamera.transform.GetChild(0).GetComponent<Camera>().orthographicSize -= orthographicIncrease;
    }

    private void IsometricDecrease()
    {
        IsomatricCamera.transform.GetChild(0).GetComponent<Camera>().orthographicSize += orthographicIncrease;
    }

    private void TopViewIncrease()
    {
        topViewCamera.transform.GetChild(0).position -= new Vector3(0f, topYIcrease, 0f);
    }

    private void TopViewDecrease()
    {
        topViewCamera.transform.GetChild(0).position += new Vector3(0f, topYIcrease, 0f);
    }
}
