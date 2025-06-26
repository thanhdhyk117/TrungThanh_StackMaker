using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private GameObject _levelCompleteUI;
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject[] _chestObjects;
    [SerializeField] private ParticleSystem[] _congratulationsParticles;

    // Start is called before the first frame update

    public void EffectEndLevel()
    {
        StartCoroutine(SetEffectEndLevel());
    }

    IEnumerator SetEffectEndLevel()
    {
        for (int i = 0; i < _chestObjects.Length; i++)
        {
            _chestObjects[i].SetActive(i % 2 != 0);
        }
        
        yield return new WaitForSeconds(1f);
        _levelCompleteUI.SetActive(true);
    }

    public void PlayParticle()
    {
        foreach (var particle in _congratulationsParticles)
        {
            particle.Play();
        }
    }
}
