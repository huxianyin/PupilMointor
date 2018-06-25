using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
class ConfigInfo
{
    public string axis;
    public string[] categorys;
    public string[] groups;
    public string name;
    public string type;
    
    string list2str<T>(T[] list)
    {
        string str = "";
        foreach (T item in list)
        {
            str += item.ToString() + " ";
        }

        return str;
    }


    public override string ToString()
    {
        return "name:" + name + " type: " + type + " axis:" + axis  + "\n" + 
            "categorys:" + list2str<string>(categorys) +"\n" + 
            "groups:" + list2str<string>(groups) + "\n";
    }
}


[Serializable]
class Config
{
    public string title;
    public List<ConfigInfo> info_list;

    public override string ToString()
    {
        string tmp = title + "\n";
        foreach (ConfigInfo one in info_list)
        {
            tmp += one.ToString() + "\n";
        }

        return tmp;
    }

}


[Serializable]
class DataInfo
{
    public string category;
    public string chart;
    public string group;
    public float[] value;

    string list2str<T>(T[] list)
    {
        string str = "";
        foreach (T item in list)
        {
            str += item.ToString() + " ";
        }

        return str;
    }

    public override string ToString()
    {
        return "chart: " + chart + " group:" + group + " category" + category + " value="+list2str<float>(value);
    }
}

[Serializable]
class Data
{
    public List<DataInfo> data_list;

    public override string ToString()
    {
        string tmp = "";
        foreach(DataInfo one in data_list)
        {
            tmp += one+"\n";
        }

        return tmp;
    }

}

