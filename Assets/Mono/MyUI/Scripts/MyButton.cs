using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
namespace MyUI
{
    public class MyButton : UIBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [Serializable]
        public class AnimeValue
        {
            public EnButtonAnimeType EnAnimeType;

            public Vector3 NewPosition;
            public Vector3 NewScale;
            public Vector3 NewAngle;
            public float SpendTime = 0.3f;
            //描边
            public float BorderSize = 5f;
            public Color BorderColor = Color.white;
            public bool UseGraphicAlpha;
        }
        public AnimeValue MouseEnterValue;
        public AnimeValue MouseClickValue;
        public AnimeValue MouseExitValue;

        public UnityEvent OnClick = new UnityEvent();
        public UnityEvent OnEnter = new UnityEvent();
        public UnityEvent OnExit = new UnityEvent();

        private Tween _clickAnime;
        private Tween _enterAnime;
        private Tween _exitAnime;
        private Sequence _sqResetAnime;
        private Sequence _sqClickAnime;
        private Vector3 _orignLocalPosition;
        private Vector3 _orignLocalScale;
        private Vector3 _orignLocalAngle;
        #region 生命周期
        protected override void OnDisable()
        {
            base.OnDisable();
            if (_sqResetAnime?.IsActive() == true)
                _sqResetAnime.Kill();
            if (_sqClickAnime?.IsActive() == true)
                _sqClickAnime.Kill();
            if (_enterAnime?.IsActive() == true)
                _enterAnime.Kill();
            if (_exitAnime?.IsActive() == true)
                _exitAnime.Kill();

        }
        protected override void Start()
        {
            base.Start();
            _orignLocalPosition = transform.localPosition;
            _orignLocalScale = transform.localScale;
            _orignLocalAngle = transform.localEulerAngles;
        }
        #endregion

        #region 接口实现
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
            SetAnime(MouseClickValue, ref _clickAnime);

            _sqClickAnime = DOTween.Sequence();
            _sqClickAnime.Append(_clickAnime);
            //复原
            switch (MouseClickValue.EnAnimeType)
            {
                case EnButtonAnimeType.Scale:
                    _sqClickAnime.Append(transform.DOScale(_orignLocalScale, MouseClickValue.SpendTime));
                    break;
                case EnButtonAnimeType.Move:
                    _sqClickAnime.Append(transform.DOLocalMove(_orignLocalPosition, MouseClickValue.SpendTime));
                    break;
                case EnButtonAnimeType.Rotate:
                    _sqClickAnime.Append(transform.DOLocalRotate(_orignLocalAngle, MouseClickValue.SpendTime));
                    break;
                default:
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke();
            SetAnime(MouseEnterValue, ref _enterAnime);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit.Invoke();
            ResetObject();

        }
        #endregion


        private void ResetObject()
        {
            if (_sqResetAnime?.IsActive() == true)
                _sqResetAnime.Kill();
            _sqResetAnime = DOTween.Sequence();
            _sqResetAnime.Append(transform.DOLocalMove(_orignLocalPosition, 0.3f));
            _sqResetAnime.Insert(0, transform.DOLocalRotate(_orignLocalAngle, 0.3f));
            _sqResetAnime.Insert(0, transform.DOScale(_orignLocalScale, 0.3f));
            _sqResetAnime.Play();
            _sqResetAnime.onKill += () =>
            {
                transform.localPosition = _orignLocalPosition;
                transform.localEulerAngles = _orignLocalAngle;
                transform.localScale = _orignLocalScale;
            };
            if (TryGetComponent<ModelOutline>(out ModelOutline line))
            {
                line.enabled = false;
            }
        }
        private void SetAnime(AnimeValue newValue,ref Tween anime)
        {
            if (anime?.IsActive() == true)
                anime.Kill();
            switch (newValue.EnAnimeType)
            {
                case EnButtonAnimeType.None:
                    break;
                case EnButtonAnimeType.Scale:
                    anime = transform.DOScale(newValue.NewScale, newValue.SpendTime);
                    break;
                case EnButtonAnimeType.Move:
                    anime = transform.DOBlendableMoveBy(newValue.NewPosition, newValue.SpendTime);
                    break;
                case EnButtonAnimeType.Rotate:
                    anime = transform.DOBlendableRotateBy(newValue.NewAngle, newValue.SpendTime);
                    break;
                case EnButtonAnimeType.Outline:
                    if (!TryGetComponent<Outline>(out Outline line))
                    {
                        line = gameObject.AddComponent<Outline>();
                    }
                    line.enabled = true;
                    line.effectColor = newValue.BorderColor;
                    line.effectDistance = new Vector2(newValue.BorderSize, -newValue.BorderSize); 
                    line.useGraphicAlpha = newValue.UseGraphicAlpha;
                    break;
                default:
                    break;
            }
        }
    }

    
}
