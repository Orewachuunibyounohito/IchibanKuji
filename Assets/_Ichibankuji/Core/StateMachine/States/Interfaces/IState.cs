namespace Ichibankuji.Core.StateMachines
{
    public interface IState
    {
        void Enter();
        void FrameUpdate();
        void PhysicsUpdate();
        void LateUpdate();
        void Exit();
    }
}