using UnityEngine;

public class DestroyHelper : MonoBehaviour
{
    [SerializeField]
    private GameObject referenceObject;

    public void DestoryObject()
    {
        Destroy(referenceObject);
    }
}
