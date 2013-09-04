using UnityEngine;
using System.Collections.Generic;

public class gamemanager : MonoBehaviour {

    public AudioClip snd_wrong = null;
    public AudioClip snd_right = null;


    public int numberOfCards = 3;
    public GameObject[] availableCards = new GameObject[1];
    public Vector3 offset = Vector3.zero;
    private GameObject rightAnswer = null;
    public GameObject soundCard = null;

    public float startTime = 0;
    private float startTimer = 0;
    private bool gameActive = false;


    public float winTime = 0;
    private float winTimer = 0f;
    private bool victory = false;

    public float punishTime = 1;
    private float punishTimer = 0f;
    private bool punished = false;


    private bool inputEnabled = false;

    private List<GameObject> cards = new List<GameObject>();
    private List<GameObject> cardPool = new List<GameObject>();
    private List<Vector3> cardSpawns = new List<Vector3>();

    
	// Use this for initialization
	void Start () {

        Initialize();	
	}
	
	// Update is called once per frame
	public void Update () {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();            
        }

        startTimer += Time.deltaTime;
        if (startTime <= startTimer && !gameActive)
            ActivateGame();


        if (victory)
        {
            winTimer += Time.deltaTime;
               
            //for (int i = 0; i < cards.Count; i++)
            //{
            //    cards[i].transform.position = Vector3.Lerp(cards[i].transform.position, cardSpawns[i], .9f);
            //    //Debug.Log(cardSpawns[i]);
            //}

            if (winTime <= winTimer)
                Restart();
        }

        if (punished) 
        {
            punishTimer += Time.deltaTime;
            if (punishTime <= punishTimer){
                punishTimer = 0f;
                inputEnabled = true;
                punished = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && inputEnabled && !rightAnswer.GetComponentInChildren<Animation>().IsPlaying("turn"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {


                if (hit.collider.gameObject == rightAnswer)
                {
                    
                    Win();
                }
                else if (hit.collider.gameObject == soundCard && !soundCard.animation.IsPlaying("soundscale"))
                {
                    PlayAnswer(); 
                                   
                }
                else if (hit.collider.tag == "card")
                {
                    if (hit.collider.gameObject.GetComponentInChildren<Animation>().IsPlaying("idle")) 
                    {
                        hit.collider.gameObject.GetComponentInChildren<Animation>().CrossFade("turnBack");
                        hit.collider.gameObject.GetComponentInChildren<Animation>().CrossFadeQueued("startPoint", 0.5f);

                        Punish(); 
                    }
                    
                                       

                }


            }
        }
	}

    void Initialize() {
        for (int i = 0; i < availableCards.Length; i++)
        {
            cardPool.Add(availableCards[i].gameObject);
        }


        for (int i = 0; i < numberOfCards; i++)
        {
            int randomIndex = Random.Range(0, cardPool.Count);
            GameObject card = (GameObject)Instantiate(cardPool[randomIndex], transform.position + offset * i, transform.rotation);
            Vector3 startPos = transform.position + offset*i;
            cardSpawns.Add(startPos);
   
            cards.Add(card);
            cardPool.RemoveAt(randomIndex);
        }

        rightAnswer = cards[Random.Range(1, cards.Count)].gameObject;

        PlayAnswer();
        gameActive = false;
        startTimer = 0;

        
    
    }
    void ActivateGame()
    {
        
        inputEnabled = true;
        foreach (GameObject card in cards)
        {
            card.GetComponentInChildren<Animation>().Play();
        }
      

        gameActive = true;
    }

    void Restart() 
    {
        for(int i = cards.Count -1; i >= 0; i--)
        {
            Destroy(cards[i].gameObject);
            cardPool.Clear();
            cards.RemoveAt(i);
            cardSpawns.RemoveAt(i);
        }
        victory = false;
        winTimer = 0f;
        Initialize();
    }
    void Win() 
    {
        Debug.Log("Congratulations, that's right!");
        inputEnabled = false;
        audio.clip = snd_right;
        audio.Play();
        foreach (GameObject card in cards)
        {
            if (card.GetComponentInChildren<Animation>().IsPlaying("idle"))
            {
                card.GetComponentInChildren<Animation>().CrossFade("turnBack");
                card.GetComponentInChildren<Animation>().CrossFadeQueued("startPoint", 0.5f);
            }

        }
        rightAnswer.GetComponentInChildren<Animation>().CrossFade("answerscale");
        rightAnswer.GetComponentInChildren<Animation>().CrossFadeQueued("turnBack");
        rightAnswer.GetComponentInChildren<Animation>().CrossFadeQueued("startPoint");
            victory = true;
    }
    void Punish()
    {
        audio.clip = snd_wrong;
        audio.Play();
        inputEnabled = false;
        punished = true;
    }
    void PlayAnswer() 
    {
        soundCard.audio.clip = rightAnswer.audio.clip;
        soundCard.audio.Play();
        soundCard.animation.Play();
    }
}
