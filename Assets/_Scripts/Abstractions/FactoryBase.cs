namespace _Scripts.Abstractions
{
    public abstract class FactoryBase<T>
    {
        public abstract T Produce();
    }
    public abstract class FactoryBase<T,TP1>
    {
        public abstract T Produce(TP1 parameter);
    }
    public abstract class FactoryBase<T,TP1,TP2>
    {
        public abstract T Produce(TP1 parameter1,TP2 parameter2);
    }
    public abstract class FactoryBase<T,TP1,TP2,TP3>
    {
        public abstract T Produce(TP1 parameter1,TP2 parameter2,TP3 parameter3);
    }
}