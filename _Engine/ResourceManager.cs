using UnityEngine;

public class ResourceManager:Singleton<ResourceManager>
{
    public void Load(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        Object curPrefab = Resources.Load(path, typeof(GameObject));
        GameObject prefab = MonoBehaviour.Instantiate(curPrefab) as GameObject;

    }
}
