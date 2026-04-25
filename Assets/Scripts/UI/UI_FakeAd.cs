using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace StickmanBrainrot.UI
{
    /// <summary>
    /// Controls the display of a fake ad overlay during the Fake Ad event.
    /// </summary>
    public class UI_FakeAd : MonoBehaviour
    {
        [Header("Ad Settings")]
        [SerializeField] private GameObject adContainer; // Panel containing the fake ad graphic
        
        private Coroutine adRoutine;

        private void Start()
        {
            if (adContainer != null)
            {
                adContainer.SetActive(false); // Ensure it's hidden by default
            }
        }

        public void ShowAd(float duration)
        {
            if (adContainer == null) return;

            if (adRoutine != null) StopCoroutine(adRoutine);
            
            adRoutine = StartCoroutine(AdDisplayRoutine(duration));
        }

        public void HideAd()
        {
            if (adContainer != null)
            {
                adContainer.SetActive(false);
            }
            
            if (adRoutine != null)
            {
                StopCoroutine(adRoutine);
                adRoutine = null;
            }
        }

        private IEnumerator AdDisplayRoutine(float duration)
        {
            adContainer.SetActive(true);
            
            // Wait for the duration of the ad
            yield return new WaitForSeconds(duration);
            
            adContainer.SetActive(false);
            adRoutine = null;
        }
        
        // Function that could be hooked up to a fake "Close [X]" button
        public void OnClickCloseButton()
        {
            HideAd();
        }
    }
}
