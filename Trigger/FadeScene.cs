using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScene : MonoBehaviour {
    [SerializeField] private string faderName;
    private GameObject faderPre;
    private GameObject faderClone;
    private UISprite faderSpirte;
    [SerializeField] private bool fading = false;
    private bool fadingOut=false;
    private float alpha = 0f;
    private float fadeTime=0.1f;

    // Use this for initialization
    void Start () {
        faderPre = Resources.Load("Prefabs/" + faderName) as GameObject;
        if (faderPre == null)
        {
            Debug.Log("过渡图片未加载");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (fading)
        {
            if (fadingOut)
            {
                alpha = Mathf.Lerp(alpha, 255, fadeTime);
                if (255 - alpha < 1f)
                {
                    fadingOut = false;
                }
                Debug.Log("FadingOut alpha= " + alpha);
            }
            else
            {
                alpha = Mathf.Lerp(alpha, 0, fadeTime * Time.deltaTime);
                if (alpha < 1f)
                {
                    fading= false;
                    Destroy(faderClone);
                }
                Debug.Log("FadingIn alpha= " + alpha);

            }

            faderSpirte.color = new Color(255, 255, 255, alpha);
            Debug.Log("faderSpirte.color= " + faderSpirte.color);


        } 

    }
    public void Fade(float time)
    {
        fading = true;
        fadeTime = time;
        faderClone = GameObject.Instantiate(faderPre);
        faderClone.transform.parent = GameObject.FindGameObjectWithTag("GameController").transform;
        fadingOut = true;
        alpha = 0;

        UISprite[] faderSpirtes = faderClone.GetComponentsInChildren<UISprite>();
        for(int i=0;i<faderSpirtes.Length; i++)
        {
            if (faderSpirtes[i].gameObject.name == "faderSprite")
            {
                faderSpirte = faderSpirtes[i];
            }
            
        }

    }
}
