using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleObstacleScript : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject tooglePrefab;

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Script")]
    [SerializeField] private ObstacleScriptableObject obstacleData;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private ObstacleManager obstacleManager;

    int maxToggle = 0;

    private void CreateToggle(int id)
    {
        maxToggle = id;

        for(int i = 0; i < id; i++)
        {
            GameObject obj = Instantiate(tooglePrefab, transform.position, Quaternion.identity, transform);
            obj.GetComponent<Toggle>().isOn = false;
        }

        if(obstacleData.obstacleGrid.Count < id)
        {
            for(int i = obstacleData.obstacleGrid.Count; i < id; i++)
            {
                obstacleData.obstacleGrid.Add(false);
            }
        }
        else if(obstacleData.obstacleGrid.Count > id)
        {

            for(int i = obstacleData.obstacleGrid.Count; i > id; i--)
            {
                obstacleData.obstacleGrid.RemoveAt(i - 1);
            }
        }
    }

    private void SetPosition(Transform targetTransform)
    {
        foreach(Transform child in targetTransform)
        {
            int id = child.GetComponent<GridScript>()._id;

            transform.GetChild(id).position = _camera.WorldToScreenPoint(child.position);

            transform.GetChild(id).gameObject.SetActive(false);

            Toggle toggle = transform.GetChild(id).GetComponent<Toggle>();

            if(obstacleData.obstacleGrid.Count < maxToggle)
                ToggleChange(id, toggle.isOn);
            else
                toggle.isOn = obstacleData.obstacleGrid[id];
            
            toggle.onValueChanged.AddListener( delegate { ToggleChange(id, toggle.isOn); });
        }
    }

    private void ChangePosition(Transform targetTransform)
    {
        foreach(Transform child in targetTransform)
        {
            int id = child.GetComponent<GridScript>()._id;

            transform.GetChild(id).position = _camera.WorldToScreenPoint(child.position);
        }
    }

    private void ActivateToggle()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void DisableToggle()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void ToggleChange(int id, bool isOn)
    {
        if(playerScript._currentIndex != id)
        {
            obstacleManager.ChangeToggle(id, isOn);
        }

        obstacleData.runTimeChange = true;
    }
}
