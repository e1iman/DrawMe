using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    #region UI
    public static GM master;
    [SerializeField]
    Text scoreUi;
    [SerializeField]
    Text accurcityUi;
    [SerializeField]
    Text timeUi;
    [SerializeField]
    Image img;
    [SerializeField]
    GameObject playButton;
    #endregion

    [SerializeField]
    GameObject particleSystem;// mouse follower particle

    #region timeRules
    [SerializeField]
    float startTimeLeft = 30;
    [SerializeField]
    float decrement = 2.25f;
    [SerializeField]
    float minTimeLeft = 2.25f;
    #endregion
    float timeLeft = 0;// if 0 - gameover

    int score = 0;

    [SerializeField]
    GameObject[] arr;// array of images to draw

	void Awake () {
        if (master == null)
        {
            master = this;
        }

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == null) throw new MissingComponentException("there is null element in arr: " + gameObject.name + " at index: " + i);
            arr[i].SetActive(false);
        }
        
        playButton.GetComponent<Button>().onClick.AddListener(() => NewGame());
	}

    void NewGame()
    {
        score = 0;
        playButton.GetComponentInChildren<Text>().text = "Retry";
        playButton.SetActive(false);
        particleSystem.SetActive(true);
        gameObject.GetComponent<DrawFrame>().gameObject.SetActive(true);
        timeLeft = startTimeLeft;

        StartCoroutine("Timer");
        img.color = Color.white;
        scoreUi.text = "";
    }

    void GameOver()
    {
        playButton.SetActive(true);
        particleSystem.SetActive(false);
        gameObject.GetComponent<DrawFrame>().gameObject.SetActive(false);

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].SetActive(false);
        }

        StopCoroutine("Timer");
        timeUi.text = "";
        accurcityUi.text = "";
        scoreUi.text = "Points: " + score;
        img.color = Color.clear;
    }

    IEnumerator Timer()
    {
        while (true)
        {
            timeLeft -= Time.deltaTime;
            timeUi.text = "Time: " + timeLeft.ToString("F1") + " s";
            if (timeLeft <= 0)
            {
                GameOver();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public GameObject GetRandomImage()
    {
        return GetRandomImage(null);
    }
    public GameObject GetRandomImage(GameObject _input)
    {
        GameObject _output;
        do
        {
            _output = arr[Random.Range(0, arr.Length)];
        }
        while (_output == _input);

        img.sprite = _output.GetComponent<SpriteRenderer>().sprite;
        timeLeft = startTimeLeft - decrement * score;
        timeLeft = Mathf.Clamp(timeLeft, minTimeLeft, startTimeLeft);
        return _output;
    }

    public void AddScore(int _score)
    {
        score += _score;
    }

    public void SetAccText(float _acc)
    {
        accurcityUi.text = "Acc: " + _acc.ToString("F1") + "%";
    }
}
