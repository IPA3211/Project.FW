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
        "ImageSetting","SetImage1","SetImage2", "Setimage3","Focus" ,"Go" };
    
    string[] imageLink;


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
        if (CommandNumber == -1)
        {
            nameText.text = "" + data[line]["Char"];
            dialogText.text = "" + data[line]["Dialog"];

            for (int i = 0; i < imageLink.Length; i++) {
                if (nameText.text.Equals(imageLink[i])) {
                    Focus(imageObjs[i]); 
                }
            }
        }
        else if (CommandNumber == 0)
        {
            EventLoad("" + data[line]["Dialog"]);

            line++;
            CheckCommand();
            ExecuteCommand();
        }
        else if (CommandNumber == 1)
        {
            ImageLink();

            line++;
            CheckCommand();
            ExecuteCommand();
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
    }

    void Focus(GameObject obj) {
    }

    void SetImage(GameObject imageObj, Sprite sprite) {
        imageObj.SetActive(true);
        imageObj.GetComponent<Image>().overrideSprite = sprite;
        imageObj.GetComponent<Image>().color = UNFOCUSED_COLOR;
    }
}
