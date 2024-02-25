using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//Search for //NOT FINISHED to continue work
public class PlayerController : MonoBehaviour
{
    CharacterController charController
    {
        get => transform.parent.GetComponent<CharacterController>();
    }

    public Vector3 facingDirection = Vector3.zero;//not yet implemented but would be good to see in editor for debugging

    public float moveDelay = 0f;

    public GameObject deathPanel;

    public UnityEvent onSuccessfulMove = new UnityEvent();
    public UnityEvent onSuccessfulForwardMove = new UnityEvent();

    private void Start()
    {
        Time.timeScale = 1;
        deathPanel.SetActive(false);
    }

    void Update()
    {
        Vector3 moveDir = DetemineMoveDirection();
        if (moveDir != Vector3.zero)
        {
            AttemptMove(moveDir);
        }
        else
        {
            charController.center = transform.position;
        }
    }

    Vector3 DetemineMoveDirection()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            facingDirection = Vector3.forward * 2.5f;
            return facingDirection;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            facingDirection = Vector3.left * 2.5f;
            return facingDirection;
        }
        if( Input.GetKeyDown(KeyCode.D)) 
        {
            facingDirection = Vector3.right * 2.5f;
            return facingDirection;
        }
        return Vector3.zero;
    }

    #region Movement and static obsticle/hazard handling

    //NOT FINISHED
    void AttemptMove(Vector3 moveDir)
    {
        Debug.Log("Attempting to move");
        //accept input vector, shoot raycast in that direction, detect collider. if collider has a script of type 

        GameObject moveSpace = SearchMoveDirection(moveDir);

        #region Check move validity

        //check to see if the move is valid within the bounds of the map
        if (IsMoveOutOfMapBounds(moveDir) == true) { return; } 

        //check if its a static obsticle
        if (moveSpace != null)
        {
            if (CheckForObstruction(moveSpace)) { return; }
        }
        

        #endregion

        MovePlayer(moveDir);
        SetCurrentTileAsParent();
    }

    private void MovePlayer(Vector3 direction)
    {
        //as the direction with be a type of Vector3.(direction), it should be of value 1, and move the char controller properly
        charController.Move(direction);
        onSuccessfulMove.Invoke();
        if(direction == Vector3.forward)
        {
            onSuccessfulForwardMove.Invoke();
        }
    }

    private void SetCurrentTileAsParent()
    {
        //shoot raycast down to get tile row
        RaycastHit hit;
        const float length = 1.2f;
        // Perform the raycast
        if (Physics.Raycast(transform.position, Vector3.down, out hit, length))
        {
            GameObject hitObject = hit.collider.gameObject;
            //set the transform of the player to be the object
            transform.parent.parent = hitObject.transform;
        }
        else
        {
            Die();
        }
    }

    #endregion


    #region Searching the move raycast
    GameObject SearchMoveDirection(Vector3 direction)
    {
        RaycastHit hit;
        const float length = 2.5f;
        // Perform the raycast
        if (Physics.Raycast(transform.position, direction, out hit, length))
        {
            GameObject hitObject = hit.collider.gameObject;
            // Check if the hit object has the desired script attached
            if (hitObject.GetComponent<Obsticle>() != null)
            {
                return hit.collider.gameObject;
            }
        }
        //if nothing is in front of the object, return null
        return null;
    }

    Obsticle FindObsticle(GameObject searchedObject)
    {
        return searchedObject.GetComponent<Obsticle>();
    }

    bool IsMoveOutOfMapBounds(Vector3 localDirection)
    {
        //creates a world space vector that shows the potnetial move direction
        Vector3 actualizedMoveDirection = transform.parent.position + localDirection;
        //cons for known outer bounds
        const float minXBound = 3;
        const float maxXBound = 73;
        if (actualizedMoveDirection.x > maxXBound || actualizedMoveDirection.x < minXBound) { return true; }
        return false;
    }

    bool CheckForObstruction(GameObject foundObject)
    {
        Obsticle obsticle = FindObsticle(foundObject);
        if(obsticle.obstructionType == Obstruction.StaticObstruction) { return true; }
        return false;
    }

    #endregion

    private void Die()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Hazard>() != null)
        {
            Obsticle obsticle = collider.gameObject.GetComponent<Hazard>();
            if (obsticle.obstructionType == Obstruction.StaticHazard || obsticle.obstructionType == Obstruction.DynamicHazard)
            {
                // player dies
                Die();
            }
        }
    }

}
