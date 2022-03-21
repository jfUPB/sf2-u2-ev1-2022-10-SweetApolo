using System;
using UnityEngine;
using System.IO.Ports;

public class SerialThreadBinary : AbstractSerialThread
{
    private byte[] buffer = new byte[12];

    public SerialThreadBinary(string portName,
                                       int baudRate,
                                       int delayBeforeReconnecting,
                                       int maxUnreadMessages,
                                       bool dropOldMessage)
        : base(portName, baudRate, delayBeforeReconnecting, maxUnreadMessages, false, dropOldMessage)
    {

    }

    protected override void SendToWire(object message, SerialPort serialPort)
    {
        byte[] binaryMessage = (byte[])message;
        serialPort.Write(binaryMessage, 0, binaryMessage.Length);
    }

    protected override object ReadFromWire(SerialPort serialPort)
    {
        byte[] buff = null; 
        

        if (serialPort.BytesToRead > 2)
        {
            serialPort.Read(buffer, 0, 3);
            byte[] returnBuffer = new byte[3];
            System.Array.Copy(buffer, returnBuffer, 3);
            return returnBuffer;
        }

        return buff;

    }
}
