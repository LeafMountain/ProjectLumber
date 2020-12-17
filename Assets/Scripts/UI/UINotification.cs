using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DG.Tweening;

public class UINotification : MonoBehaviour
{
    public static void ShowNotification(Interactable interactable, string text)
    {
        Addressables.LoadAssetAsync<GameObject>("UI/GeneralNotification").Completed += (result) =>
        {
            GameObject go = Instantiate(result.Result, UI.instance.transform);
            go.transform.position = Camera.main.WorldToScreenPoint(interactable.transform.position);
            go.GetComponentInChildren<TMP_Text>().SetText(text);
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(1f, .1f).onComplete += () =>
            {
                go.transform.DOMove(go.transform.position + new Vector3(0f, 100f, 0f), 3f).onComplete += () => go.transform.DOScale(0f, .1f).onComplete = () => Destroy(go);
            };
        };
    }
}
