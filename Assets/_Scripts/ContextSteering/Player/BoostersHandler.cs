using UnityEngine;
using UnityEngine.Events;

public class BoostersHandler : MonoBehaviour
{
    private AgentMover playerAgentMover;    // To control player movement speed
    private PlayerWeapon playerWeapon;      // To control player weapon damage
    private PlayerHealth playerHealth;      // To control player health

    private PlayerInput playerInput;        // To get messages about booster usage


    [Header("Boosters Settings")]
    [SerializeField] private int speedBoosterCount = 3;
    [SerializeField] private float speedBoosterDuration = 5f;
    [SerializeField] private float speedBoosterMultiplier = 2f;

    [SerializeField] private int strengthBoosterCount = 3;
    [SerializeField] private float strengthBoosterDuration = 5f;
    [SerializeField] private float strengthBoosterMultiplier = 2f;

    [SerializeField] private int healthBoosterCount = 3;
    //[SerializeField] private float healthBoosterDuration = 5f;
    [SerializeField] private int healthBoosterHealAmount = 2;

    [SerializeField] private int CoinCount = 10;

    public int GetPlayerSpeedBoosterCount => speedBoosterCount;
    public int GetPlayerStrengthBoosterCount => strengthBoosterCount;
    public int GetPlayerHealthBoosterCount => healthBoosterCount;
    public int GetPlayerCoinCount => CoinCount;

    public float GetPlayerSpeedBoosterDuration => speedBoosterDuration;
    public float GetPlayerStrengthBoosterDuration => strengthBoosterDuration;


    public UnityEvent<int> OnSpeedBoosterCountChanged;
    public UnityEvent<int> OnStrengthBoosterCountChanged;
    public UnityEvent<int> OnHealthBoosterCountChanged;
    public UnityEvent<int> OnCoinCountChanged;

    private int cachedPlayerAttackDamage;
    private float cachedPlayerMoveSpeed;


    //private Rigidbody2D playerRb;

    public int SetPlayerSpeedBoosterCount
    {
        get => speedBoosterCount;
        set
        {
            speedBoosterCount = value;
            OnSpeedBoosterCountChanged?.Invoke(speedBoosterCount);
        }
    }

    public int SetPlayerStrengthBoosterCount
    {
        get => strengthBoosterCount;
        set
        {
            strengthBoosterCount = value;
            OnStrengthBoosterCountChanged?.Invoke(strengthBoosterCount);
        }
    }

    public int SetPlayerHealthBoosterCount
    {
        get => healthBoosterCount;
        set
        {
            healthBoosterCount = value;
            OnHealthBoosterCountChanged?.Invoke(healthBoosterCount);
        }
    }

    public int SetPlayerCoinCount
    {
        get => CoinCount;
        set
        {
            CoinCount = value;
        }
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerAgentMover = GetComponent<AgentMover>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerHealth = GetComponent<PlayerHealth>();

        playerInput.OnSpeedBoosterConsumed.AddListener(HandleSpeedBooster);
        playerInput.OnStrengthBoosterConsumed.AddListener(HandleStrengthBooster);
        playerInput.OnHealthBoosterConsumed.AddListener(HandleHealthBooster);

        cachedPlayerAttackDamage = playerWeapon.PlayerAttackDamage;
        cachedPlayerMoveSpeed = playerAgentMover.MaxSpeed;

        //playerRb = GetComponent<Rigidbody2D>();
    }

    private void HandleCoinChange(int updatedCoinCount)
    {
        CoinCount = updatedCoinCount;

    }

    private void HandleHealthBooster()
    {
        if (healthBoosterCount > 0)
        {
            if (playerHealth.CurrentHealth == playerHealth.MaxHealth)
            {
                Debug.Log("Player is already at max health"); return;
            }

            healthBoosterCount--;
            playerHealth.HealPlayer(healthBoosterHealAmount);
            OnHealthBoosterCountChanged?.Invoke(healthBoosterCount);
        }
    }

    private void HandleStrengthBooster()
    {                                   // Check if the player is already using a strength booster
        if (strengthBoosterCount > 0 && cachedPlayerAttackDamage * strengthBoosterMultiplier > playerWeapon.PlayerAttackDamage)
        {
            strengthBoosterCount--;
            playerWeapon.SetDamageMultiplier(strengthBoosterMultiplier, strengthBoosterDuration, cachedPlayerAttackDamage);
            OnStrengthBoosterCountChanged?.Invoke(strengthBoosterCount);
        }
    }


    private void HandleSpeedBooster()
    {                                // Check if the player is already using a speed booster
        if (speedBoosterCount > 0 && cachedPlayerMoveSpeed * speedBoosterMultiplier > playerAgentMover.MaxSpeed)
        {
            speedBoosterCount--;
            playerAgentMover.SetSpeedMultiplier(speedBoosterMultiplier, speedBoosterDuration, cachedPlayerMoveSpeed);
            OnSpeedBoosterCountChanged?.Invoke(speedBoosterCount);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedBooster"))
        {
            speedBoosterCount++;
            OnSpeedBoosterCountChanged?.Invoke(speedBoosterCount);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("StrengthBooster"))
        {
            strengthBoosterCount++;
            OnStrengthBoosterCountChanged?.Invoke(strengthBoosterCount);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HealthBooster"))
        {
            healthBoosterCount++;
            OnHealthBoosterCountChanged?.Invoke(healthBoosterCount);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Coin"))
        {
            CoinCount++;
            OnCoinCountChanged?.Invoke(CoinCount);
            Destroy(other.gameObject);
        }
    }
}
