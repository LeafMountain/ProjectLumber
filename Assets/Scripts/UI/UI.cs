using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UI : MonoBehaviour
{
    public static UI Instance;

    public ResourceNotification resourceNotificationPrefab;

    public Transform lowLevel;
    public Transform midLevel;
    public Transform highLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void ShowNotification(Interactable interactable, string text)
    {
        Vector3 notificationPosition = Camera.main.WorldToScreenPoint(interactable.transform.position);
        Addressables.InstantiateAsync("UI/GeneralNotification", UI.Instance.midLevel).Completed += (result) =>
        {
            result.Result.transform.position = notificationPosition;
            result.Result.GetComponentInChildren<TMP_Text>().SetText(text);
            result.Result.transform.localScale = Vector3.zero;
            result.Result.transform.DOScale(1f, .1f).onComplete += () =>
            {
                result.Result.transform.DOMove(result.Result.transform.position + new Vector3(0f, 100f, 0f), 3f).onComplete += () => result.Result.transform.DOScale(0f, .1f).onComplete = () => Destroy(result.Result);
            };
        };
    }

    public static ResourceNotification GetResourceNotification()
    {
        return Instantiate(Instance.resourceNotificationPrefab, Instance.lowLevel);
    }
}
