namespace _Scripts.Abstractions.Interfaces
{
    public interface IBlockTrashService
    {
        public void Trash(BlockPresenterBase block);
        public bool MayTrash(BlockPresenterBase block);
    }
}
