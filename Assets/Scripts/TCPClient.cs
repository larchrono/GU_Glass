using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;


public class TCPClient : MonoBehaviour {

	public static event EventHandler<OrienArgs> recievePosition;

	public InputField ifield;

	private String host;
	private Int32 port = 25566;

	internal Boolean socket_ready = false;
	internal String input_buffer = "";
	TcpClient tcp_socket;
	NetworkStream net_stream;

	StreamWriter socket_writer;
	StreamReader socket_reader;

	Thread clientSock;

	void Start(){
		host = ifield.text;
	}


	public void InitTCP(Button sender)
	{
		setupSocket ();

		if (socket_ready) {
			clientSock = new Thread (readSocket);
			clientSock.Start ();
			sender.interactable = false;

			InvokeRepeating("IntervalOrienSend", 2f, 0.2f);
		}
	}

	public void SetHost(string s)
	{
		host = s;
	}

	void OnApplicationQuit()
	{
		if(clientSock != null)
			clientSock.Abort ();
		closeSocket();
	}

	public void readSocket()
	{
		while (true) {
			Thread.Sleep (20);

			Debug.Log ("in thread");

			if (socket_ready) {
				Debug.Log ("in thread Ready");

				Byte[] data = new Byte[256];
				String responseData = String.Empty;
				Int32 bytes = 0;

				// *** networkStream.Read will let programe get Stuck ***
				bytes = net_stream.Read(data, 0, data.Length);

				responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

				//Debug.Log (responseData);

				//Recieve Data Will Be   245,135,90[/TCP]   , str 不會包含[/TCP]
				Char delimiter = ',';
				Char delimiterEnd = '[';
				String[] clearString = responseData.Split(delimiterEnd);
				String[] substrings = clearString[0].Split(delimiter);

				if (substrings.Length > 2) {
					Debug.Log (substrings [0] + " _ " + substrings [1] + " _ " + substrings [2]);
					if (recievePosition != null) {
						recievePosition.Invoke (this, new OrienArgs(substrings [0],substrings [1],substrings [2]));
					}
				}
				else {
					Debug.Log (substrings [0] + " _ " + substrings [1]);
					if (recievePosition != null) {
						recievePosition.Invoke (this, new OrienArgs(substrings [0],substrings [1],"0"));
					}
				}
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
		CancelInvoke("IntervalOrienSend");
	}


	void IntervalOrienSend(){
		//writeSocket ("Hello");
		writeSocket ("5:"+CallJava.current.Orien_Z);
	}
}