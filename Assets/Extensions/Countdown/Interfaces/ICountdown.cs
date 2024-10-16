namespace ChuuniExtension.CountdownTools
{
    public interface ICountdown
    {
        float DeltaTime{ get; }
        bool TimesUp{ get; }

        void Update();
        void Reset();
    }

    public interface ICountdown_Coroutine
    {
        float DeltaTime{ get; }
        bool TimesUp{ get; }

        void Start();
        void Reset();
    }
}
