using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImportHelper : MonoBehaviour
{
    GameObject[] _cardPanelOnHand;
    GameObject[] _cardPanel;
    GameObject _canvas;
    Image[] _image;
    float _step;
    float _radiusCircle = 650f;
    int _numberCards = 10;
    int _cardsOnHand = 5;
    int _imagSpriteCounter;
    int _counterChangedCards;
    int _randomHpManaAttack;
    Vector2 _deckPosition;
    Vector2 _playerDeckPosition;
    // ...
    private void Awake()
    {
       
        _counterChangedCards = 0;
        _deckPosition = new Vector2(1000f, 850f);
        _playerDeckPosition = new Vector2(1000f, -300f);
        _image = new Image[_numberCards];
        _step = -0.3f;
        _canvas = GameObject.Find("Canvas") as GameObject;        
        _cardPanel = new GameObject[_numberCards];
        _cardPanelOnHand = new GameObject[_numberCards];
        for (int i = 0; i < _numberCards; i++)
        {
            if (i < _cardsOnHand)
            {
                Vector2 pos = Circle(_playerDeckPosition, _radiusCircle);
                Quaternion rot = Quaternion.FromToRotation(Vector2.down, _playerDeckPosition - pos);
                _cardPanelOnHand[i] = Instantiate(Resources.Load("Prefabs/CardPanel"), pos, rot,_canvas.transform) as GameObject;
                _cardPanelOnHand[i].name = "card" + i;
                _image[i] = _cardPanelOnHand[i].GetComponent<Image>();
               

            }
            else
            {
                _cardPanel[i] = Instantiate(Resources.Load("Prefabs/CardPanel"), _deckPosition, Quaternion.identity, _canvas.transform) as GameObject;
                _cardPanel[i].name = "card" + i;
                _image[i] = _cardPanel[i].GetComponent<Image>();
              
            }
        }

    }
    public void Start()
    {

       for (_imagSpriteCounter = 0; _imagSpriteCounter < _numberCards; _imagSpriteCounter++)
       {
          DownloadImage("https://picsum.photos/200/300", _imagSpriteCounter);
       }
        
    }

    public void DownloadImage(string url, int i)
    {
        StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                if (texture == null)
                    Debug.Log("Null");     
               _image[i].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            }
        }));
    }

    IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }
    Vector3 Circle(Vector2 center, float radius)
    {
        
        _step = _step + 0.1f;
        float ang = _step * 180;
        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);        
        return pos;
    }
    public void ChangeCards()
    {
        if (_counterChangedCards < _cardsOnHand)
        {
            Text[] textboxes = new Text[3];            
            Debug.Log(textboxes.Length);
            Debug.Log(_cardPanelOnHand[_counterChangedCards]);
            textboxes = _cardPanelOnHand[_counterChangedCards].GetComponentsInChildren<Text>();
            Debug.Log(textboxes.Length);
            int randomValue = UnityEngine.Random.Range(0, 3);
            Debug.Log(randomValue);
            _randomHpManaAttack = UnityEngine.Random.Range(1, 10);
            _cardPanelOnHand[_counterChangedCards].transform.SetSiblingIndex(10);
            
            
            Animator animator = textboxes[randomValue].GetComponent<Animator>();           
            animator.SetBool("ParameterChanged", true);
            StartCoroutine(ChangeParametr(textboxes[randomValue], animator));
            
            _counterChangedCards++;

        }
        else
        {
            _counterChangedCards = 0;
            Text[] textboxes = new Text[3];
            Debug.Log(textboxes.Length);
            Debug.Log(_cardPanelOnHand[_counterChangedCards]);
            textboxes = _cardPanelOnHand[_counterChangedCards].GetComponentsInChildren<Text>();
            Debug.Log(textboxes.Length);
            int randomValue = UnityEngine.Random.Range(0, 3);
            Debug.Log(randomValue);
            _randomHpManaAttack = UnityEngine.Random.Range(1, 10);
            _cardPanelOnHand[_counterChangedCards].transform.SetSiblingIndex(10);
            
            
            Animator animator = textboxes[randomValue].GetComponent<Animator>();
            animator.SetBool("ParameterChanged", true);
            StartCoroutine(ChangeParametr(textboxes[randomValue], animator));            
            _counterChangedCards++;
        }
    }
    IEnumerator ChangeParametr(Text text, Animator animator)
    {
        text.text = _randomHpManaAttack.ToString();
        
        yield return new WaitForSeconds(1f);
        
        animator.SetBool("ParameterChanged", false);
       
      
    }
}