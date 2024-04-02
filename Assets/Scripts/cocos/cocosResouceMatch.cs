using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 预制件/场景中的资源  与文件夹中的资源 对比。一般是用于依赖查看
/// </summary>
public class cocosResouceMatch : MonoBehaviour
{
    public InputField prefabOrScenePath;
    public InputField resourceFilePath;
    public InputField exportFilePath;
    public Text prefabOrScenePath_errorTip;
    public Text resourceFilePath_errorTip;
    public Text exportFilePath_errorTip;
    public Text successTip;
    public Button sureBtn;
    // Start is called before the first frame update
    void Start()
    {
        prefabOrScenePath_errorTip.gameObject.SetActive(false);
        resourceFilePath_errorTip.gameObject.SetActive(false);
        exportFilePath_errorTip.gameObject.SetActive(false);
        successTip.gameObject.SetActive(false);
        sureBtn.onClick.AddListener(this.sureClick);

        if (PlayerPrefs.HasKey("path_1")) prefabOrScenePath.text = PlayerPrefs.GetString("path_1");
        if (PlayerPrefs.HasKey("path_2")) resourceFilePath.text = PlayerPrefs.GetString("path_2");
        if (PlayerPrefs.HasKey("path_3")) exportFilePath.text = PlayerPrefs.GetString("path_3");
    }

    public void sureClick()
    {
        StopCoroutine(ShowSuccessTip());
        prefabOrScenePath_errorTip.gameObject.SetActive(false);
        resourceFilePath_errorTip.gameObject.SetActive(false);
        exportFilePath_errorTip.gameObject.SetActive(false);
        successTip.gameObject.SetActive(false);

        string prefabPath = prefabOrScenePath.text;
        if (string.IsNullOrEmpty(prefabPath))
        {
            prefabOrScenePath_errorTip.gameObject.SetActive(true);
            prefabOrScenePath_errorTip.text = "请填写正确的路径";
            return;
        }

        string resoucePath = resourceFilePath.text;
        if (string.IsNullOrEmpty(resoucePath))
        {
            resourceFilePath_errorTip.gameObject.SetActive(true);
            resourceFilePath_errorTip.text = "请填写正确的路径";
            return;
        }

        string exportPath = resourceFilePath.text;
        if (string.IsNullOrEmpty(exportPath))
        {
            exportFilePath_errorTip.gameObject.SetActive(true);
            exportFilePath_errorTip.text = "请填写正确的路径";
            return;
        }
        var _UUID = "";
        Dictionary<string, string> uuidInfo = new Dictionary<string, string>();
        List<string> allPath = null;
        try
        {
            allPath = new List<string>(Directory.GetFiles(resourceFilePath.text, "*.meta", SearchOption.AllDirectories));
        }
        catch (System.Exception ex)
        {
            resourceFilePath_errorTip.gameObject.SetActive(true);
            resourceFilePath_errorTip.text = ex.Message;
            return;
        }

        foreach (var path in allPath)
        {
            string metaJsonStr = File.ReadAllText(path);
            JObject targetJson = JObject.Parse(metaJsonStr);

            var fileName = Path.GetFileNameWithoutExtension(path);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            string importerStr = targetJson["importer"].ToString();
            if (importerStr == "texture" || importerStr == "audio-clip" || importerStr == "bitmap-font" || 
                importerStr == "javascript"|| importerStr == "spine" || importerStr == "animation-clip"
                )
            {
                if (importerStr == "texture")
                    _UUID = targetJson["subMetas"][fileName]["uuid"].ToString();
                else if (importerStr == "javascript")
                    _UUID = cocosCompressUUID.CompressHex(targetJson["uuid"].ToString());
                else
                    _UUID = targetJson["uuid"].ToString();
                if (!uuidInfo.ContainsKey(fileName))
                    uuidInfo.Add(fileName, _UUID);
            }
        }
        if (uuidInfo.Count == 0)
        {
            Debug.LogError("resourceFilePath 中没找到目标资源");
            resourceFilePath_errorTip.gameObject.SetActive(true);
            resourceFilePath_errorTip.text = "resourceFilePath 中没找到目标资源";
            return;
        }
        List<string> prefabOrScene = null;
        try
        {
            prefabOrScene = new List<string>(Directory.GetFiles(prefabOrScenePath.text, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".prefab") || s.EndsWith(".fire")));
        }
        catch (System.Exception ex)
        {
            prefabOrScenePath_errorTip.gameObject.SetActive(true);
            prefabOrScenePath_errorTip.text = ex.Message;
            return;
        }
        Dictionary<string,string> prefabOrSceneMap = new Dictionary<string,string>();

        foreach (var path in prefabOrScene)
        {
            string contentStr = File.ReadAllText(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            prefabOrSceneMap.Add(fileName, contentStr);
        }
        if (prefabOrSceneMap.Count == 0)
        {
            Debug.LogError("prefabOrScenePath 中没找到预制件或者场景");
            prefabOrScenePath_errorTip.gameObject.SetActive(true);
            prefabOrScenePath_errorTip.text = "prefabOrScenePath 中没找到预制件或者场景";
            return;
        }

        Dictionary<string,List<string>> exportUsedDic = new Dictionary<string,List<string>>();
        List<string> exportNotusedList = new List<string>();

        foreach (var item in prefabOrSceneMap)
        {
            string content = item.Value;
            foreach (var uInfo in uuidInfo)
            {
                string _k = uInfo.Key + "(" + uInfo.Value + ")";
                if (content.Contains(uInfo.Value))
                {
                    if (!exportUsedDic.ContainsKey(_k))
                        exportUsedDic.Add(_k, new List<string>());
                    if (exportUsedDic.ContainsKey(_k) && !exportUsedDic[_k].Contains(item.Key))
                        exportUsedDic[_k].Add(item.Key);
                }       
            }
        }
        foreach (var uInfo in uuidInfo)
        {
            string _k = uInfo.Key + "(" + uInfo.Value + ")";
            if (!exportNotusedList.Contains(_k) && !exportUsedDic.ContainsKey(_k))
                exportNotusedList.Add(_k);
        }

        string exportUsedStr = "";

        foreach (var item in exportUsedDic)
        {
            string _s = "预制件|场景：";
            foreach (var c in item.Value)
                _s += c.ToString()+"  ,";
            string _v = item.Key + "  =>  " + _s;
            Debug.Log(_v);
            exportUsedStr += _v + "\n";
        
        }
        string exportNotusedStr = "";
        foreach (var item in exportNotusedList)
        {
            Debug.LogError(item);
            exportNotusedStr += item + "\n";
        }
        try
        {
            File.WriteAllText(exportFilePath.text + "/场景或者预制件已经被使用的资源.txt", exportUsedStr);
            File.WriteAllText(exportFilePath.text + "/场景或者预制件没有依赖或者使用的资源.txt", exportNotusedStr);
        }
        catch (System.Exception ex)
        {
            exportFilePath_errorTip.gameObject.SetActive(true);
            exportFilePath_errorTip.text = ex.Message;
            return;
        }

        PlayerPrefs.SetString("path_1", prefabOrScenePath.text);
        PlayerPrefs.SetString("path_2", resourceFilePath.text);
        PlayerPrefs.SetString("path_3", exportFilePath.text);
        StartCoroutine(ShowSuccessTip());
    }

    public IEnumerator ShowSuccessTip()
    {
        successTip.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        successTip.gameObject.SetActive(false);
    }
}
