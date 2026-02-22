using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlipTheCard : MonoBehaviour
{
    public int index = 0;
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

    public void SetCardBase()
    {
        if (!SaveManager.Instance.state.cards[index].isMatched)
        {
            isFlipped = false;
            bg.gameObject.SetActive(true);
            bgOutline = bg.GetComponent<Outline>();
            bgColor = bg.color;
        }
        else
        {
            isFlipped = true;
            bg.gameObject.SetActive(false);
        }
        item.gameObject.SetActive(false);
    }

    public void OnClickCard()
    {
        if (MainMenuGUI.instance.open)
        {
            MainMenuGUI.instance.open = false;
            MainMenuGUI.instance.settings.Play("Settings_Close_Ingame");
        }

        if (isFlipped || AutoGridFit.instance.isCardMatching || AutoGridFit.instance.isGridDisplayOver) return;

        if (SaveManager.Instance.state.isSound)
            flipSound.Play();

        if (SaveManager.Instance.state.isVibration)
        {
            Vibration.Init();
            Vibration.VibratePeek();
        }

        isFlipped = true;

        if (InGameGUI.instance.tapCount >= 2)
        {
            InGameGUI.instance.tapCount = 0;
            InGameGUI.instance.prevSelected = null;
        }

        InGameGUI.instance.tapCount++;

        prevCardRef = InGameGUI.instance.prevSelected;

        FlipCard();
        if (prevCardRef != null && prevCardRef != this)
        {
            StartCoroutine(ComparePair(prevCardRef, this));
        }
        InGameGUI.instance.prevSelected = this;
    }

    public void FlipCard()
    {
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
        flipSeq.Append(transform.DOPunchScale(Vector3.one * 0.07f, 0.1f));
    }

    IEnumerator ComparePair(FlipTheCard card_1, FlipTheCard card_2)
    {
        yield return new WaitForSeconds(0.3f);

        if (card_1 == null || card_2 == null)
            yield break;

        if (card_1.cardIndex == card_2.cardIndex)
        {
            AutoGridFit.instance.isCardMatching = true;
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
        if (SaveManager.Instance.state.isSound)
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

        float targetSize = Mathf.Min(Screen.width, Screen.height) * 0.35f;
        float currentSize = rect.rect.width;
        float dynamicScale = targetSize / currentSize;

        dynamicScale = Mathf.Clamp(dynamicScale, 0.8f, 1.8f);
        Vector2 targetPos = isPrevCard
            ? center + Vector2.right * offsetX
            : center + Vector2.left * offsetX;

        seq.Join(rect.DOAnchorPos(targetPos, 0.35f).SetEase(Ease.OutBack));
        seq.Join(rect.DOScale(originalScale * dynamicScale, 0.35f).SetEase(Ease.OutBack));
    }

    void MergeCards()
    {
        if (SaveManager.Instance.state.isSound)
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
            AutoGridFit.instance.isCardMatching = false;
            SaveManager.Instance.state.cards[index].isMatched = true;
        });
    }
}