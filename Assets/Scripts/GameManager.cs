using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerHandler playerHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //windows for debug
            MoveToPosition(Camera.main.ScreenPointToRay(Input.mousePosition));
        }
    }

    private void MoveToPosition(Ray raycast)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.tag.Equals("Ground"))
            {
                //Debug.Log(raycastHit.collider.name);
                //vvvalue = raycastHit.collider.name;
                playerHandler.MoveToPosition(raycastHit.point);
               
            }
        }
    }
}
