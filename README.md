# unityARProject

1. Have Unity 5.2.1f1 (32 bit) for vuforia
2. Clone git repository
3. Open up project folder and open up the game bombgame.unity
4. Under Editor->Project Settings->Editor Settings->Version Control
  * Change mode to "visible meta files"
5. Go to Cs130 project on github and install vuforia unity 5-0-6.unitypackage 
  * or download unity 5-0-6 unity package on vuforia.com 

TO PLAYTEST GAME IN EDITOR:

6. Open the latest scene: Assets/Scenes/genes_playground
  * (this will have a little Unity symbol to the left of the file name)
7. Once the scene is done loading into the Unity Editor, hit the play button at the top.
  * NOTE: You must have a webcam installed for the game to work.

TO DEPLOY THE GAME IN ANDROID:

8. Under File->Build Settings choose Android as the Platform and click Build
9. Install the apk file in your phone

      
TO RUN AUTOMATED TESTS:

10. Download the "unity test tools" from the Asset store
  1. In the Editor, go to Window->Asset Store
  2. Search for "unity test tools"
  3. download (you need to log in to your unity account) and import the package
11. Our test script is in: Asset/Editor/MyTest.cs
12. On the toolbar at the top of the editor, open Unit Test Tools->Unit Test Runner
13. Run the game (click the play button)
14. In the Unit Test Runner window, right click MyTests->RunTest, and run the test (game must be running)
  * NOTE: All tests run as coroutines, therefore, the Unit Test window cannot return failures. **Test failures will print to the console instead!**
15. Brief description of the tests: we currently have four tests
    starting from 0s: defuser successfully defuse the bomb and win the shared mode
    starting from 25s: defuser successfully defuse the bomb and win the multishared mode (consists only one player)
    starting from 50s: defuser does not defuse the bomb and planter win the shared mode
    starting from 95s: defuser does not defuse the bomb and planter win the multishared mode (consists only one player)

TO EDIT GAME:

6. Create your own scene to play around in under Assets/Scenes
  * NOTE: You will be using your own scene for playtesting the features you add
7. Make sure you tell the team whenever you edit shared classes such as Gamestate
