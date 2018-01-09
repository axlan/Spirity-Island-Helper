using UnityEngine;
using System.Collections;


using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.
    private int playerCount = 2;
    
    private Button startButton;
    private Dropdown numPlayersDropdown;
    private Text endGameText;

    private Button addFearButton;
    private Text fearPoolText;
    private Text fearAtLevelText;
    private Text fearActiveText;
    private Text fearEarnedText;
    private int fearEarnedCount;
    private int fearPoolCount = 0;
    private int fearCardCount = 0;
    private GameObject fear2TabImage;
    private GameObject fear3TabImage;
    private GameObject fear2FullImage;
    private GameObject fear3FullImage;
    private GameObject fearBackImage;

    private Button removeBlightButton;
    private Text blightText;
    private int blightCount;
    private FearCards fearCards;

    public enum InvadePhase
    {
        TimePass,
        Invade
    }
    private static readonly string[] LAND0_SET = { "j1", "m1", "s1", "w1" };
    private static readonly string[] LAND1_SET = { "j1", "m1", "s1", "w1", "coastal2" };
    private static readonly string[] LAND2_SET = { "mj3", "sm3", "mw3", "jw3", "js3", "sw3" };
    private static readonly string[][] LAND_SETS = { LAND0_SET, LAND1_SET, LAND2_SET };
    private static List<string>[] invader_deck;
    private static List<string> invader_cards_showing;
    private Image invadeRavageImage;
    private Image invadeBuildImage;
    private Image invadeExploreImage;
    private Image invadeDiscardImage;
    private InvadePhase invadePhase;
    private Button invadeButton;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //This is called each time a scene is loaded.
    void OnLevelWasLoaded(int index)
    {
        //Call InitGame to initialize our level.
        InitGame();
    }

    void SetInvadeSprite(Image image, string spriteName)
    {
        Sprite sprite = Resources.Load<Sprite>(spriteName);
        image.sprite = sprite;
    }

    int GetFearLevel()
    {
        return fearCardCount / 3 + 1;
    }

    void UpdateBoard()
    {
        if (fearPoolCount == 0)
        {
            fearPoolCount = playerCount * 4;
            fearCardCount++;
            fearEarnedCount++;
        }
        fearPoolText.text = "" + fearPoolCount + " Fear in Pool";
        int cardsAtLevel = 3 - fearCardCount % 3;
        int fearLevel = GetFearLevel();
        if (fearLevel == 4)
        {
            fearAtLevelText.text = "";
        } else
        {

            fearAtLevelText.text = cardsAtLevel + " Level " + fearLevel + " Cards Left";
        }
        fearBackImage.SetActive(fearLevel < 4);
        fear2TabImage.SetActive(fearLevel < 2);
        fear3TabImage.SetActive(fearLevel < 3);
        fear2FullImage.SetActive(fearLevel == 2);
        fear3FullImage.SetActive(fearLevel > 2);
        fearEarnedText.text = fearEarnedCount + " Earned Fear";

        blightText.text = "" + blightCount + " Blight in Pool";

        if (invadePhase == InvadePhase.TimePass)
        {
            if (invader_deck[0].Count > 0)
            {
                SetInvadeSprite(invadeExploreImage, "invadeback1");
            } else if (invader_deck[1].Count > 0)
            {
                SetInvadeSprite(invadeExploreImage, "invadeback2");
            } else if (invader_deck[2].Count > 0)
            {
                SetInvadeSprite(invadeExploreImage, "invadeback3");
            }
            else
            {
                invadeExploreImage.gameObject.SetActive(false);
            }
            if (invader_cards_showing.Count > 0)
            {
                invadeBuildImage.gameObject.SetActive(true);
                SetInvadeSprite(invadeBuildImage, invader_cards_showing[0]);
            }
            if (invader_cards_showing.Count > 1)
            {
                invadeRavageImage.gameObject.SetActive(true);
                SetInvadeSprite(invadeRavageImage, invader_cards_showing[1]);
            }
            if (invader_cards_showing.Count > 2)
            {
                invadeDiscardImage.gameObject.SetActive(true);
                SetInvadeSprite(invadeDiscardImage, invader_cards_showing[2]);
            }
        }
        else
        {
            SetInvadeSprite(invadeExploreImage, invader_cards_showing[0]);
            if (invader_cards_showing.Count > 1)
            {
                invadeBuildImage.gameObject.SetActive(true);
                SetInvadeSprite(invadeBuildImage, invader_cards_showing[1]);
            }
            if (invader_cards_showing.Count > 2)
            {
                invadeRavageImage.gameObject.SetActive(true);
                SetInvadeSprite(invadeRavageImage, invader_cards_showing[2]);
            }
            if (invader_cards_showing.Count > 3)
            {
                invadeDiscardImage.gameObject.SetActive(true);
                SetInvadeSprite(invadeDiscardImage, invader_cards_showing[3]);
            }
        }
    }

    void AddFear()
    {
        if (fearCardCount < 9)
        {
            fearPoolCount--;
        }
        UpdateBoard();
        if (fearCardCount == 9)
        {
            endGameText.text = "Spirits Win";
            endGameText.gameObject.SetActive(true);
        }
    }

    void AdvanceInvaders()
    {
        switch (invadePhase)
        {
            case InvadePhase.TimePass:
                string newCard = null;
                for (int i = 0; i < invader_deck.Length; i++)
                {
                    if (invader_deck[i].Count > 0)
                    {
                        newCard = invader_deck[i][0];
                        invader_deck[i].Remove(newCard);
                        break;
                    }
                }
                if (newCard == null)
                {
                    endGameText.text = "Invaders Win";
                    endGameText.gameObject.SetActive(true);
                    return;
                }
                invadePhase = InvadePhase.Invade;
                invader_cards_showing.Insert(0, newCard);
                int fearLevel = GetFearLevel();
                for (; fearEarnedCount > 0; fearEarnedCount--)
                {
                    FearCard next = fearCards.GetNextCard();
                    fearActiveText.text += next.name + ": " + next.levels[fearLevel - 1] + "\n";
                }
                break;
            default:
                invadePhase = InvadePhase.TimePass;
                fearActiveText.text = "";
                break;
        }
        UpdateBoard();
    }

    void InitBoard()
    {
        fearCardCount = -1;
        fearEarnedCount = -1;
        fearPoolCount = 0;
        fearCards = new FearCards();
        playerCount = numPlayersDropdown.value + 2;
        blightCount = playerCount * 5;

        invader_deck = new List<string>[3];
        for (int i = 0; i < LAND_SETS.Length; i++)
        {
            invader_deck[i] = new List<string>(LAND_SETS[i]);
            FearCards.Shuffle(invader_deck[i]);
            invader_deck[i].RemoveAt(0);
        }
        invadePhase = InvadePhase.TimePass;
        invadeExploreImage.gameObject.SetActive(true);
        invadeBuildImage.gameObject.SetActive(false);
        invadeRavageImage.gameObject.SetActive(false);
        invadeDiscardImage.gameObject.SetActive(false);
        invader_cards_showing = new List<string>();

        endGameText.gameObject.SetActive(false);

        UpdateBoard();
    }

    void RemoveBlight()
    {
        if (blightCount > 0)
        {
            blightCount--;
            UpdateBoard();
        }
        if (blightCount == 0)
        {
            endGameText.text = "Blighted Island";
            endGameText.gameObject.SetActive(true);
        }
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;

        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
        fearPoolText = GameObject.Find("FearPoolText").GetComponent<Text>();
        fearAtLevelText = GameObject.Find("FearAtLevelText").GetComponent<Text>();
        fearEarnedText = GameObject.Find("FearEarnedText").GetComponent<Text>();
        fearActiveText = GameObject.Find("FearActiveText").GetComponent<Text>();

        addFearButton = GameObject.Find("AddFearButton").GetComponent<Button>();
        addFearButton.onClick.AddListener(AddFear);

        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        startButton.onClick.AddListener(InitBoard);

        numPlayersDropdown = GameObject.Find("NumPlayersDropdown").GetComponent<Dropdown>();
        
        fear2TabImage = GameObject.Find("Fear2TabImage");
        fear3TabImage = GameObject.Find("Fear3TabImage");
        fear2FullImage = GameObject.Find("Fear2FullImage");
        fear3FullImage = GameObject.Find("Fear3FullImage");
        fearBackImage = GameObject.Find("FearBackImage");

        removeBlightButton = GameObject.Find("BlightButton").GetComponent<Button>();
        blightText = GameObject.Find("BlightText").GetComponent<Text>();
        removeBlightButton.onClick.AddListener(RemoveBlight);

        invadeExploreImage = GameObject.Find("InvadeExploreImage").GetComponent<Image>();
        invadeBuildImage = GameObject.Find("InvadeBuildImage").GetComponent<Image>();
        invadeRavageImage = GameObject.Find("InvadeRavageImage").GetComponent<Image>();
        invadeDiscardImage = GameObject.Find("InvadeDiscardImage").GetComponent<Image>();
        invadeButton = GameObject.Find("InvadeButton").GetComponent<Button>();
        invadeButton.onClick.AddListener(AdvanceInvaders);

        endGameText = GameObject.Find("EndGameText").GetComponent<Text>();

        InitBoard();

        //Set levelImage to active blocking player's view of the game board during setup.
        //levelImage.SetActive(true);

    }


    //Update is called every frame.
    void Update()
    {

    }

}