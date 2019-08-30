using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingProps_Manager : MonoBehaviour
{
    public FloatingPropsManager_ScriptableObject _ScriptableObject;

    public Transform centerpoint;

    private  float minX, maxX, minZ, maxZ;

    // Start is called before the first frame update
    void Start()
    {
        minX = centerpoint.transform.position.x - 50.0f;
        maxX = centerpoint.transform.position.x + 50.0f;

        minZ = centerpoint.transform.position.z - 50.0f;
        maxZ = centerpoint.transform.position.z + 50.0f;

        StartCoroutine("GenerateProps");
    }

   IEnumerator GenerateProps()
    {
        yield return new WaitForSeconds(0.1f);

        Generation();
    }

    void Generation()
    {
        int maxPropsType = _ScriptableObject.floatingObjects.Count;
        Debug.Log("Total types = " + maxPropsType);

        for(int i=0;i<maxPropsType;i++)
        {
            int maxToGenerate = _ScriptableObject.floatingObjects[i].maxCanBeInstatiate;
            for(int j=0;j<maxToGenerate;j++)
            {
                GameObject g = Instantiate(_ScriptableObject.floatingObjects[i]._prefab, new Vector3(Random.Range(minX, maxX), centerpoint.transform.position.y, Random.Range(minZ, maxZ)), Quaternion.identity) as GameObject;
                //if (_ScriptableObject.floatingObjects[i].canInteractable)
                //    g.AddComponent<Valve.VR.InteractionSystem.Interactable>();
                g.transform.parent = centerpoint;
            }
        }
    }
}
