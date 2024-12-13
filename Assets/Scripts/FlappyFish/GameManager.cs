using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int winningScore = 25;
    public static GameManager instance;
    public GameObject openingCanvas;
    public GameObject losingCanvas;
    public TMP_Text losingScoreText;
    public GameObject winningCanvas;
    public TMP_Text winningScoreText;

    public TMP_Text[] openingMessageLines;
    public TMP_Text losingMessage;
    public TMP_Text winningMessage;

    private FishTemplateProvider fishTemplateProvider;

    public SpriteRenderer lineSpriteRenderer;
    public SpriteRenderer colorSpriteRenderer;

    public Image[] lineImages;
    public Image[] colorImages;

    private int deadFishId;

    private FishAPI fishApi;
    private FlappyFishAudioPlayer audioPlayer;
    private AudioSource bgm;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        audioPlayer = GameObject.Find("FlappyFishAudioPlayer").GetComponent<FlappyFishAudioPlayer>();
        fishTemplateProvider = GameObject.Find("FishTemplateProvider").GetComponent<FishTemplateProvider>();
        fishApi = GameObject.Find("FishApi").GetComponent<FishAPI>();
        bgm = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

        SetDeadFish(RestartGameState.Instance.deadFishPlaying);

        if (RestartGameState.Instance.isFirstTime)
        {
            Time.timeScale = 0f;
        }
        else
        {
            StartGame();
        }
    }

    public void SetDeadFish(DeadFishGetObject deadFishGetObject)
    {
        lineSpriteRenderer.sprite = fishTemplateProvider.GetSpritePair(deadFishGetObject.template).outlineSprite;
        colorSpriteRenderer.sprite = Utils.GetSpriteFromEncodedString(deadFishGetObject.sprite);
        colorSpriteRenderer.color = Color.white;

        foreach (var lineImage in lineImages)
        {
            lineImage.sprite = fishTemplateProvider.GetSpritePair(deadFishGetObject.template).outlineSprite;
        }

        foreach (var colorImage in colorImages)
        {
            colorImage.sprite = Utils.GetSpriteFromEncodedString(deadFishGetObject.sprite);
        }

        deadFishId = deadFishGetObject.id;
        SetOpeningMessage(deadFishGetObject);
        winningMessage.text = winningMessage.text.Replace("FISHNAME", deadFishGetObject.name);
        losingMessage.text = losingMessage.text
            .Replace("FISHNAME", deadFishGetObject.name)
            .Replace("[X]", winningScore.ToString());
    }

    private void SetOpeningMessage(DeadFishGetObject deadFishGetObject)
    {
        string whatHeNeeds = "FOOD";

        if (deadFishGetObject.causeOfDeath.ToLower() == "loneliness") whatHeNeeds = "LOVE";

        foreach (var openingMessageLine in openingMessageLines)
        {
            openingMessageLine.text = openingMessageLine.text
                .Replace("FISHNAME", deadFishGetObject.name)
                .Replace("xx.xx.xx", Utils.GetDateMiniString(deadFishGetObject.dateOfBirth))
                .Replace("yy.yy.yy", Utils.GetDateMiniString(deadFishGetObject.dateOfDeath))
                .Replace("[X]", deadFishGetObject.daysLived.ToString())
                .Replace("CAUSEOFDEATH", deadFishGetObject.causeOfDeath)
                .Replace("FOOD", whatHeNeeds); 
        }
    }

    public void StartGame()
    {
        bgm.Play();
        openingCanvas.SetActive(false);
        Time.timeScale = 1f;
        RestartGameState.Instance.isFirstTime = false;
    }

    public async Task GameOver()
    {
        Time.timeScale = 0f;
        int gameScore = Score.instance.score;
        if (gameScore >= winningScore)
        {
            bgm.volume = 0;
            audioPlayer.PlayReviveClip();
            winningCanvas.SetActive(true);
            winningScoreText.text += gameScore;
            FishGetObject revivedFish = await fishApi.ReviveDeadFish(deadFishId);
            FishStore.Instance.StoreFish(revivedFish);
            FishStore.Instance.RemoveDeadFish(deadFishId);
        }
        else
        {
            audioPlayer.PlayDeathClip();
            losingCanvas.SetActive(true);
            losingScoreText.text += gameScore;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBackToGraveyard()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("S10-Graveyard");
        RestartGameState.Instance.isFirstTime = true;
    }

    public void GoBackToAquarium()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("S11-Audio");
        RestartGameState.Instance.isFirstTime = true;
    }
}