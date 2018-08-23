using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;
using UnityEngine.UI;
using System.Linq;

public class View : MonoBehaviour {

    public GameObject graph_chart;
    public float float_time = 0.5f;

    Dictionary<string,List<DoubleVector2>> graph_data;
    int pageSize = 100;

    void Start()
    {
        if (graph_chart != null)
        {
            graph_data = new Dictionary<string, List<DoubleVector2>>();
        }

    }
	
    void Update()
    {
    }

    void ConfigureCharts(Config config)
    {
        //List<ConfigInfo> info_list = config.info_list;
        Debug.Log("Make configuration");
    }

    void UpdateData(Data data)
    {
        foreach (DataInfo info in data.data_list)
        {
            string chart_name = info.chart;

            if (!chart_name.Contains("pie") & !chart_name.Contains("bar") & graph_chart != null)
            {
                //Debug.LogWarning(chart_name+info.category);
                graph_chart.GetComponent<GraphChartBase>().DataSource.AddPointToCategoryRealtime(info.category, (double)info.value[0], (double)info.value[1]);
                // store all the history data of graph
                if (!graph_data.ContainsKey(info.category))
                {
                    graph_data[info.category] = new List<DoubleVector2>();
                }
                graph_data[info.category].Add(new DoubleVector2((double)info.value[0], (double)info.value[1]));
            }

        }

    }

}
