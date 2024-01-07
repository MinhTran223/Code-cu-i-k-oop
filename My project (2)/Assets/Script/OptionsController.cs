using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject sizeForm, modeForm, gameField, gameField4x4, gameField6x6, gameField8x8, gameField10x10;

    [SerializeField]
    private GameObject playButton,pauseButton, resumeButton, option, savedNotification;

    [SerializeField]
    private TMP_Text timerText;
    private int timeClick,timer=0;
    public void OpenModeForm()
    {
        modeForm.SetActive(true);
        sizeForm.SetActive(false);
        gameField.SetActive(false);

    }
    public void OpenSizeForm()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(true);
        gameField.SetActive(false);

    }
    public void OpenGameField4x4()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField4x4.SetActive(true);
        gameField6x6.SetActive(false);
        gameField8x8.SetActive(false);
        gameField10x10.SetActive(false);

    }

    public void OpenGameField6x6()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField4x4.SetActive(false);
        gameField6x6.SetActive(true);
        gameField8x8.SetActive(false);
        gameField10x10.SetActive(false);
    }
    public void OpenGameField8x8()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField4x4.SetActive(false);
        gameField6x6.SetActive(false);
        gameField8x8.SetActive(true);
        gameField10x10.SetActive(false);
    }
    public void OpenGameField10x10()
    {
        modeForm.SetActive(false);
        sizeForm.SetActive(false);
        gameField.SetActive(true);
        gameField4x4.SetActive(false);
        gameField6x6.SetActive(false);
        gameField8x8.SetActive(false);
        gameField10x10.SetActive(true);
    }

    public void PlayGame()
    {
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Button");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].GetComponent<Button>().interactable = true;

        }
        timeClick=(int) Time.time;
    }

    public void PauseGame()
    {
        timer = timer+ (int)Time.time - timeClick;
        option.SetActive(true);
    }

    public void Resume()
    {
        timeClick = (int)Time.time;
        option.SetActive(false);    
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

    private void Update()
    {
        if (playButton.activeSelf == false && option.activeSelf == false)
            timerText.text = FomatTime(timer + (int)Time.time - timeClick);
    }
}
