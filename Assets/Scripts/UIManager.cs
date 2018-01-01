using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Animator MenuAnimator;
    public Text PointsText;
    public Text LivesText;
    public Text TimerText;
    public Image Menu;
    public GameManager MyGameManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void SetPoints(int p)
    { 
        PointsText.text = string.Format("{0}: {1}", "Points", p);
    }

    public void UpdateLives(int l)
    {
        LivesText.text = string.Format("{0}: {1}", "Lives", l);
    }

    public void UpdateTimer(float t)
    {
        TimerText.text = (Mathf.Round(t * 10f) / 10f).ToString(CultureInfo.CurrentCulture);
    }

    public void EnableMenu(bool b)
    {
        MenuAnimator.SetBool("GameOver", b);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        EnableMenu(false);
        SetPoints(0);
        UpdateLives(1);
        MyGameManager.RestartGame();
    }
}
