using System;
using System.Collections.Generic;
using Ichibankuji.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.Generators
{
    public partial class StubBoard : MonoBehaviour
    {
        [ShowInInspector]
        public BoardBlockForTitle TitleBlock;
        [ShowInInspector]
        public Dictionary<Level, BoardBlock> RareBlocks;
        [ShowInInspector]
        public Dictionary<Level, BoardBlock> NormalBlocks;

        public Transform TitleBlockTrans;
        public Transform RareBlockTrans;
        public Transform NormalBlockTrans;
        public Cinemachine.CinemachineVirtualCamera vCam;
        public float Width;
        public float Height;
        public float Spacing;

        public Action<Cinemachine.CinemachineVirtualCamera> Created;

        private void Awake(){
            TitleBlockTrans = transform.Find("TitleBlock");
            RareBlockTrans = transform.Find("RareBlock");
            NormalBlockTrans = transform.Find("NormalBlock");
            vCam = transform.Find("VCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        }

        public void AddStub(GameObject stubObject, Level level){
            if(RareBlocks.ContainsKey(level)){
                RareBlocks[level].AddStub(stubObject);
            }else{
                NormalBlocks[level].AddStub(stubObject);
            }
        }

        public void OnCreated() => Created.Invoke(vCam);
    }
}