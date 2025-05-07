using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    public List<Image> _calendar;
    public GameObject dayCellPrefab;
    public Transform gridParent;
    
    //Timing variables
    public SharedTimingData sharedTimingData;
    public float timing = 0f;
    private int currentDay = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 1; i <= 31; i++)
        {
            //Initiate:
            GameObject dayCell = Instantiate(dayCellPrefab, gridParent);
            //Set text:
            dayCell.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            _calendar.Add(dayCell.GetComponent<Image>());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDay < 31 && (sharedTimingData.Stage == 7 || sharedTimingData.Stage == 8))
        {
            switch (timing)
            {
                case < 4:
                    _calendar[currentDay].color = Color.Lerp(Color.white, Color.red, (timing / 4));
                    timing += (Time.deltaTime * sharedTimingData.Speed);
                    break;
                default:
                    timing = 0;
                    //_calendar[currentDay].color = Color.red;
                    currentDay++;
                    //calendar[currentDay].GetComponent<Image>().color = Color.red;
                    break;
            }
        }
    }
}
