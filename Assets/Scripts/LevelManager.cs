using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Don’t destroy when scenes change
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate LevelManagers
        }
    }
    private void Start()
    {
        // Nothing here now
    }

    public void OnApplicationQuit()
    {
        // Reset levels only if Level 8 was completed before quit
        if (PlayerPrefs.GetInt("ShouldResetLevelsNextTime", 0) == 1)
        {
            ResetAllLevels();
            PlayerPrefs.SetInt("ShouldResetLevelsNextTime", 0);
            PlayerPrefs.Save();
        }
    }

    public void ResetAllLevels()
    {
        for (int i = 2; i <= 8; i++)
        {
            PlayerPrefs.SetInt("Level" + i, 0);
        }

        PlayerPrefs.SetInt("Level1", 1); // Always keep Level 1 unlocked
        PlayerPrefs.Save();
    }
    public void ResetLevelsFromButton()
    {
        // Lock Level 2–8
        for (int i = 2; i <= 8; i++)
        {
            PlayerPrefs.SetInt("Level" + i, 0);
        }

        // Always keep Level 1 unlocked
        PlayerPrefs.SetInt("Level1", 1);

        // Prevent auto-reset on quit
        PlayerPrefs.SetInt("ShouldResetLevelsNextTime", 0);

        PlayerPrefs.Save();

        // Reload the scene so UI updates
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}