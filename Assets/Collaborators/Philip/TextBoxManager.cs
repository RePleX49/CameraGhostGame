using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBoxManager : MonoBehaviour
{
    public TextAsset textfile;
    public List<string> textLines;
    public int currentLine;
    public int stopLine;

    public bool startedPhase; //int that lets you know that a phase has already begun (useful in calling coroutines)

    public TMP_Text text;
    public TMP_Text personTalking;

    public Coroutine currentScrollType;

    public float typeSpeed;       //how fast the scroll works
    public bool isTyping;       //used for checking if textScroll is happening
    public bool cancelTyping;   //used to check if player wants to cancel the scroll

    public float stayTime; //how long the text will stay before fading out
    public bool goText;    //if coroutine is happening

    public GameObject personTalkingBox;
    public GameObject textbox;

    public int textNumber;
    public GameObject eventManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        eventManager = GameObject.Find("EventManager");
        textNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
        textbox = GameObject.Find("Player(Clone)/UI_Canvas/Image");
        personTalkingBox = GameObject.Find("Player(Clone)/UI_Canvas/Image (1)");
        textbox.SetActive(true);
        personTalkingBox.SetActive(true);
        text = GameObject.Find("Player(Clone)/UI_Canvas/Image/Text (TMP)").GetComponent<TextMeshProUGUI>();
        personTalking = GameObject.Find("Player(Clone)/UI_Canvas/Image (1)/Text (TMP)").GetComponent<TextMeshProUGUI>();
        text.gameObject.SetActive(true);
        textLines = new List<string>(textfile.text.Split('\n'));
        currentLine = 0;
        //text.text = textLines[currentLine];
        typeSpeed = 0.03f;
        isTyping = false;
        cancelTyping = false;
        stayTime = 999f;
        goText = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!goText)
        {
            Scroll();
            goText = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && currentLine == textLines.Count - 1 && cancelTyping)
        {
            text.text = "";
            eventManager.SendMessage("FinishDialogue", textNumber);
            personTalkingBox.gameObject.SetActive(false);
            textbox.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !cancelTyping)
        {
            cancelTyping = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && cancelTyping)
        {
            NextLine();
        }
    }

    public void Scroll()
    {
        currentScrollType = StartCoroutine(ScrollTyping());
    }

    public void NextLine()
    {
        StopCoroutine(currentScrollType);
        currentLine++;
        goText = false;
    }

    public IEnumerator ScrollTyping()
    {
        int index = 0;
        text.text = "";
        int talkingID = int.Parse(textLines[currentLine].Substring(0, 1));
        if(talkingID == 0)
        {
            text.alignment = TextAlignmentOptions.Center;
            personTalkingBox.gameObject.SetActive(false);
        }
        else if(talkingID == 1)
        {
            text.alignment = TextAlignmentOptions.TopLeft;
            personTalkingBox.gameObject.SetActive(true);
            personTalking.text = "PLAYER";
        }
        else if(talkingID == 2)
        {
            text.alignment = TextAlignmentOptions.Left;
            personTalkingBox.gameObject.SetActive(true);
            personTalking.text = "CECIL";
        }
        isTyping = true;
        cancelTyping = false;
        string currentTextLine = textLines[currentLine].Substring(1);
        while (isTyping && !cancelTyping && (index < currentTextLine.Length - 1))
        {
            text.text += currentTextLine[index];
            index++;
            yield return new WaitForSeconds(typeSpeed);
        }
        cancelTyping = true;
        text.text = textLines[currentLine].Substring(1);
        yield return new WaitForSeconds(stayTime);
        currentLine++;
        goText = false;
    }
}
