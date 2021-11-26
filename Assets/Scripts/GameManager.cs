using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    //inspector set
    [SerializeField] TextMeshProUGUI txtAlive;
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] TextMeshProUGUI txtNeedFertilizer;
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] GameObject panelStart;
    [SerializeField] GameObject panelInstructions;
    [SerializeField] GameObject panelGame;
    [SerializeField] GameObject panelFinish;

    private List<Crop> cropList = new List<Crop>();

    private PlayerHandler playerHandler;
    private GameObject[] prefabsPlant;
    private LayerMask groundLayer;
    private List<int> plantlessCropIndexList;
    private List<int> plantedCropIndexList;

    private int healthyPlants;
    private int deadPlants;

    private readonly float timeStart = 2.5f * 60f;
    private float timeLeft;

    private bool isPlaying;

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
        for (int i = 0; i < cropArrObj.Length; i++)
        {
            GameObject obj = cropArrObj[i];
            obj.name = "Crop_" + i;
            cropList.Add(obj.GetComponent<Crop>());
        }

        //set crop index list
        InitializeCropIndexList();

        //init IO
        panelStart.SetActive(true);
        panelInstructions.SetActive(false);
        panelGame.SetActive(false);
        panelFinish.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
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
        
    }


    private IEnumerator AddPlantToRandomCrop()
    {
        while (true)
        {
            float waitSeconds = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitSeconds);

            //check if can spawn more plants
            if (plantlessCropIndexList.Count <= 0)
            {
                StopCoroutine(AddPlantToRandomCrop());
            }
            else
            {
                //choose random plantless crop index
                int randomCropPositionIndex = Random.Range(0, plantlessCropIndexList.Count);
                int cropIndex = plantlessCropIndexList[randomCropPositionIndex];

                //update index list
                plantedCropIndexList.Add(plantlessCropIndexList[randomCropPositionIndex]);
                plantlessCropIndexList.RemoveAt(randomCropPositionIndex);

                //set random plant to crop
                int randomPlantType = Random.Range(0, 3);
                Transform cropPosition = cropList[cropIndex].transform;
                Plant plant = GetPlantByType(randomPlantType, cropPosition);
                plant.Rename(cropIndex);
                cropList[cropIndex].SetPlant(plant);
            }
        }
    }


    private IEnumerator CheckTimer()
    {
        timeLeft = timeStart;
        while (isPlaying)
        {
            if (timeLeft <= 0)
            {
                isPlaying = false;
                panelFinish.SetActive(true);
                txtScore.SetText("Score: " + (plantedCropIndexList.Count - deadPlants));
                StopCoroutine(CheckTimer());
            }

            txtTimer.text = "| " + GetTimeLeft() + " |";

            timeLeft--;


            yield return new WaitForSeconds(1);
        }
    }

    private string GetTimeLeft()
    {
        int min = (int)(timeLeft / 60);
        int sec = (int)(timeLeft - (min * 60));

        return min + " m " + sec + " s";
    }

    //ABSTRACTION
    private void InitializeCropIndexList()
    {
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
    private void MoveToPositionAndFertilize(Ray raycast)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            GameObject obj = raycastHit.collider.gameObject;
            if (obj.tag.Equals("Plant") || obj.tag.Equals("Crop"))
            {
                int index = int.Parse(obj.name.Substring(obj.name.IndexOf("_") + 1));
                Plant plant = cropList[index].GetPlant();
                if (plant != null)
                {
                    if (playerHandler.ObjectCollided != null && playerHandler.ObjectCollided.name.EndsWith("_" + index))
                    {
                        //if already near plant, just ferilize
                        playerHandler.Fetrilize(plant);
                    }
                    else
                    {
                        //go to plant, then fertilize
                        playerHandler.MoveToPosition(raycastHit.point, plant);
                    }
                }
            }
        }
    }

    public void DeclarePlantState(Plant.State state)
    {
        if (state.Equals(Plant.State.HEALTHY))
        {
            healthyPlants++;
        }
        else if (state.Equals(Plant.State.NEED_FERTILIZER))
        {
            healthyPlants--;
        }
        else
        {
            deadPlants++;
        }

        Debug.Log("Healthy: " + healthyPlants
            + " | Need Fertilizer: " + (plantedCropIndexList.Count - (deadPlants + healthyPlants))
            + " | Alive: " + (plantedCropIndexList.Count - deadPlants)
            + " | Dead: " + deadPlants);

        txtAlive.SetText("ALIVE: " + (plantedCropIndexList.Count - deadPlants));
        txtNeedFertilizer.SetText("NEED FERTILIZER: " + (plantedCropIndexList.Count - (deadPlants + healthyPlants)));
    }


    #region Events

    public void OnClickStart() {

        isPlaying = true;
        panelStart.SetActive(false);
        panelGame.SetActive(true);

        //start timer
        StartCoroutine(CheckTimer());

        //spwan plants
        StartCoroutine(AddPlantToRandomCrop());
    }

    public void OnClickInstructions()
    {
        panelStart.SetActive(false);
        panelInstructions.SetActive(true);
    }
    public void OnClickBack()
    {
        panelStart.SetActive(true);
        panelInstructions.SetActive(false);
    }

    public void OnClickReplay()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    #endregion
}
static class Extension
{
    public static string ListToString(this List<int> list)
    {
        string str = "";
        foreach (int i in list)
        {
            str += i + ",";
        }
        if (str.Length > 0)
        {
            str = str.Substring(0, str.Length - 2);
        }
        return str;
    }

}