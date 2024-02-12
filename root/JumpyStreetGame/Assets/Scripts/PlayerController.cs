using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    Transform playerTransform
    {
        get => transform.parent;
    }
    CharacterController charController
    {
        get => GetComponent<CharacterController>();
    }

    public float moveDelay = 0f;
    const int moveDistance = 1;


    #region Movement and static obsticle/hazard handling

    //NOT FINISHED
    void AttemptMove()
    {
        //accept input vector, shoot raycast in that direction, detect collider. if collider has a script of type 
        Vector3 move = playerTransform.position;
        GameObject moveSpace = SearchMoveDirection(move);

        #region Check move validity

        //check to see if the move is valid within the bounds of the map
        if (IsMoveOutOfMapBounds() == true) { return; } 

        //check if its a static obsticle
        if(CheckForObstruction(moveSpace)) { return; }

        #endregion

        MovePlayer(move);
    }

    private void MovePlayer(Vector3 direction)
    {
        //as the direction with be a type of Vector3.(direction), it should be of value 1, and move the char controller properly
        charController.Move(direction);
    }

    //NOT FINISHED
    private void HandleHazard()
    {
        // not sure on what to do here, maybe just use a unity event and play the death animation/ show the death UI?
    }

    #endregion


    #region Searching the move raycast
    GameObject SearchMoveDirection(Vector3 direction)
    {
        RaycastHit hit;
        const int length = 1;
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
        //this code will determine how the movement system works, if its viable to move on, it should, if its static it should block movement and immedeiately allow theplayer to attempt to move again
        return searchedObject.GetComponent<Obsticle>();
    }

    //NOT FINISHED
    bool IsMoveOutOfMapBounds()
    {
        const int minBound = 15;
        const int maxBound = 0;


        return false;
    }

    bool CheckForObstruction(GameObject foundObject)
    {
        Obsticle obsticle = FindObsticle(foundObject);
        if(obsticle.obstructionType == Obstruction.StaticObstruction) { return true; }
        return false;
    }

    #endregion




    private void OnCollisionEnter(Collision collision)
    {
        
    }

}
