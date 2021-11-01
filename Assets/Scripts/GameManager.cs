using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //inspector set
    [SerializeField] private List<Crop> cropList;

    private PlayerHandler playerHandler;
    private GameObject[] prefabsPlant;
    private LayerMask groundLayer;
    private List<int> plantlessCropIndexList;
    private List<int> plantedCropIndexList;


    // Start is called before the first frame update
    void Start()
    {
        //set player
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();

        //set prefabs
        prefabsPlant = Resources.LoadAll<GameObject>("Prefabs/Plant");

        //set layer mask
        groundLayer = LayerMask.NameToLayer("Ground");
      
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
            TryToFertilize(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }


    private IEnumerator AddPlantToRandomCrop()
    {
        yield return new WaitForSeconds(1);

        //check if can spawn more plants
        if (plantlessCropIndexList.Count <= 0)
        {
            StopCoroutine(AddPlantToRandomCrop());
        }

        //choose random plantless crop index
        int randomCropPositionIndex = Random.Range(0, plantlessCropIndexList.Count);

        //set random plant to crop
        int randomPlantType = Random.Range(0, 3);
        randomPlantType = 0; //TODO remove this line
        Transform cropPosition = cropList[plantlessCropIndexList[randomCropPositionIndex]].transform;
        Plant plant = GetPlantByType(randomPlantType, cropPosition);
        plant.Rename(randomCropPositionIndex);
        cropList[plantlessCropIndexList[randomCropPositionIndex]].SetPlant(plant);

        //update index list
        plantedCropIndexList.Add(plantlessCropIndexList[randomCropPositionIndex]);
        plantlessCropIndexList.Remove(randomCropPositionIndex);
    }

    //ABSTRACTION
    private void InitializeCropIndexList() {
        plantedCropIndexList = new List<int>();
        plantlessCropIndexList = new List<int>();
        for (int i = 0; i < cropList.Count; i++)
        {
            plantlessCropIndexList.Add(0);
        }
    }

    //ABSTRACTION
    private Plant GetPlantByType(int type, Transform transform)
    {
        GameObject plant = Instantiate(prefabsPlant[type], transform, true);
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

    private void TryToFertilize(Ray raycast) {
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            GameObject obj = raycastHit.collider.gameObject;
            if (obj.tag.Equals("Plant"))
            {
                int index = int.Parse(obj.name.Substring(obj.name.IndexOf("_")+1));
                if (playerHandler.ObjectCollided != null && playerHandler.ObjectCollided.name.EndsWith("_" + index))
                {
                    Plant plant = cropList[index].GetPlant();
                    playerHandler.Fetrilize(plant);
                }
            }
        }
    }
}
