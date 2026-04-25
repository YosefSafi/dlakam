using UnityEngine;
using StickmanBrainrot.UI;

namespace StickmanBrainrot.Events
{
    /// <summary>
    /// Event that displays a fake advertisement pop-up to distract the player.
    /// </summary>
    public class Event_FakeAd : Event_Base
    {
        private UI_FakeAd uiFakeAd;

        private void Awake()
        {
            eventName = "FAKE AD";
            uiFakeAd = Object.FindAnyObjectByType<UI_FakeAd>();
        }

        protected override void OnEventStart()
        {
            if (uiFakeAd == null) uiFakeAd = Object.FindAnyObjectByType<UI_FakeAd>();
            
            if (uiFakeAd != null)
            {
                // Trigger the fake ad for the duration of the event
                uiFakeAd.ShowAd(defaultDuration);
            }
            else
            {
                Debug.LogWarning("Event_FakeAd could not find UI_FakeAd component in scene.");
            }
        }

        protected override void OnEventEnd()
        {
            if (uiFakeAd != null)
            {
                uiFakeAd.HideAd();
            }
        }
    }
}
