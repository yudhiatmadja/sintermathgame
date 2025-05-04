using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels;
    private Dictionary<string, GameObject> panelDictionary;
    private HashSet<string> answeredPanels = new HashSet<string>();

    private void Start()
    {
        panelDictionary = new Dictionary<string, GameObject>();

        foreach (GameObject panel in panels)
        {
            if (panel != null)
            {
                panelDictionary.Add(panel.name, panel);
                panel.SetActive(false);
            }
        }
    }

    public void ShowPanel(string panelName)
    {
        // foreach (var panel in panelDictionary.Values)
        // {
        //     panel.SetActive(false);
        // }

        if (answeredPanels.Contains(panelName))
        {
            Debug.Log(panelName + " sudah dijawab benar, tidak bisa dibuka lagi!");
            return;
        }


        if (panelDictionary.ContainsKey(panelName))
        {
            panelDictionary[panelName].SetActive(true);
            Debug.Log(panelName + " muncul");
        }
        else
        {
            Debug.LogWarning("Panel dengan nama " + panelName + " tidak ditemukan!");
        }
    }
    public void CloseAllPanels()
    {
        foreach (var panel in panelDictionary.Values)
        {
            panel.SetActive(false);
        }
    }

    public void MarkPanelAsAnswered(string panelName)
    {
        if (!answeredPanels.Contains(panelName))
        {
            answeredPanels.Add(panelName);
            Debug.Log(panelName + " ditandai sudah dijawab benar.");
        }
    }

}
