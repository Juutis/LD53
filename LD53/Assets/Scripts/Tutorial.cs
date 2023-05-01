using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial1;
    [SerializeField]
    private GameObject tutorial1part2;

    [SerializeField]
    private GameObject tutorial2;

    [SerializeField]
    private GameObject tutorial3; 

    [SerializeField]
    private GameObject tutorial4;

    [SerializeField]
    private GameObject bg;

    public static Tutorial main;

    void Awake() {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        bg.SetActive(false);
        tutorial1.SetActive(false);
        Invoke("Show1", 1.0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
            if (tutorial1.activeInHierarchy) {
                Ok1();
            }
            if (tutorial1part2.activeInHierarchy) {
                Ok1part2();
            }
            if (tutorial2.activeInHierarchy) {
                Ok2();
            }
            if (tutorial3.activeInHierarchy) {
                Ok3();
            }
            if (tutorial4.activeInHierarchy) {
                Ok4();
            }
        }
    }

    public void Show1() {
        bg.SetActive(true);
        tutorial1.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Show1part2() {
        bg.SetActive(true);
        tutorial1part2.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Show2() {
        bg.SetActive(true);
        tutorial2.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Show3() {
        bg.SetActive(true);
        tutorial3.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Show4() {
        bg.SetActive(true);
        tutorial4.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Skip() {
        bg.SetActive(false);
        WorldManager.main.Tutorial = false;
        Time.timeScale = 1.0f;
    }

    public void Ok1() {
        bg.SetActive(false);
        tutorial1.SetActive(false);
        Time.timeScale = 1.0f;
        Invoke("Show1part2", 0.5f);
    }

    public void Ok1part2() {
        bg.SetActive(false);
        tutorial1part2.SetActive(false);
        Time.timeScale = 1.0f;
        Invoke("Show2", 4.0f);
    }

    public void Ok2() {
        bg.SetActive(false);
        tutorial2.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Ok3() {
        bg.SetActive(false);
        tutorial3.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Ok4() {
        bg.SetActive(false);
        tutorial4.SetActive(false);
        Time.timeScale = 1.0f;
        WorldManager.main.Tutorial = false;
    }
}
