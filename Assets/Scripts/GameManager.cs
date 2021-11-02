using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //inspector set
    private List<Crop> cropList = new List<Crop>();

    private PlayerHandler playerHandler;
    private GameObject[] prefabsPlant;
    private LayerMask groundLayer;
    private List<int> plantlessCropIndexList;
    private List<int> plantedCropIndexList;


    // Start is called before the first frame update
    void Start()
    {
        //set player
        playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();

        //set prefabs
        prefabsPlant = Resources.LoadAll<GameObject>("Prefabs/Plant");

        //set layer mask
        groundLayer = LayerMask.NameToLayer("Ground");

        //set crop list
        GameObject[] cropArrObj = GameObject.FindGameObjectsWithTag("Crop");
        for (int i=0; i<cropArrObj.Length; i++)
        {
            GameObject obj = cropArrObj[i];
            obj.name = "Crop_" + i;
            cropList.Add(obj.GetComponent<Crop>());
        }

        //set crop index list
        InitializeCropIndexList();

        //spwan plants
        StartCoroutine(AddPlantToRandomCrop());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToPosition(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
        else if (Input.GetMouseButton(1))
        {
            MoveToPositionAndFertilize(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }


    private IEnumerator AddPlantToRandomCrop()
    {
        while (true) {
            yield return new WaitForSeconds(1);

            //choose random plantless crop index
            int randomCropPositionIndex = Random.Range(0, plantlessCropIndexList.Count);
            Debug.Log("randomCropPositionIndex: " + randomCropPositionIndex);
            Debug.Log("crop: " + cropList[plantlessCropIndexList[randomCropPositionIndex]].name);

            //set random plant to crop
            int randomPlantType = Random.Range(0, 3);
            randomPlantType = 0; //TODO remove this line
            Transform cropPosition = cropList[plantlessCropIndexList[randomCropPositionIndex]].transform;
            Plant plant = GetPlantByType(randomPlantType, cropPosition);
            plant.Rename(plantedCropIndexList.Count);
            cropList[plantlessCropIndexList[randomCropPositionIndex]].SetPlant(plant);

            //update index list
            plantedCropIndexList.Add(plantlessCropIndexList[randomCropPositionIndex]);
            plantlessCropIndexList.Remove(randomCropPositionIndex);

            //check if can spawn more plants
            if (plantlessCropIndexList.Count <= 0)
            {
                StopCoroutine(AddPlantToRandomCrop());
            }
        }
    }

    //ABSTRACTION
    private void InitializeCropIndexList() {
        plantedCropIndexList = new List<int>();
        plantlessCropIndexList = new List<int>();
        for (int i = 0; i < cropList.Count; i++)
        {
            plantlessCropIndexList.Add(i);
        }
    }

    //ABSTRACTION
    private Plant GetPlantByType(int type, Transform transform)
    {
        GameObject plant = Instantiate(prefabsPlant[type], transform, true);
        plant.transform.localPosition = Vector3.zero;
        return plant.GetComponent<Plant>();
    }

    //ABSTRACTION
    private void MoveToPosition(Ray raycast)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.gameObject.layer.Equals(groundLayer.value))
            {
                playerHandler.MoveToPosition(raycastHit.point);
            }
        }
    }

    //ABSTRACTION
    private void MoveToPositionAndFertilize(Ray raycast) {
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            GameObject obj = raycastHit.collider.gameObject;
            if (obj.tag.Equals("Plant"))
            {
                int index = int.Parse(obj.name.Substring(obj.name.IndexOf("_")+1));
                Plant plant = cropList[index].GetPlant();
                if (playerHandler.ObjectCollided != null && playerHandler.ObjectCollided.name.EndsWith("_" + index))
                {
                    //if already near plant, just ferilize
                    playerHandler.Fetrilize(plant);
                }
                else {
                    //go to plant, then fertilize
                    playerHandler.MoveToPosition(raycastHit.point, plant);
                }
            }
        }
    }
}
