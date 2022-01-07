using Core.Managers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManagerTest
{
    private Mock<ISpawner> mockSpawner;
    private Mock<IBattleSystemUtils> mockBattleSystemUtils;
    private Mock<IDeckManager> mockDeckManager;
    private GameObject playerHand;
    private Mock<IMinionBehaviour> mockMinionBehaviour;
    readonly float scaleFactor = 0.75f;
    readonly float separationFactor = 0.05f;

    private CardManager cardManager;

    [SetUp]
    public void SetUp()
    {
        mockSpawner = new Mock<ISpawner>(MockBehavior.Strict);
        mockBattleSystemUtils = new Mock<IBattleSystemUtils>(MockBehavior.Strict);
        mockDeckManager = new Mock<IDeckManager>(MockBehavior.Strict);
        playerHand = new GameObject();
        mockMinionBehaviour = new Mock<IMinionBehaviour>(MockBehavior.Strict);

        cardManager = null;

        MockFunctionSetup();
    }

    [TearDown]
    public void TearDown()
    {
        cardManager = null;

        mockMinionBehaviour = null;
        playerHand = null;
        mockDeckManager = null;
        mockBattleSystemUtils = null;
        mockSpawner = null;
    }

    private void MockFunctionSetup()
    {
        GameObject spawnedCard = new GameObject();
        MinionCardData cardData = ScriptableObject.CreateInstance<MinionCardData>();


        mockDeckManager.Setup(mock => mock.DrawCard())
                       .Returns(cardData);

        mockSpawner.Setup(mock => mock.SpawnCard(It.IsAny<MinionCardData>()))
                   .Returns(spawnedCard);
        
        mockBattleSystemUtils.Setup(mock => mock.GetMinionBehaviour(It.IsAny<GameObject>()))
                             .Returns(mockMinionBehaviour.Object);

        mockMinionBehaviour.Setup(mock => mock.SetInitialPosition(It.IsAny<Vector3>()));

        mockMinionBehaviour.Setup(mock => mock.LerpToInitialPosition(It.IsAny<Action>())).Callback<Action>(param => param());
    }

    private void DrawSetup()
    {
        List<GameObject> children = new List<GameObject> { new GameObject(), new GameObject(), new GameObject(), new GameObject() };

        mockBattleSystemUtils.Setup(mock => mock.GetChildren(It.IsAny<GameObject>())).Returns(children);
    }

    [Test]
    public void CallInstantiate_ExpectNoErrors()
    {

        cardManager = new CardManager(mockSpawner.Object,
                                      mockBattleSystemUtils.Object,
                                      mockDeckManager.Object,
                                      playerHand,
                                      scaleFactor,
                                      separationFactor);

        cardManager.InstantiateCards();

        mockDeckManager.Verify(mock => mock.DrawCard(), Times.Exactly(3));
        mockSpawner.Verify(mock => mock.SpawnCard(It.IsAny<MinionCardData>()), Times.Exactly(3));
        MinionBehaviourExpectations(3);
    }

    [Test]
    public void CallDraw_ExpectNoErrors()
    {
        DrawSetup();

        cardManager = new CardManager(mockSpawner.Object,
                                      mockBattleSystemUtils.Object,
                                      mockDeckManager.Object,
                                      playerHand,
                                      scaleFactor,
                                      separationFactor);

        cardManager.InstantiateCards();
        cardManager.Draw();

        mockBattleSystemUtils.Verify(mock => mock.GetChildren(It.IsAny<GameObject>()), Times.Once());
        mockSpawner.Verify(mock => mock.SpawnCard(It.IsAny<MinionCardData>()), Times.Exactly(4));
        MinionBehaviourExpectations(7);
    }

    private void MinionBehaviourExpectations(int callCount)
    {
        mockBattleSystemUtils.Verify(mock => mock.GetMinionBehaviour(It.IsAny<GameObject>()), Times.Exactly(callCount));
        mockMinionBehaviour.Verify(mock => mock.SetInitialPosition(It.IsAny<Vector3>()), Times.Exactly(callCount));
        mockMinionBehaviour.Verify(mock => mock.LerpToInitialPosition(It.IsAny<Action>()), Times.Exactly(callCount));
    }
}
