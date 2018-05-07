using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;


public class MyTcpClient : MonoBehaviour {

	public delegate void RecieveTouchEvent(ArgsPosition[] args);
	public static event RecieveTouchEvent recieveTouchEvent;

	public static MyTcpClient curent;

	public InputField ifield;
	Button _sender;
	bool turnOnSender = false;

	private String host;
	private Int32 port = 25568;

	internal Boolean socket_ready = false;
	internal String input_buffer = "";
	TcpClient tcp_socket;
	NetworkStream net_stream;

	StreamWriter socket_writer;
	StreamReader socket_reader;

	Thread clientSock;

	void Awake(){
		curent = this;
	}

	void Start(){
		ifield.text = PlayerPrefs.GetString("IPHost", "127.0.0.1");
		host = ifield.text;
	}

	void Update(){
		if (turnOnSender) {
			_sender.interactable = true;
			turnOnSender = false;
		}
	}

	public void InitTCP(Button sender)
	{
		setupSocket ();

		if (socket_ready) {
			Debug.Log ("Connect to Server at port :" + port);
			clientSock = new Thread (readSocket);
			clientSock.Start ();
			sender.interactable = false;
			_sender = sender;
			//InvokeRepeating("IntervalOrienSend", 2f, 0.2f);
		}
	}

	public void SetHost(string s){
		host = s;
		PlayerPrefs.SetString("IPHost", s);
	}

	void OnApplicationQuit()
	{
		if(clientSock != null)
			clientSock.Abort ();
		closeSocket();
	}

	//Glass Client just need to Receive Touch data from Server
	public void readSocket()
	{
		while (true) {
			Thread.Sleep (20);

			if (socket_ready) {

				Byte[] data = new Byte[256];
				String responseData = String.Empty;
				Int32 bytes = 0;
				
				try
				{
					// *** networkStream.Read will let programe get Stuck ***
					bytes = net_stream.Read(data, 0, data.Length);
				} catch (System.IO.IOException)
				{
					print("Disconnect Exception");
					turnOnSender = true;
					closeSocket();
					break;
				}
				if(bytes == 0)
				{
					print("Disconnect Action");
					turnOnSender = true;
					closeSocket();
					break;
				}

				responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);

				//Recieve Data Will Be   245,135,90[/TCP]   , str 不會包含[/TCP]
				char delimiter = ',';
				char delimiterEnd = '[';
				string[] clearString = responseData.Split (delimiterEnd);  // => 245,135,90
				string[] substrings = clearString [0].Split (delimiter); // => 245  135  90

				if (substrings.Length > 2) {

					int DataNum = 0;
					int.TryParse (substrings [0], out DataNum);

					Debug.Log ("Data Number : " + DataNum);

					if (DataNum == 0 || substrings.Length < DataNum*2 + 1)
						continue;

					ArgsPosition[] myArgs = new ArgsPosition[DataNum];
					for (int i = 0; i < DataNum; i++) {

						myArgs [i] = new ArgsPosition ();
						myArgs [i].x = System.Convert.ToInt32 (substrings [1 + i*2]);
						myArgs [i].y = System.Convert.ToInt32 (substrings [2 + i*2]);
					}

					if (recieveTouchEvent != null) {
						recieveTouchEvent.Invoke (myArgs);
					}

				} // end Length

			}
		}
	}

	public void setupSocket()
	{
		try
		{
			tcp_socket = new TcpClient(host, port);

			net_stream = tcp_socket.GetStream();
			socket_writer = new StreamWriter(net_stream);
			socket_reader = new StreamReader(net_stream);

			socket_ready = true;
		}
		catch (Exception e)
		{
			// Something went wrong
			Debug.Log("Socket error: " + e);
		}
	}

	public void writeSocket(string line)
	{
		if (!socket_ready)
			return;

		line = line + "[/TCP]";
		socket_writer.Write(line);
		socket_writer.Flush();
		Debug.Log ("Write:" + line);
	}

	public void closeSocket()
	{
		if (!socket_ready)
			return;

		socket_writer.Close();
		socket_reader.Close();
		tcp_socket.Close();
		socket_ready = false;
		//CancelInvoke("IntervalOrienSend");
	}


	void IntervalOrienSend(){
		//writeSocket ("Hello");
		//writeSocket ("5:"+CallJava.current.Orien_Z);
	}
}