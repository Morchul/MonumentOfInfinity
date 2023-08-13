using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTooltip : MonoBehaviour
{
    UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        uiManager.AddBuildingName(this);
        gameObject.SetActive(uiManager.ShowBuildingNames);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }

    private void OnDestroy()
    {
        uiManager.RemoveBuildingName(this);
    }
}
