using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ۼ��� : �̼�ȣ
/// ��� : ��ȭâ ���� �� ���� ����
/// </summary>
public class StartChat : MonoBehaviour
{
    [SerializeField]
    private string[] texts;
    [SerializeField]
    private Sprite[] LargeImgs;
    [SerializeField]
    private string[] sounds = null;
    [SerializeField]
    private bool time = true;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private Sprite[] itemImg;

    private int currentClickCount = 0;
    private int currentClickCountImg = 0;
    private int currentClickCountSound = 0;

    // EventTrigger����� ��� ���
    public void Click()
    {
        Chat.Instance.Item = itemName;
        Chat.Instance.StartChat = this.gameObject;

        if (Chat.Instance.IsActivateChat == false)
        {
            if (texts.Length > 0 && texts[currentClickCount] != "" && LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(texts[currentClickCount], LargeImgs[currentClickCountImg], time);
            }
            else if (texts.Length > 0 && texts[currentClickCount] != "")
            {
                Chat.Instance.ActivateChat(texts[currentClickCount], time);
            }
            else if (LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(LargeImgs[currentClickCountImg], time);
            }

            if (sounds.Length > 0 && sounds[currentClickCountSound] != "")
            {
                SoundManager.Instance.SetAndPlaySFX(sounds[currentClickCountSound]);
            }


            if (currentClickCount < texts.Length - 1)
            {
                currentClickCount++;
            }
            if (currentClickCountImg < LargeImgs.Length - 1)
            {
                currentClickCountImg++;
            }
            if (currentClickCountSound < sounds.Length - 1)
            {
                currentClickCountSound++;
            }
        }
    }

    // Collider�� �־� ��� �� �� �ִ� 3D������Ʈ�� ���
    public void OnMouseDown()
    {
        Chat.Instance.Item = itemName;
        Chat.Instance.StartChat = this.gameObject;

        if (Chat.Instance.IsActivateChat == false)
        {
            if (texts.Length > 0 && texts[currentClickCount] != "" && LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(texts[currentClickCount], LargeImgs[currentClickCountImg], time);
            }
            else if (texts.Length > 0 && texts[currentClickCount] != "")
            {
                Chat.Instance.ActivateChat(texts[currentClickCount], time);
            }
            else if (LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(LargeImgs[currentClickCountImg], time);
            }

            if (sounds.Length > 0 && sounds[currentClickCountSound] != "")
            {
                SoundManager.Instance.SetAndPlaySFX(sounds[currentClickCountSound]);
            }


            if (currentClickCount < texts.Length - 1)
            {
                currentClickCount++;
            }
            if (currentClickCountImg < LargeImgs.Length - 1)
            {
                currentClickCountImg++;
            }
            if (currentClickCountSound < sounds.Length - 1)
            {
                currentClickCountSound++;
            }
        }
    }

    public void SetLargeImgs(Sprite[] LargeImgs, bool click = false)
    {
        this.LargeImgs = LargeImgs;

        if (click)
        {
            Click();
        }
    }

    public Sprite[] getItemImg() { return itemImg; }
}