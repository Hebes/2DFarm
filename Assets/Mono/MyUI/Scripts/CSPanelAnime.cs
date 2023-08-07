using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

namespace MyUI
{
    public class CSPanelAnime : UIBehaviour
    {
        [System.Serializable]
        public class AnimeValue
        {
            public EnPanelAnimeType AnimeType;
            public EnMoveDir MoveDir;
            public EnValueType ValueType;
            public Vector3 Offset;

            public EnFadeType FadeType;
            public float Alpha = 0;

            public float SpendTime = 0.5f;
        }
        public List<AnimeValue> LiShowValues;
        public List<AnimeValue> LiHideValues;
        public UnityEvent OnShow = new UnityEvent();
        public UnityEvent OnHide = new UnityEvent();
        private Sequence _sqShowAnime;
        private Sequence _sqHideAnime;
        private RectTransform _rectTran;
        private Image _imgThis;
        private Vector2 _orignSizeDelta;
        private Vector2 _orignScale;
        public bool IsShow = false;
        protected override void Awake()
        {
            _imgThis = GetComponent<Image>();
            _rectTran = GetComponent<RectTransform>();
            _orignSizeDelta = _rectTran.sizeDelta;
            _orignScale = transform.localScale;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (_sqShowAnime?.IsActive() == true)
                _sqShowAnime.Kill();
            if (_sqHideAnime?.IsActive() == true)
                _sqHideAnime.Kill();
        }
        private Vector3 OffsetPos(AnimeValue newValue)
        {
            Vector3 newOffset = Vector3.zero;
            switch (newValue.ValueType)
            {
                case EnValueType.Default:
                    //Rect rect = _rectTran.rect;
                    newOffset = _orignSizeDelta * transform.localScale;
                    switch (newValue.MoveDir)
                    {
                        case EnMoveDir.Up:
                            newOffset = new Vector3(0, newOffset.y, 0);
                            break;
                        case EnMoveDir.Left:
                            newOffset = new Vector3(-newOffset.x, 0, 0);
                            break;
                        case EnMoveDir.Down:
                            newOffset = new Vector3(0, -newOffset.y, 0);
                            break;
                        case EnMoveDir.Right:
                            newOffset = new Vector3(newOffset.x, 0, 0);
                            break;
                        default:
                            break;
                    }
                    break;
                case EnValueType.Custom:
                    newOffset = newValue.Offset;
                    break;
                default:
                    break;
            }
            return newOffset;
        }
        /// <summary>
        /// 添加渐变动画
        /// </summary>
        private void FadeAnime(AnimeValue item, ref Sequence sq)
        {
            switch (item.FadeType)
            {
                case EnFadeType.Single:
                    sq.Insert(0, _imgThis.DOFade(item.Alpha, item.SpendTime));
                    break;
                case EnFadeType.Group:
                    if (TryGetComponent<CanvasGroup>(out CanvasGroup group))
                    {
                        sq.Insert(0, group.DOFade(item.Alpha, item.SpendTime));
                    }
                    else
                    {
                        Debug.LogError($"{gameObject.name}没有CanvasGroup组件");
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 添加至动画队列
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="liAnimes"></param>
        private void AddToSequence(ref Sequence sq, List<AnimeValue> liAnimes)
        {
            if (sq?.IsActive() == true)
                sq.Kill();
            sq = DOTween.Sequence();
            Vector2 newSizeDelta = Vector2.zero;
            Vector2 newScale = Vector2.zero;
            Vector2 newPos = Vector2.zero;
            float moveTime = -1;//用于修正动画的播放时间
            foreach (var item in liAnimes)
            {
                switch (item.AnimeType)
                {
                    case EnPanelAnimeType.Move:
                        newPos += (Vector2)OffsetPos(item);
                        if (moveTime == -1)
                        {
                            moveTime = item.SpendTime;
                        }
                        else if (moveTime > item.SpendTime)
                            moveTime = item.SpendTime;
                        break;
                    case EnPanelAnimeType.Scale:
                        newScale += (Vector2)item.Offset;
                        sq.Insert(0, transform.DOBlendableScaleBy(item.Offset, item.SpendTime));
                        break;
                    case EnPanelAnimeType.Rotate:
                        sq.Insert(0, transform.DOBlendableLocalRotateBy(item.Offset, item.SpendTime));
                        break;
                    case EnPanelAnimeType.Size:
                        Vector2 offsetSize = item.Offset;
                        Vector2 sizeDelta = _orignSizeDelta + offsetSize;
                        newSizeDelta += (Vector2)item.Offset;
                        sq.Insert(0, _rectTran.DOSizeDelta(sizeDelta, item.SpendTime));
                        break;
                    case EnPanelAnimeType.Fade:
                        FadeAnime(item, ref sq);
                        break;
                    default:
                        break;
                }
            }
            //真正位移距离 = 设定位移距离 + 尺寸增量 * |缩放比例|
            if (moveTime != -1)
            {
                Vector2 absScale = new Vector2(Mathf.Abs(newScale.x), Mathf.Abs(newScale.y));
                Vector2 truePos = newPos + absScale * newSizeDelta;
                Debug.Log($"修正后位移距离{truePos},原距离增量{newPos}");
                sq.Insert(0, transform.DOBlendableLocalMoveBy(truePos, moveTime));
            }
        }
        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            //IsShow = true;
            AddToSequence(ref _sqShowAnime, LiShowValues);
            _sqShowAnime.Play();
            _sqShowAnime.onComplete += () =>
            {
                OnShow.Invoke();
            };
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            //IsShow = false;
            AddToSequence(ref _sqHideAnime, LiHideValues);
            _sqHideAnime.Play();
            _sqHideAnime.onComplete += () =>
            {
                OnHide.Invoke();
            };
        }
        /// <summary>
        /// 播放显示/隐藏动画
        /// </summary>
        private void PlayAnime()
        {
            IsShow = !IsShow;
            PlayAnime(IsShow);
        }
        /// <summary>
        /// 播放显示/隐藏动画
        /// </summary>
        /// <param name="isShow">是否显示</param>
        public void PlayAnime(bool isShow)
        {
            switch (isShow)
            {
                case true:
                    Show();
                    break;
                default:
                    Hide();
                    break;
            }
        }
    }

}

