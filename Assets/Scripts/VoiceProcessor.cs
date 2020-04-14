using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;


public class VoiceProcessor : MonoBehaviour {
    OrderManager orderMan;
    SpeechInputHandler sih;
    string current_phrase = "";
    VoiceResponseManager soundManager;

    private void Start() {
        sih = GetComponent<SpeechInputHandler>();
        soundManager = FindObjectOfType<VoiceResponseManager>();
    }
    /// <summary>
    ///     Called when the user is informing the number of picking items.
    ///         The function will translate the user's voice and compare to the required 
    ///         number of the items. Then, response the feedback to the user.     
    /// </summary>
    public void pickItem() {
        if (!orderMan) {
            orderMan = FindObjectOfType<OrderManager>();
        }
        string phrase = sih.receiveCurrentPhrase();
        
        if (sih.Keywords[orderMan.getCurrentItem().QuantityRequired - 1].Keyword == phrase) {
            print("nice");
            orderMan.nextOrder();
            orderMan.deactivateVoice();
        }
        else
        // if the user call the wrong number
        //  tell the player how many left he/she has to pick/return
        {
            int keywordIndex = 0;
            for (int i = 0; i < 5; i++)
            {
                // Check what the user speaks
                if (sih.Keywords[i].Keyword == phrase)
                {
                    keywordIndex = i;
                    break;
                }
            }
            // use what the speaker speak compare to the required quantity
            // and play the sound
            int diffQuantity = keywordIndex + 1 - orderMan.getCurrentItem().QuantityRequired;
            soundManager.PlayIncorrectQuantityPick(diffQuantity);
        }
    }

    /// <summary>
    ///     to test what the user speak 
    /// </summary>
    /// <param name="phrase">The user phrase</param>
    public void receiveSpeechInput(string phrase) {
        print(GetComponent<SpeechInputHandler>().receiveCurrentPhrase());
    }
}

