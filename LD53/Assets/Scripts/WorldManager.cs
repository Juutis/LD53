using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{

    [SerializeField]
    private GameObject world;

    float worldWidth = 5000;
    float visionRange = 2000;

    [SerializeField]
    Toggle invert;

    [SerializeField]
    Toggle mute;

    public bool invertControls;

    private GameObject player;
    private Rigidbody playerRb;
    [SerializeField]
    private GameObject[] stuffToMoveWithPlayer;
    private float boundDistance;

    [SerializeField]
    private Transform[] cinemachineTargeObjects;

    // Start is called before the first frame update

    public static WorldManager main;

    public List<Camp> camps;

    public bool Tutorial = true;

    [SerializeField]
    private GameObject youWin;

    public int campCount;

    public void RegisterCamp(Camp camp) {
        camps.Add(camp);
        campCount++;
    }

    public bool Win = false;

    public void CampCompleted(Camp camp) {
        camps.Remove(camp);
        if (camps.Count == 0 && !Win) {
            youWin.SetActive(true);
            Time.timeScale = 0.0f;
            Win = true;
        }
    }

    void Awake() {
        main = this;
    }

    void Start()
    {
        var offset = worldWidth + visionRange * 2;
        for (var i = -1; i <= 1; i++) {
            for (var j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) continue;
                var obj = Instantiate(world);
                obj.transform.position = world.transform.position + new Vector3(i * offset, 0.0f, j * offset);
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();

        boundDistance = worldWidth / 2.0f + visionRange;
        MainMenu();
    }


    public void HideYouWin() {
        youWin.SetActive(false);
        Time.timeScale = 1.0f;
    }

    [SerializeField]
    private AudioSource music;

    // Update is called once per frame
    void Update()
    {
        invertControls = invert.isOn;

        music.enabled = !mute.isOn;

        current.text = (campCount - camps.Count) + "";
        total.text = "/ " + campCount;

        var pos = playerRb.position;
        if (pos.x > boundDistance) {
            var offset = new Vector3(-2 * boundDistance, 0f, 0f);
            pos += offset;
            player.transform.position = pos;
            playerRb.position = pos;
            MoveRelatedStuff(offset);
        }
        if (pos.x < -boundDistance) {
            var offset = new Vector3(2 * boundDistance, 0f, 0f);
            pos += offset;
            player.transform.position = pos;
            playerRb.position = pos;
            MoveRelatedStuff(offset);
        }
        if (pos.z > boundDistance) {
            var offset = new Vector3(0f, 0f, -2 * boundDistance);
            pos += offset;
            player.transform.position = pos;
            playerRb.position = pos;
            MoveRelatedStuff(offset);
        }
        if (pos.z < -boundDistance) {
            var offset = new Vector3(0f, 0f, 2 * boundDistance);
            pos += offset;
            player.transform.position = pos;
            playerRb.position = pos;
            MoveRelatedStuff(offset);
        }

        if (Time.timeScale > 0.5f) {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
                MainMenu();
            }
        }
    }

    private void MoveRelatedStuff(Vector3 offset) {
        foreach(var item in stuffToMoveWithPlayer) {
            var pos = item.transform.position;
            pos += offset;
            item.transform.position = pos;
        }
        int numVcams = Cinemachine.CinemachineCore.Instance.VirtualCameraCount;
        for (int i = 0; i < numVcams; ++i) {
            foreach(var target in cinemachineTargeObjects) {
                Cinemachine.CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(target, offset);
            }
        }
            
    }

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    [SerializeField]
    private TMPro.TextMeshProUGUI current;
    [SerializeField]
    private TMPro.TextMeshProUGUI total;
    public void MainMenu() {
        Time.timeScale = 0.0f;
        menu.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1.0f;
        menu.SetActive(false);
        text.text = "Resume";
    }
}
