using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlipTheCard : MonoBehaviour
{
    public bool isFlipped = false;
    public RectTransform rect;

    public Image bg;
    public Image item;

    public int cardIndex;

    private Outline bgOutline;
    private Color bgColor;

    private Coroutine flipCo;
    private FlipTheCard prevCardRef;

    private Sequence flipSeq;


    private void Start()
    {
        isFlipped = false;
        bg.gameObject.SetActive(true);
        bgOutline = bg.GetComponent<Outline>();
        bgColor = bg.color;
        item.gameObject.SetActive(false);
    }

    public void OnClickCard()
    {
        if (isFlipped) return;

        isFlipped = true;

        if (InGameGUI.instance.tapCount >= 2)
        {
            InGameGUI.instance.tapCount = 0;
            InGameGUI.instance.prevSelected = null;
        }

        InGameGUI.instance.tapCount++;

        prevCardRef = InGameGUI.instance.prevSelected;

        if (flipSeq != null)
            flipSeq.Kill();

        flipSeq = DOTween.Sequence();

        flipSeq.Append(transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f).SetEase(Ease.InQuad));
        flipSeq.AppendCallback(() =>
        {
            item.gameObject.SetActive(true);
            bg.color = Color.white;
            bgOutline.effectColor = bgColor;
        });
        flipSeq.Append(transform.DOLocalRotate(new Vector3(0, 180, 0), 0.1f).SetEase(Ease.OutQuad));
        flipSeq.Append(transform.DOPunchScale(Vector3.one * 0.07f, 0.1f));//.OnComplete(() => CheckBeforeCardClose(isCardMatched));

        if (prevCardRef != null && prevCardRef != this)
        {
            StartCoroutine(ComparePair(prevCardRef, this));
        }
        InGameGUI.instance.prevSelected = this;
    }


    IEnumerator ComparePair(FlipTheCard card_1, FlipTheCard card_2)
    {
        yield return new WaitForSeconds(0.3f);

        if (card_1 == null || card_2 == null)
            yield break;

        if (card_1.cardIndex == card_2.cardIndex)
        {
            Debug.Log("CARDS MATCHED");
        }
        else
        {
            if (card_1.isFlipped)
                card_1.CloseTheCard();

            if (card_2.isFlipped)
                card_2.CloseTheCard();
        }
    }

    public void CloseTheCard()
    {
        flipCo = StartCoroutine(CloseTheCardCo());
    }

    IEnumerator CloseTheCardCo()
    {
        yield return new WaitForSeconds(0f);

        if (flipSeq != null)
            flipSeq.Kill();

        flipSeq = DOTween.Sequence();
        flipSeq.Append(transform.DOLocalRotate(new Vector3(0, 90, 0), 0.15f).SetEase(Ease.OutQuad));
        flipSeq.AppendCallback(() =>
        {
            item.gameObject.SetActive(false);
            bg.color = bgColor;
            bgOutline.effectColor = Color.white;
            isFlipped = false;
        });
        flipSeq.Append(transform.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.OutQuad));
    }
}