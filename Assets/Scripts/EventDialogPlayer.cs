using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDialogPlayer : MonoBehaviour {

    public GameObject nameObj, dialogObj;
    public GameObject[] imageObjs;

    public Color UNFOCUSED_COLOR;

    Text nameText, dialogText;
    string[] COMMAND_STRING = { "EventLoad",
        "ImageSetting",
        "SetImage1",
        "SetImage2",
        "SetImage3",
        "Focus",
        "Jump",
        "Button"};
    
    string[] imageLink;
    bool[] isFocused, forceFocus;
    GameObject nowFocused, preFocused;

    List<Dictionary<string, object>> data;
    int CommandNumber = -1;
    int line = 0;

    // Use this for initializatio
    void Start() {
        nameText = nameObj.GetComponent<Text>();
        dialogText = dialogObj.GetComponent<Text>();

        data = CSVReader.Read("Events/event1");

        CheckCommand();
        ExecuteCommand();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            line++;
            CheckCommand();
            ExecuteCommand();
        }
    }

    void ExecuteCommand() {
        switch (CommandNumber) {
            case -1: // IDLE
                nameText.text = "" + data[line]["Char"];
                dialogText.text = "" + data[line]["Dialog"];

                for (int i = 0; i < imageLink.Length; i++)
                {
                    if (nameText.text.Equals(imageLink[i]))
                    {
                        StopCoroutine("UnFocus");
                        StopCoroutine("Focus");
                        StartCoroutine("Focus", imageObjs[i]);
                        StartCoroutine("UnFocus", preFocused);
                    }
                }
                break;
            case 0: // EventLoad
                EventLoad("" + data[line]["Dialog"]);
                break;
            case 1: // ImageLink
                ImageLink();
                break;
            case 2: // SetImage1
                SetImage(imageObjs[0], Resources.Load<Sprite>("" + data[line]["Dialog"]));
                line++;
                CheckCommand();
                ExecuteCommand();
                break;
            case 3: // SetImage2
                SetImage(imageObjs[1], Resources.Load<Sprite>("" + data[line]["Dialog"]));
                line++;
                CheckCommand();
                ExecuteCommand();
                break;
            case 4: // SetImage3
                SetImage(imageObjs[2], Resources.Load<Sprite>("" + data[line]["Dialog"]));
                line++;
                CheckCommand();
                ExecuteCommand();
                break;
            case 5: // Focus
                StartCoroutine("Focus", imageObjs[(int)data[line]["Dialog"]]);
                break;
            case 6: // Jump
                break;
            case 7: // Button
                break;
        }
    }

    void CheckCommand() {
        CommandNumber = -1;
        for (int i = 0; i < COMMAND_STRING.Length; i++)
        {
            if (data[line]["Char"].Equals(COMMAND_STRING[i]))
            {
                CommandNumber = i;
                break;
            }
        }
        Debug.Log(CommandNumber);
    }

    void EventLoad(string path) {
        data = CSVReader.Read(path);
        line = 0;

        CheckCommand();
        ExecuteCommand();
    }

    void ImageLink() {
        int linkAmount = (int)data[line]["Dialog"];
        imageLink = new string[linkAmount];
        Debug.Log(linkAmount);
        for (int i = 0; i < linkAmount; i++)
        {
            line++;
            if (i < 3)
            {
                imageLink[i] = "" + data[line]["Char"];
                SetImage(imageObjs[i], Resources.Load<Sprite>("" + data[line]["Dialog"]));
                Debug.Log("char " + data[line]["Char"] + " dialog " + data[line]["Dialog"]);    

            }
        }

        line++;
        CheckCommand();
        ExecuteCommand();
    }

    void SetImage(GameObject imageObj, Sprite sprite) {
        imageObj.SetActive(true);
        imageObj.GetComponent<Image>().overrideSprite = sprite;
        imageObj.GetComponent<Image>().color = UNFOCUSED_COLOR;
    }

    IEnumerator Focus(GameObject obj) {
        preFocused = nowFocused;
        nowFocused = obj;

        for (int i = 0; i < 500; i++)
        {
            obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, Color.white, 0.1f);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator UnFocus(GameObject obj)
    {
        for (int i = 0; i < 500; i++)
        {
            obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, UNFOCUSED_COLOR, 0.1f);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
