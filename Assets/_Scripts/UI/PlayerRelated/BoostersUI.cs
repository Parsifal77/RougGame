using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostersUI : MonoBehaviour
{

    private Image healthPotionUIImage;
    private Image speedPotionUIImage;
    private Image strengthPotionUIImage;

    private TextMeshProUGUI healthPotionText;
    private TextMeshProUGUI speedPotionText;
    private TextMeshProUGUI strengthPotionText;




    private BoostersHandler playerBoostersHandler;
    private void Start()
    {
        RoomContentGenerator roomGenerator = FindObjectOfType<RoomContentGenerator>();
        if (roomGenerator != null)
        {
            roomGenerator.DungeonContentGenerated.AddListener(OnDungeonContentGenerated);
        }

        healthPotionUIImage = transform.Find("HealthBooster").GetComponent<Image>();
        speedPotionUIImage = transform.Find("SpeedBooster").GetComponent<Image>();
        strengthPotionUIImage = transform.Find("StrengthBooster").GetComponent<Image>();

        healthPotionText = transform.Find("HealthBooster").GetComponentInChildren<TextMeshProUGUI>(true);
        speedPotionText = transform.Find("SpeedBooster").GetComponentInChildren<TextMeshProUGUI>(true);
        strengthPotionText = transform.Find("StrengthBooster").GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void OnDungeonContentGenerated()
    {
        playerBoostersHandler = GameObject.FindWithTag("Player").GetComponent<BoostersHandler>();

        playerBoostersHandler.OnSpeedBoosterCountChanged.AddListener(UpdateSpeedBoosterCount);
        playerBoostersHandler.OnStrengthBoosterCountChanged.AddListener(UpdateStrengthBoosterCount);
        playerBoostersHandler.OnHealthBoosterCountChanged.AddListener(UpdateHealthBoosterCount);


        // Display booster counts for the first time
        healthPotionText.text = playerBoostersHandler.GetPlayerHealthBoosterCount.ToString();
        speedPotionText.text = playerBoostersHandler.GetPlayerSpeedBoosterCount.ToString();
        strengthPotionText.text = playerBoostersHandler.GetPlayerStrengthBoosterCount.ToString();
    }

    private void UpdateHealthBoosterCount(int updatedCount)
    {
        healthPotionText.text = updatedCount.ToString();

        if (updatedCount < playerBoostersHandler.GetPlayerHealthBoosterCount)
            healthPotionUIImage.fillAmount = 0;
    }

    private void UpdateStrengthBoosterCount(int updatedCount)
    {
        strengthPotionText.text = updatedCount.ToString();

        if (updatedCount < playerBoostersHandler.GetPlayerStrengthBoosterCount)
            strengthPotionUIImage.fillAmount = 0;
    }

    private void UpdateSpeedBoosterCount(int updatedCount)
    {
        speedPotionText.text = updatedCount.ToString();

        if (updatedCount < playerBoostersHandler.GetPlayerSpeedBoosterCount)
            speedPotionUIImage.fillAmount = 0;
    }


    private void Update()
    {
        healthPotionUIImage.fillAmount += 1 * Time.deltaTime;
        speedPotionUIImage.fillAmount += 1 / playerBoostersHandler.GetPlayerSpeedBoosterDuration * Time.deltaTime;
        strengthPotionUIImage.fillAmount += 1 / playerBoostersHandler.GetPlayerStrengthBoosterDuration * Time.deltaTime;
    }

    private void Awake()
    {
        Start();
    }
}
