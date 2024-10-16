using System.Collections.Generic;
using System.Linq;
using Ichibankuji.Core;
using Ichibankuji.Core.RewardPools;
using Ichibankuji.TransferStructure;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ichibankuji.SO
{
    [CreateAssetMenu(menuName = "Ichibankuji/Data/Pool", fileName = "New Pool Data")]
    public class PoolData : ScriptableObject
    {
        public List<RewardData> Rewards;

        [SerializeField, ReadOnly]
        private int totalAmount;

        public bool IsEmpty => totalAmount == 0;

        public RewardPool CreatePool(GachaSystem gachaSystem){
            List<Reward> rewards = new();
            Rewards.Aggregate(rewards, (rewards, rewardData) => {
                rewards.Add(rewardData.GenerateReward());
                return rewards;
            });
            string poolName = name.IndexOf("Pool") != -1? name.Remove(name.LastIndexOf("Pool"))
                                                        : name;
            Debug.Log($"PoolData.name: {name}");
            return RewardPool.CreatePoolAndTicketsAndItems(gachaSystem, poolName, rewards);
        }

        private void OnValidate(){
            if(Rewards.Count != 0){
                totalAmount = Rewards.Sum((reward) => reward.Amount);
            }else{
                totalAmount = 0;
            }
        }
    }
}