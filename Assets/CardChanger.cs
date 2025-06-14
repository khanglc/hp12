using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CardChanger : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public GameObject[] cardObjects; // Các Prefab lá bài
    private Dictionary<string, GameObject> spawnedCards = new Dictionary<string, GameObject>();

    void OnEnable() => imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    void OnDisable() => imageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            SpawnCard(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateCard(trackedImage);
        }
    }

    private void SpawnCard(ARTrackedImage trackedImage)
    {
        if (spawnedCards.ContainsKey(trackedImage.referenceImage.name)) return;
        GameObject newCard = Instantiate(cardObjects[Random.Range(0, cardObjects.Length)], trackedImage.transform);
        spawnedCards[trackedImage.referenceImage.name] = newCard;
    }

    private void UpdateCard(ARTrackedImage trackedImage)
    {
        if (spawnedCards.TryGetValue(trackedImage.referenceImage.name, out GameObject card))
        {
            card.transform.position = trackedImage.transform.position;
            card.transform.rotation = trackedImage.transform.rotation;
        }
    }
}
