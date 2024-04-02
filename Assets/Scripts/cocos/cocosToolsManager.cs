using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class cocosToolsManager : MonoBehaviour
{
    public GameObject entryMianPanel;
    public cocosPrefabChange changeToolPanel;
    public cocosResouceMatch matchToolPanel;
    public cocosResouceReplace replaceToolPanel;

    public Button backBtn;
    public Button entryChangeTool;
    public Button entryMatchTool;
    public Button entryReplaceTool;
    void Start()
    {
        changeToolPanel.gameObject.SetActive(false);
        matchToolPanel.gameObject.SetActive(false);
        replaceToolPanel.gameObject.SetActive(false);

        entryMianPanel.gameObject.SetActive(true);
        backBtn.gameObject.SetActive(false);
        backBtn.onClick.AddListener(this.ClickBackBtn);
        entryChangeTool.onClick.AddListener(this.ClickEntryChangeTool);
        entryMatchTool.onClick.AddListener(this.ClickEntryMatchTool);
        entryReplaceTool.onClick.AddListener(this.ClickEntryReplaceTool);
    }

    public void ClickBackBtn()
    {
        changeToolPanel.gameObject.SetActive(false);
        matchToolPanel.gameObject.SetActive(false);
        replaceToolPanel.gameObject.SetActive(false);

        backBtn.gameObject.SetActive(false);
        entryMianPanel.gameObject.SetActive(true);
    }

    public void ClickEntryChangeTool()
    {
        changeToolPanel.gameObject.SetActive(true);
        matchToolPanel.gameObject.SetActive(false);
        replaceToolPanel.gameObject.SetActive(false);

        entryMianPanel.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);
    }

    public void ClickEntryMatchTool()
    {
        changeToolPanel.gameObject.SetActive(false);
        matchToolPanel.gameObject.SetActive(true);
        replaceToolPanel.gameObject.SetActive(false);

        entryMianPanel.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);
    }

    public void ClickEntryReplaceTool()
    {
        changeToolPanel.gameObject.SetActive(false);
        matchToolPanel.gameObject.SetActive(false);
        replaceToolPanel.gameObject.SetActive(true);

        entryMianPanel.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);
    }
}
