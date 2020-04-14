using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour {

    private Order order;
    private int currItemIndex = 0;
    TargetManager targetManager;
    private bool voiceEnable = false;
    float voiceActivatePeriod = 120;
    Coroutine voiceTimer;
    VoiceResponseManager soundManager;
    public Text HudValues;


    // Start is called before the first frame update
    void Start() {
        order = new Order();
        targetManager = FindObjectOfType<TargetManager>();
        targetManager.orderManager = this;
        targetManager.findItem(order.items[currItemIndex].Aisle, order.items[currItemIndex].Bin);
        soundManager = FindObjectOfType<VoiceResponseManager>();
        UpdateHud();
    }
    /// <summary>
    ///     To call the next order when the current order is done
    /// </summary>
    public void nextOrder() {
        currItemIndex++;
        if (currItemIndex < order.items.Count) {
            targetManager.findItem(order.items[currItemIndex].Aisle, order.items[currItemIndex].Bin);
            deactivateVoice();
            soundManager.PlaySuccess();
            UpdateHud();
        }
    }


    public Item getCurrentItem() {
        return order.items[currItemIndex];
    }

    public void activateVoice() {
        /*
         * use only if the customer want to turn off the voice automatically after 
         * [voiceActivatePeriod] seconds
            StopAllCoroutines();
            voiceTimer = StartCoroutine(StartVoiceTimer());
        */
        voiceEnable = true;
    }

    public bool getVoiceEnable() {
        return voiceEnable;
    }

    public void deactivateVoice() {
        StopAllCoroutines();
        voiceEnable = false;
    }


    public void SetHud(Text hud) {
        HudValues = hud;
    }

    /// <summary>
    ///     To display info of the current item to the Hud
    /// </summary>
    private void UpdateHud() {
        HudValues.text = order.GetHudValuesText(currItemIndex);
    }

    /// <summary>
    ///     Use when the client wants to turn off the microphone automatically 
    ///     after [voiceActivatePeriod]
    /// </summary>

    IEnumerator StartVoiceTimer() {
        voiceEnable = true;
        yield return new WaitForSeconds(voiceActivatePeriod);
        voiceEnable = false;
    }
}
