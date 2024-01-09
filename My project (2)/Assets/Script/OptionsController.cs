using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject sizeForm, modeForm, gameField, gameField4x4, gameField6x6, gameField8x8, gameField10x10, onePlayerField, player1Field, player2Field;

    [SerializeField]
    private GameObject playButton, pauseButton, option, savedNotification, winNotification;

    [SerializeField]
    private TMP_Text timeText, timePlayer1Text, timePlayer2Text, notification;

    [SerializeField]
    private Image fillPlayer1, fillPlayer2;

    private float timeClick,time=0;
    public void OpenModeForm()
    {
        modeForm.SetActive(true);
        sizeForm.SetActive(false);
        gameField.SetActive(false);
        gameField4x4.SetActive(false);
        gameField6x6.SetActive(false);
        gameField8x8.SetActive(false);
        gameField10x10.SetActive(false);
        onePlayerField.SetActive(false);
        player1Field.SetActive(false);
        player2Field.SetActive(false);
    }
    public void OpenSizeForm1Player()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(true);
        gameField.SetActive(false);
        onePlayerField.SetActive(true);
    }
    public void OpenSizeForm2Player()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(true);
        gameField.SetActive(false);
        player1Field.SetActive(true);
        player2Field.SetActive(true);
    }
    public void OpenGameField4x4()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField4x4.SetActive(true);
    }

    public void OpenGameField6x6()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);  
        gameField6x6.SetActive(true);

    }
    public void OpenGameField8x8()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField8x8.SetActive(true);
    }
    public void OpenGameField10x10()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField10x10.SetActive(true);
    }

    private void DisableCard()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Card");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].GetComponent<Button>().interactable = false;

        }
    }

    private void EnableCard()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Card");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].GetComponent<Button>().interactable = true;

        }
    }
    private int duration = 600, timePlayer1=1, timePlayer2=1;
    private bool pausePlayer1 = true, pausePlayer2 = false;
    private void RunTurnTimer1(Image fillPlayer, TMP_Text timePlayerText)
    {

        timePlayer1 = duration;
        StartCoroutine(UpdateTimer1(fillPlayer, timePlayerText));
    }
    private IEnumerator UpdateTimer1(Image fillPlayer, TMP_Text timePlayerText)
    {

        while (timePlayer1 >= 0)
        {
            if (!pausePlayer1)
            {
                timePlayerText.text = string.Format("{0:00}: {1:00}", timePlayer1 / 60, timePlayer1 % 60);
                fillPlayer.fillAmount = Mathf.InverseLerp(0, duration, timePlayer1);
                timePlayer1--;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }
    private void RunTurnTimer2(Image fillPlayer, TMP_Text timePlayerText)
    {

        timePlayer2 = duration;
        StartCoroutine(UpdateTimer2(fillPlayer, timePlayerText));
    }
    private IEnumerator UpdateTimer2(Image fillPlayer, TMP_Text timePlayerText)
    {

        while (timePlayer2 >= 0)
        {
            if (!pausePlayer2)
            {
                timePlayerText.text = string.Format("{0:00}: {1:00}", timePlayer2 / 60, timePlayer2 % 60);
                fillPlayer.fillAmount = Mathf.InverseLerp(0, duration, timePlayer2);
                timePlayer2--;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    public void PlayGame()
    {
        CheckGameField().pausePlayer1 = false;
        CheckGameField().pausePlayer2 = true;
        player1Field.GetComponent<Image>().color = Color.green;
        player2Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        EnableCard();
        timeClick= Time.time;
        RunTurnTimer1(fillPlayer1, timePlayer1Text);
        RunTurnTimer2(fillPlayer2, timePlayer2Text);
    }

    int status;
    public void PauseGame()
    {
        time = time+ Time.time - timeClick;
        option.SetActive(true);
        pauseButton.SetActive(false);
        DisableCard();
        if (CheckGameField().pausePlayer1 == false)
            status = 1;
        else 
            status=2;
        CheckGameField().pausePlayer1 = true;
        CheckGameField().pausePlayer2 = true;
    }

    public void Resume()
    {
        timeClick = (int)Time.time;
        option.SetActive(false);
        pauseButton.SetActive(true);
        EnableCard();
        if (status == 1)
            CheckGameField().pausePlayer1 = false;
        else
            CheckGameField().pausePlayer2 = false;
    }

    private GameController CheckGameField()
    {
        if (gameField4x4.activeSelf == true)
            return gameField4x4.GetComponent<GameController>();
        else if(gameField6x6.activeSelf == true)
            return gameField6x6.GetComponent<GameController>();
        else if (gameField8x8.activeSelf == true)
            return gameField8x8.GetComponent<GameController>();
        else 
            return gameField10x10.GetComponent<GameController>();
    }
    public void Restart()
    {
        time = 0;
        timeText.text = "00:00:00";
        option.SetActive(false);
        CheckGameField().Start();
        StopAllCoroutines();
        playButton.SetActive(true);
        timePlayer1 = 1;
        timePlayer2 = 1;
        timePlayer1Text.text = string.Format("{0:00}: {1:00}", 0, 0);
        fillPlayer1.fillAmount = Mathf.InverseLerp(0, duration, 600);
        timePlayer2Text.text = string.Format("{0:00}: {1:00}", 0, 0);
        fillPlayer2.fillAmount = Mathf.InverseLerp(0, duration, 600);
        player1Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        player2Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void Menu()
    {
        option.SetActive(false);
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        SceneManager.LoadScene("Menu&form");
    }

    public void Exit()
    {
        savedNotification.SetActive(true);
        option.SetActive(false);
    }

    public void no_Save()
    {
        savedNotification.SetActive(false);
        Application.Quit();
    }

    public void yes_Save()
    {
        savedNotification.SetActive(false);
        Application.Quit();
    }

    public void PlayAgain()
    {
        winNotification.SetActive(false);
        Restart();
    }

    public void NewGame()
    {
        winNotification.SetActive(false);
        OpenModeForm();
        Restart();
    }
    private string FomatTime(float time)
    {
        int intTime = (int)time;
        int hour = intTime / 3600;
        int minute = intTime / 60;
        int second = intTime % 60;
        string stringTime=string.Format("{0:00}: {1:00}: {2:00}",hour,minute,second);
        return stringTime;
    }

    private bool IsEndGame()
    {

        if (gameField4x4.GetComponent<GameController>().pairs == 8 || gameField6x6.GetComponent<GameController>().pairs == 18 || gameField8x8.GetComponent<GameController>().pairs == 32 || gameField10x10.GetComponent<GameController>().pairs == 50 || timePlayer2 <= 0 || timePlayer1 <= 0){
            return true; 
        }
        else 
        return false;
    }
    public void BacktoMenu()
    {
        PlayerPrefs.SetInt("IsLoggedIn", 1);
        SceneManager.LoadScene("Menu&form");
    }

    private void Update()
    {
        if (playButton.activeSelf == false && option.activeSelf == false)
        {
            timeText.text = FomatTime(time + Time.time - timeClick);
        }
        if (IsEndGame())
        {
            if (onePlayerField.activeSelf == true)
                notification.text = "You win";
            else if (CheckGameField().pairsPlayer1 > CheckGameField().pairsPlayer2)
                notification.text = "Player 1 win";
            else if (CheckGameField().pairsPlayer1 < CheckGameField().pairsPlayer2)
                notification.text = "Player 2 win";
            else if (timePlayer1 > timePlayer2)
                notification.text = "Player 1 win";
            else
                notification.text = "Player 2 win";
            winNotification.SetActive(true);
            CheckGameField().pairs = 0;
            timePlayer1 = 1;
            timePlayer2 = 1;
            CheckGameField().pausePlayer1 = true;
            CheckGameField().pausePlayer2 = true;
            player1Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            player2Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        pausePlayer1 = CheckGameField().pausePlayer1;
        pausePlayer2 = CheckGameField().pausePlayer2;
    }
}
