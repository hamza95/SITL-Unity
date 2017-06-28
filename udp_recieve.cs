using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
public class udp_recieve : MonoBehaviour {
    public UdpClient udp_client;
    int port = 5503;
    string ip = "127.0.0.1";
    int i=411;
    public byte[] b=new byte[20], s;
    public char[] c;
    string data = "works";
    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Loopback, 5503);
    // Use this for initialization
    Socket client;

    public static int FG_MAX_ENGINES = 4;
    public static int FG_MAX_WHEELS = 3;
    public static int FG_MAX_TANKS = 4;

    UInt32 version;       // increment when data values change
    UInt32 padding;       // padding

    // Positions
    double longitude;       // geodetic (radians)
    double latitude;        // geodetic (radians)
    double altitude;        // above sea level (meters)
    float agl;          // above ground level (meters)
    float phi;          // roll (radians)
    float theta;        // pitch (radians)
    float psi;          // yaw or true heading (radians)
    float alpha;                // angle of attack (radians)
    float beta;                 // side slip angle (radians)

    // Velocities
    float phidot;       // roll rate (radians/sec)
    float thetadot;     // pitch rate (radians/sec)
    float psidot;       // yaw rate (radians/sec)
    float vcas;             // calibrated airspeed
    float climb_rate;       // feet per second
    float v_north;              // north velocity in local/body frame, fps
    float v_east;               // east velocity in local/body frame, fps
    float v_down;               // down/vertical velocity in local/body frame, fps
    float v_wind_body_north;    // north velocity in local/body frame
                                // relative to local airmass, fps
    float v_wind_body_east;     // east velocity in local/body frame
                                // relative to local airmass, fps
    float v_wind_body_down;     // down/vertical velocity in local/body
                                // frame relative to local airmass, fps

    // Accelerations
    float A_X_pilot;        // X accel in body frame ft/sec^2
    float A_Y_pilot;        // Y accel in body frame ft/sec^2
    float A_Z_pilot;        // Z accel in body frame ft/sec^2

    // Stall
    float stall_warning;        // 0.0 - 1.0 indicating the amount of stall
    float slip_deg;     // slip ball deflection

    // Pressure

    // Engine status
    UInt32 num_engines;        // Number of valid engines
    UInt32 []eng_state = new UInt32[FG_MAX_ENGINES];// Engine state (off, cranking, running)
    float []rpm = new float[FG_MAX_ENGINES];       // Engine RPM rev/min
    float []fuel_flow = new float[FG_MAX_ENGINES]; // Fuel flow gallons/hr
    float []fuel_px = new float[FG_MAX_ENGINES];   // Fuel pressure psi
    float []egt = new float[FG_MAX_ENGINES];       // Exhuast gas temp deg F
    float []cht = new float[FG_MAX_ENGINES];       // Cylinder head temp deg F
    float []mp_osi = new float[FG_MAX_ENGINES];    // Manifold pressure
    float []tit = new float[FG_MAX_ENGINES];       // Turbine Inlet Temperature
    float []oil_temp = new float[FG_MAX_ENGINES];  // Oil temp deg F
    float []oil_px = new float[FG_MAX_ENGINES];    // Oil pressure psi

    // Consumables
    UInt32 num_tanks;     // Max number of fuel tanks
    float []fuel_quantity=new float [FG_MAX_TANKS];

    // Gear status
    UInt32 num_wheels;
    UInt32 []wow = new UInt32[FG_MAX_WHEELS];
    float []gear_pos = new float[FG_MAX_WHEELS];
    float []gear_steer = new float[FG_MAX_WHEELS];
    float []gear_compression = new float[FG_MAX_WHEELS];

    // Environment
    UInt32 cur_time;           // current unix time
                               // FIXME: make this uint64_t before 2038
    UInt32 warp;                // offset in seconds to unix time
    float visibility;            // visibility in meters (for env. effects)

    // Control surface positions (normalized values)
    float elevator;
    float elevator_trim_tab;
    float left_flap;
    float right_flap;
    float left_aileron;
    float right_aileron;
    float rudder;
    float nose_wheel;
    float speedbrake;
    float spoilers;


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
            //data=System.Text.Encoding.Default.GetString(b);
            //data=Encoding.Default.GetString(b);
            //Debug.Log(data);
            // Debug.Log(b);
            //Debug.Log(b);
            //data = Encoding.ASCII.GetString(b);
            //data=Convert.ToBase64String(b);
            //data = Convert.ToString(b);
            data =BitConverter.ToString(b);
            Debug.Log(data);

            
            UInt32 buf = UInt32.Parse(data);
            //uint32_t* buf = (uint32_t*)this;
            for (UInt16 y = 0; y < sizeof(*data)/ 4;y++)
            {
                data[i]=
            }
            for (uint16_t i = 0; i < sizeof(*this)/ 4; i++) {
                buf[i] = ntohl(buf[i]);
            }
            // fixup the 3 doubles
            buf = (uint32_t*)&longitude;
            uint32_t tmp;
            for (uint8_t i = 0; i < 3; i++)
            {
                tmp = buf[0];
                buf[0] = buf[1];
                buf[1] = tmp;
                buf += 2;
            }

            //Debug.Log(data.ToString());
            //s = Encoding.ASCII.GetBytes(data);
            //Debug.Log(s);
        }
        catch
        {
            i=i+10;
            b = new byte[i];
            Debug.Log("catch"+i);

        }

    }
}

