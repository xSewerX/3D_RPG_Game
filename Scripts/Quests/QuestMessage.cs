using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestMessage : MonoBehaviour
{
    public static QuestMessage instance;
    public GameObject messagePrefab;

    private void Awake()
    {
        instance = this;
    }

    public void WriteMessage(string message)
    {
       GameObject go = Instantiate(messagePrefab, transform);
        go.GetComponent<TextMeshProUGUI>().text = message;

        go.transform.SetAsFirstSibling();

        Destroy(go, 1.5f);
    }
}
