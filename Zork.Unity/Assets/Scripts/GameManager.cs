using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zork.Common;
using Newtonsoft.Json;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        game.Player.LocationChanged += Player_LocationChanged;
        game.Player.MovesChanged += Player_MoveChanged;
        game.Player.ScoreChanged += Player_ScoreChanged;
        game.Run(InputService, OutputService);
        LocationText.text = game.Player.CurrentRoom.Name;
    }

    private void Player_LocationChanged(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    private void Player_MoveChanged(object sender, int moves)
    {
        MovesText.text = $"Moves: {moves}";
    }

    private void Player_ScoreChanged(object sender, int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    private void Start()
    {
        InputService.SetFocus();
        LocationText.text = game.Player.CurrentRoom.Name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if (game.isRunning == false)
        {
            UnityEditor.EditorApplication.isPlaying = false;

            Application.Quit();
        }
    }

    private Game game;
}
