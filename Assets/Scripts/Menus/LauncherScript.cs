using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RogueCaml
{
    public class LauncherScript : MonoBehaviour
    {

        public TMP_InputField pseudoField;
        public TMP_InputField roomField;
        public Button[] processButtons;

        public void privateHandler()
        {
            string pseudoField = this.pseudoField.text;
            if (pseudoField == "") pseudoField = "Player";
            
            Debug.Log($"Connect process for {pseudoField}");
            foreach (Button b in processButtons)
            {
                b.interactable = false;
            }
            
            if (roomField.text == "") Debug.Log("Please enter valid room name!");
            else NetworkManager.Instance.ConnectPlayer(pseudoField, roomField.text);
        }

        public void randomHandler()
        {
            string pseudoField = this.pseudoField.text;
            if (pseudoField == "") pseudoField = "Player";
            
            Debug.Log($"Connect process for {pseudoField}");
            foreach (Button b in processButtons)
            {
                b.interactable = false;
            }
            NetworkManager.Instance.ConnectPlayer(pseudoField, "default");
        }
    }
}
