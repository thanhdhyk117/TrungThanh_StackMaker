using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public Image slider;
    private int _timeToLoad = 3;
    [SerializeField] private RectTransform childSlider;
}
