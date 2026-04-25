using UnityEngine;
using StickmanBrainrot.UI;

namespace StickmanBrainrot.Events
{
    /// <summary>
    /// Event that spawns random NPC/brainrot texts rapidly on screen.
    /// </summary>
    public class Event_TextSpam : Event_Base
    {
        private UI_TextSpam uiTextSpam;

        private void Awake()
        {
            eventName = "TEXT SPAM";
            uiTextSpam = Object.FindAnyObjectByType<UI_TextSpam>();
        }

        protected override void OnEventStart()
        {
            if (uiTextSpam == null) uiTextSpam = Object.FindAnyObjectByType<UI_TextSpam>();
            
            if (uiTextSpam != null)
            {
                uiTextSpam.StartSpam(defaultDuration);
            }
            else
            {
                Debug.LogWarning("Event_TextSpam could not find UI_TextSpam component in scene.");
            }
        }

        protected override void OnEventEnd()
        {
            if (uiTextSpam != null)
            {
                uiTextSpam.StopSpam();
            }
        }
    }
}
