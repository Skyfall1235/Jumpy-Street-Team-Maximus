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

    public float moveDelay = 0.1f; // time it takes for the player to move

    public float moveDistance = 2.5f; // how far the player moves

    //cons for known outer bounds
    private const float minXBound = 3;
    private const float maxXBound = 73;

    private bool canMove = true; // if the player is able to move

    public Animator anim;

    public GameObject deathPanel;

    public UnityEvent onSuccessfulMove = new UnityEvent();
    public UnityEvent onSuccessfulForwardMove = new UnityEvent();

    private void Start()
    {
        canMove = true;
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
            // resets the character controller position if the player is on a log
            charController.center = transform.position;
            // if a log carrys the player out of bounds, they die
            if (transform.position.x < minXBound || transform.position.x > maxXBound)
            {
                Die();
            }
        }
    }

    Vector3 DetemineMoveDirection()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                facingDirection = Vector3.forward * moveDistance;
                return facingDirection;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                facingDirection = Vector3.left * moveDistance;
                return facingDirection;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                facingDirection = Vector3.right * moveDistance;
                return facingDirection;
            }
            return Vector3.zero;
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
        StartCoroutine(MovePlayer(moveDir));
    }

    // Moves the player frame by frame
    private IEnumerator MovePlayer(Vector3 direction)
    {
        // disconnects the player from the current parent (so if the player is on a log, it will not move with the log while hopping)
        transform.parent.parent = null;
        // plays the hopping animation
        anim.SetTrigger("Move");
        // player cannot move again until the current movement is done
        canMove = false;

        for (float i = 0; i < moveDelay; i += Time.deltaTime)
        {
            float dist = Time.deltaTime / moveDelay;
            charController.Move(direction * dist);
            yield return null;
        }

        onSuccessfulMove.Invoke();
        if(direction == Vector3.forward * moveDistance)
        {
            onSuccessfulForwardMove.Invoke();
        }
        // allows the player to move again
        canMove = true;
        // realigns player position
        SnapPlayerToGrid();
        // attaches player to the tile it is standing on
        SetCurrentTileAsParent();
        Debug.Log(transform.parent.position);
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
        else // if there is no ground beneath the player, they fall and die (the water tile is lowered slightly so the raycast will not reach it)
        {
            Die();
        }
    }

    // Adjusts the player's position so it is aligned with everything else
    private void SnapPlayerToGrid()
    {
        // Gets the player position and calculates where it should be
        float gridSpaceX = Mathf.Round(transform.parent.position.x / moveDistance);
        float gridSpaceZ = Mathf.Round((transform.parent.position.z - 1.25f) / moveDistance);
        // Sets the position
        Vector3 newPosition = new Vector3(gridSpaceX * moveDistance, 1.5f, (gridSpaceZ * moveDistance) + 1.25f);
        transform.parent.SetPositionAndRotation(newPosition, Quaternion.identity);
    }

    #endregion


    #region Searching the move raycast
    GameObject SearchMoveDirection(Vector3 direction)
    {
        RaycastHit hit;
        float length = moveDistance;
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
        canMove = false;
        deathPanel.SetActive(true);
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
