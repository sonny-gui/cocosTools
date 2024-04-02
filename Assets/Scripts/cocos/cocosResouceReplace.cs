using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class cocosResouceReplace : MonoBehaviour
{
    public InputField prefabOrScenePath;
    public InputField newResourceFilePath;
    public InputField oldResourceFilePath;
    public Text prefabOrScenePath_errorTip;
    public Text newResourceFilePath_errorTip;
    public Text oldResourceFilePath_errorTip;
    public Text successTip;
    public Button sureBtn;

    // Start is called before the first frame update
    void Start()
    {
        prefabOrScenePath_errorTip.gameObject.SetActive(false);
        newResourceFilePath_errorTip.gameObject.SetActive(false);
        oldResourceFilePath_errorTip.gameObject.SetActive(false);
        successTip.gameObject.SetActive(false);
        sureBtn.onClick.AddListener(this.sureClick);

        if (PlayerPrefs.HasKey("spath_1")) prefabOrScenePath.text = PlayerPrefs.GetString("spath_1");
        if (PlayerPrefs.HasKey("spath_2")) newResourceFilePath.text = PlayerPrefs.GetString("spath_2");
        if (PlayerPrefs.HasKey("spath_3")) oldResourceFilePath.text = PlayerPrefs.GetString("spath_3");
    }

    // Update is called once per frame
    void sureClick()
    {
        StopCoroutine(ShowSuccessTip());
        prefabOrScenePath_errorTip.gameObject.SetActive(false);
        newResourceFilePath_errorTip.gameObject.SetActive(false);
        oldResourceFilePath_errorTip.gameObject.SetActive(false);
        successTip.gameObject.SetActive(false);


        Dictionary<string, string> newRes = new Dictionary<string, string>();
        try
        {
            newRes = this.GetResourceData(newResourceFilePath.text);
            if (newRes.Count == 0)
            {
                Debug.LogError("resourceFilePath 中没找到目标资源");
                newResourceFilePath_errorTip.gameObject.SetActive(true);
                newResourceFilePath_errorTip.text = "resourceFilePath 中没找到目标资源";
                return;
            }
        }
        catch (System.Exception ex)
        {
            newResourceFilePath_errorTip.gameObject.SetActive(true);
            newResourceFilePath_errorTip.text = ex.Message;
            return;
        }


        Dictionary<string, string> oldRes = new Dictionary<string, string>();
        try
        {
            oldRes = this.GetResourceData(oldResourceFilePath.text);
            if (oldRes.Count == 0)
            {
                Debug.LogError("resourceFilePath 中没找到目标资源");
                oldResourceFilePath_errorTip.gameObject.SetActive(true);
                oldResourceFilePath_errorTip.text = "resourceFilePath 中没找到目标资源";
                return;
            }
        }
        catch (System.Exception ex)
        {
            oldResourceFilePath_errorTip.gameObject.SetActive(true);
            oldResourceFilePath_errorTip.text = ex.Message;
            return;
        }

        Dictionary<string,string> prefabOrSceneMap = new Dictionary<string, string>();
        try
        {
            prefabOrSceneMap = this.GetSceneAndPrabInfo(prefabOrScenePath.text);
            if (prefabOrSceneMap.Count == 0)
            {
                Debug.LogError("resourceFilePath 中没找到目标资源");
                prefabOrScenePath_errorTip.gameObject.SetActive(true);
                prefabOrScenePath_errorTip.text = "resourceFilePath 中没找到目标资源";
                return;
            }
        }
        catch (System.Exception ex)
        {
            prefabOrScenePath_errorTip.gameObject.SetActive(true);
            prefabOrScenePath_errorTip.text = ex.Message;
            return;
        }

        foreach (var item in oldRes)
        {
            foreach (var item_1 in prefabOrSceneMap)
            {
                string prefabOrSceneJson = item_1.Value;
                if (prefabOrSceneJson.Contains(item.Value) && newRes.ContainsKey(item.Key))
                {
                    prefabOrSceneJson = prefabOrSceneJson.Replace(item.Value, newRes[item.Key]);
                    File.WriteAllText(item_1.Key, prefabOrSceneJson);
                }
            }
        }

        PlayerPrefs.SetString("spath_1", prefabOrScenePath.text);
        PlayerPrefs.SetString("spath_2", newResourceFilePath.text);
        PlayerPrefs.SetString("spath_3", oldResourceFilePath.text);
        StartCoroutine(ShowSuccessTip());
    }

    public Dictionary<string ,string> GetResourceData(string resPath)
    {
        var _UUID = "";
        Dictionary<string, string> uuidInfo = new Dictionary<string, string>();
        List<string> allPath = new List<string>(Directory.GetFiles(resPath, "*.meta", SearchOption.AllDirectories));

        foreach (var path in allPath)
        {
            string metaJsonStr = File.ReadAllText(path);
            JObject targetJson = JObject.Parse(metaJsonStr);

            var fileName = Path.GetFileNameWithoutExtension(path);
            fileName = Path.GetFileNameWithoutExtension(fileName);
            string importerStr = targetJson["importer"].ToString();
            if (importerStr == "texture" || importerStr == "audio-clip" || importerStr == "bitmap-font" ||
                importerStr == "javascript" || importerStr == "spine" || importerStr == "animation-clip"
                )
            {
                if (importerStr == "texture") _UUID = targetJson["subMetas"][fileName]["uuid"].ToString();
                else if (importerStr == "javascript") _UUID = cocosCompressUUID.CompressHex(targetJson["uuid"].ToString());
                else _UUID = targetJson["uuid"].ToString();

                if (!uuidInfo.ContainsKey(fileName)) uuidInfo.Add(fileName, _UUID);
            }
        }
        return uuidInfo;
    }

    public Dictionary<string, string> GetSceneAndPrabInfo(string resPath)
    {
        List<string> prefabOrScene = new List<string>(Directory.GetFiles(prefabOrScenePath.text, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".prefab") || s.EndsWith(".fire"))); ;
        Dictionary<string,string> pathAndJson = new Dictionary<string, string>();
        foreach (var path in prefabOrScene)
        {
            string contentStr = File.ReadAllText(path);
            pathAndJson.Add(path,contentStr);
        }
        return pathAndJson;
    }

    public IEnumerator ShowSuccessTip()
    {
        successTip.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        successTip.gameObject.SetActive(false);
    }
}
