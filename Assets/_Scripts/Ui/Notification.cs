using Cysharp.Threading.Tasks;
using DG.Tweening;
using DI;
using TMPro;
using UnityEngine;

namespace _Scripts.Ui
{
    public class Notification
    {
        private TMP_Text _messageText;
        private CanvasGroup _group;
        private Sequence _showSequence;
        private Sequence _hideSequence;
        
        [Inject]
        private void Inject(CanvasGroup group)
        {
            _group = group;
            _messageText = group.GetComponentInChildren<TMP_Text>();
        }

        public async UniTask Show(string message)
        {
            await HideNotification();
            SetMessage(message);
            FadeInOut();
        }
    
        private void SetMessage(string message)
        {
            _messageText.text = message;
        }

        private void FadeInOut()
        {
            _showSequence.Kill();
            var fadeIn = DOTween.To(() => _group.alpha, x => _group.alpha = x, 1, .5f);
            var fadeOut = DOTween.To(() => _group.alpha, x => _group.alpha = x, 0, .5f);
            _showSequence = DOTween.Sequence().Append(fadeIn).AppendInterval(1).Append(fadeOut);
        }

        private async UniTask HideNotification()
        {
            var hide = DOTween.To(() => _group.alpha, x => _group.alpha = x, 0, .35f);
            _showSequence.Kill();
            _hideSequence = DOTween.Sequence(hide);
            await _hideSequence.AsyncWaitForCompletion().AsUniTask();
        }
    }
}