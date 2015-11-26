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
        public void RunTest()
        {
            Debug.Log("Running Failing Tests, All failures should return an error");
            Debug.Log("All unit tests should return positive (since we are running in a coroutine)");

            GameManager gameManager = GameManager.Instance();
            Assert.That(gameManager != null);
            gameManager.StartCoroutine(SharedModeTest());
            gameManager.StartCoroutine(SharedModeTestPlanterWins());
        }

        IEnumerator SharedModeTest()
        {
            Debug.Log("Running SharedModeTest: Defuser wins");
            GameManager gameManager = GameManager.Instance();
            Assert.That(gameManager != null);
            Assert.That(gameManager.mainMenuState != null);
            Assert.That(gameManager.currentState == gameManager.mainMenuState);
            gameManager.mainMenuState.ToSharedModeMenu();
            Assert.That(gameManager.currentState == gameManager.sharedModeMenuState);
            GameObject.Find("SMM_PlanterNameInputField").GetComponent<InputField>().text = "Player1";
            GameObject.Find("SMM_DefuserNameInputField").GetComponent<InputField>().text = "Player2";
            gameManager.sharedModeMenuState.PlantBomb();
            Assert.That(gameManager.currentState == gameManager.plantBombState);
            gameManager.plantBombState.ArmBomb();

            // Wait for bomb to plant
            yield return new WaitForSeconds(6f);

            gameManager.plantBombState.PassPhone();
            Assert.That(gameManager.currentState == gameManager.passingState);
            gameManager.passingState.DefuseBomb();
            Assert.That(gameManager.currentState == gameManager.defuseState);
            gameManager.defuseState.AllBombsDefused();

            // Wait for the other coroutine in DefuseState
            yield return new WaitForSeconds(6f);

            Assert.That(gameManager.currentState == gameManager.gameOverState);
            Assert.That(!gameManager.player.getPlayerOneWins());
            gameManager.currentState.ToMainMenu();
            Debug.Log("Finished SharedModeTest: Defuser wins");
        }

        IEnumerator SharedModeTestPlanterWins()
        {
            yield return new WaitForSeconds(15f);
            Debug.Log("Running SharedModeTest: Defuser wins");
            GameManager gameManager = GameManager.Instance();
            Assert.That(gameManager != null);
            Assert.That(gameManager.mainMenuState != null);
            Assert.That(gameManager.currentState == gameManager.mainMenuState);
            gameManager.mainMenuState.ToSharedModeMenu();
            Assert.That(gameManager.currentState == gameManager.sharedModeMenuState);
            GameObject.Find("SMM_PlanterNameInputField").GetComponent<InputField>().text = "Player1";
            GameObject.Find("SMM_DefuserNameInputField").GetComponent<InputField>().text = "Player2";
            gameManager.sharedModeMenuState.PlantBomb();
            Assert.That(gameManager.currentState == gameManager.plantBombState);
            gameManager.plantBombState.ArmBomb();

            // Wait for bomb to plant
            yield return new WaitForSeconds(6f);

            gameManager.plantBombState.PassPhone();
            Assert.That(gameManager.currentState == gameManager.passingState);
            gameManager.passingState.DefuseBomb();
            Assert.That(gameManager.currentState == gameManager.defuseState);

            // Wait for the other coroutine in DefuseState
            yield return new WaitForSeconds(31f);

            Assert.That(gameManager.currentState == gameManager.gameOverState);
            Assert.That(gameManager.player.getPlayerOneWins());
            gameManager.currentState.ToMainMenu();
            Debug.Log("Finished SharedModeTest: Planter wins");
        }
    }
}
