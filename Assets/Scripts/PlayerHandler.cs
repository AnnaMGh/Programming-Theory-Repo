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

    // ENCAPSULATION of the variable newPosition
    public void MoveToPosition(Vector3 position) {
        newPosition = position;
    }

    public void Fetrilize(Plant plant) {
        if (!isFertilizing)
        {
            isFertilizing = true;
            anim.SetBool("Crouch_b", isFertilizing);
            Invoke("StopFertilize", 1f);

            plant.Fertilize();
        }
    }

    public void StopFertilize() {
        isFertilizing = false;
        anim.SetBool("Crouch_b", isFertilizing);
    }


}
