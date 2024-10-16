using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Ichibankuji.Core.RewardPools;
using Ichibankuji.SO;
using ChuuniExtension;

public class GachaTest
{
    private Reward reward1, reward2;
    private List<Reward> sample;
    private RewardPool onePiece;

    [SetUp]
    public void SetUp(){
        reward1 = new Reward(){
            Item = new Item{ Name = "One", Description = "" },
            Amount = 1
        };
        reward2 = new Reward(){
            Item = new Item{ Name = "Two", Description = "" },
            Amount = 10
        };
        sample = new List<Reward>{
            new Reward{
                Item = new Item{ Name = "A", Description = "UR" },
                Amount = 1
            },
            new Reward{
                Item = new Item{ Name = "B", Description = "SSR" },
                Amount = 10
            },
            new Reward{
                Item = new Item{ Name = "C", Description = "SR" },
                Amount = 15
            },
            new Reward{
                Item = new Item{ Name = "D", Description = "R" },
                Amount = 100
            },
            new Reward{
                Item = new Item{ Name = "E", Description = "N" },
                Amount = 500
            },
        };
        onePiece = Resources.Load<PoolConfig>(ResourcesPath.POOL_CONFIG)
                            .Pools
                            .Find((pool) => pool.name == "OnePiecePool")
                            .CreatePool(null);
    }

    [Category("Gacha/Reward")]
    [Test]
    public void GivenReward1AndReward2WhenName1EqualsName2ThenReward1EqualsReward2(){
        reward1.Item.Name = "One";
        reward2.Item.Name = "One";

        Assert.IsTrue(reward1.Equals(reward2));
    }

    [Category("Gacha/Reward")]
    [Test]
    public void CreateRewardPoolAndPrint(){
        RewardPool pool = RewardPool.CreatePool("sample", sample);

        string actual = pool.PrintRewards();
        string expected = string.Join(", ", sample);

        Assert.AreEqual(expected, actual);
    }


    [Category("Gacha/Reward")]
    [Test]
    public void RuffiSOGenerateItemEqualsItemNameIsRuffi(){
        ItemDataConfig itemConfig = Resources.Load<ItemDataConfig>("Test/Configs/Items");
        ItemData ruffiData = itemConfig.Items.Find((itemContent) => itemContent.Item.Name == "Ruffi").Item;

        Item actual = ruffiData.GenerateItem();
        Item expected = new Item{ Name = "Ruffi" };

        Assert.AreEqual(expected, actual);
    }

    [Category("Gacha/Pool")]
    [Test]
    public void OnePiecePoolGenerateItemsThenCountEqualsSumOfAmount(){
        // int randomSeed = 10;
        List<Item> items = onePiece.GenerateItems();

        int actual = items.Count;
        int expected = onePiece.TotalAmount;

        Assert.AreEqual(expected, actual);
    }

    // Cannot pass, non-fixed random
    [Category("Gacha/Pool")]
    [Test]
    public void OnePiecePoolShuffle(){
        List<Item> onePieceItems = onePiece.GenerateItems();
        List<Item> newItems = new(onePieceItems);
        onePiece.GenerateItems();
        onePiece.Shuffle();

        string actual = onePiece.PrintItems();
        string expected = string.Join(", ", newItems);
        Debug.Log(actual);

        Assert.AreEqual(expected, actual);
    }

    [Category("Gacha/Pool")]
    [Test]
    public void OnePiecePoolFixedShuffle(){
        int swapCount = 500;
        int randomSeed = 10;
        List<Item> onePieceItems = onePiece.GenerateItems();
        List<Item> newItems = new(onePieceItems);
        onePiece.FixedShuffle(swapCount, randomSeed);
        newItems.RandomSwap(swapCount, randomSeed);


        string actual = onePiece.PrintItems();
        string expected = string.Join(", ", newItems);

        Assert.AreEqual(expected, actual);
        // Assert.AreEqual(newItems.GetHashCode(), onePieceItems.GetHashCode());
    }
}