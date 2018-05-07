using UnityEngine;  
using System.Collections;  
//引入庫  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;  

public class Clientsc : MonoBehaviour  
{  
	string editString="hello wolrd"; //編輯框文字  

	Socket serverSocket; //服務器端socket  
	IPAddress ip; //主機ip  
	IPEndPoint ipEnd;   
	string recvStr; //接收的字符串  
	string sendStr; //發送的字符串  
	byte[] recvData=new byte[1024]; //接收的數據，必須為字節  
	byte[] sendData=new byte[1024]; //發送的數據，必須為字節  
	int recvLen; //接收的數據長度  
	Thread connectThread; //連接線程  

	//初始化  
	void InitSocket()  
	{  
		//定義服務器的IP和端口，端口與服務器對應  
		ip=IPAddress.Parse("127.0.0.1"); //可以是局域網或互聯網ip，此處是本機  
		ipEnd=new IPEndPoint(ip,25566);  


		//開啟一個線程連接，必須的，否則主線程卡死  
		connectThread=new Thread(new ThreadStart(SocketReceive));  
		connectThread.Start();  
	}  

	void SocketConnet()  
	{  
		if(serverSocket!=null)  
			serverSocket.Close();  
		//定義套接字類型,必須在子線程中定義  
		serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);  
		print("ready to connect");  
		//連接  
		serverSocket.Connect(ipEnd);  

		//輸出初次連接收到的字符串  
		recvLen=serverSocket.Receive(recvData);  
		recvStr=Encoding.ASCII.GetString(recvData,0,recvLen);  
		print(recvStr);  
	}  

	void SocketSend(string sendStr)  
	{  
		//清空發送緩存  
		sendData=new byte[1024];  
		//數據類型轉換  
		sendData=Encoding.ASCII.GetBytes(sendStr);  
		//發送  
		serverSocket.Send(sendData,sendData.Length,SocketFlags.None);  
	}  

	void SocketReceive()  
	{  
		SocketConnet();  
		//不斷接收服務器發來的數據  
		while(true)  
		{  
			recvData=new byte[1024];  
			recvLen=serverSocket.Receive(recvData);  
			if(recvLen==0)  
			{  
				SocketConnet();  
				continue;  
			}  
			recvStr=Encoding.ASCII.GetString(recvData,0,recvLen);  
			print(recvStr);  
		}  
	}  

	void SocketQuit()  
	{  
		//關閉線程  
		if(connectThread!=null)  
		{  
			connectThread.Interrupt();  
			connectThread.Abort();  
		}  
		//最後關閉服務器  
		if(serverSocket!=null)  
			serverSocket.Close();  
		print("diconnect");  
	}  

	// Use this for initialization  
	void Start()  
	{  
		InitSocket();  
	}  

	void OnGUI()  
	{  
		editString=GUI.TextField(new Rect(10,10,100,20),editString);  
		if(GUI.Button(new Rect(10,30,60,20),"send"))  
			SocketSend(editString);  
	}  

	// Update is called once per frame  
	void Update()  
	{  

	}  

	//程序退出則關閉連接  
	void OnApplicationQuit()  
	{  
		SocketQuit();  
	}  
}  