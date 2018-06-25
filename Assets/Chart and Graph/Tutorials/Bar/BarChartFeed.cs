using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class BarChartFeed : MonoBehaviour {
	void Start () {
        BarChart barChart = GetComponent<BarChart>();
        BarAnimation barAnimation = GetComponent<BarAnimation>();
        if (barChart != null)
        {
            barChart.DataSource.SlideValue("Player 1", "Value 1", Random.value * 20,3f);
            barChart.DataSource.SlideValue("Player 2", "Value 1", Random.value * 20, 3f);
            //barAnimation.Animate();
            //float _value = Random.value * 20;
            //float _time = 4.0f;
            //string _group = "Player 1";
            //string _category = "Value 1";
            //barChart.DataSource.SetValue(_group, _category, _value);
            //barChart.DataSource.SlideValue(_group, _category, _value, _time);
        }
    }
    private void Update()
    {
    }
}
