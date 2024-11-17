using System;
using System.Collections.Generic;

public abstract class SmartDevice
{
    public string Name { get; set; }
    public bool IsOn { get; set; }
    public DateTime TimeOn { get; set; }

    public SmartDevice(string name)
    {
        Name = name;
        IsOn = false;
        TimeOn = DateTime.MinValue; }

   
    public abstract void TurnOn();
    public abstract void TurnOff();
    public abstract string GetStatus();
    public abstract double GetTimeOn();
}

public class SmartLight : SmartDevice
{
    public SmartLight(string name) : base(name) { }

    public override void TurnOn()
    {
        IsOn = true;
        TimeOn = DateTime.Now;
    }

    public override void TurnOff()
    {
        IsOn = false;
        TimeOn = DateTime.MinValue; 
        // Reset time when turned off
    }

    public override string GetStatus()
    {
        return IsOn ? "On" : "Off";
    }

    public override double GetTimeOn()
    {
        if (IsOn)
        {
            return (DateTime.Now - TimeOn).TotalSeconds;
        }
        return 0;
    }
}

public class SmartHeater : SmartDevice
{
    public SmartHeater(string name) : base(name) { }

    public override void TurnOn()
    {
        IsOn = true;
        TimeOn = DateTime.Now;
    }

    public override void TurnOff()
    {
        IsOn = false;
        TimeOn = DateTime.MinValue;
    }

    public override string GetStatus()
    {
        return IsOn ? "On" : "Off";
    }

    public override double GetTimeOn()
    {
        if (IsOn)
        {
            return (DateTime.Now - TimeOn).TotalSeconds;
        }
        return 0;
    }
}

public class SmartTV : SmartDevice
{
    public SmartTV(string name) : base(name) { }

    public override void TurnOn()
    {
        IsOn = true;
        TimeOn = DateTime.Now;
    }

    public override void TurnOff()
    {
        IsOn = false;
        TimeOn = DateTime.MinValue;
    }

    public override string GetStatus()
    {
        return IsOn ? "On" : "Off";
    }

    public override double GetTimeOn()
    {
        if (IsOn)
        {
            return (DateTime.Now - TimeOn).TotalSeconds;
        }
        return 0;
    }
}

public class Room
{
    public string Name { get; set; }
    private List<SmartDevice> devices;

    public Room(string name)
    {
        Name = name;
        devices = new List<SmartDevice>();
    }

    public void AddDevice(SmartDevice device)
    {
        devices.Add(device);
    }

    public void TurnOnAllLights()
    {
        foreach (var device in devices)
        {
            if (device is SmartLight)
                device.TurnOn();
        }
    }

    public void TurnOffAllLights()
    {
        foreach (var device in devices)
        {
            if (device is SmartLight)
                device.TurnOff();
        }
    }

    public void TurnOnOffDevice(string deviceName, bool turnOn)
    {
        foreach (var device in devices)
        {
            if (device.Name == deviceName)
            {
                if (turnOn)
                    device.TurnOn();
                else
                    device.TurnOff();
            }
        }
    }

    public void TurnOnOffAllDevices(bool turnOn)
    {
        foreach (var device in devices)
        {
            if (turnOn)
                device.TurnOn();
            else
                device.TurnOff();
        }
    }

    public void ReportAllItems()
    {
        foreach (var device in devices)
        {
            Console.WriteLine($"{device.Name} - {device.GetStatus()}");
        }
    }

    public void ReportItemsOn()
    {
        foreach (var device in devices)
        {
            if (device.IsOn)
            {
                Console.WriteLine($"{device.Name} is On");
            }
        }
    }

    public void ReportLongestOnDevice()
    {
        SmartDevice longestOnDevice = null;
        foreach (var device in devices)
        {
            if (longestOnDevice == null || (device.IsOn && device.GetTimeOn() > longestOnDevice.GetTimeOn()))
            {
                longestOnDevice = device;
            }
        }

        if (longestOnDevice != null)
        {
            Console.WriteLine($"{longestOnDevice.Name} has been on for {longestOnDevice.GetTimeOn()} seconds.");
        }
    }
}

public class House
{
    private List<Room> rooms;

    public House()
    {
        rooms = new List<Room>();
    }

    public void AddRoom(Room room)
    {
        rooms.Add(room);
    }

    public void ReportAllRoomsStatus()
    {
        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name}");
            room.ReportAllItems();
        }
    }

    public void TurnOnOffAllDevices(bool turnOn)
    {
        foreach (var room in rooms)
        {
            room.TurnOnOffAllDevices(turnOn);
        }
    }
}

public class Program
{
    public static void Main()
    {
    
        SmartLight light = new SmartLight("Living Room Light");
        SmartHeater heater = new SmartHeater("Living Room Heater");
        SmartTV tv = new SmartTV("Living Room TV");

        Room livingRoom = new Room("Living Room");
        livingRoom.AddDevice(light);
        livingRoom.AddDevice(heater);
        livingRoom.AddDevice(tv);

        House myHouse = new House();
        myHouse.AddRoom(livingRoom);


        light.TurnOn();
        heater.TurnOn();
        myHouse.ReportAllRoomsStatus();
        heater.TurnOff();
        myHouse.ReportAllRoomsStatus();
    }
}