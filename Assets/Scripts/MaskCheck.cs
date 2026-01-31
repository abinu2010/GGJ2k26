using UnityEngine;

public class MaskCheck : MonoBehaviour
{
    public static MaskCheck Instance { get; private set; }

    public bool piece1;
    public bool piece2;

    public GameObject victoryRoot;
    public GameObject fullMaskVisual;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (victoryRoot != null) victoryRoot.SetActive(false);
        if (fullMaskVisual != null) fullMaskVisual.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect(int id)
    {
        if (id == 1) piece1 = true;
        if (id == 2) piece2 = true;

        if (piece1 && piece2)
        {
            if (victoryRoot != null) victoryRoot.SetActive(true);
            if (fullMaskVisual != null) fullMaskVisual.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
