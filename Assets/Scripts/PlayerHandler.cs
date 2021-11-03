using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public float speed;
    private Animator anim;

    private Vector3 newPosition;
    private const float accuracy = 0.5f;
    private bool isFertilizing;
    private Plant plantToFertilize;


    private const float bound= 20;

    // ENCAPSULATION
    public GameObject ObjectCollided { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        newPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //check
        CheckBounderies();

        //move
        Vector3 direction = newPosition - this.transform.position;
        if (direction.magnitude > accuracy)
        {
            anim.SetFloat("Speed_f", 0.5f);

            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            this.transform.LookAt(newPosition);
        }
        else if (anim.GetFloat("Speed_f") > 0f)
        {
            anim.SetFloat("Speed_f", 0f);
            if (plantToFertilize!=null)
            {
                Fetrilize(plantToFertilize);
            }
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        //save last plant collided
        if (collider.tag.Equals("Plant"))
        {
            ObjectCollided = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        //save last plant collided
        if (collider.tag.Equals("Plant"))
        {
            ObjectCollided = null;
        }
    }

    //ABSTRACTION
    private void CheckBounderies() {

        //vertical bounderies
        if (this.transform.position.z < -bound)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,-bound);
            newPosition = this.transform.position;
        }
        if (this.transform.position.z > bound)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, bound);
            newPosition = this.transform.position;
        }

        //horizontal bounderies
        if (this.transform.position.x < -bound)
        {
            this.transform.position = new Vector3(-bound, this.transform.position.y, this.transform.position.z); 
            newPosition = this.transform.position;
        } 
        if (this.transform.position.x > bound)
        {
            this.transform.position = new Vector3(bound, this.transform.position.y, this.transform.position.z);
            newPosition = this.transform.position;
        }
    }


    //ENCAPSULATION
    public void MoveToPosition(Vector3 position) {
        plantToFertilize = null;
        newPosition = position;
    }

    //ENCAPSULATION AND OVERLOAD
    public void MoveToPosition(Vector3 position, Plant plant) {
        plantToFertilize = plant;
        newPosition = position;
    }

    public void Fetrilize(Plant plant) {
        if (!isFertilizing && plant.state.Equals(Plant.State.NEED_FERTILIZER))
        {
            isFertilizing = true;
            anim.SetBool("Crouch_b", isFertilizing);
            plant.Fertilize();
            Invoke("StopFertilize", 1f);
        }
    }

    public void StopFertilize() {
        isFertilizing = false;
        plantToFertilize = null;
        anim.SetBool("Crouch_b", isFertilizing);
    }
}
