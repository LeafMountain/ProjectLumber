using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuildMenu : UIElement
{
    public static bool building;

    public BuildingData[] buildings;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    public List<GameObject> spawnedButtons = new List<GameObject>();

    public LayerMask placementLayer;

    // Read only
    private BuildingData placingBuildingData;
    private Building placingBuilding;
    private Vector3 placementPosition;
    private Vector3 desiredRotation;
    private float rotationTimeStamp;

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Open()
    {
        while (spawnedButtons.Count > 0)
        {
            Destroy(spawnedButtons[0]);
            spawnedButtons.RemoveAt(0);
        }

        for (int i = 0; i < buildings.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TMP_Text>().SetText(buildings[i].name);
            int tempIndex = i;
            button.GetComponent<Button>().onClick.AddListener(() => StartPlacingBuilding(buildings[tempIndex]));
            spawnedButtons.Add(button);
        }
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        if (placingBuilding)
        {
            Destroy(placingBuilding.gameObject);
        }
        BuildMenu.building = false;
    }

    protected override void Update()
    {
        base.Update();
        if (topElement)
        {
            if (placingBuilding)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(placingBuilding.gameObject);
                    BuildMenu.building = false;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    PlaceBuilding();
                }
                else if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    float cooldown = .2f;
                    if (Time.timeSinceLevelLoad - rotationTimeStamp > cooldown)
                    {
                        rotationTimeStamp = Time.timeSinceLevelLoad;
                        int direction = Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : -1;
                        desiredRotation = desiredRotation + new Vector3(0f, 90f * direction, 0f);
                    }
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, placementLayer))
                    {
                        placementPosition = hit.point;
                        placementPosition.x = Mathf.Round(placementPosition.x);
                        placementPosition.z = Mathf.Round(placementPosition.z);
                    }
                    placingBuilding.transform.position = Vector3.Lerp(placingBuilding.transform.position, placementPosition, Time.deltaTime * 20);
                    placingBuilding.transform.rotation = Quaternion.Lerp(placingBuilding.transform.rotation, Quaternion.Euler(desiredRotation), Time.deltaTime * 20);
                }
            }
        }
    }

    public void StartPlacingBuilding(BuildingData building)
    {
        placingBuildingData = building;
        desiredRotation = Vector3.zero;
        if (placingBuilding)
        {
            Destroy(placingBuilding.gameObject);
        }
        placingBuilding = Instantiate(building.prefab);
        placingBuilding.SetState(Building.State.Placing);
        placingBuilding.GetComponent<Interactable>().enabled = false;
        placingBuilding.GetComponent<Collider>().enabled = false;
        if(placingBuilding.navObstacle)
        {
            placingBuilding.navObstacle.enabled = false;
        }

        BuildMenu.building = true;
    }

    public void PlaceBuilding()
    {
        placingBuilding.GetComponent<Interactable>().enabled = true;
        placingBuilding.GetComponent<Collider>().enabled = true;
        if (placingBuilding.navObstacle)
        {
            placingBuilding.navObstacle.enabled = true;
        }
        placingBuilding.transform.DOScaleY(.1f, .2f).SetEase(Ease.OutElastic);
        placingBuilding.transform.position = placementPosition;
        placingBuilding.transform.rotation = Quaternion.Euler(desiredRotation);
        placingBuilding.SetState(Building.State.Building);
        placingBuilding = null;
        building = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            StartPlacingBuilding(placingBuildingData);
        }
    }
}
