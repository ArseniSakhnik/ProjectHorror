using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRiddleNum : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] text;
    public string solution;
    public string currentString;
    public GameObject parentObject;
    // Start is called before the first frame update
    
    public void SetParentData(GameObject parent)
    {
        parentObject = parent;
        solution = parentObject.GetComponent<RiddleNumLock>().solution;
        foreach (var item in text)
        {
            item.text = "0";
        }
    } 
    

    public void CheckSolution()
    {
        currentString = "";

        foreach (var item in text)
        {
            currentString += item.text;
        }

        if (currentString == solution)
        {
            GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "Opened";
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1000);
            parentObject.SetActive(false);
            StartCoroutine(ExecuteAfterTime(3));
        }
    }

    public IEnumerator ExecuteAfterTime(float timeInSec)
    {
        PlayerController.isblockInteraction = false;
        PlayerController.isblockReading = false;
        PlayerController.isblockInventory = false;
        PlayerController.isblockShooting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        yield return new WaitForSeconds(timeInSec);
        if (GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text == "Opened")
        {
            GameObject.Find("SubtitlesInfo").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1000);
        gameObject.SetActive(false);
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
        CheckSolution();
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
        CheckSolution();
    }

}
