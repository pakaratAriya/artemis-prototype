using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    //private Text directionTxt;
    public Target curTarget;
    public List<Target> allTargets = new List<Target>();
    public List<TargetAnchor> allAnchors = new List<TargetAnchor>();
    public List<TargetAnchor> curAnchor = new List<TargetAnchor>();
    public float turningOffsetPercent = 0.95f;
    public float secondaryTuningOffset = 0.7f;
    public float offsetDistance = 2;
    private float curIndex;
    public Material target_material;
    public Material default_material;
    public Material green_material;
    private float aisleWidth = 5.1f;
    public OrderManager orderManager;
    private GameObject findingPathObject;
    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        
        findingPathObject = new GameObject("findingPathObject");
        lr = GetComponent<LineRenderer>();
        findingPathObject.transform.position = transform.position;
        //directionTxt = GameObject.FindGameObjectWithTag("DirectionText").GetComponent<Text>();
        allTargets.AddRange(FindObjectsOfType<TargetItem>());
        allAnchors.AddRange(FindObjectsOfType<TargetAnchor>());
        //directionTxt.text = "";
        if (curTarget)
        {
            curIndex = allTargets.IndexOf(curTarget);
            curTarget.GetComponent<MeshRenderer>().material = target_material;
            FindAnchor();
        }
        StartCoroutine(detectLine());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeUIText()
    {
        if (Mathf.Abs(curTarget.transform.position.z - transform.position.z) > aisleWidth / 3)
        {
            TargetAnchor ta = GetClosestAnchor();
            //directionTxt.text = findDirection(ta.transform.position);
        }
        else
        {
            //directionTxt.text = FindTargetItem(curTarget.transform.position);
        }
    }

    private TargetAnchor GetClosestAnchor()
    {
        TargetAnchor ta = null;
        float closestDistance = Mathf.Infinity;
        foreach (TargetAnchor anchor in curAnchor)
        {
            if (Vector3.Distance(anchor.transform.position, transform.position) < closestDistance)
            {
                ta = anchor;
                closestDistance = Vector3.Distance(anchor.transform.position, transform.position);
            }
        }
        return ta;
    }

    private void ChangeLineRenderer()
    {
        List<Target> targets = GetTargetRoute();
        Vector3[] pos_arr;
        if (targets.Count >= 2)
        {
            pos_arr = new Vector3[targets.Count + 2];
            pos_arr[0] = new Vector3(transform.position.x, -0.5f, transform.position.z);
            for (int i = 0; i < targets.Count - 1; i++)
            {
                Vector3 pos = targets[i].transform.position;
                pos_arr[i + 1] = new Vector3(pos.x, -0.5f, pos.z);
            }
            pos_arr[targets.Count] = new Vector3(targets[targets.Count - 1].transform.position.x, -0.5f, targets[targets.Count - 2].transform.position.z);
            pos_arr[targets.Count + 1] = new Vector3(targets[targets.Count - 1].transform.position.x, -0.5f, targets[targets.Count - 1].transform.position.z);
        }
        else
        {
            TargetAnchor ta = GetClosestAnchor();
            pos_arr = new Vector3[3];
            pos_arr[0] = new Vector3(transform.position.x, -0.5f, transform.position.z);
            pos_arr[1] = new Vector3(targets[0].transform.position.x, -0.5f, ta.transform.position.z);
            pos_arr[2] = new Vector3(targets[0].transform.position.x, -0.5f, targets[0].transform.position.z);
        }
        lr.positionCount = pos_arr.Length;
        lr.SetPositions(pos_arr);
    }

    private List<Target> GetTargetRoute()
    {
        List<Target> targets = new List<Target>();
        findingPathObject.transform.position = transform.position;
        while(findingPathObject.transform.position != curTarget.transform.position)
        {
            if (Mathf.Abs(curTarget.transform.position.z - findingPathObject.transform.position.z) > aisleWidth / 3)
            {
                TargetAnchor ta = null;
                float closestDistance = Mathf.Infinity;
                foreach (TargetAnchor anchor in allAnchors)
                {
                    if (Vector3.Distance(anchor.transform.position, transform.position) < closestDistance && !targets.Contains(anchor))
                    {
                        ta = anchor;
                        closestDistance = Vector3.Distance(anchor.transform.position, transform.position);
                    }
                }
                if (ta)
                {
                    targets.Add(ta);
                    findingPathObject.transform.position = ta.transform.position;
                }
                else
                {
                    print("ERRor");
                    break;
                } 
        }
            else
            {
                targets.Add(curTarget);
                findingPathObject.transform.position = curTarget.transform.position;
            }
        }    
        return targets;
    }

 
    private int getCurrentAisle() {
        Target ta = null;
        float closestDistance = Mathf.Infinity;
        foreach (Target anchor in allTargets) {
            if (Vector3.Distance(anchor.transform.position, transform.position) < closestDistance) {
                ta = anchor;
                closestDistance = Vector3.Distance(anchor.transform.position, transform.position);
            }
        }
        return ta.aisle;
    }

    private void FindAnchor()
    {
        curAnchor.Clear();
        foreach(TargetAnchor anchor in allAnchors)
        {
            if(curTarget.aisle == anchor.aisle)
            {
                curAnchor.Add(anchor);
                
            }
        }
    }

    public string findDirection(Vector3 targetPos)
    {
        string message = "";
        Vector3 cam_forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 target_dir = targetPos - transform.position;
        if(Mathf.Abs(targetPos.x - transform.position.x) > offsetDistance)
        {
            if (target_dir.x > 0)
            {
                if (cam_forward.x > turningOffsetPercent)
                {
                    message += "go forward";
                }
                else
                {
                    if (cam_forward.z > 0)
                    {
                        message += "turn right";
                    }
                    if (cam_forward.z < 0)
                    {
                        message += "turn left";
                    }
                }
            }
            else
            {
                if (cam_forward.x < -turningOffsetPercent)
                {
                    message += "go forward";
                }
                else
                {
                    if (cam_forward.z < 0)
                    {
                        message += "turn right";
                    }
                    if (cam_forward.z > 0)
                    {
                        message += "turn left";
                    }
                }
            }
        }
        else if (Mathf.Abs(targetPos.z - transform.position.z) > offsetDistance)
        {
            if (target_dir.z > 0)
            {
                if (cam_forward.z > turningOffsetPercent)
                {
                    message += "go forward";
                }
                else
                {
                    if (cam_forward.x < 0)
                    {
                        message += "turn right";
                    }
                    if (cam_forward.x > 0)
                    {
                        message += "turn left";
                    }
                }
            }
            else
            {
                if (cam_forward.z < -turningOffsetPercent)
                {
                    message += "go forward";
                }
                else
                {
                    if (cam_forward.x > 0)
                    {
                        message += "turn right";
                    }
                    if (cam_forward.x < 0)
                    {
                        message += "turn left";
                    }
                }
            }
        }
        else
        {
            message = "Here";
            if(Vector3.Distance(transform.position,curTarget.transform.position)<offsetDistance)
                SwitchTarget();
            
        }
        if (cam_forward.x * target_dir.x < 0 && cam_forward.z * target_dir.z < 0 && Vector3.Distance(targetPos,transform.position) > offsetDistance)
        {
            message = "turn back";
        }
        return message;
        
    }

    public string FindTargetItem(Vector3 targetPos) {
        string message = "";
        Vector3 cam_forward = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 target_dir = targetPos - transform.position;
        print(Mathf.Abs(targetPos.x-transform.position.x));
        if (Mathf.Abs(targetPos.x - transform.position.x) > offsetDistance) {
            if (target_dir.x > 0) {
                if (cam_forward.x > turningOffsetPercent) {
                    message += "go forward";
                } else {
                    if (cam_forward.z > 0) {
                        message += "turn right";
                    }
                    if (cam_forward.z < 0) {
                        message += "turn left";
                    }
                }
            } else {
                if (cam_forward.x < -turningOffsetPercent) {
                    message += "go forward";
                } else {
                    if (cam_forward.z < 0) {
                        message += "turn right";
                    }
                    if (cam_forward.z > 0) {
                        message += "turn left";
                    }
                }
            }
        } else if (Mathf.Abs(targetPos.z - transform.position.z) > aisleWidth/3) {
            if (target_dir.z > 0) {
                if (cam_forward.z > turningOffsetPercent) {
                    message += "go forward";
                } else {
                    if (cam_forward.x < 0) {
                        message += "turn right";
                    }
                    if (cam_forward.x > 0) {
                        message += "turn left";
                    }
                }
            } else {
                if (cam_forward.z < -turningOffsetPercent) {
                    message += "go forward";
                } else {
                    if (cam_forward.x > 0) {
                        message += "turn right";
                    }
                    if (cam_forward.x < 0) {
                        message += "turn left";
                    }
                }
            }
        } else {
            message = "Here";
            if (Vector3.Distance(transform.position, curTarget.transform.position) < offsetDistance * 2)
            {
                //orderManager.nextOrder();
            }
                
                

        }
        if (cam_forward.x * target_dir.x < 0 && cam_forward.z * target_dir.z < 0 && Vector3.Distance(targetPos, transform.position) > offsetDistance) {
            message = "turn back";
        }
        return message;
    }

    public Target SwitchTarget()
    {
        curTarget.GetComponent<MeshRenderer>().material = default_material;
        int randIndex = Random.Range(0, allTargets.Count);
        while (randIndex == curIndex)
            randIndex = Random.Range(0, allTargets.Count);
        curTarget = allTargets[randIndex];
        curTarget.GetComponent<MeshRenderer>().material = target_material;
        FindAnchor();
        return allTargets[randIndex];
        
    }

    public void findItem(int aisle, int bin) {
        if(curTarget)
            curTarget.GetComponent<MeshRenderer>().material = default_material;
        foreach(TargetItem ti in allTargets)
        {
            if (ti.aisle == aisle && ti.bin == bin)
            {
                curTarget = ti;
                curIndex = allTargets.IndexOf(curTarget);
                curTarget.GetComponent<MeshRenderer>().material = target_material;
                FindAnchor();
                return;
            }
        }
    }

    private IEnumerator detectLine()
    {
        while (true)
        {

            ChangeLineRenderer();
            if (!orderManager)
            {
                orderManager = FindObjectOfType<OrderManager>();
            }
            else
            {
                if (Vector3.Distance(transform.position, curTarget.transform.position) < offsetDistance && getCurrentAisle() == orderManager.getCurrentItem().Aisle)
                {

                    orderManager.activateVoice();
                    curTarget.GetComponent<MeshRenderer>().material = green_material;
                }
                else
                {
                    orderManager.deactivateVoice();
                    curTarget.GetComponent<MeshRenderer>().material = target_material;
                }
            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }

}
