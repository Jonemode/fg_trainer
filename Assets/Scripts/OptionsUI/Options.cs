using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField]
    public Button optionsOpenButton;

    [SerializeField]
    public Button optionsCloseButton;

    [SerializeField]
    public GameObject optionsDialog;

    // Start is called before the first frame update
    void Start()
    {
        // Set size of options dialog to the overlay canvas size
        RectTransform overlayCanvasRect = optionsDialog.transform.parent.GetComponent<RectTransform>();
        RectTransform optionsDialogRect = optionsDialog.GetComponent<RectTransform>();
        optionsDialogRect.sizeDelta = new Vector2(overlayCanvasRect.sizeDelta.x, overlayCanvasRect.sizeDelta.y);

        optionsOpenButton.onClick.AddListener(() => {
            optionsDialog.SetActive(true);
        });

        optionsCloseButton.onClick.AddListener(() => {
            optionsDialog.SetActive(false);
        });
    }
}
