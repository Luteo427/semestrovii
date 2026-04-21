using UnityEngine;
using System.Collections;

public class SequentialActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] [Range(0.1f, 10f)] private float _switchInterval = 2.0f;
    [SerializeField] private bool _loop = true;

    private void Start()
    {
        if (_objects == null || _objects.Length == 0) return;

        foreach (GameObject obj in _objects)
        {
            if (obj != null) obj.SetActive(false);
        }

        StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        int currentIndex = 0;

        while (true)
        {
            if (_objects[currentIndex] != null)
            {
                _objects[currentIndex].SetActive(true);
            }

            yield return new WaitForSeconds(_switchInterval);

            if (_objects[currentIndex] != null)
            {
                _objects[currentIndex].SetActive(false);
            }

            currentIndex++;

            if (currentIndex >= _objects.Length)
            {
                if (_loop)
                {
                    currentIndex = 0;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}