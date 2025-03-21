// Copyright (C) 2019 gamevanilla - All rights reserved.
// This code can only be used under the standard Unity Asset Store EULA,
// a copy of which is available at https://unity.com/legal/as-terms.

using UnityEngine;
using UnityEngine.UI;

namespace UltimateClean
{
    /// <summary>
    /// This class handles updating the sound UI widgets depending on the player's selection.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        private Slider m_soundSlider;
        private GameObject m_soundButton;

        private void Start()
        {
            m_soundSlider = GetComponent<Slider>();
            m_soundSlider.value = PlayerPrefs.GetInt("sound_on");
            m_soundButton = GameObject.Find("SoundButton/Button");
        }

        public void SwitchSound()
        {
            AudioListener.volume = m_soundSlider.value;
            PlayerPrefs.SetInt("sound_on", (int)m_soundSlider.value);
            if (m_soundButton != null)
            {
                m_soundButton.GetComponent<SoundButton>().ToggleSprite();
            }
        }
    }
}
