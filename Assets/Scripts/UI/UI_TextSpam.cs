using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace StickmanBrainrot.UI
{
    /// <summary>
    /// Spawns random text popups on the screen during the "TEXT SPAM" event.
    /// </summary>
    public class UI_TextSpam : MonoBehaviour
    {
        [Header("Text Spam Settings")]
        [SerializeField] private GameObject textPrefab; // A UI TextMeshPro prefab.
        [SerializeField] private Transform canvasTransform;
        [SerializeField] private string[] randomPhrases = new string[] 
        {
            "bro what are you doing",
            "this run is trash",
            "touch grass",
            "skill issue",
            "L + ratio",
            "who is playing this rn??",
            "bruh 💀",
            "is this real chat?",
            "my brain is melting"
        };

        private bool isSpamming = false;
        private List<GameObject> activeTexts = new List<GameObject>();

        private void Start()
        {
            if (canvasTransform == null)
            {
                // Try finding the main HUD canvas
                UI_HUD hud = Object.FindAnyObjectByType<UI_HUD>();
                if (hud != null) canvasTransform = hud.transform;
            }
        }

        public void StartSpam(float duration)
        {
            if (isSpamming) return;
            isSpamming = true;
            StartCoroutine(SpamRoutine(duration));
        }

        public void StopSpam()
        {
            isSpamming = false;
            // Clean up existing texts
            foreach(var txt in activeTexts)
            {
                if (txt != null) Destroy(txt);
            }
            activeTexts.Clear();
        }

        private IEnumerator SpamRoutine(float duration)
        {
            float endTime = Time.time + duration;
            while (Time.time < endTime && isSpamming)
            {
                if (textPrefab != null && canvasTransform != null)
                {
                    SpawnRandomText();
                }
                else
                {
                    // Fallback to simple logging if UI isn't fully set up yet
                    Debug.Log($"SPAM: {randomPhrases[Random.Range(0, randomPhrases.Length)]}");
                }

                // Random delay between text spawns
                yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
            }
            StopSpam();
        }

        private void SpawnRandomText()
        {
            // Keep screen clear of too many texts
            if (activeTexts.Count >= 3)
            {
                if (activeTexts[0] != null) Destroy(activeTexts[0]);
                activeTexts.RemoveAt(0);
            }

            GameObject textObj = Instantiate(textPrefab, canvasTransform);
            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            
            if (tmp != null)
            {
                tmp.text = randomPhrases[Random.Range(0, randomPhrases.Length)];
                tmp.color = new Color(Random.value, Random.value, Random.value, 1f); // Random color
            }

            // Random position on screen
            RectTransform rect = textObj.GetComponent<RectTransform>();
            if (rect != null)
            {
                float x = Random.Range(-400f, 400f);
                float y = Random.Range(-200f, 200f);
                rect.anchoredPosition = new Vector2(x, y);
            }

            activeTexts.Add(textObj);
        }
    }
}
