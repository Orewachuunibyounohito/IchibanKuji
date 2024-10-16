namespace Ichibankuji.Core.StateMachines
{
    public class StateMachine
    {
        public IState currentState;

        public virtual void Initialize(IState initialState){
            currentState = initialState;
            currentState.Enter();
        }
        public virtual void FrameUpdate() => currentState.FrameUpdate();

        public virtual void PhysicsUpdate() => currentState.PhysicsUpdate();
        public virtual void LateUpdate() => currentState.LateUpdate();
        public virtual void ChangeState(IState nextState){
            currentState.Exit();
            currentState = nextState;
            currentState.Enter();
        }
    }
}