using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartPlay : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskonClick);
    }

    // Update is called once per frame
    void TaskonClick()
    {
        SceneManager.LoadScene("MainPlay");
    }
}
