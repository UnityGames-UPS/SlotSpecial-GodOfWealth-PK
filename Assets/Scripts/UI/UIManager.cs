//using System.Diagnostics;
//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    [Header("Settings UI")]
    [SerializeField] private Button Settings_Button;

    [SerializeField]
    private Button Exit_Button;
    [SerializeField]
    private GameObject Exit_Object;
    [SerializeField]
    private RectTransform Exit_RT;

    [SerializeField]
    private Button Paytable_Button;
    [SerializeField]
    private GameObject Paytable_Object;
    [SerializeField]
    private RectTransform Paytable_RT;

    [Header("Popus UI")]
    [SerializeField]
    private GameObject MainPopup_Object;

    [Header("Paytable Popup")]

    [SerializeField]
    private TMP_Text Scatter_Text;
    [SerializeField]
    private TMP_Text BlueWild_Text;
    [SerializeField]
    private TMP_Text GoldWild_Text;

    [Header("Settings Popup")]
    [SerializeField]
    private GameObject SettingsPopup_Object;
    [SerializeField]
    private Button SettingsExit_Button;
    [SerializeField]
    private Button Sound_Button;
    [SerializeField]
    private Button Music_Button;

    [Header("Win Popup")]
    [SerializeField]
    private GameObject BigWin_Gameobject;
    [SerializeField]
    private GameObject HugeWin_Gameobject;
    [SerializeField]
    private GameObject MegaWin_GameObject;
    [SerializeField]
    private GameObject WinPopupMain_Object;
    [SerializeField]
    private TMP_Text Win_Text;
    [SerializeField] private Button SkipWinAnimation;

    [Header("FreeSpins Popup")]
    [SerializeField]
    private GameObject FreeSpinMainPopup_Object;
    [SerializeField]
    private GameObject FreeSpinPopup_Object;
    [SerializeField] private Button SkipFreeSpinAnimation;
    private bool ShowFreeSpin;
    // [SerializeField]
    // private TMP_Text Free_Text;

    [Header("Disconnection Popup")]
    [SerializeField]
    private Button CloseDisconnect_Button;
    [SerializeField]
    private GameObject DisconnectPopup_Object;

    [Header("AnotherDevice Popup")]
    // [SerializeField]
    // private GameObject ADPopup_Object;

    [Header("LowBalance Popup")]
    [SerializeField]
    private Button LBExit_Button;
    [SerializeField]
    private GameObject LBPopup_Object;

    [Header("Quit Popup")]
    [SerializeField]
    private GameObject QuitPopup_Object;
    [SerializeField]
    private Button YesQuit_Button;
    [SerializeField]
    private Button NoQuit_Button;
    [SerializeField]
    private Button CrossQuit_Button;

    [SerializeField]
    private AudioController audioController;

    [SerializeField]
    private Button GameExit_Button;

    [SerializeField]
    private SlotBehaviour slotManager;

    [SerializeField]
    private SocketIOManager socketManager;

    private bool isMusic = true;
    private bool isSound = true;
    private bool isExit = false;
    private Tween WinPopupTextTween;
    private Tween ClosePopupTween;
    internal int FreeSpins;

    [Header("Info_Pages")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button infoButton;
    public List<GameObject> infoPages;
    public List<GameObject> infoInnerDots;
    public Button infoLeftButton;
    public Button infoRightButton;
    [SerializeField] private Button infoCloseButton;

    [Header("Sound")]
    [SerializeField] private Sprite Enable_Sound_sprite;
    [SerializeField] private Sprite Disable_Sound_sprite;




    private int currentPage = 0;


    [SerializeField]
    private SymbolTextGroup[] SymbolsText;

    [SerializeField] private SymbolTextGroup ScatterFreeSpinstext;
    [SerializeField] private List<TMP_Text> InfoMultiplierPageTexts;
    private void Start()
    {

        if (Exit_Button) Exit_Button.onClick.RemoveAllListeners();
        if (Exit_Button) Exit_Button.onClick.AddListener(CloseMenu);

        if (Settings_Button) Settings_Button.onClick.RemoveAllListeners();
        if (Settings_Button) Settings_Button.onClick.AddListener(delegate { OpenPopup(SettingsPopup_Object); });


        if (SettingsExit_Button) SettingsExit_Button.onClick.RemoveAllListeners();
        if (SettingsExit_Button) SettingsExit_Button.onClick.AddListener(delegate { ClosePopup(SettingsPopup_Object); });

        if (GameExit_Button) GameExit_Button.onClick.RemoveAllListeners();
        if (GameExit_Button) GameExit_Button.onClick.AddListener(delegate
        {
            OpenPopup(QuitPopup_Object);
            Debug.Log("Quit event: pressed Big_X button");

        });

        if (NoQuit_Button) NoQuit_Button.onClick.RemoveAllListeners();
        if (NoQuit_Button) NoQuit_Button.onClick.AddListener(delegate
        {
            if (!isExit)
            {
                ClosePopup(QuitPopup_Object);
                Debug.Log("quit event: pressed NO Button ");
            }
        });

        if (CrossQuit_Button) CrossQuit_Button.onClick.RemoveAllListeners();
        if (CrossQuit_Button) CrossQuit_Button.onClick.AddListener(delegate
        {
            if (!isExit)
            {
                ClosePopup(QuitPopup_Object);
                Debug.Log("quit event: pressed Small_X Button ");

            }
        });

        if (LBExit_Button) LBExit_Button.onClick.RemoveAllListeners();
        if (LBExit_Button) LBExit_Button.onClick.AddListener(delegate { ClosePopup(LBPopup_Object); });

        if (YesQuit_Button) YesQuit_Button.onClick.RemoveAllListeners();
        if (YesQuit_Button) YesQuit_Button.onClick.AddListener(delegate
        {
            CallOnExitFunction();
            Debug.Log("quit event: pressed YES Button ");

        });

        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.RemoveAllListeners();
        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.AddListener(delegate { CallOnExitFunction(); }); //BackendChanges

        if (audioController) audioController.ToggleMute(false);

        isMusic = true;
        isSound = true;

        if (Sound_Button) Sound_Button.onClick.RemoveAllListeners();
        if (Sound_Button) Sound_Button.onClick.AddListener(ToggleSound);

        if (Music_Button) Music_Button.onClick.RemoveAllListeners();
        if (Music_Button) Music_Button.onClick.AddListener(ToggleMusic);

        if (SkipWinAnimation) SkipWinAnimation.onClick.RemoveAllListeners();
        if (SkipWinAnimation) SkipWinAnimation.onClick.AddListener(SkipWin);

        if (SkipFreeSpinAnimation) SkipFreeSpinAnimation.onClick.RemoveAllListeners();
        if (SkipFreeSpinAnimation) SkipFreeSpinAnimation.onClick.AddListener(SkipFreeSpin);

        infoLeftButton.onClick.AddListener(GoToPreviousInfoPage);
        infoRightButton.onClick.AddListener(GoToNextInfoPage);
        infoButton.onClick.AddListener(OpenInfoPanel);
        infoCloseButton.onClick.AddListener(CloseInfoPanel);
    }

    internal void LowBalPopup()
    {
        OpenPopup(LBPopup_Object);
    }

    internal void DisconnectionPopup(bool isReconnection)
    {
        if (!isExit)
        {
            OpenPopup(DisconnectPopup_Object);
        }
    }

    internal void PopulateWin(int value, double amount)
    {
        BigWin_Gameobject.SetActive(false);
        HugeWin_Gameobject.SetActive(false);
        MegaWin_GameObject.SetActive(false);

        switch (value)
        {
            case 1:
                BigWin_Gameobject.SetActive(true);
                break;
            case 2:
                HugeWin_Gameobject.SetActive(true);
                break;
            case 3:
                MegaWin_GameObject.SetActive(true);
                break;
        }

        StartPopupAnim(amount);
    }

    private void StartFreeSpins(int spins)
    {
        if (MainPopup_Object) MainPopup_Object.SetActive(false);
        if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(false);
        if (FreeSpinMainPopup_Object) FreeSpinMainPopup_Object.SetActive(false);

        slotManager.FreeSpin(spins);
    }

    internal IEnumerator FreeSpinProcess(int spins)
    {
        ShowFreeSpin = true;
        int ExtraSpins = spins - FreeSpins;
        FreeSpins = spins;
        Debug.Log("ExtraSpins: " + ExtraSpins);
        Debug.Log("Total Spins: " + spins);
        if (FreeSpinMainPopup_Object) FreeSpinMainPopup_Object.SetActive(true);
        if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(true);
        // if (Free_Text) Free_Text.text = ExtraSpins.ToString() + " Free spins awarded.";
        DOVirtual.DelayedCall(2f, () =>
        {
            ShowFreeSpin = false;
        });
        yield return new WaitUntil(() => !ShowFreeSpin);
        StartFreeSpins(spins);


    }

    void SkipFreeSpin()
    {
        ShowFreeSpin = false;
    }

    void SkipWin()
    {
        Debug.Log("Skip win called");
        if (ClosePopupTween != null)
        {
            ClosePopupTween.Kill();
            ClosePopupTween = null;
        }
        if (WinPopupTextTween != null)
        {
            WinPopupTextTween.Kill();
            WinPopupTextTween = null;
        }
        ClosePopup(WinPopupMain_Object);
        slotManager.CheckPopups = false;
    }

    private void StartPopupAnim(double amount)
    {
        double initAmount = 0;
        if (WinPopupMain_Object) WinPopupMain_Object.SetActive(true);
        WinPopupTextTween = DOTween.To(() => initAmount, (val) => initAmount = val, amount, 5f).OnUpdate(() =>
        {
            if (Win_Text) Win_Text.text = initAmount.ToString("F3");
        });

        ClosePopupTween = DOVirtual.DelayedCall(6f, () =>
        {
            Debug.Log("Delayed call triggered"); // Make sure this logs
                                                 // ClosePopup(WinPopupMain_Object);
            WinPopupMain_Object.SetActive(false);
            slotManager.CheckPopups = false;
            Debug.Log("CheckPopups false");
        });
    }

    internal void ADfunction()
    {
        // OpenPopup(ADPopup_Object);
    }

    internal void InitialiseUIData(Paylines symbolsText)
    {
        PopulateSymbolsPayout(symbolsText);

         SetMultiplierinfo();
    }

    private void PopulateSymbolsPayout(Paylines paylines)
    {
        for (int i = 0; i < SymbolsText.Length; i++)
        {
            var symbol = paylines.symbols[i];

            if (symbol.multiplier[0] != 0)
            {
                SymbolsText[i].Text5x.text = symbol.multiplier[0].ToString();
            }
            else
            {
                SymbolsText[i].Text5x.text = "";
            }

            if (symbol.multiplier[1] != 0)
            {
                SymbolsText[i].Text4x.text = symbol.multiplier[1].ToString();
            }
            else
            {
                SymbolsText[i].Text4x.text = "";
            }

            if (symbol.multiplier[2] != 0)
            {
                SymbolsText[i].Text3x.text = symbol.multiplier[2].ToString();
            }
            else
            {
                SymbolsText[i].Text3x.text = "";
            }

            Debug.Log("Symbol info: " + symbol.name);
        }


        for (int i = 0; i < paylines.symbols.Count; i++)
        {
            if (paylines.symbols[i].name.ToUpper() == "SCATTER")
            {
                string Description = paylines.symbols[i].description.ToString();
                string modifiedDescription = Description.Replace("\n ", "\n<sprite=0>");
                if (Scatter_Text) Scatter_Text.text = "<sprite=0>" + modifiedDescription;
            }
            if (paylines.symbols[i].name.ToUpper() == "BLUEWILD")
            {
                if (BlueWild_Text) BlueWild_Text.text = paylines.symbols[i].description.ToString();
            }

            if (paylines.symbols[i].name.ToUpper() == "GOLDWILD")
            {
                string Description = paylines.symbols[i].description.ToString();

                string modifiedDescription = Description.Replace("\n", "\n<sprite=0>");
                if (GoldWild_Text) GoldWild_Text.text = "<sprite=0>" + modifiedDescription;
            }
        }
        
        if(ScatterFreeSpinstext.Text5x.text!=null) ScatterFreeSpinstext.Text5x.text= socketManager.initialRootData.features.freeSpinCounts[0].ToString()+" FREE SPINS";  // paylines.symbols[i].Multiplier[0][1].ToString() + " FREE SPINS";
        ScatterFreeSpinstext.Text4x.text = socketManager.initialRootData.features.freeSpinCounts[1].ToString()+" FREE SPINS";
        ScatterFreeSpinstext.Text3x.text = socketManager.initialRootData.features.freeSpinCounts[2].ToString()+" FREE SPINS";
    }
    private void SetMultiplierinfo()
    {
        Debug.Log("Set value : 1 count: " + socketManager.initialRootData.features.allKindMults.Count);
        Debug.Log("InfoMultiplierPageTexts Count: " + InfoMultiplierPageTexts.Count);

        int count = Mathf.Min(socketManager.initialRootData.features.allKindMults.Count, InfoMultiplierPageTexts.Count);

        for (int i = 0; i < count; i++)
        {
            Debug.Log("Set value : 2  " + socketManager.initialRootData.features.allKindMults[i]);
            InfoMultiplierPageTexts[i].text = socketManager.initialRootData.features.allKindMults[i].ToString();
        }
    }


    private void CallOnExitFunction()
    {
        isExit = true;
        audioController.PlayButtonAudio();
        slotManager.CallCloseSocket();

    }

    private void OpenMenu()
    {
        audioController.PlayButtonAudio();
        if (Exit_Object) Exit_Object.SetActive(true);
        //if (About_Object) About_Object.SetActive(true);
        if (Paytable_Object) Paytable_Object.SetActive(true);
        //DOTween.To(() => About_RT.anchoredPosition, (val) => About_RT.anchoredPosition = val, new Vector2(About_RT.anchoredPosition.x, About_RT.anchoredPosition.y + 150), 0.1f).OnUpdate(() =>
        //{
        //    LayoutRebuilder.ForceRebuildLayoutImmediate(About_RT);
        //});

        DOTween.To(() => Paytable_RT.anchoredPosition, (val) => Paytable_RT.anchoredPosition = val, new Vector2(Paytable_RT.anchoredPosition.x, Paytable_RT.anchoredPosition.y + 125), 0.1f).OnUpdate(() =>
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(Paytable_RT);
        });

    }

    private void CloseMenu()
    {

        if (audioController) audioController.PlayButtonAudio();
        //DOTween.To(() => About_RT.anchoredPosition, (val) => About_RT.anchoredPosition = val, new Vector2(About_RT.anchoredPosition.x, About_RT.anchoredPosition.y - 150), 0.1f).OnUpdate(() =>
        //{
        //    LayoutRebuilder.ForceRebuildLayoutImmediate(About_RT);
        //});

        DOTween.To(() => Paytable_RT.anchoredPosition, (val) => Paytable_RT.anchoredPosition = val, new Vector2(Paytable_RT.anchoredPosition.x, Paytable_RT.anchoredPosition.y - 125), 0.1f).OnUpdate(() =>
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(Paytable_RT);
        });

        DOVirtual.DelayedCall(0.1f, () =>
         {
             if (Exit_Object) Exit_Object.SetActive(false);
             //if (About_Object) About_Object.SetActive(false);
             if (Paytable_Object) Paytable_Object.SetActive(false);
         });
    }

    private void OpenPopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();
        if (Popup) Popup.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
    }

    private void ClosePopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();
        if (Popup) Popup.SetActive(false);
        if (!DisconnectPopup_Object.activeSelf)
        {
            if (MainPopup_Object) MainPopup_Object.SetActive(false);
        }
    }

    private void ToggleMusic()
    {
        if (audioController) audioController.PlayButtonAudio();
        Image musicImage = Music_Button.gameObject.GetComponent<Image>();
        isMusic = !isMusic;
        if (isMusic)
        {
            musicImage.sprite = Enable_Sound_sprite;
            audioController.ToggleMute(false, "bg");
        }
        else
        {
            musicImage.sprite = Disable_Sound_sprite;
            audioController.ToggleMute(true, "bg");
        }
    }

    private void UrlButtons(string url)
    {
        Application.OpenURL(url);
    }

    private void ToggleSound()
    {
        if (audioController) audioController.PlayButtonAudio();
        Image musicImage = Sound_Button.gameObject.GetComponent<Image>();
        isSound = !isSound;
        if (isSound)
        {
            if (audioController) audioController.ToggleMute(false, "button");
            if (audioController) audioController.ToggleMute(false, "wl");
            musicImage.sprite = Enable_Sound_sprite;

        }
        else
        {
            if (audioController) audioController.ToggleMute(true, "button");
            if (audioController) audioController.ToggleMute(true, "wl");
            musicImage.sprite = Disable_Sound_sprite;

        }
    }

    private void OpenInfoPanel()
    {
        if (audioController) audioController.PlayButtonAudio();
        infoPanel.SetActive(true);
        UpdateInfoUI();
    }

    private void CloseInfoPanel()
    {
        if (audioController) audioController.PlayButtonAudio();

        currentPage = 0;
        infoPanel.SetActive(false);
    }

    private void UpdateInfoUI()
    {
        for (int i = 0; i < infoPages.Count; i++)
            infoPages[i].SetActive(i == currentPage);

        for (int i = 0; i < infoInnerDots.Count; i++)
            infoInnerDots[i].SetActive(i == currentPage);

    }

    private void GoToPreviousInfoPage()
    {
        if (audioController) audioController.PlayButtonAudio();
        currentPage--;
        if (currentPage < 0)
            currentPage = infoPages.Count - 1;

        UpdateInfoUI();
    }

    private void GoToNextInfoPage()
    {
        if (audioController) audioController.PlayButtonAudio();
        currentPage++;
        if (currentPage >= infoPages.Count)
            currentPage = 0;

        UpdateInfoUI();
    }

}

[System.Serializable]
public class SymbolTextGroup
{
    public TMP_Text Text5x;
    public TMP_Text Text4x;
    public TMP_Text Text3x;
}
