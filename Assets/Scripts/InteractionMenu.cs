using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMenu : UIElement
{
    public static InteractionMenu Instance;
    public float startAngle = 45f;

    public float openRadius = 200f;
    public float radius = 3f;
    private int size;
    public CanvasGroup canvasGroup;
    public InteractionButton interactionButtonPrefab;
    public bool open;

    private Tween tweener;
    private Interactable target;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Interaction menu already exists.");
            Destroy(this);
        }
        Instance = this;
        Close();
    }

    private void Update()
    {
        if (open && target)
        {
            Collider collider = target.GetComponent<Collider>();
            transform.position = Camera.main.WorldToScreenPoint(collider.bounds.center);
        }
    }

    public static void OpenMenu(Interactable interactable)
    {
        Instance.target = interactable;
        Instance.Open();
    }

    public static void CloseMenu()
    {
        Instance.target = null;
        Instance.Close();
    }

    public override void Open()
    {
        if (spawnedButtons.Count > 0)
        {
            for (int i = spawnedButtons.Count - 1; i >= 0; i--)
            {
                Destroy(spawnedButtons[i]);
            }
        }
        spawnedButtons.Clear();

        IInteractable[] interactions = target.GetComponents<IInteractable>();
        for (int i = 0; i < interactions.Length; i++)
        {
            if (!interactions[i].IsEnabled())
            {
                continue;
            }
            InteractionButton interactionButton = Instantiate(interactionButtonPrefab, transform);
            spawnedButtons.Add(interactionButton.gameObject);
            string interactionName = interactions[i].GetName();
            int index = i;
            interactionButton.button.onClick.AddListener(() =>
            {
                Debug.Log(interactionName);
                InteractionSystem.SendInteraction(interactions[index]);
                Close();
            });

            interactionButton.text.enabled = interactions[i].GetIcon() == null;
            interactionButton.icon.enabled = interactions[i].GetIcon();

            interactionButton.text.SetText(interactions[i].GetName());
            interactionButton.icon.sprite = interactions[i].GetIcon();

            interactionButton.info = interactions[i].GetName();
        }

        gameObject.SetActive(true);
        float duration = .5f;
        open = true;
        tweener.Kill();
        canvasGroup.alpha = 1;
        tweener = DOTween.To(x => SetRadius(x), 0f, openRadius, duration).SetEase(Ease.OutElastic);
    }

    public override void Close()
    {
        float duration = .1f;
        open = false;
        tweener.Kill();
        tweener = DOTween.To(x => SetRadius(x), radius, 0f, duration).SetEase(Ease.InCubic);
        tweener.onComplete += () => canvasGroup.DOFade(0f, duration * .5f);
        tweener.onComplete += () => gameObject.SetActive(false);
    }

    private void SetRadius(float radius)
    {
        this.radius = radius;
        float angle = startAngle;

        for (int i = 0; i < (spawnedButtons.Count); i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Vector2 position = new Vector2(x, y) + (Vector2)transform.position;
            spawnedButtons[i].transform.position = position;

            angle += (360f / spawnedButtons.Count);
        }
    }
}