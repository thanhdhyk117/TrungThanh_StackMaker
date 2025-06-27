using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private GameObject _levelCompleteUI;
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject[] _chestObjects;
    [SerializeField] private ParticleSystem[] _congratulationsParticles;

    [SerializeField] private TextMeshProUGUI _coinComplete;
    [SerializeField] private Button _btnComplete;
    [SerializeField] private Button _btnReload;

    // Start is called before the first frame update

    private void Start()
    {
        if (_levelCompleteUI != null)
        {
            _levelCompleteUI.SetActive(false);
        }
        ActionComplete();
    }

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
        _coinComplete.text = _player.Score.ToString();
        _levelCompleteUI.SetActive(true);
        _player.enabled = false;
    }

    public void PlayParticle()
    {
        foreach (var particle in _congratulationsParticles)
        {
            particle.Play();
        }
    }

    private void ActionComplete()
    {
        if (_btnComplete != null)
        {
            _btnComplete.onClick.RemoveAllListeners();
            _btnComplete.onClick.AddListener(() =>
            {
                LevelManager.Instance.AddScore(_player.Score);
                LevelManager.Instance.NextLevel();
                Debug.Log("Level Completed! Score: " + LevelManager.Instance.GetCurrentScore());
            });
        }
        else
        {
            Debug.LogWarning("Button Complete is not assigned.");
        }

        if (_btnReload != null)
        {
            _btnReload.onClick.RemoveAllListeners();
            _btnReload.onClick.AddListener(() =>
            {
                LevelManager.Instance.ReloadLevel();
                Debug.Log("Level Reloaded!");
            });
        }
        else
        {
            Debug.LogWarning("Button Reload is not assigned.");
        }
    }
}

