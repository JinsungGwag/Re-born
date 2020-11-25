using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ۼ��� : �̼�ȣ
/// ��� : ��ȭâ ����
/// </summary>
public class Chat : SingletonBase<Chat>
{
    [SerializeField]
    private Text mChatText;
    [SerializeField]
    private float mActivateTime = 10.0f;

    private Canvas mChatCanvas;
    private Sprite mChatImage;
    private EndChat mEndChat;
    private Image mPopUpPanelImage;
    private Image mPopUpImage;
    private Image mChatPanelImage;
    private Coroutine mTickCoroutine = null;

    private bool mbIsActivateChat = false;
    public bool IsActivateChat
    {
        get
        {
            return mbIsActivateChat;
        }
        private set
        {

        }
    }
    private string mItem = null;
    public string Item
    {
        get
        {
            return mItem;
        }
        set
        {
            mItem = value;
        }
    }
    private GameObject mStartChat = null;
    public GameObject StartChat
    {
        get
        {
            return mStartChat;
        }
        set
        {
            mStartChat = value;
        }
    }

    public void ActivateChat(string text, Sprite spriteOrNull, bool time)
    {
        if (time == true)
        {
            if (mTickCoroutine != null)
            {
                StopCoroutine(mTickCoroutine);
            }

            mTickCoroutine = StartCoroutine(TickActivateTime());
        }
        
        mChatCanvas.enabled = true;

        if (text.Trim() != ""
            && spriteOrNull != null)
        {
            mChatText.text = text;
            mChatPanelImage.enabled = true;

            mPopUpPanelImage.enabled = true;
            mPopUpImage.enabled = true;
            mPopUpImage.sprite = spriteOrNull;
        }
        else if (text.Trim() != "")
        {
            mChatText.text = text;
            mChatPanelImage.enabled = true;
        }
        else if (spriteOrNull != null)
        {
            mPopUpPanelImage.enabled = true;
            mPopUpImage.enabled = true;
            mPopUpImage.sprite = spriteOrNull;
        }
        else
        {
            mChatCanvas.enabled = false;
        }
    }

    public void DeactivateChat()
    {
        if (mTickCoroutine != null)
        {
            StopCoroutine(mTickCoroutine);
        }
        mTickCoroutine = null;
        
        mChatText.text = "";
        mChatPanelImage.enabled = false;

        mPopUpImage.sprite = null;
        mPopUpImage.enabled = false;
        mPopUpPanelImage.enabled = false;

        mChatCanvas.enabled = false;

        mEndChat.enabled = false;

        //if (item != "" && item != null)
        //{
        //    if (Inventory.instance.UseSelectedItem(item))
        //    {
        //        Sprite[] img = startChat.GetComponent<StartChat>().getItemImg();
        //        startChat.GetComponent<StartChat>().SetLargeImgs(img, true);
        //        return;
        //    }
        //}
    }

    private void Awake()
    {
        if (instance != null &&
            instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        mChatCanvas = GetComponent<Canvas>();
        mEndChat = GetComponent<EndChat>();
        mEndChat.endChatEvent += DeactivateChat;

        mChatPanelImage = transform.Find("ChatPanel").GetComponent<Image>();
        mPopUpPanelImage = transform.Find("ImagePanel").GetComponent<Image>();
        mPopUpImage = transform.Find("ImagePanel").transform.Find("Image").GetComponent<Image>();
        mPopUpImage.sprite = null;

        DeactivateChat();
    }

    private IEnumerator TickActivateTime()
    {
        float tickTime = 0.0f;

        while (tickTime <= mActivateTime)
        {
            tickTime = tickTime + Time.deltaTime;

            yield return null;
        }

        DeactivateChat();
    }

}
