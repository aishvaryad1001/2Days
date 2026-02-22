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

    public AudioSource flipSound;

    private Sequence flipSeq;

    private Outline bgOutline;
    private Color bgColor;

    private Transform originalParent;
    private Vector3 originalScale;

    private Coroutine flipCo;
    private FlipTheCard prevCardRef;

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

        if (SoundManager.instance.isSoundOn)
            flipSound.Play();

        if (SoundManager.instance.isVibratrionOn)
        {
            Vibration.Init();
            Vibration.VibratePeek();
        }

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
            InGameGUI.instance.currCombo++;
            InGameGUI.instance.ShowComboFX();

            card_1.TakeOutAndFocus(false);
            card_2.TakeOutAndFocus(true);
            yield return new WaitForSeconds(0.3f);
            card_1.MergeCards();
            card_2.MergeCards();
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
        if (SoundManager.instance.isSoundOn)
        {
            SoundManager.instance.gameSound.clip = SoundManager.instance.wrongMatch;
            SoundManager.instance.gameSound.Play();
        }
        InGameGUI.instance.currCombo = -1;

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


    void TakeOutAndFocus(bool isPrevCard)
    {
        if (flipCo != null)
        {
            StopCoroutine(flipCo);

            item.gameObject.SetActive(true);
            bg.color = Color.white;
            bgOutline.effectColor = bgColor;
            transform.localEulerAngles = Vector3.zero;
        }

        if (flipSeq != null)
            flipSeq.Kill();

        originalParent = rect.parent;
        originalScale = rect.localScale;

        rect.SetParent(InGameGUI.instance.layout.transform, true);
        rect.SetAsLastSibling();

        Sequence seq = DOTween.Sequence();

        Vector2 center = InGameGUI.instance.CenterPosition();
        float offsetX = 250f;

        Vector2 targetPos = isPrevCard
            ? center + Vector2.right * offsetX
            : center + Vector2.left * offsetX;

        seq.Join(rect.DOAnchorPos(targetPos, 0.35f).SetEase(Ease.OutBack));
        seq.Join(rect.DOScale(originalScale * 1.8f, 0.35f).SetEase(Ease.OutBack));
    }

    void MergeCards()
    {
        if (SoundManager.instance.isSoundOn)
        {
            SoundManager.instance.gameSound.clip = SoundManager.instance.rightMatch;
            SoundManager.instance.gameSound.Play();
        }

        Vector3 center = InGameGUI.instance.CenterPosition();

        Sequence mergeSeq = DOTween.Sequence();

        mergeSeq.Join(rect.DOAnchorPos(center, 0.25f).SetEase(Ease.InBack));
        mergeSeq.Append(rect.DOScale(Vector3.zero, 0.25f));
        mergeSeq.InsertCallback(0.25f + 0.125f, () =>
        {
            InGameGUI.instance.fx.SetActive(true);
        });
        mergeSeq.OnComplete(() =>
        {
            rect.gameObject.SetActive(false);
            InGameGUI.instance.UpdateScore(1);
            InGameGUI.instance.CheckIfAllCardsPaired();
            AutoGridFit.instance.matchedCards.Add(rect.gameObject);
        });
    }
}