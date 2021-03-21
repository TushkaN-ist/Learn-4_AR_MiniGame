using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameCore : MonoBehaviour
{
    public ARRaycastManager arRaycastManager;
    public TamagochiPets tamagochiObject;
    public GameObject apple;

    TamagochiPets _tamagochiObject;


    public void InstanceOnTap(Ray ray){
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (arRaycastManager.Raycast(ray, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes)){
            Vector3 point = ray.GetPoint(hits[0].distance);
            if (_tamagochiObject == null)
            {
                _tamagochiObject = tamagochiObject.Clone(point, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }else{
                GameObject _apple = Instantiate(apple, point,Quaternion.identity);
                _tamagochiObject.SetTarget(_apple.transform);
            }
        }
    }
}
