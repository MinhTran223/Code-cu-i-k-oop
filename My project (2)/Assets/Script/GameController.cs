
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Transform gameField;

    [SerializeField]
    private GameObject _card;

    [SerializeField]
    private Sprite backgroundImage;

    [SerializeField]
    private GameObject onePlayerField, player1Field, player2Field;

    [SerializeField]
    private TMP_Text movesText, pairsText, pairsPlayer1Text, pairsPlayer2Text;


    public Sprite[] sprites;
    public List<Button> buttons = new List<Button>();
    private List<int> locations = new List<int>();
    private int firstSelectedCard = -1, secondSelectedCard = -2, size;
    public int moves = 0, pairs = 0, pairsPlayer1 = 0, pairsPlayer2 = 0;
    private bool turnPlayer1 = true;
    public bool pausePlayer1 = true, pausePlayer2 = true;
    void Awake()
    {
        GridLayoutGroup group = gameField.GetComponent<GridLayoutGroup>();
        size = group.constraintCount * group.constraintCount;
        sprites = Resources.LoadAll<Sprite>("Native Images");
        for (int i = 0; i < size; i++)
        {
            GameObject card = Instantiate(_card);
            card.name = i.ToString();
            card.transform.SetParent(gameField, false);
        }
    }

    public void Start()
    {
        SetUpData();
        SetCards();
        AddListeners();
        GetRandom();
    }

    private void SetUpData()
    {
        turnPlayer1 = true;
        moves = 0;
        pairs = 0;
        pausePlayer1 = true;
        pausePlayer2 = true;
        pairsPlayer1 = 0;
        pairsPlayer2 = 0;
        movesText.text = "Moves: " + moves.ToString();
        pairsText.text = "Pairs: " + pairs.ToString();
        pairsPlayer1Text.text = "Pairs:" + pairsPlayer1.ToString();
        pairsPlayer2Text.text = "Pairs:" + pairsPlayer2.ToString();
        locations.Clear();
    }
    private void GetRandom()
    {

        while (locations.Count < size)
        {
            int j = Random.Range(0, 52);

            if (!locations.Contains(j))
            {
                locations.Add(j);
                locations.Add(j);
            }
        }

        for (int i = 0; i < locations.Count; i++)
        {
            int tmp = locations[i];
            int j = Random.Range(i, locations.Count);
            locations[i] = locations[j];
            locations[j] = tmp;
        }
    }
    void AddListeners()
    {

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PickCard());
        }
    }

    public void PickCard()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int index1 = int.Parse(name);
        int index2 = locations[index1];


        if (firstSelectedCard < 0)
        {

            buttons[index1].image.sprite = sprites[index2];
            firstSelectedCard = index1;
        }
        else if (firstSelectedCard != index1 && secondSelectedCard < 0)
        {
            buttons[index1].image.sprite = sprites[index2];
            secondSelectedCard = index1;
            StartCoroutine(CheckGuessed());
        }

    }
    private IEnumerator CheckGuessed()
    {
        yield return new WaitForSeconds(0.5f);
        moves++;
        movesText.text = "Moves: " + moves.ToString();
        if (locations[firstSelectedCard] != locations[secondSelectedCard])
        {
            if (turnPlayer1)
                turnPlayer1 = false;
            else
                turnPlayer1 = true;
            buttons[firstSelectedCard].image.sprite = backgroundImage;
            buttons[secondSelectedCard].image.sprite = backgroundImage;
        }
        else
        {

            pairs++;
            pairsText.text = "Pairs: " + pairs.ToString();

            if (turnPlayer1)
            {
                pairsPlayer1++;
                pairsPlayer1Text.text = "Pairs: " + pairsPlayer1.ToString();
            }
            else
            {
                pairsPlayer2++;
                pairsPlayer2Text.text = "Pairs: " + pairsPlayer2.ToString();
            }

            buttons[firstSelectedCard].interactable = false;
            buttons[secondSelectedCard].interactable = false;
            buttons[firstSelectedCard].image.color = new Color(0, 0, 0, 0);
            buttons[secondSelectedCard].image.color = new Color(0, 0, 0, 0);
        }
        if (turnPlayer1)
        {
            player1Field.GetComponent<Image>().color = Color.green;
            player2Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            pausePlayer1 = false;
            pausePlayer2 = true;
        }
        else
        {
            player2Field.GetComponent<Image>().color = Color.red;
            player1Field.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            pausePlayer1 = true;
            pausePlayer2 = false;
        }
        firstSelectedCard = -1;
        secondSelectedCard = -2;
    }
    void SetCards()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Card");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            buttons.Add(gameObjects[i].GetComponent<Button>());
            buttons[i].image.sprite = backgroundImage;
            buttons[i].image.color = new Color(255, 255, 255, 255);
        }
    }
}
