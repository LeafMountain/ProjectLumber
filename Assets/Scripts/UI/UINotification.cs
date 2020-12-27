using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DG.Tweening;

public class UINotification : MonoBehaviour
{
    // public static void ShowNotification(Interactable interactable, string text)
    // {
    //     Vector3 notificationPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
    //     Addressables.InstantiateAsync("UI/GeneralNotification", UI.Instance.transform).Completed += (result) =>
    //     {
    //         result.Result.transform.position = notificationPosition;
    //         result.Result.GetComponentInChildren<TMP_Text>().SetText(text);
    //         result.Result.transform.localScale = Vector3.zero;
    //         result.Result.transform.DOScale(1f, .1f).onComplete += () =>
    //         {
    //             result.Result.transform.DOMove(result.Result.transform.position + new Vector3(0f, 100f, 0f), 3f).onComplete += () => result.Result.transform.DOScale(0f, .1f).onComplete = () => Destroy(result.Result);
    //         };
    //     };
    // }
}
