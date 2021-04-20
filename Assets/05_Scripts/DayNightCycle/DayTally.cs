using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTally : MonoBehaviour
{
    private int _numberOfDay;
    private int _numberOfYear;
    public Text dayText;
    public Text yearText;

    // Start is called before the first frame update
    void Start()
    {
        //_numberOfDay = ExperimentalGameManager.currentManager.GetCurrentDay();
        


    }

    // Update is called once per frame
    void Update()
    {
       /*_numberOfDay = ExperimentalGameManager.currentManager.GetCurrentDay();
        dayText.text = "DAY " + (_numberOfDay + 1);

        _numberOfYear = ExperimentalGameManager.currentManager.GetCurrentYear();
        yearText.text = "YEAR " + _numberOfYear;*/
    }
}
