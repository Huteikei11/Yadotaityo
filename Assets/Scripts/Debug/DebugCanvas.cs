using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] private EcstasyManager EcstasyManager;
    [SerializeField] private SleepManager SleepManager;
    [SerializeField] private OppaiManager OppaiManager;
    [SerializeField] private GameTimer gametimer;
    [SerializeField] private TextMeshProUGUI Animepattern;
    [SerializeField] private TextMeshProUGUI SleepDeep;
    [SerializeField] private TextMeshProUGUI Ecstacy;
    [SerializeField] private TextMeshProUGUI Face;
    [SerializeField] private TextMeshProUGUI Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animepattern.text = "Animation:"+OppaiManager.animePattern.ToString();
        SleepDeep.text = "Sleep:"+SleepManager.sleepDeep.ToString();
        Ecstacy.text = "Ecstacy:"+EcstasyManager.ecstacyGage.ToString();
        Face.text = "Facepattern:"+SleepManager.facePattern.ToString();
        Timer.text = "Time:"+gametimer.GetFormattedTime();

    }
}
