using System;

namespace Ichibankuji.Core.StateMachines
{
    [Serializable]
    public class GachaStateMachine : StateMachine
    {
        public NormalState NormalState;
        public FocusState FocusState;

        public GachaStateMachine(GachaSystem system){
            NormalState = new NormalState(this, system);
            FocusState = new FocusState(this, system);

            NormalState.SetFocusState(FocusState);
            FocusState.SetNormalState(NormalState);
        }
    }
}