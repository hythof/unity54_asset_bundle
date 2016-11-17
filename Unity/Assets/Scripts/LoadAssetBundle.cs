using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;

public class LoadAssetBundle : MonoBehaviour
{
    [SerializeField]
    Transform parent;

    int version = 1;
    string blackUrl;
    string whiteUrl;

    void Awake()
    {
        Caching.CleanCache();
        var dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Application.streamingAssetsPath)));
        blackUrl = string.Format("file:///{0}/AssetBundles/black", dir);
        whiteUrl = string.Format("file:///{0}/AssetBundles/white", dir);
    }

    public void DownloadBlack()
    {
        StartCoroutine(download(blackUrl));
    }

    public void DownloadWhite()
    {
        StartCoroutine(download(whiteUrl));
    }

    public void DownloadNowWait()
    {
        downloadNoWait(blackUrl);
    }

    IEnumerator download(string url)
    {
        Debug.Log("download " + url);
        using (var www = WWW.LoadFromCacheOrDownload(url, version))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }

            foreach (var name in www.assetBundle.GetAllAssetNames())
            {
                Debug.Log("downloaded " + name);
            }

            var req = www.assetBundle.LoadAllAssetsAsync<GameObject>();
            yield return req;
            var count = 0;
            foreach (var asset in req.allAssets)
            {
                Debug.Log("instanciate "+ asset);
                var go = GameObject.Instantiate<GameObject>((GameObject)asset);
                go.transform.parent = parent;
                go.transform.localPosition= new Vector3(0, count * 200, 0);
                go.transform.rotation = Quaternion.identity;
            }
        }
    }

    void downloadNoWait(string url)
    {
        Debug.Log("download no wait " + url);
        using (var www = WWW.LoadFromCacheOrDownload(url, version))
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
            }

            foreach (var name in www.assetBundle.GetAllAssetNames())
            {
                Debug.Log("downloaded " + name);
            }

            var req = www.assetBundle.LoadAllAssetsAsync<GameObject>();
            var count = 0;
            foreach (var asset in req.allAssets)
            {
                Debug.Log("instanciate " + asset);
                var go = GameObject.Instantiate<GameObject>((GameObject)asset);
                go.transform.parent = parent;
                go.transform.localPosition = new Vector3(0, count * 200, 0);
                go.transform.rotation = Quaternion.identity;
            }
        }
    }
}