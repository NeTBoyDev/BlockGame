using UnityEngine;

namespace _Scripts.Abstractions.Interfaces
{
    public interface ISaveService<T>
    {
        public void Save(T data);
        public T Load();
    }
    
    public interface ISaveService<T,P1>
    {
        public void Save(T data);
        public T Load(P1 parameter);
    }
    
    public interface ISaveService<T,P1,P2>
    {
        public void Save(T data);
        public T Load(P1 parameter1,P2 parameter2);
    }
}
