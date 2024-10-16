using System;
using System.Collections.Generic;
using ChuuniExtension;
using Ichibankuji.Core.RewardPools;
using Ichibankuji.Core.StateMachines;
using Ichibankuji.Core.UI;
using Ichibankuji.SO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Ichibankuji.Core
{
    public class GachaSystem : MonoBehaviour
    {
        private const float TEAR_OUT_MULTIPLIER = 0.00075f;
        private const float FOCUS_POSITION_MULTIPLIER = 1.75f;

        public static Transform TicketCollection;
        public static Vector3 FocusedPosition{
            get => Camera.main.transform.position + Camera.main.transform.forward * FOCUS_POSITION_MULTIPLIER;
        }
        public static Bound3D Bound;

        private static PoolConfig poolConfig;

        private RewardPool currentPool;
        private List<TMP_Dropdown.OptionData> options;

        [Title("Gameplay Config")]
        [SerializeField]
        [LabelText("Viewport Move")]
        public ViewportMoveModuleContent ViewportMoveModuleContent;

        private GameObject selectedTicket;
        private GameObject selectedStub;

        private bool isFocusMode = false;

        [SerializeField]
        private Bound3D startBound;
        [SerializeField]
        private Vector3 focusedPosition;
        [SerializeField]
        private GameObject focusMask;

        [SerializeField]
        private bool UseDevelopMode = false;
        [Title("DevelopMode"), Indent]
        [SerializeField]
        [Range(0.0001f, 0.005f)]
        [ShowIf("@UseDevelopMode")]
        private float tearOutMultiplier = TEAR_OUT_MULTIPLIER;
        [Indent]
        [SerializeField]
        [ShowIf("@UseDevelopMode")]
        private float autoDrawInterval = 0.2f;
        private DevelopModule developModule;

        [SerializeField]
        [Title("UI")]
        private GameObject MainPanel;

        [Title("Debug Info")]
        public GachaStateMachine stateMachine;

        public PoolConfig PoolConfig => poolConfig;
        public Bound3D StartBound => startBound;
        public GameObject SelectedTicket{
            get => selectedTicket;
            set => selectedTicket = value;
        }
        public GameObject SelectedStub{
            get => selectedStub;
            set => selectedStub = value;
        }
        public RewardPool CurrentPool{
            get => currentPool;
            set => currentPool = value;
        }
        public GameObject FocusMask => focusMask;
        public bool IsFocusMode => isFocusMode;
        public float AutoDrawInterval => autoDrawInterval;

        public Action OnPoolDone;
        public Action SystemReset;

        private void Awake(){
            poolConfig = Resources.Load<PoolConfig>(ResourcesPath.POOL_CONFIG);
            TicketCollection = GetComponent<CollectionManager>().TicketCollection;
            Bound = startBound;
            GenerateDropdownOptions(poolConfig);
            stateMachine = new GachaStateMachine(this);
            stateMachine.NormalState.SetTicketVCam(ViewportMoveModuleContent.TicketVCam);
            stateMachine.NormalState.ToTicket();

            #if UNITY_EDITOR
            if(UseDevelopMode){
                developModule = new DevelopModule(this);
            }else{
                developModule = null;
            }
            #endif
        }

        private void Start(){
            GenerateMainPanel();
            stateMachine.Initialize(stateMachine.NormalState);
        }

        private void Update() => stateMachine.FrameUpdate();
        private void FixedUpdate() => stateMachine.PhysicsUpdate();
        private void LateUpdate() => stateMachine.LateUpdate();

        private void OnEnable(){
            developModule?.Enable();
        }
        private void OnDisable(){
            developModule?.Disable();
        }
        
        private void GenerateMainPanel(){
            MainPanelPresenter presenter = MainPanelGenerator.GenerateMainPanelPresenterForGachaSystem(this, MainPanel, options);
            presenter.RegisterValueChangedEvent(GenerateRewardPool);
            presenter.RegisterTicketOrBoardClickedEvent(ActiveStubBoardSwitcher);
            presenter.RegisterPickByRandomClickedEvent(ComputerPickByRandom);
            presenter.RegisterPositionConfirmClickedEvent(ComputerPickByPosition(presenter));
        }

        private void GenerateDropdownOptions(PoolConfig poolConfig){
            options = new(){ new TMP_Dropdown.OptionData(){ text = "None" } };
            foreach (var pool in poolConfig.Pools){
                string poolName = pool.name.IndexOf("Pool") == -1 ? pool.name
                                                                : pool.name.Remove(pool.name.IndexOf("Pool"));
                options.Add(new TMP_Dropdown.OptionData{ text = poolName });
            }
        }

        private void GenerateRewardPool(int index){
            ResetSystem();

            int adjustIndex = index - 1;
            if(adjustIndex < 0){
                Debug.Log("Select None.");
                OnPoolDone.Invoke();
                return ;
            }
            PoolData poolData = poolConfig.Pools[adjustIndex];
            if(poolData.IsEmpty){
                Debug.Log("Pool is EMPTY!");
                OnPoolDone.Invoke();
                return ;
            }
            RewardPool pool = poolData.CreatePool(this);
            pool.OnDone += ActivatePoolButton;
            pool.DisplayTicketsWithDelayAndPosition(startBound, RewardPool.PositionGeneratedMode.Tidy);
            currentPool = pool;
        }

        private void ResetSystem(){
            selectedTicket = null;
            selectedStub = null;
            currentPool?.Destroy();
            currentPool = null;
            stateMachine.NormalState.ToTicket();
            stateMachine.NormalState.SetBoardVCam(null);
            SystemReset?.Invoke();
        }

        private void ActivatePoolButton() => OnPoolDone.Invoke();
        
        private void ActiveStubBoardSwitcher(){
            if(isFocusMode){ return ; }
            if(currentPool == null){ return ; }
            currentPool.ActiveStubBoardSwitcher();
            MainPanel.GetComponent<MainPanelPresenter>().TicketOrBoardSpriteSwitch();
            stateMachine.NormalState.SwitchView();
        }

        public void ComputerPickByRandom() => stateMachine.NormalState.PickByRandom(this);
        public UnityAction ComputerPickByPosition(MainPanelPresenter presenter) =>
            () => stateMachine.NormalState.PickByPosition(this, presenter.GetPosition());

        public float GetFocusContent() => UseDevelopMode? tearOutMultiplier : TEAR_OUT_MULTIPLIER;
    }
}