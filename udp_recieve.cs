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
    Int32 warp;                // offset in seconds to unix time
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
    char []info=new char[500];
    string []inf;
    //int a;
    Int64 a;
    float var;
    void Start () {
        Debug.Log("started");
        client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        client.Bind(RemoteIpEndPoint);
        data = RemoteIpEndPoint.ToString();
        Debug.Log(data);
        Debug.Log("bind complete");





    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            client.Receive(b);
            data =BitConverter.ToString(b);
            Debug.Log(data);
            inf = data.Split('-');
            //Debug.Log("before conv"+inf[0]+inf[1]+inf[2]+inf[3]);

            a =int.Parse(inf[3], System.Globalization.NumberStyles.HexNumber);
            version=Convert.ToUInt32(a);
            Debug.Log("Version:" + version);


            a = Int64.Parse(inf[8] + inf[9] + inf[10] + inf[11] + inf[12] + 
                inf[13] + inf[14] + inf[15]
                , System.Globalization.NumberStyles.HexNumber);
            latitude = Convert.ToDouble(a);
            Debug.Log("Latitude:" + latitude);

            a = Int64.Parse(inf[16] + inf[17] + inf[18] + inf[19] + inf[20] +
                inf[21] + inf[22] + inf[23]
                , System.Globalization.NumberStyles.HexNumber);
            longitude = Convert.ToDouble(a);
            Debug.Log("Longitude:" + longitude);

            a = Int64.Parse(inf[24] + inf[25] + inf[26] + inf[27] + inf[28] +
                inf[29] + inf[30] + inf[31]
                , System.Globalization.NumberStyles.HexNumber);
            altitude = Convert.ToDouble(a);
            Debug.Log("Altitude:" + altitude);

            var = Int32.Parse(inf[32] + inf[33] + inf[34] + inf[35], System.Globalization.NumberStyles.HexNumber);
            agl = var;
            Debug.Log("Above ground level:" + agl);

            var = Int32.Parse(inf[36] + inf[37] + inf[38] + inf[39], System.Globalization.NumberStyles.HexNumber);
            phi = var;
            Debug.Log("Roll:" + phi);

            var = Int32.Parse(inf[40] + inf[41] + inf[42] + inf[43], System.Globalization.NumberStyles.HexNumber);
            theta = var;
            Debug.Log("Pitch:" + theta);

            var = Int32.Parse(inf[44] + inf[45] + inf[46] + inf[47], System.Globalization.NumberStyles.HexNumber);
            psi = var;
            Debug.Log("Yaw:" + psi);

            var = Int32.Parse(inf[48] + inf[49] + inf[50] + inf[51], System.Globalization.NumberStyles.HexNumber);
            alpha = var;
            Debug.Log("Angle of attack:" + alpha);

            var = Int32.Parse(inf[52] + inf[53] + inf[54] + inf[55], System.Globalization.NumberStyles.HexNumber);
            beta = var;
            Debug.Log("Side slip angle:" + beta);

            var = Int32.Parse(inf[56] + inf[57] + inf[58] + inf[59], System.Globalization.NumberStyles.HexNumber);
            phidot = var;
            Debug.Log("Roll rate:" + phidot);

            var = Int32.Parse(inf[60] + inf[61] + inf[62] + inf[63], System.Globalization.NumberStyles.HexNumber);
            thetadot = var;
            Debug.Log("Pitch rate:" + thetadot);

            var = Int32.Parse(inf[64] + inf[65] + inf[66] + inf[67], System.Globalization.NumberStyles.HexNumber);
            psidot = var;
            Debug.Log("Yaw rate:" + psidot);

            var = Int32.Parse(inf[64] + inf[65] + inf[66] + inf[67], System.Globalization.NumberStyles.HexNumber);
            vcas = var;
            Debug.Log("Callibrated air speed:" + vcas);

            var = Int32.Parse(inf[68] + inf[69] + inf[70] + inf[71], System.Globalization.NumberStyles.HexNumber);
            climb_rate = var;
            Debug.Log("Feet per second:" + climb_rate);

            var = Int32.Parse(inf[72] + inf[73] + inf[74] + inf[75], System.Globalization.NumberStyles.HexNumber);
            v_north = var;
            Debug.Log("North velocity:" + v_north);

            var = Int32.Parse(inf[76] + inf[77] + inf[78] + inf[79], System.Globalization.NumberStyles.HexNumber);
            v_east = var;
            Debug.Log("East velocity:" + v_east);

            var = Int32.Parse(inf[80] + inf[81] + inf[82] + inf[83], System.Globalization.NumberStyles.HexNumber);
            v_down = var;
            Debug.Log("Down velocity:" + v_down);

            var = Int32.Parse(inf[84] + inf[85] + inf[86] + inf[87], System.Globalization.NumberStyles.HexNumber);
            v_wind_body_north = var;
            Debug.Log("North body in local frame:" + v_wind_body_north);

            var = Int32.Parse(inf[88] + inf[89] + inf[90] + inf[91], System.Globalization.NumberStyles.HexNumber);
            v_wind_body_east = var;
            Debug.Log("East body in local frame:" + v_wind_body_east);

            var = Int32.Parse(inf[92] + inf[93] + inf[94] + inf[95], System.Globalization.NumberStyles.HexNumber);
            v_wind_body_down = var;
            Debug.Log("Down body in local frame:" + v_wind_body_down);

            var = Int32.Parse(inf[96] + inf[97] + inf[98] + inf[99], System.Globalization.NumberStyles.HexNumber);
            A_X_pilot = var;
            Debug.Log("X acceleration:" + A_X_pilot);

            var = Int32.Parse(inf[100] + inf[101] + inf[102] + inf[103], System.Globalization.NumberStyles.HexNumber);
            A_Y_pilot = var;
            Debug.Log("Y acceleration:" + A_Y_pilot);

            var = Int32.Parse(inf[104] + inf[105] + inf[106] + inf[107], System.Globalization.NumberStyles.HexNumber);
            A_Z_pilot = var;
            Debug.Log("Z acceleration:" + A_Z_pilot);

            
            var = Int32.Parse(inf[108] + inf[109] + inf[110] + inf[111], System.Globalization.NumberStyles.HexNumber);
            stall_warning = var;
            Debug.Log("Stall warning:" + stall_warning);

            var = Int32.Parse(inf[112] + inf[113] + inf[114] + inf[115], System.Globalization.NumberStyles.HexNumber);
            slip_deg = var;
            Debug.Log("Slip degree:" + slip_deg);

            a = int.Parse(inf[116]+ inf[117] + inf[118] + inf[119], System.Globalization.NumberStyles.HexNumber);
            num_engines = Convert.ToUInt32(a);
            Debug.Log("No of engines:" + num_engines);

            a = int.Parse(inf[120] + inf[121] + inf[122] + inf[123], System.Globalization.NumberStyles.HexNumber);
            eng_state[0] = Convert.ToUInt32(a);
            Debug.Log("Engine state:" + eng_state[0]);

            a = int.Parse(inf[124] + inf[125] + inf[126] + inf[127], System.Globalization.NumberStyles.HexNumber);
            eng_state[1] = Convert.ToUInt32(a);
            Debug.Log("Engine state:" + eng_state[1]);

            a = int.Parse(inf[128] + inf[129] + inf[130] + inf[131], System.Globalization.NumberStyles.HexNumber);
            eng_state[2] = Convert.ToUInt32(a);
            Debug.Log("Engine state:" + eng_state[2]);

            a = int.Parse(inf[132] + inf[133] + inf[134] + inf[135], System.Globalization.NumberStyles.HexNumber);
            eng_state[3] = Convert.ToUInt32(a);
            Debug.Log("Engine state:" + eng_state[3]);

            var = Int32.Parse(inf[136] + inf[137] + inf[138] + inf[139], System.Globalization.NumberStyles.HexNumber);
            rpm[0] = var;
            Debug.Log("rpm:" + rpm[0]);

            var = Int32.Parse(inf[140] + inf[141] + inf[142] + inf[143], System.Globalization.NumberStyles.HexNumber);
            rpm[1] = var;
            Debug.Log("rpm:" + rpm[1]);

            var = Int32.Parse(inf[144] + inf[145] + inf[146] + inf[147], System.Globalization.NumberStyles.HexNumber);
            rpm[2] = var;
            Debug.Log("rpm:" + rpm[2]);

            var = Int32.Parse(inf[148] + inf[149] + inf[150] + inf[151], System.Globalization.NumberStyles.HexNumber);
            rpm[3] = var;
            Debug.Log("rpm:" + rpm[3]);

            var = Int32.Parse(inf[152] + inf[153] + inf[154] + inf[155], System.Globalization.NumberStyles.HexNumber);
            fuel_flow[0] = var;
            Debug.Log("Fuel flow:" + fuel_flow[0]);

            var = Int32.Parse(inf[156] + inf[157] + inf[158] + inf[159], System.Globalization.NumberStyles.HexNumber);
            fuel_flow[1] = var;
            Debug.Log("Fuel flow:" + fuel_flow[1]);

            var = Int32.Parse(inf[160] + inf[161] + inf[162] + inf[163], System.Globalization.NumberStyles.HexNumber);
            fuel_flow[2] = var;
            Debug.Log("Fuel flow:" + fuel_flow[2]);

            var = Int32.Parse(inf[164] + inf[165] + inf[166] + inf[167], System.Globalization.NumberStyles.HexNumber);
            fuel_flow[3] = var;
            Debug.Log("Fuel flow:" + fuel_flow[3]);

            var = Int32.Parse(inf[168] + inf[169] + inf[170] + inf[171], System.Globalization.NumberStyles.HexNumber);
            fuel_px[0] = var;
            Debug.Log("Fuel pressure:" + fuel_px[0]);

            var = Int32.Parse(inf[172] + inf[173] + inf[174] + inf[175], System.Globalization.NumberStyles.HexNumber);
            fuel_px[1] = var;
            Debug.Log("Fuel pressure:" + fuel_px[1]);

            var = Int32.Parse(inf[176] + inf[177] + inf[178] + inf[179], System.Globalization.NumberStyles.HexNumber);
            fuel_px[2] = var;
            Debug.Log("Fuel pressure:" + fuel_px[2]);

            var = Int32.Parse(inf[180] + inf[181] + inf[182] + inf[183], System.Globalization.NumberStyles.HexNumber);
            fuel_px[3] = var;
            Debug.Log("Fuel pressure:" + fuel_px[3]);

            var = Int32.Parse(inf[184] + inf[185] + inf[186] + inf[187], System.Globalization.NumberStyles.HexNumber);
            egt[0] = var;
            Debug.Log("Exhaust gas temp:" + egt[0]);

            var = Int32.Parse(inf[188] + inf[189] + inf[190] + inf[191], System.Globalization.NumberStyles.HexNumber);
            egt[1] = var;
            Debug.Log("Exhaust gas temp:" + egt[1]);

            var = Int32.Parse(inf[192] + inf[193] + inf[194] + inf[195], System.Globalization.NumberStyles.HexNumber);
            egt[2] = var;
            Debug.Log("Exhaust gas temp:" + egt[2]);

            var = Int32.Parse(inf[196] + inf[197] + inf[198] + inf[199], System.Globalization.NumberStyles.HexNumber);
            egt[3] = var;
            Debug.Log("Exhaust gas temp:" + egt[3]);

            var = Int32.Parse(inf[200] + inf[201] + inf[202] + inf[203], System.Globalization.NumberStyles.HexNumber);
            cht[0] = var;
            Debug.Log("Cylinder head temp:" + cht[0]);

            var = Int32.Parse(inf[204] + inf[205] + inf[206] + inf[207], System.Globalization.NumberStyles.HexNumber);
            cht[1] = var;
            Debug.Log("Cylinder head temp:" + cht[1]);

            var = Int32.Parse(inf[208] + inf[209] + inf[210] + inf[211], System.Globalization.NumberStyles.HexNumber);
            cht[2] = var;
            Debug.Log("Cylinder head temp:" + cht[2]);

            var = Int32.Parse(inf[212] + inf[213] + inf[214] + inf[215], System.Globalization.NumberStyles.HexNumber);
            cht[3] = var;
            Debug.Log("Cylinder head temp:" + cht[3]);

            var = Int32.Parse(inf[216] + inf[217] + inf[218] + inf[219], System.Globalization.NumberStyles.HexNumber);
            mp_osi[0] = var;
            Debug.Log("Manifold pressure:" + mp_osi[0]);

            var = Int32.Parse(inf[220] + inf[221] + inf[222] + inf[223], System.Globalization.NumberStyles.HexNumber);
            mp_osi[1] = var;
            Debug.Log("Manifold pressure:" + mp_osi[1]);

            var = Int32.Parse(inf[224] + inf[225] + inf[226] + inf[227], System.Globalization.NumberStyles.HexNumber);
            mp_osi[2] = var;
            Debug.Log("Manifold pressure:" + mp_osi[2]);

            var = Int32.Parse(inf[228] + inf[229] + inf[230] + inf[231], System.Globalization.NumberStyles.HexNumber);
            mp_osi[3] = var;
            Debug.Log("Manifold pressure:" + mp_osi[3]);

            var = Int32.Parse(inf[232] + inf[233] + inf[234] + inf[235], System.Globalization.NumberStyles.HexNumber);
            tit[0] = var;
            Debug.Log("Turbine inlet temp:" + tit[0]);

            var = Int32.Parse(inf[236] + inf[237] + inf[238] + inf[239], System.Globalization.NumberStyles.HexNumber);
            tit[1] = var;
            Debug.Log("Turbine inlet temp:" + tit[1]);

            var = Int32.Parse(inf[240] + inf[241] + inf[242] + inf[243], System.Globalization.NumberStyles.HexNumber);
            tit[2] = var;
            Debug.Log("Turbine inlet temp:" + tit[2]);

            var = Int32.Parse(inf[244] + inf[245] + inf[246] + inf[247], System.Globalization.NumberStyles.HexNumber);
            tit[3] = var;
            Debug.Log("Turbine inlet temp:" + tit[3]);

            var = Int32.Parse(inf[248] + inf[249] + inf[250] + inf[251], System.Globalization.NumberStyles.HexNumber);
            oil_temp[0] = var;
            Debug.Log("Oil temp:" + oil_temp[0]);

            var = Int32.Parse(inf[252] + inf[253] + inf[254] + inf[255], System.Globalization.NumberStyles.HexNumber);
            oil_temp[1] = var;
            Debug.Log("Oil temp:" + oil_temp[1]);

            var = Int32.Parse(inf[256] + inf[257] + inf[258] + inf[259], System.Globalization.NumberStyles.HexNumber);
            oil_temp[2] = var;
            Debug.Log("Oil temp:" + oil_temp[2]);

            var = Int32.Parse(inf[260] + inf[261] + inf[262] + inf[263], System.Globalization.NumberStyles.HexNumber);
            oil_temp[3] = var;
            Debug.Log("Oil temp:" + oil_temp[3]);

            var = Int32.Parse(inf[264] + inf[265] + inf[266] + inf[267], System.Globalization.NumberStyles.HexNumber);
            oil_px[0] = var;
            Debug.Log("Oil pressure:" + oil_px[0]);

            var = Int32.Parse(inf[268] + inf[269] + inf[270] + inf[271], System.Globalization.NumberStyles.HexNumber);
            oil_px[1] = var;
            Debug.Log("Oil pressure:" + oil_px[1]);

            var = Int32.Parse(inf[272] + inf[273] + inf[274] + inf[275], System.Globalization.NumberStyles.HexNumber);
            oil_px[2] = var;
            Debug.Log("Oil pressure:" + oil_px[2]);


            var = Int32.Parse(inf[276] + inf[277] + inf[278] + inf[279], System.Globalization.NumberStyles.HexNumber);
            oil_px[3] = var;
            Debug.Log("Oil pressure:" + oil_px[3]);

            a = int.Parse(inf[280] + inf[281] + inf[282] + inf[283], System.Globalization.NumberStyles.HexNumber);
            num_tanks = Convert.ToUInt32(a);
            Debug.Log("Max fuel tanks:" + num_tanks);

            var = Int32.Parse(inf[284] + inf[285] + inf[286] + inf[287], System.Globalization.NumberStyles.HexNumber);
            fuel_quantity[0] = var;
            Debug.Log("Fuel quantity:" + fuel_quantity[0]);

            var = Int32.Parse(inf[288] + inf[289] + inf[290] + inf[291], System.Globalization.NumberStyles.HexNumber);
            fuel_quantity[1] = var;
            Debug.Log("Fuel quantity:" + fuel_quantity[1]);

            var = Int32.Parse(inf[292] + inf[293] + inf[294] + inf[295], System.Globalization.NumberStyles.HexNumber);
            fuel_quantity[2] = var;
            Debug.Log("Fuel quantity:" + fuel_quantity[2]);

            var = Int32.Parse(inf[296] + inf[297] + inf[298] + inf[299], System.Globalization.NumberStyles.HexNumber);
            fuel_quantity[3] = var;
            Debug.Log("Fuel quantity:" + fuel_quantity[3]);

            a = int.Parse(inf[300] + inf[301] + inf[302] + inf[303], System.Globalization.NumberStyles.HexNumber);
            num_wheels = Convert.ToUInt32(a);
            Debug.Log("No of wheels:" + num_wheels);

            a = int.Parse(inf[304] + inf[305] + inf[306] + inf[307], System.Globalization.NumberStyles.HexNumber);
            wow[0] = Convert.ToUInt32(a);
            Debug.Log("Wow:" + wow[0]);

            a = int.Parse(inf[308] + inf[309] + inf[310] + inf[311], System.Globalization.NumberStyles.HexNumber);
            wow[1] = Convert.ToUInt32(a);
            Debug.Log("Wow:" + wow[1]);

            a = int.Parse(inf[312] + inf[313] + inf[314] + inf[315], System.Globalization.NumberStyles.HexNumber);
            wow[2] = Convert.ToUInt32(a);
            Debug.Log("Wow:" + wow[2]);

            var = Int32.Parse(inf[316] + inf[317] + inf[318] + inf[319], System.Globalization.NumberStyles.HexNumber);
            gear_pos[0] = var;
            Debug.Log("Gear pos:" + gear_pos[0]);

            var = Int32.Parse(inf[320] + inf[321] + inf[322] + inf[323], System.Globalization.NumberStyles.HexNumber);
            gear_pos[1] = var;
            Debug.Log("Gear pos:" + gear_pos[1]);

            var = Int32.Parse(inf[324] + inf[325] + inf[326] + inf[327], System.Globalization.NumberStyles.HexNumber);
            gear_pos[2] = var;
            Debug.Log("Gear pos:" + gear_pos[2]);

            var = Int32.Parse(inf[328] + inf[329] + inf[330] + inf[331], System.Globalization.NumberStyles.HexNumber);
            gear_steer[0] = var;
            Debug.Log("Gear steer:" + gear_steer[0]);

            var = Int32.Parse(inf[332] + inf[333] + inf[334] + inf[335], System.Globalization.NumberStyles.HexNumber);
            gear_steer[1] = var;
            Debug.Log("Gear steer:" + gear_steer[1]);

            var = Int32.Parse(inf[336] + inf[337] + inf[338] + inf[339], System.Globalization.NumberStyles.HexNumber);
            gear_steer[2] = var;
            Debug.Log("Gear steer:" + gear_steer[2]);

            var = Int32.Parse(inf[340] + inf[341] + inf[342] + inf[343], System.Globalization.NumberStyles.HexNumber);
            gear_compression[0] = var;
            Debug.Log("Gear compression:" + gear_compression[0]);

            var = Int32.Parse(inf[344] + inf[345] + inf[346] + inf[347], System.Globalization.NumberStyles.HexNumber);
            gear_compression[1] = var;
            Debug.Log("Gear compression:" + gear_compression[1]);

            var = Int32.Parse(inf[348] + inf[349] + inf[350] + inf[351], System.Globalization.NumberStyles.HexNumber);
            gear_compression[2] = var;
            Debug.Log("Gear compression:" + gear_compression[2]);

            a = int.Parse(inf[352] + inf[353] + inf[354] + inf[355], System.Globalization.NumberStyles.HexNumber);
            cur_time = Convert.ToUInt32(a);
            Debug.Log("Cur time:" + cur_time);

            a = int.Parse(inf[356] + inf[357] + inf[358] + inf[359], System.Globalization.NumberStyles.HexNumber);
            warp = Convert.ToInt32(a);
            Debug.Log("Wrap:" + warp);

            var = Int32.Parse(inf[360] + inf[361] + inf[362] + inf[363], System.Globalization.NumberStyles.HexNumber);
            visibility = var;
            Debug.Log("Visibility:" + visibility);

            var = Int32.Parse(inf[364] + inf[365] + inf[366] + inf[367], System.Globalization.NumberStyles.HexNumber);
            elevator = var;
            Debug.Log("Elevator:" + elevator);

            var = Int32.Parse(inf[368] + inf[369] + inf[370] + inf[371], System.Globalization.NumberStyles.HexNumber);
            elevator_trim_tab = var;
            Debug.Log("Elevator trim tab:" + elevator_trim_tab);

            var = Int32.Parse(inf[372] + inf[373] + inf[374] + inf[375], System.Globalization.NumberStyles.HexNumber);
            left_flap = var;
            Debug.Log("Left flap:" + left_flap);

            var = Int32.Parse(inf[376] + inf[377] + inf[378] + inf[379], System.Globalization.NumberStyles.HexNumber);
            right_flap = var;
            Debug.Log("Right flap:" + right_flap);

            var = Int32.Parse(inf[380] + inf[381] + inf[382] + inf[383], System.Globalization.NumberStyles.HexNumber);
            left_aileron = var;
            Debug.Log("Left aileron:" + left_aileron);

            var = Int32.Parse(inf[384] + inf[385] + inf[386] + inf[387], System.Globalization.NumberStyles.HexNumber);
            right_aileron = var;
            Debug.Log("Right aileron:" + right_aileron);

            var = Int32.Parse(inf[388] + inf[389] + inf[390] + inf[391], System.Globalization.NumberStyles.HexNumber);
            rudder = var;
            Debug.Log("Rudder:" + rudder);

            var = Int32.Parse(inf[392] + inf[393] + inf[394] + inf[395], System.Globalization.NumberStyles.HexNumber);
            nose_wheel = var;
            Debug.Log("Nose wheel:" + nose_wheel);

            var = Int32.Parse(inf[396] + inf[397] + inf[398] + inf[399], System.Globalization.NumberStyles.HexNumber);
            speedbrake = var;
            Debug.Log("Speed brake:" + speedbrake);

            var = Int32.Parse(inf[400] + inf[401] + inf[402] + inf[403], System.Globalization.NumberStyles.HexNumber);
            spoilers = var;
            Debug.Log("Spoilers:" + spoilers);

        }

        catch
        {
            i=i+10;
            b = new byte[i];

        }

    }
}

