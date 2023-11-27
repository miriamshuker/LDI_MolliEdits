using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

//TODO: Possibly rename this to focus on UIAnimationManager? That's all this does.
public class PhoneManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static PhoneManager Instance { get; private set; }
    public TextManager tManager;
    public NoteManager nManager;
    //public PhotoManager pManager;

    //Reference to other UIElement, to hide it when this is focused
    public enum PhoneState
    {
        NONE,
        HOVER,
        UNHOVER,
        FOCUS,
        FOCUSED,
        UNFOCUS,
        HIDE,
        HIDDEN,
        UNHIDE,
        ALERT
    }

    public void SetTimeManager(TimeManager tm)
    {
        timeManager = tm;
    }

    public enum PhoneApp
    {
        NONE,
        HOME,
        TEXTS,
        NOTES,
        PHOTOS,
        SETTINGS,
    }
    //TODO: Hover currently only works if the mouse ENTERS the object, not if it's already there.
    //Could possibly be because the other IEnumerators are getting called and changing the state before a previous one can finish
    //TODO: Rename functions and IEnumerators to match (e.g. Focus() and PlayFocus() or Focus() and IFocus())

    public PhoneState phoneState;
    public PhoneApp phoneApp;
    public float focusAnimationTime;
    public float hideAnimationTime;
    public float alertAnimationTime;
    public bool isFocused;
    public Animator animator;
    public Button putAway;
    public CanvasGroup canvasGroup;
    [SerializeField]
    private TimeManager timeManager;

    [SerializeField]
    private RectTransform _home, _texts, _notes, _photos, _settings;
    private RectTransform _current;
    private AppManager[] apps;

    private bool isClosable = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }
    void Start()
    {
        canvasGroup.interactable = false;

        phoneApp = PhoneApp.HOME;
        _current = _home;
        _texts.gameObject.SetActive(false);
        apps = FindObjectsOfType<AppManager>(true);
        OpenApp("home");
    }
    void Update()
    {
        if (!GameManager.Instance.inConvo && Input.GetKeyDown(KeyCode.Mouse0) && phoneState == PhoneState.UNHOVER) //also, check if you're texting or in a story-specific message
        {
            Unfocus();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (phoneState == PhoneState.NONE || phoneState == PhoneState.UNHOVER)
        {
            phoneState = PhoneState.HOVER;
            if (!GameManager.Instance.inConvo)
            {
                animator.SetInteger("State", (int)phoneState);
            }
            Debug.Log("Mouse enter");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (phoneState == PhoneState.NONE || phoneState == PhoneState.HOVER)
        {
            phoneState = PhoneState.UNHOVER;
            animator.SetInteger("State", (int)phoneState);
            Debug.Log("Mouse exit");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameManager.Instance.inConvo && (phoneState == PhoneState.NONE || phoneState == PhoneState.HOVER))
        {
            Focus();
        }
    }
    public void SetInteractable(bool setting)
    {
        canvasGroup.interactable = setting;
    }
    //Previously Enter/Exit
    public void Focus()
    {
        Debug.Log("enter");
        StartCoroutine(PlayFocus());
    }
    [YarnCommand("unfocus")]
    public void Unfocus()
    {
        //pc.SetPlayerState(PlayerControl.PlayerState.BUSY);
        GameManager.Instance.isBusy = true;
        StartCoroutine(PlayUnfocus());
    }
    IEnumerator PlayFocus()
    {
        //pc.SetPlayerState(PlayerControl.PlayerState.BUSY);
        canvasGroup.interactable = true;
        GameManager.Instance.isBusy = true;
        //putAway.gameObject.SetActive(true);
        animator.SetInteger("State", (int)PhoneState.FOCUS);
        yield return new WaitForSeconds(focusAnimationTime);
        animator.SetInteger("State", (int)PhoneState.FOCUSED);

        isFocused = true;
        //Debug.Log("Focus");
    }
    IEnumerator PlayUnfocus()
    {
        //putAway.gameObject.SetActive(false);
        canvasGroup.interactable = false;

        OpenApp("home");
        animator.SetInteger("State", (int)PhoneState.UNFOCUS);
        yield return new WaitForSeconds(focusAnimationTime);
        animator.SetInteger("State", (int)PhoneState.NONE);
        GameManager.Instance.isBusy = false;
        isFocused = false;
        //pc.SetPlayerState(PlayerControl.PlayerState.NONE);
        //Debug.Log("Unfocus");
    }
    [YarnCommand("unhide")]
    public void Unhide()
    {
        StartCoroutine(PlayUnhide());
    }
    [YarnCommand("hide")]
    public void Hide()
    {
        StartCoroutine(PlayHide());
    }
    IEnumerator PlayHide()
    {
        //pc.SetPlayerState(PlayerControl.PlayerState.BUSY);
        GameManager.Instance.isBusy = true;
        animator.SetInteger("State", (int)PhoneState.HIDE);
        yield return null;
        //yield return new WaitForSeconds(hideAnimationTime);
        animator.SetInteger("State", (int)PhoneState.HIDDEN);

        isFocused = false;
        Debug.Log("Hide");
    }
    IEnumerator PlayUnhide()
    {
        animator.SetInteger("State", (int)PhoneState.UNHIDE);
        yield return new WaitForSeconds(hideAnimationTime);
        animator.SetInteger("State", (int)PhoneState.NONE);
        GameManager.Instance.isBusy = false;
        isFocused = false;
        //pc.SetPlayerState(PlayerControl.PlayerState.NONE);
        Debug.Log("Unhide");
    }

    public void Alert()
    {
        StartCoroutine(PlayAlert());
        Debug.Log("alert");
    }
    IEnumerator PlayAlert()
    {
        Debug.Log("alerting");
        animator.SetInteger("State", (int)PhoneState.ALERT);
        yield return new WaitForSeconds(alertAnimationTime);
        animator.SetInteger("State", (int)PhoneState.NONE);
        Debug.Log("returning to default");
    }

    public void OpenApp(string app)
    {
        PhoneApp p = (PhoneApp)Enum.Parse(typeof(PhoneApp), app, true);
        if (p == phoneApp)
        {
            return;
        }

        phoneApp = p;
        Debug.Log(timeManager != null);
        //timeManager.SetState(p == PhoneApp.HOME);

        if (p == PhoneApp.HOME)
        {
            FocusApp(_home, false);
            foreach (AppManager a in apps)
            {
                a.Back();
            }
        }
        else if (p == PhoneApp.NOTES)
        {
            FocusApp(_notes);
        }
        else if (p == PhoneApp.TEXTS)
        {
            FocusApp(_texts);
        }
    }
    void FocusApp(RectTransform rt, bool fromRight = true)
    {
        _home.gameObject.SetActive(false);
        _texts.gameObject.SetActive(false);
        _notes.gameObject.SetActive(false);
        //_photos.gameObject.SetActive(false);
        //_settings.gameObject.SetActive(false);
        _home.anchoredPosition = new Vector2(0, 0f);
        _texts.anchoredPosition = new Vector2(0, 0f);
        _notes.anchoredPosition = new Vector2(0, 0f);
        //_photos.anchoredPosition = new Vector2(0, 0f);
        //_settings.anchoredPosition = new Vector2(0, 0f);

        if (_current != null)
            _current.gameObject.SetActive(true);

        if (fromRight)
        {
            rt.anchoredPosition = new Vector2(56, 0f);
            rt.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutQuart);
            if (_current != null)
                _current.DOAnchorPosX(-56, 0.5f).SetEase(Ease.OutQuart);
            rt.gameObject.SetActive(true);
        }
        else
        {
            rt.anchoredPosition = new Vector2(-56, 0f);
            rt.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutQuart);
            if (_current != null)
                _current.DOAnchorPosX(56, 0.5f).SetEase(Ease.OutQuart);
            rt.gameObject.SetActive(true);
        }

        _current = rt;

    }
    public void ClearBackable()
    {
        tManager.SetBackable(true);
        nManager.SetBackable(true);
    }
    public void OpenText(string contactName, Message m)
    {
        tManager.ForceOpen(contactName, m);
        StartCoroutine(Test(.5f));
    }
    IEnumerator Test(float dur)
    {
        yield return new WaitForSeconds(dur);
        Focus();
        OpenApp("texts");
        tManager.SetBackable(false);
    }
    public void NotifyText(string contactName, Message m)
    {
        tManager.Notify(contactName, m);
    }
    public void NotifyText(Message m)
    {
        tManager.Notify(m);
    }
    public void NotifyNote(string header)
    {
        nManager.ShowNote(header);
    }
}
