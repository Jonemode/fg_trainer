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
        optionsOpenButton.onClick.AddListener(() => {
            optionsDialog.SetActive(true);
        });

        optionsCloseButton.onClick.AddListener(() => {
            optionsDialog.SetActive(false);
        });
    }
}
