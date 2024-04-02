using UnityEngine;

public class LoadBar : MonoBehaviour
{
    [Range(0,1)]
    public float progress;

    private static Quaternion lastRotation;

    void Start()
    {
        transform.rotation = lastRotation;
    }

    void Update()
    {
        transform.Rotate(0, 0, progress, Space.Self); 
    }

    public void saveRotation()
    {
        lastRotation = transform.rotation;
    }

}
