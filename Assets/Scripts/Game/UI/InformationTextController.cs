using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InformationTextController : MonoBehaviour
{
    Queue<string> informationTexts;

    private float timer;

    [SerializeField] Text[] texts;
    // Start is called before the first frame update
    void Start()
    {
        informationTexts = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 5)
        {
            AddInformationText("");
        }

        int j = 0;
        for(int i = informationTexts.Count - 1; i >= 0; --i)
        {
            texts[j++].text = informationTexts.ElementAt(i);
        }
    }

    public void AddInformationText(string text)
    {
        timer = 0;
        if (informationTexts.Count == 5)
            informationTexts.Dequeue();
        informationTexts.Enqueue(text);
    }
}
