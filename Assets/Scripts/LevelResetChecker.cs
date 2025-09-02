using UnityEngine;
using UnityEngine.UI;

public class LevelResetChecker : MonoBehaviour
{
    public Button[] levelButtons;      // Level buttons (Level 1 to 8)
    public GameObject[] lockIcons;     // Lock icons over each level button (Level 1 to 8)

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1;

            // Default: Level 1 unlocked, rest locked unless PlayerPrefs says otherwise
            bool isUnlocked = PlayerPrefs.GetInt("Level" + levelNumber, levelNumber == 1 ? 1 : 0) == 1;

            levelButtons[i].interactable = isUnlocked;

            // Manage lock image visibility
            if (lockIcons != null && i < lockIcons.Length && lockIcons[i] != null)
            {
                lockIcons[i].SetActive(!isUnlocked);  // Show lock only if level is locked
            }
        }
    }
}
