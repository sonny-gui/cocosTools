using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class cocosPrefabChange : MonoBehaviour
{
    //public string prefabPath = "C:\cocos\workSpace\hello-world\assets\New Node.prefab";
    //public string imageMetaPath = "C:\cocos\workSpace\hello-world\assets";

    public InputField prefabOrScenePath;
    public InputField imageFilePath;
    public Text prefabOrScenePath_errorTip;
    public Text imageFilePath_errorTip;
    public Text successTip;
    public Button sureBtn;

    void Start()
    {
        prefabOrScenePath_errorTip.gameObject.SetActive(false);
        imageFilePath_errorTip.gameObject.SetActive(false);
        successTip.gameObject.SetActive(false);
        sureBtn.onClick.AddListener(this.sureClick);
    }

    public void sureClick()
    {
        StopCoroutine(successTipFade());
        prefabOrScenePath_errorTip.gameObject.SetActive(false);
        imageFilePath_errorTip.gameObject.SetActive(false);
        successTip.gameObject.SetActive(false);

        string prefabPath = prefabOrScenePath.text;
        if (string.IsNullOrEmpty(prefabPath))
        {
            prefabOrScenePath_errorTip.gameObject.SetActive(true);
            prefabOrScenePath_errorTip.text = "请填写正确的路径";
            return;
        }

        string imageMetaPath = imageFilePath.text;
        if (string.IsNullOrEmpty(imageMetaPath))
        {
            imageFilePath_errorTip.gameObject.SetActive(true);
            imageFilePath_errorTip.text = "请填写正确的路径";
            return;
        }

        Dictionary<string, string> imgUuids = new Dictionary<string, string>();
        List<string> allPath = new List<string>(Directory.GetFiles(imageMetaPath));
        var vaildAllPath = from path in allPath
                           where path.EndsWith(".png.meta") //cocos 支持图片类型
                           || path.EndsWith(".jpg.meta")
                           || path.EndsWith(".webp.meta")
                           || path.EndsWith(".pvr.meta")
                           || path.EndsWith(".etc1.meta")
                           || path.EndsWith(".etc2.meta")
                           || path.EndsWith(".astc.meta")
                           select path;
        foreach (string path in vaildAllPath)
        {
            string metaJsonStr = File.ReadAllText(path);
            JObject targetJson = JObject.Parse(metaJsonStr);
            var value = Path.GetFileNameWithoutExtension(path);
            value = Path.GetFileNameWithoutExtension(value);
            var key = targetJson["subMetas"][value]["uuid"].ToString();
            imgUuids.Add(key, value);
        }

        string jsonStr = File.ReadAllText(prefabPath);
        JArray result = JArray.Parse(jsonStr);
        foreach (var item in result)
        {
            if (item["__type__"] != null && (string)item["__type__"] == "cc.Sprite")
            {
                var uuid = item["_spriteFrame"]["__uuid__"].ToString();
                if (uuid != null && imgUuids.ContainsKey(uuid))
                {
                    int jsonArrIndex = (int)item["node"]["__id__"];
                    if (result[jsonArrIndex]["__type__"] != null && (string)result[jsonArrIndex]["__type__"] == "cc.Node")
                    {
                        result[jsonArrIndex]["_name"] = imgUuids[uuid];
                    }
                }
            }
        }
        string newJStr = result.ToString();
        File.WriteAllText(prefabPath, newJStr);
        successTip.gameObject.SetActive(true);
        Debug.Log("更改成功");
        StartCoroutine(successTipFade());
    }

    public IEnumerator successTipFade()
    {
        yield return new WaitForSeconds(5); 
        successTip.gameObject.SetActive(false);
    }
}
