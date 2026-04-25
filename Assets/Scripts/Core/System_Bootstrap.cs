using UnityEngine;
using UnityEngine.UI;
using StickmanBrainrot.Player;
using StickmanBrainrot.Systems;
using StickmanBrainrot.UI;
using StickmanBrainrot.Events;
using TMPro;

namespace StickmanBrainrot.Core
{
    /// <summary>
    /// Automatically builds the full scene and UI for testing when Play is pressed.
    /// </summary>
    public class System_Bootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void AutoBuild()
        {
            // Only build if we don't already have managers
            if (Object.FindAnyObjectByType<System_GameManager>() != null)
            {
                Debug.Log("BOOTSTRAP: Game already built, skipping auto-build.");
                return;
            }

            GameObject bootstrapObj = new GameObject("Bootstrap_Builder");
            var builder = bootstrapObj.AddComponent<System_Bootstrap>();
            builder.BuildWorld();
        }

        public void BuildWorld()
        {
            Debug.Log("BOOTSTRAP: Building the complete Brainrot world...");

            // 0. Setup Lighting
            Light dirLight = Object.FindAnyObjectByType<Light>();
            if (dirLight == null)
            {
                GameObject lightGO = new GameObject("Directional Light");
                Light l = lightGO.AddComponent<Light>();
                l.type = LightType.Directional;
                lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            }

            // 1. Create Ground
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "FLOOR";
            floor.transform.localScale = new Vector3(3, 1, 100);
            floor.transform.position = new Vector3(0, 0, 500);
            floor.AddComponent<System_EndlessFloor>();

            // Give floor a dark grey material
            Renderer floorRend = floor.GetComponent<Renderer>();
            Material floorMat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            floorMat.color = Color.gray;
            floorRend.material = floorMat;

            // 2. Create Generated Prefabs (Coins & Obstacles)
            GameObject coinPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            coinPrefab.name = "Coin_Prefab";
            coinPrefab.tag = "Coin";
            coinPrefab.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            Renderer coinRend = coinPrefab.GetComponent<Renderer>();
            Material coinMat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            coinMat.color = Color.yellow;
            coinRend.material = coinMat;
            Collider coinCol = coinPrefab.GetComponent<Collider>();
            coinCol.isTrigger = true;
            coinPrefab.SetActive(false); // Hide the prefab

            GameObject obsPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obsPrefab.name = "Obstacle_Prefab";
            obsPrefab.tag = "Obstacle";
            obsPrefab.transform.localScale = new Vector3(2f, 2f, 2f);
            Renderer obsRend = obsPrefab.GetComponent<Renderer>();
            Material obsMat = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            obsMat.color = Color.red;
            obsRend.material = obsMat;
            Collider obsCol = obsPrefab.GetComponent<Collider>();
            obsCol.isTrigger = true;
            obsPrefab.AddComponent<System_Obstacle>();
            obsPrefab.SetActive(false);

            // 3. Create Player
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
            player.name = "PLAYER_CUBE";
            player.tag = "Player";
            player.transform.position = new Vector3(0, 1, 0);
            
            player.AddComponent<Player_Controller>();
            player.AddComponent<Player_SwipeHandler>();
            player.AddComponent<System_Collision>();
            
            // Re-bind Input actions
            var pInput = player.AddComponent<UnityEngine.InputSystem.PlayerInput>();
            pInput.actions = Resources.Load<UnityEngine.InputSystem.InputActionAsset>("PlayerInputActions"); 
            
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            // 4. Create Managers
            GameObject managers = new GameObject("_MANAGERS");
            managers.AddComponent<System_GameManager>();
            managers.AddComponent<System_ScoreManager>();
            
            var spawnManager = managers.AddComponent<System_SpawnManager>();
            // Inject prefabs via reflection to bypass private serialized fields
            var obsArray = new GameObject[] { obsPrefab };
            typeof(System_SpawnManager).GetField("obstaclePrefabs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(spawnManager, obsArray);
            typeof(System_SpawnManager).GetField("coinPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(spawnManager, coinPrefab);
            typeof(System_SpawnManager).GetField("playerTransform", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(spawnManager, player.transform);

            // Events
            managers.AddComponent<System_EventManager>();
            managers.AddComponent<Event_CameraShake>();
            managers.AddComponent<Event_SpeedBurst>();
            managers.AddComponent<Event_TextSpam>();
            managers.AddComponent<Event_FakeAd>();

            // 5. Build UI Canvas
            GameObject canvasGO = new GameObject("MainCanvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();

            // HUD
            GameObject hudGO = new GameObject("HUD");
            hudGO.transform.SetParent(canvasGO.transform, false);
            UI_HUD hud = hudGO.AddComponent<UI_HUD>();

            GameObject scoreGO = new GameObject("ScoreText");
            scoreGO.transform.SetParent(hudGO.transform, false);
            var scoreTmp = scoreGO.AddComponent<TextMeshProUGUI>();
            scoreTmp.text = "SCORE: 00000";
            scoreTmp.fontSize = 40;
            scoreTmp.alignment = TextAlignmentOptions.TopLeft;
            var scoreRect = scoreGO.GetComponent<RectTransform>();
            scoreRect.anchorMin = new Vector2(0, 1);
            scoreRect.anchorMax = new Vector2(0, 1);
            scoreRect.pivot = new Vector2(0, 1);
            scoreRect.anchoredPosition = new Vector2(20, -20);

            GameObject coinGO = new GameObject("CoinText");
            coinGO.transform.SetParent(hudGO.transform, false);
            var coinTmp = coinGO.AddComponent<TextMeshProUGUI>();
            coinTmp.text = "COINS: 0";
            coinTmp.fontSize = 40;
            coinTmp.color = Color.yellow;
            coinTmp.alignment = TextAlignmentOptions.TopRight;
            var coinRect = coinGO.GetComponent<RectTransform>();
            coinRect.anchorMin = new Vector2(1, 1);
            coinRect.anchorMax = new Vector2(1, 1);
            coinRect.pivot = new Vector2(1, 1);
            coinRect.anchoredPosition = new Vector2(-20, -20);

            typeof(UI_HUD).GetField("scoreText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(hud, scoreTmp);
            typeof(UI_HUD).GetField("coinText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(hud, coinTmp);

            // Fake Ad
            GameObject fakeAdGO = new GameObject("UI_FakeAd");
            fakeAdGO.transform.SetParent(canvasGO.transform, false);
            UI_FakeAd fakeAdScript = fakeAdGO.AddComponent<UI_FakeAd>();
            
            GameObject adPanel = new GameObject("AdPanel");
            adPanel.transform.SetParent(fakeAdGO.transform, false);
            var adImg = adPanel.AddComponent<Image>();
            adImg.color = Color.magenta; // Obnoxious color
            var adRect = adPanel.GetComponent<RectTransform>();
            adRect.anchorMin = new Vector2(0.1f, 0.1f);
            adRect.anchorMax = new Vector2(0.9f, 0.9f);

            GameObject adTextGO = new GameObject("AdText");
            adTextGO.transform.SetParent(adPanel.transform, false);
            var adText = adTextGO.AddComponent<TextMeshProUGUI>();
            adText.text = "DOWNLOAD NOW!!! \n FREE ROBUX!!!";
            adText.fontSize = 80;
            adText.color = Color.yellow;
            adText.alignment = TextAlignmentOptions.Center;
            var adTextRect = adTextGO.GetComponent<RectTransform>();
            adTextRect.anchorMin = Vector2.zero;
            adTextRect.anchorMax = Vector2.one;
            adTextRect.sizeDelta = Vector2.zero;

            typeof(UI_FakeAd).GetField("adContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(fakeAdScript, adPanel);

            // Text Spam
            GameObject textSpamGO = new GameObject("UI_TextSpam");
            textSpamGO.transform.SetParent(canvasGO.transform, false);
            UI_TextSpam textSpamScript = textSpamGO.AddComponent<UI_TextSpam>();
            
            GameObject spamPrefab = new GameObject("SpamTextPrefab");
            var spamTmp = spamPrefab.AddComponent<TextMeshProUGUI>();
            spamTmp.fontSize = 60;
            spamTmp.alignment = TextAlignmentOptions.Center;
            spamTmp.outlineWidth = 0.2f;
            spamPrefab.SetActive(false);

            typeof(UI_TextSpam).GetField("textPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(textSpamScript, spamPrefab);
            typeof(UI_TextSpam).GetField("canvasTransform", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(textSpamScript, canvasGO.transform);

            // Game Over
            GameObject gameOverGO = new GameObject("UI_GameOver");
            gameOverGO.transform.SetParent(canvasGO.transform, false);
            UI_GameOver gameOverScript = gameOverGO.AddComponent<UI_GameOver>();
            
            GameObject goPanel = new GameObject("GameOverPanel");
            goPanel.transform.SetParent(gameOverGO.transform, false);
            var goImg = goPanel.AddComponent<Image>();
            goImg.color = new Color(0, 0, 0, 0.8f);
            var goRect = goPanel.GetComponent<RectTransform>();
            goRect.anchorMin = Vector2.zero;
            goRect.anchorMax = Vector2.one;

            GameObject goTitle = new GameObject("Title");
            goTitle.transform.SetParent(goPanel.transform, false);
            var goTitleTmp = goTitle.AddComponent<TextMeshProUGUI>();
            goTitleTmp.text = "GAME OVER";
            goTitleTmp.fontSize = 100;
            goTitleTmp.color = Color.red;
            goTitleTmp.alignment = TextAlignmentOptions.Center;
            var titleRect = goTitle.GetComponent<RectTransform>();
            titleRect.anchoredPosition = new Vector2(0, 200);

            GameObject goScore = new GameObject("FinalScore");
            goScore.transform.SetParent(goPanel.transform, false);
            var goScoreTmp = goScore.AddComponent<TextMeshProUGUI>();
            goScoreTmp.fontSize = 60;
            goScoreTmp.alignment = TextAlignmentOptions.Center;
            var scoreFinalRect = goScore.GetComponent<RectTransform>();
            scoreFinalRect.anchoredPosition = new Vector2(0, 50);

            GameObject goCoins = new GameObject("FinalCoins");
            goCoins.transform.SetParent(goPanel.transform, false);
            var goCoinsTmp = goCoins.AddComponent<TextMeshProUGUI>();
            goCoinsTmp.fontSize = 60;
            goCoinsTmp.alignment = TextAlignmentOptions.Center;
            var coinsFinalRect = goCoins.GetComponent<RectTransform>();
            coinsFinalRect.anchoredPosition = new Vector2(0, -50);

            typeof(UI_GameOver).GetField("gameOverPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameOverScript, goPanel);
            typeof(UI_GameOver).GetField("finalScoreText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameOverScript, goScoreTmp);
            typeof(UI_GameOver).GetField("finalCoinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameOverScript, goCoinsTmp);

            // Create Event System
            if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject es = new GameObject("EventSystem");
                es.AddComponent<UnityEngine.EventSystems.EventSystem>();
                es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            // 6. Setup Camera
            Camera mainCam = Camera.main;
            if (mainCam == null)
            {
                GameObject camGO = new GameObject("Main Camera");
                mainCam = camGO.AddComponent<Camera>();
                camGO.tag = "MainCamera";
            }
            var follow = mainCam.gameObject.AddComponent<System_CameraFollow>();
            follow.SetTarget(player.transform);

            Debug.Log("BOOTSTRAP COMPLETE: Full UI, Managers, Prefabs, and Player are configured!");
            Destroy(this.gameObject); // Cleanup bootstrap
        }
    }
}
