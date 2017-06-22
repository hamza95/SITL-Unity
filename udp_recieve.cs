using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Net;
using System.Text;
using System.Net.Sockets;
public class udp_recieve : MonoBehaviour {
    public UdpClient udp_client;
    int port = 14550;
    string ip = "127.0.0.1";
    int i=20;
    public byte[] b=new byte[20], s;
    string data = "works";
    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Loopback, 14550);
    // Use this for initialization
    Socket client;
    void Start () {
        //udp_client = new UdpClient();
        
        Debug.Log("started");
        client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        client.Bind(RemoteIpEndPoint);
        data = RemoteIpEndPoint.ToString();
        Debug.Log(data);
        //udp_client.JoinMulticastGroup(IPAddress.Parse("224.5.23.2"));
        Debug.Log("bind complete");
        //client.ReceiveBufferSize = 16384;
        //client.Receive(b);
        //Debug.Log("recieved");

        //EndPoint Remote = (EndPoint)(RemoteIpEndPoint);
        //int recv = client.ReceiveFrom(b,ref Remote);
        /*
        data = Encoding.ASCII.GetString(b);
        Debug.Log(data.ToString());
        s = Encoding.ASCII.GetBytes(data);
        Debug.Log(s);
        */
    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            client.Receive(b);
            data = Encoding.ASCII.GetString(b);
            Debug.Log(data);
            //Debug.Log(data.ToString());
            s = Encoding.ASCII.GetBytes(data);
            //Debug.Log(s);
        }
        catch
        {
            i++;
            b = new byte[i];

        }

    }
}

