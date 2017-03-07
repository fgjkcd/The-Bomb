using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

/* This class provides blob data for the game (you just lost it) */
/* Sie baut einen TCP Socket auf Port 5050 auf ( = Port, der in Processing zum Senden benutzt wird) und nimmt Daten aus der Verbindung entgegen) */

/* Use the Blobs class to access blobdata from anywhere with Blobs.blobs */
/* Use foreach(Blob blob in Blobs.blobs) { } to loop through blobs (and also laugh while writing it) */
/* Use the center variable ( blob.center ) to access the coordinates of a blob */

public class TCPListener : ConcurrentWorker {
	//Network
	internal Boolean socketReady = false;
	TcpClient mySocket;
	NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	String Host = "localhost";
	Int32 Port = 5050;
	String theLine;

	//Blobdata
	List<Blob> blobs = new List<Blob>();
	//How many pieces of information per blob
	//int dataamount = 2;

	protected override void init() {
		closeSocket();
		setupSocket();
	}

	protected override bool loop(bool interrupted) {
		//print("Reading socket...");
		readSocket();
		return true;
	}

	// **********************************************

	public void setupSocket() {
		try {
			mySocket = new TcpClient(Host, Port);
			theStream = mySocket.GetStream();
			theWriter = new StreamWriter(theStream);
			theReader = new StreamReader(theStream);
			socketReady = true;
			Debug.Log("setup successful");
		}
		catch (Exception e) {
			Debug.Log("Socket error: " + e);
		}
	}

	public void writeSocket(string theLine) {
		if (!socketReady)
			return;
		String foo = theLine + "\r\n";
		theWriter.Write(foo);
		theWriter.Flush();
	}

	public String readSocket() {
		//print("readSocket");
		if (!socketReady)
		{
			//            print("not ready");
			return "";
		}
		if (theStream.DataAvailable) {
			//print("data available");
			theLine = theReader.ReadLine();
			SetBlobData(theLine);
//			           print(theLine);
			return theReader.ReadLine();

		}

		//print("neither");

		return "";
	}

	public void closeSocket() {
		if (!socketReady) {
			return;
		}

		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
	}

	// Sanitize data if I can bother to write a function for it
	// Fuck it
	public void SanitizeBlobData()  {

	}

	// Map the data received by the listener to actual blobs and players
	public void SetBlobData(String data) {
		//Debug.Log(data);

		String[] split_data = data.Split(' ');
		float[] int_data = new float[split_data.Length];

		//Transform string array into int array
		for(int i = 0; i < split_data.Length; i++) {
			int_data[i] = Convert.ToSingle(split_data[i]);
		}

		//		Blobs.badData = false;
		//		if ((split_data.Length - 1) % 6 != 0 || data.StartsWith(" ")) {
		//			print("Dropping invalid data: " + data);
		//			Blobs.badData = true;
		//			return;
		//		}



		//Get blob amount
		int amount = (int) int_data[0];

		//print("-------");
		//print(data);
		//print("amount: " + amount + ", int_data.Length: " + int_data.Length);
		//print(amount);

		//Maybe save blobs temporarily for rudimentary checks (assign blobs to nearest positions and such)

		//Reset blobs
		blobs.Clear();

		//Set blob data
		for(int i = 0; i < amount; i++) {
			blobs.Add(new Blob( new Vector2(int_data[1 + i * 2], int_data[2 + i * 2])));
		}

		foreach(Blob blob in blobs) {
			//print(blob.center);
		}

		//wat
		Blobs.setBlobs(blobs);

	}

}


//Blobs are a basic class with a few variables

public class Blob
{
	public Vector2 center;

	public Blob(Vector2 centerpoint)
	{
		center = centerpoint;
	}

	public override string ToString()
	{
		return center.ToString();
	}
}

//Blobs can be accessed from anywhere
public static class Blobs
{
	private static List<Blob> blobs = new List<Blob>();
	public static bool badData, newData;

    private static Blob[] empty = new Blob[0];

	public static void setBlobs(List<Blob> newBlobs) {
		lock (blobs) {
			blobs = newBlobs;
            newData = true;
		}
	}

	public static Blob[] getBlobs() {
        if (!newData)
            return empty;

        Blob[] result;
		lock (blobs) {
			result = new Blob[blobs.Count];
			blobs.CopyTo(result);
            newData = false;
		}
		return result;
	}

}
