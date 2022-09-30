using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class GameController : MonoBehaviour
{
    public TMP_Text timeField;
    public TMP_Text wordToFind;
    public TMP_Text usedLetter;
    public GameObject[] hangman;
    public GameObject winText;
    public GameObject loseText;
    public GameObject replayButton;
    private float time;
    //private string[] fruits = { "APPLE", "ORANGE", "CARROT", "MANGO", "PASSION FRUIT", "AVOCADO", "BROCOLI", "RED CHILI", "PAPAYA", "GRAPES", "ONION", "CURRY BERRY", "KIWI", "PINEAPPLE", "BANANA", "PEACH", "MELON" };
    // File from Assets folder
    private string[] words = File.ReadAllLines(@"Assets/Words.txt");
    private string chosenWord;
    private string hiddenWord;
    private int fails;
    private bool gameEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        //chosenWord = fruits[Random.Range(0, fruits.Length)];
        //chosenWord.IndexOf("");
        chosenWord = words[Random.Range(0, words.Length)];
        hiddenWord = "";
        usedLetter.text = "The letters that were used are: ";

        for (int i = 0; i < chosenWord.Length; i++)
        {
            char letter = chosenWord[i];
            if (char.IsWhiteSpace(letter))
            {
                hiddenWord += "-";
            }
            else
            {
                hiddenWord += "_";
            }
        }
        wordToFind.text = hiddenWord;
        Debug.Log(chosenWord);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd == false)
        {
            time += Time.deltaTime;
            timeField.text = time.ToString("00:00:00");
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1)
        {
            string pressedLetter = e.keyCode.ToString();
            if (chosenWord.Contains(pressedLetter))
            {
                int i = chosenWord.IndexOf(pressedLetter);
                while (i != -1)
                {
                    // Set new hidden word to everything before the i,
                    // change the i to the letter pressed, and everything after the i 
                    // hiddenWord = _ _ N _ _    // on index 2 word: DENIS
                    // chosenWord = D E _ I S 
                    hiddenWord = hiddenWord.Substring(0, i) + pressedLetter + hiddenWord.Substring(i + 1);
                    chosenWord = chosenWord.Substring(0, i) + "_" + chosenWord.Substring(i + 1);
                    i = chosenWord.IndexOf(pressedLetter);
                }
                wordToFind.text = hiddenWord;
            }
            else
            {
                usedLetter.text += pressedLetter + ", ";
                // add a hang man body parts
                hangman[fails].SetActive(true);
                fails++;
            }
            // Case lost game
            if (fails == hangman.Length)
            {
                loseText.SetActive(true);
                replayButton.SetActive(true);
                gameEnd = true;
            }
            // Case won game
            if (!hiddenWord.Contains("_"))
            {
                winText.SetActive(true);
                replayButton.SetActive(true);
                gameEnd = true;
            }
        }
    }

}
