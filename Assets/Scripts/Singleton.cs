using UnityEngine;

public class Singleton : MonoBehaviour
{
    public string ID;
    
    void Awake()
    {
        if (ID == "")
        {
            Debug.LogWarning(gameObject.name + " Singleton has no ID. Please assign ID in Inspector.");
            return;
        }

        int instanceCount = 0;

        foreach (Singleton instance in FindObjectsOfType<Singleton>())
        {
            if (instance.ID == ID) instanceCount++;
        }
        
        if (instanceCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
