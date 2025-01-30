
using UnityEngine;

public class Puzzle3 : MonoBehaviour
{
    //These 2 define the 2 objects in the game that will be selected.
    public GameObject ObjectPlace;
    public GameObject ObjectAnswer;
    //This is the distance the object has from the answer that will then lock it into place if correct.
    public float DropDistance;
    //This will lock the object in place after the correct answer.
    private bool islocked;
    Vector3 ObjectStart;

    void Start()
    {
        //This will save the ObjectStart as the in-game ObjectPlaces starting position.
        ObjectStart = ObjectPlace.transform.position;
    }

    public void DragObject()
    {
        //If the object isn't answered/not locked.
        if(islocked == false)
        {
            //The object will move with the mouse input.
            ObjectPlace.transform.position = Input.mousePosition;
        }
    }

    public void DropObject()
    {
        //This is the distance from the answer object.
        float Distance = Vector3.Distance(ObjectPlace.transform.position, ObjectAnswer.transform.position);
        //If the Distance is further away from the answer than the DropDistance
        if(Distance < DropDistance)
        {
            //Then the object becomes locked, and the ObjectPlace becomes the ObjectAnswers position.
            islocked = true;
            ObjectPlace.transform.position = ObjectAnswer.transform.position;
        }
        else
        {
            //Else, the ObjectPlace returns to its original position.
            ObjectPlace.transform.position = ObjectStart;
        }
    }
    
    void Update () {
        //The game will constantly be checking to see if the player has completed the puzzle, which will load a congrats text.
    }
}
