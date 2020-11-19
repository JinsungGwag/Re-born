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

    private int currentClickCount = 0;
    private int currentClickCountImg = 0;
    private int currentClickCountSound = 0;

    // EventTrigger����� ��� ���
    public void Click()
    {
        if (Chat.Instance.IsActivateChat == false)
        {
            if (texts.Length > 0 && texts[currentClickCount] != "" && LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(texts[currentClickCount], LargeImgs[currentClickCountImg]);
            }
            else if (texts.Length > 0 && texts[currentClickCount] != "")
            {
                Chat.Instance.ActivateChat(texts[currentClickCount]);
            }
            else if (LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(LargeImgs[currentClickCountImg]);
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
        if (Chat.Instance.IsActivateChat == false)
        {
            if (texts.Length > 0 && texts[currentClickCount] != "" && LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(texts[currentClickCount], LargeImgs[currentClickCountImg]);
            }
            else if (texts.Length > 0 && texts[currentClickCount] != "")
            {
                Chat.Instance.ActivateChat(texts[currentClickCount]);
            }
            else if (LargeImgs.Length > 0 && LargeImgs[currentClickCountImg] != null)
            {
                Chat.Instance.ActivateChat(LargeImgs[currentClickCountImg]);
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
}