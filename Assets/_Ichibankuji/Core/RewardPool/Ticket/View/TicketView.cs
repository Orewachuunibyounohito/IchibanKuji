using System;
using System.Collections.Generic;
using System.Linq;
using ChuuniExtension;
using Ichibankuji.SO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ichibankuji.Core.Tickets
{
    [Serializable]
    public class TicketView
    {
        private const string STUB_MATERIAL_OBJECT_PATH = "Stub/StubObject";

        public static GameObject TicketPrefab;

        private static Dictionary<Level, Material> StubLevelMap;
        private static Vector3 explostionOffset => new Bound3D(1f, -1f,
                                                               0.75f, 0.75f,
                                                               1f, -1f).GenerateInsidePositonByRandom();
        private static Vector2Int EmptyPosition => new Vector2Int(-1, -1);

        private GameObject ticketObject;
        private Vector2Int slotPosition;

        private Rigidbody rb => ticketObject.GetComponent<Rigidbody>();

        public GameObject gameObject => ticketObject;

        static TicketView(){
            TicketPrefab = Resources.Load<PrefabsConfig>(ResourcesPath.PREFABS_CONFIG).Ticket;
            var StubMaterials = Resources.Load<MaterialsConfig>(ResourcesPath.MATERIALS_CONFIG).StubMaterials;
            StubLevelMap = new();
            StubLevelMap = StubMaterials.Aggregate(StubLevelMap, (StubLevelMap, content) => {
                StubLevelMap.Add(content.Level, content.Material);
                return StubLevelMap;
            });
        }
        public TicketView(GameObject ticketObject, Level level){
            this.ticketObject = ticketObject; 
            var stubObject = this.ticketObject.transform.Find(STUB_MATERIAL_OBJECT_PATH);
            if(!StubLevelMap.ContainsKey(level)){ Debug.LogWarning($"No [{level}] material in Material Config."); }
            stubObject.GetComponent<MeshRenderer>().material = StubLevelMap[level];
            slotPosition = EmptyPosition;
        }

        public void DisplayView() => ticketObject.SetActive(true);
        public void HideView() => ticketObject.SetActive(false);
        public void ToFocusMode(Vector3 newPosition){
            SetPosition(newPosition);
            RotationReset();
            ForceAndTorqueZero();
            rb.useGravity = false;
        }

        private void ForceAndTorqueZero(){
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void ExitFocusMode(){
            rb.useGravity = true;
            if (slotPosition != EmptyPosition)
            {
                var spacing = new Vector2(0.95f, 0.75f);
                var leftUpPosition = new Vector3(-4.35f, 0, 3.85f);
                var position = leftUpPosition + new Vector3(spacing.x * slotPosition.x, 0, -spacing.y * slotPosition.y);
                ticketObject.transform.position = position;
            }else{
                rb.AddExplosionForce(rb.mass*50, rb.transform.position+explostionOffset, 1);
            }
        }

        public void SetSlotPosition(Vector2Int newPosition) => slotPosition = newPosition;
        public void SetPosition(Vector3 newPosition){
            ticketObject.transform.position = newPosition;
        }
        public void RotationReset() => ticketObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}