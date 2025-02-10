using _Scripts.Abstractions;

namespace _Scripts.Entities.Block
{
    public class BlockPresenter : BlockPresenterBase
    {
        public override void DestroyBlock()
        {
            Play(Model.DestroyClip);
            var viewTween = View.DestroyBlock();
            viewTween.onComplete += () => base.DestroyBlock();
        }
    }
}
