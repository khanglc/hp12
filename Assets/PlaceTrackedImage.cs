using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceTrackedImage : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private GameObject[] prefabs; // Các prefab sẽ xuất hiện trên hình ảnh
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Khi có hình ảnh mới được phát hiện
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateSpawnedObject(trackedImage);
        }

        // Khi hình ảnh thay đổi (di chuyển, xoay, thay đổi trạng thái)
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateSpawnedObject(trackedImage);
        }

        // Khi hình ảnh bị mất
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                Destroy(spawnedObjects[trackedImage.referenceImage.name]);
                spawnedObjects.Remove(trackedImage.referenceImage.name);
            }
        }
    }

    private void UpdateSpawnedObject(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        // Nếu chưa có đối tượng nào gắn với hình ảnh này, tạo mới
        if (!spawnedObjects.ContainsKey(imageName))
        {
            foreach (var prefab in prefabs)
            {
                if (prefab.name == imageName) // Đảm bảo prefab có tên giống với hình ảnh reference
                {
                    GameObject newObject = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    spawnedObjects[imageName] = newObject;
                }
            }
        }
        else
        {
            // Cập nhật vị trí nếu đối tượng đã được tạo
            GameObject spawnedObject = spawnedObjects[imageName];
            spawnedObject.transform.position = trackedImage.transform.position;
            spawnedObject.transform.rotation = trackedImage.transform.rotation;
        }
    }
}
