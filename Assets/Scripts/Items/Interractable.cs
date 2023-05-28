using UnityEngine;

namespace RogueCaml
{
    public class Interractable : ObjectCharacteristics
    {
        [SerializeField]
        public Action action;

        private bool isInRange = false;
        // Start is called before the first frame update
            
        void Update()
        {
            if (isInRange)
            {
                TryInterract();
            }
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enter the fields");
            GameObject obj = other.gameObject;
            ObjectCharacteristics x = obj. GetComponent<ObjectCharacteristics>();
            if (x.IsPlayer)
            { 
                isInRange= true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exit the fields");
            GameObject obj = other.gameObject;
            ObjectCharacteristics x = obj.GetComponent<ObjectCharacteristics>();
            if (x.IsPlayer)
            {
                isInRange = false;
            }
        }
        
        
        private void TryInterract()
        {
            if (isInRange && Input.GetKeyDown(KeyCode.E) )
            {
                action.Interract();
            }
        }
    }
}