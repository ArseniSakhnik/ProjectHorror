using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRiddleNum : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseNum(GameObject textfield)
    {
        int curNum =  int.Parse(textfield.GetComponent<TMPro.TextMeshProUGUI>().text);
        if (curNum == 0)
        {
            curNum = 9;
        }
        else curNum--;
        textfield.GetComponent<TMPro.TextMeshProUGUI>().text = curNum.ToString();
    }

    public void IncreaseNum(GameObject textfield)
    {
        int curNum = int.Parse(textfield.GetComponent<TMPro.TextMeshProUGUI>().text);
        if (curNum == 9)
        {
            curNum = 0;
        }
        else curNum++;
        textfield.GetComponent<TMPro.TextMeshProUGUI>().text = curNum.ToString();
    }

}
