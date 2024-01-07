
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
    [SerializeField] private Transform gameField;

    [SerializeField] private GameObject _card;

    [SerializeField] private TMP_Text movesText, pairsText;

    public List<Button> buttons = new List<Button>();
    [SerializeField] private Sprite backgroundImage;
    public Sprite[] sprites;
    private List<int> locations = new List<int>();
    private int firstSelectedCard=-1,secondSelectedCard=-2,size;
    private int moves=0, pairs=0;

    void Awake()
    {
        GridLayoutGroup group=gameField.GetComponent<GridLayoutGroup>();
        size=group.constraintCount*group.constraintCount;
        sprites = Resources.LoadAll<Sprite>("Native Images");
        for (int i = 0; i < size; i++)
        {
            GameObject card = Instantiate(_card);
            card.name = i.ToString();
            card.transform.SetParent(gameField, false);
        }
    }

    void Start()
    {
        GetButtons();
        AddListeners();
        GetRandom();
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
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(() => PickCard());
        }
    }
    
    public void PickCard()
    {
        string name=EventSystem.current.currentSelectedGameObject.name;
        int index1 = int.Parse(name);
        int index2= locations[index1];


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
            buttons[firstSelectedCard].image.sprite = backgroundImage;
            buttons[secondSelectedCard].image.sprite = backgroundImage;
        }
        else
        {
            pairs++;
            pairsText.text = "Pairs: " + pairs.ToString();
            buttons[firstSelectedCard].interactable = false;
            buttons[secondSelectedCard].interactable = false;
            buttons[firstSelectedCard].image.color = new Color(0, 0, 0, 0);
            buttons[secondSelectedCard].image.color = new Color(0, 0, 0, 0);
        }
        firstSelectedCard = -1;
        secondSelectedCard = -2;
    }
    void GetButtons() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Button");
        for (int i= 0;i < gameObjects.Length;i++)
        {
            buttons.Add(gameObjects[i].GetComponent<Button>());
            buttons[i].image.sprite = backgroundImage;
        }
    }

    void CalculateSizeCell()
    {
        float widthPanel=gameField.GetComponent<RectTransform>().rect.width;
        float heightPanel = gameField.GetComponent<RectTransform>().rect.height;
        float n = gameField.GetComponent<GridLayoutGroup>().constraintCount;
        float widthCell = (widthPanel - (n - 1) * 10) / n - (float)0.5;
        float heightCell = (heightPanel - (n - 1) * 10) / n - (float)0.5;
        gameField.GetComponent<GridLayoutGroup>().cellSize = new Vector2(widthCell, heightCell);
    }
    void Update()
    {
        // CalculateSizeCell();
    }
}
