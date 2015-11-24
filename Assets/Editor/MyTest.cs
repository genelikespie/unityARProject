using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityTest
{
    [TestFixture]
    [Category("My Tests")]
    internal class MyTests : MonoBehaviour
    {
        [Test]
        [Category("Failing Tests")]
        public void SharedModeTest()
        {
            GameManager gameManager = GameManager.Instance();
            Assert.That(gameManager != null);
            Assert.That(gameManager.currentState == gameManager.mainMenuState);
            gameManager.mainMenuState.ToSharedModeMenu();
            Assert.That(gameManager.currentState == gameManager.sharedModeMenuState);
            GameObject.Find("SMM_PlanterNameInputField").GetComponent<InputField>().text = "Player1";
            GameObject.Find("SMM_DefuserNameInputField").GetComponent<InputField>().text = "Player2";
            gameManager.sharedModeMenuState.PlantBomb();
            Assert.That(gameManager.currentState == gameManager.plantBombState);
            gameManager.plantBombState.ArmBomb();
            gameManager.plantBombState.PassPhone();
            Assert.That(gameManager.currentState == gameManager.passingState);
            gameManager.passingState.DefuseBomb();
            Assert.That(gameManager.currentState == gameManager.defuseState);
            gameManager.defuseState.AllBombsDefused();
            Assert.That(gameManager.currentState == gameManager.gameOverState);
            Assert.That(gameManager.player.getPlayerOneWins());
        }

    }
}
