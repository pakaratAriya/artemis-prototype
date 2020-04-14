using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using Renderer = UnityEngine.Renderer;

public class AnchorScript : MonoBehaviour
{

    public WorldAnchorManager worldAnchorManager;
    // Start is called before the first frame update
    void Start()
    {
        AnchorIt();

    }

    /// <summary>
    ///     Use when the user release the anchor in Hololens
    /// </summary>
    public void AnchorIt()
    {
        worldAnchorManager.AttachAnchor(this.gameObject);
        this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    /// <summary>
    ///     Use when the user pick the anchor in Hololens
    /// </summary>
    public void ReleaseAnchor()
    {
        worldAnchorManager.RemoveAnchor(this.gameObject);
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

}
