using System.Collections;
using System.Collections.Generic;
using RogueCaml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml
{
    public class LauncherScript : MonoBehaviour
    {

        public GameManager gameManager;
        public TMP_InputField pseudo;
        public TMP_InputField gameName;
        public Button[] playButtons;

        public void privateHandler()
        {
            string pseudo = this.pseudo.text;
            if (pseudo == "") pseudo = "Player";
            
            Debug.Log($"Connect process for {pseudo}");
            foreach (Button b in playButtons)
            {
                b.interactable = false;
            }
            gameManager.ConnectPlayer(pseudo, gameName.text);
        }

        public void randomHandler()
        {
            string pseudo = this.pseudo.text;
            if (pseudo == "") pseudo = "Player";
            
            Debug.Log($"Connect process for {pseudo}");
            foreach (Button b in playButtons)
            {
                b.interactable = false;
            }
            gameManager.ConnectPlayer(pseudo, "default");
        }
    }
}
