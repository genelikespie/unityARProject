using UnityEngine;

public class ObjectTarget : MonoBehaviour
{
    public GameObject objectToPlace;
    public int maxQuantity = -1;

    int numberCreated = 0;

    public GameObject GetObject(Vector3 position)
    {
        if (maxQuantity > -1 && numberCreated >= maxQuantity)
            return null;

        numberCreated++;

        return Instantiate(objectToPlace, position, Quaternion.identity) as GameObject;
    }
}
