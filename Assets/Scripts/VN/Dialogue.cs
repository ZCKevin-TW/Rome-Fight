using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI DialogueText;
    public TextMeshProUGUI SpeakerName;
    public float textSpeed;
    private int index;
    private string[] words;
    List<string> lines = new List<string>();
    [SerializeField] string DialogueFileName;
    [SerializeField] string NPCname;
    [SerializeField] string Name4Animation;
    [SerializeField] CucumberIntroEvent cucumberIntroEvent;
    [SerializeField] FightingResults record;
    bool IsWin { get => record == null ? false : record.playerWin; }
    [SerializeField] bool AfterFight = false;
    Animator animator;

    private void Start()
    {
        if (NPCname != "NONE")
        {
            var NPC = GameObject.Find(NPCname);
            animator = NPC.GetComponent<Animator>();
            Debug.Log(animator);
        }
        ResetDialogue();
    }
    public void ResetDialogue()
    {
        DialogueText.text = string.Empty;
        index = 0;

        lines.Clear();
        try
        {
            if (AfterFight)
                DialogueFileName = (IsWin)? "Win.txt" : "Lose.txt";

            using (StreamReader sr = new StreamReader("Assets/Dialogues/" + DialogueFileName))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line) || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"file not found {ex.FileName}");
        }

        words = lines[index].Split("#");
        StartCoroutine(TypeLine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (DialogueText.text == words[1])
                Nextline();
            else
            {
                StopAllCoroutines();
                DialogueText.text = words[1];
            }
        }
    }

    void Nextline()
    {
        if(index < lines.Count - 1)
        {
            index++;
            DialogueText.text = string.Empty;
            words = lines[index].Split("#");
            Debug.Log(words.Length);
            if (animator && words[0] == Name4Animation && words.Length >= 3)
            {
                animator.Play(words[2]);
            }
            StartCoroutine(TypeLine());
        }
        else
        {
            if (NPCname == "Ccucumber1")
            {
                cucumberIntroEvent.IntroOutro();
            }
            if (AfterFight)
            {
                if(IsWin) SceneManager.LoadScene("EndMenu");
                else SceneManager.LoadScene("MainPlay");
            }

            gameObject.SetActive(false);
           
        }
    }


    IEnumerator TypeLine()
    {
        SpeakerName.text = words[0];
        foreach (char c in words[1].ToCharArray())
        {
            if (c == 'n')
                DialogueText.text += '\n';
            else
                DialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
