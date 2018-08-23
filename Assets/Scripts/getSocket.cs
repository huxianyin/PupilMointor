using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using UnityEngine;

public class getSocket : MonoBehaviour
{
    public string host = "192.168.11.19";
    public int port = 10086;
    private string message;
    private Socket client;
    private byte[] messTmp;
    private Thread clientReceiveThread;
    private bool receive_data = false;
    private GameObject view;
    private SendMessageHelper messageHelper;

    // Use this for initialization
    void Start()
    {
        receive_data = false;
        messTmp = new byte[2048];
        view = this.GetComponent<View>().gameObject;
        messageHelper = this.GetComponent<SendMessageHelper>();
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(host), port));
            clientReceiveThread = new Thread(new ThreadStart(GetMessage));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    T ReadToObject<T>(string json) where T : new()
    {
        T deserializedUser = new T();
        try
        {
            deserializedUser = JsonUtility.FromJson<T>(json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return deserializedUser;
    }

    void GetMessage()
    {
        while (true)
        {
            var count = client.Receive(messTmp);
            string mess_str = Encoding.UTF8.GetString(messTmp, 0, count);
            if (count != 0)
            {
                if(mess_str.Contains("start send data"))
                {
                    receive_data = true;
                    continue;
                }

                if(receive_data == true)
                {
                    string[] data_pieces = mess_str.Split(';');
                    string data_str = "{\"data_list\":[";
                    for (int i = 0; i < data_pieces.Length-1; i++)
                    {
                        if (i != data_pieces.Length - 2)
                        { data_str += data_pieces[i] + ","; }
                        else
                        { data_str += data_pieces[i] + "]}"; }
                    }
                    Debug.Log("data_pieces[0]= "+data_pieces[0]);
                    Data frame = ReadToObject<Data>(data_str);
                    message = frame.ToString();
                    Debug.Log(message);
                    SendMessageContext context = new SendMessageContext(view.gameObject, "UpdateData",frame, SendMessageOptions.RequireReceiver);
                    SendMessageHelper.RegisterSendMessage(context);
                }
                else
                {
                    Config frame = ReadToObject<Config>(mess_str);
                    message = frame.ToString();
                    Debug.Log(message);
                    SendMessageContext context = new SendMessageContext(view.gameObject, "ConfigureCharts", frame, SendMessageOptions.RequireReceiver);
                    SendMessageHelper.RegisterSendMessage(context);
                }
            }
            Array.Clear(messTmp, 0, count);
        }
    }

    void OnApplicationQuit()
    {
        receive_data = false;
        client.Close();
        clientReceiveThread.Abort();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }

}