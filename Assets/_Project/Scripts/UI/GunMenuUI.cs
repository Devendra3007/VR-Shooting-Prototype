using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunMenuUI : MonoBehaviour
{
    [Header("Place Holders")]
    [SerializeField] private TextMeshProUGUI gunNameText;
    [SerializeField] private TextMeshProUGUI gunDamageText;
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private Image gunImage;
    [SerializeField] private Sprite[] gunSprites;

    [Header("Buttons")]
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button selectButton;

    private readonly Vector2[] sizeDeltas = new Vector2[]
    {
        new Vector2(73.84615f,55.6213f),
        new Vector2(94.8f,45.8f),
        new Vector2(84,82),
    };

    private int currentGunIndex = 0;

    public void Initialize()
    {
        previousButton.onClick.AddListener(PreviousGun);
        nextButton.onClick.AddListener(NextGun);
        selectButton.onClick.AddListener(SelectGun);
        UpdateCurrentGunUI();
    }

    private void NextGun()
    {
        currentGunIndex++;

        if(currentGunIndex > 2)
        {
            currentGunIndex = 2;
        }

        UpdateCurrentGunUI();
    }

    private void PreviousGun()
    {
        currentGunIndex--;
        if(currentGunIndex < 0)
        {
            currentGunIndex = 0;
        }

        UpdateCurrentGunUI();
    }

    private void UpdateCurrentGunUI()
    {
        switch (currentGunIndex)
        {
            case 0:
                gunNameText.text = "Type 1";
                gunDamageText.text = "Damage: 3";
                fireRateText.text = "Fire Rate: 1";

                break;
            case 1:
                gunNameText.text = "Type 2";
                gunDamageText.text = "Damage: 2";
                fireRateText.text = "Fire Rate: 2";
                break;
            case 2:
                gunNameText.text = "Type 3";
                gunDamageText.text = "Damage: 1";
                fireRateText.text = "Fire Rate: 5";
                break;
        }

        // Set the gun image and adjust size
        gunImage.sprite = gunSprites[currentGunIndex];
        gunImage.rectTransform.sizeDelta = sizeDeltas[currentGunIndex];

        // Hide navigation buttons at ends
        previousButton.gameObject.SetActive(currentGunIndex > 0);
        nextButton.gameObject.SetActive(currentGunIndex < gunSprites.Length - 1);

    }

    public void SelectGun()
    {
        GameManager.Instance.SetCurrentSelectedGun((Gun.GunType)currentGunIndex);
        Show(false);
    }

    public void Show(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
