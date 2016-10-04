using System.Threading;
using OpenHardwareMonitor.Hardware;

namespace Hardware_Monitor
{
    class AsyncWriteData  
    {
        Computer mycomputer;

        public AsyncWriteData()
        {
            mycomputer = new Computer();
        }

        public void writeData()
        {
            mycomputer.Open();
            mycomputer.GPUEnabled = true;
            mycomputer.CPUEnabled = true;
            mycomputer.RAMEnabled = true;

            while (true)
            {
                executeWriteData();
                Thread.Sleep(750);
            }
        }

        public object getCpuCounter()
        {
            string output = "";

            foreach (var hardwareitem in mycomputer.Hardware)
            {
                if (hardwareitem.HardwareType == HardwareType.CPU)
                {
                    hardwareitem.Update();
                    foreach (IHardware subHardware in hardwareitem.SubHardware)
                        subHardware.Update();
                    foreach (var sensor in hardwareitem.Sensors)
                    {
                        if (sensor.Name == "CPU Total")
                        {
                            sensor.Hardware.Update();
                            output = sensor.Value.ToString();
                        }
                    }
                }
            }
            return output;
        }

        public object getGpuTemp()
        {
            string output = "";


            foreach (var hardwareitem in mycomputer.Hardware)
            {
                if (hardwareitem.HardwareType == HardwareType.GpuAti || hardwareitem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareitem.Update();
                    foreach (IHardware subHardware in hardwareitem.SubHardware)
                        subHardware.Update();
                    foreach (var sensor in hardwareitem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (sensor.Name == "GPU Core")
                            {
                                sensor.Hardware.Update();
                                output = sensor.Value.ToString();
                            }
                        }
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Get RAM Available
        /// </summary>
        /// <returns></returns>
        public object getRamCounter()
        {
            string output = "";

            foreach (var hardwareitem in mycomputer.Hardware)
            {
                if (hardwareitem.HardwareType == HardwareType.RAM)
                {
                    hardwareitem.Update();
                    foreach (IHardware subHardware in hardwareitem.SubHardware)
                        subHardware.Update();
                    foreach (var sensor in hardwareitem.Sensors)
                    {
                        if (sensor.Name == "Available Memory")
                        {
                            sensor.Hardware.Update();
                            output = (sensor.Value * 1000).ToString();
                        }
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Get GPU Usage
        /// </summary>
        /// <returns></returns>
        public object getGpuCounter()
        {
            string output = "";


            foreach (var hardwareitem in mycomputer.Hardware)
            {
                if (hardwareitem.HardwareType == HardwareType.GpuAti || hardwareitem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareitem.Update();
                    foreach (IHardware subHardware in hardwareitem.SubHardware)
                        subHardware.Update();
                    foreach (var sensor in hardwareitem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (sensor.Name == "GPU Core")
                            {
                                sensor.Hardware.Update();
                                output = sensor.Value.ToString();
                            }
                        }
                    }
                }
            }
            return output;
        }

        public object getGpuCoreCounter()
        {
            string output = "";


            foreach (var hardwareitem in mycomputer.Hardware)
            {
                if (hardwareitem.HardwareType == HardwareType.GpuAti || hardwareitem.HardwareType == HardwareType.GpuNvidia)
                {
                    hardwareitem.Update();
                    foreach (IHardware subHardware in hardwareitem.SubHardware)
                        subHardware.Update();
                    foreach (var sensor in hardwareitem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            if (sensor.Name == "GPU Core")
                            {
                                sensor.Hardware.Update();
                                output = sensor.Value.ToString();
                            }
                        }
                    }
                }
            }
            return output;
        }

        private void executeWriteData()
        {
            string cpuuse = "";
            string ramuse = "";
            string gpuuse = "";
            string gpucore = "";
            string gputempuse = "";

            cpuuse = getCpuCounter().ToString();
            ramuse = getRamCounter().ToString();
            gpuuse = getGpuCounter().ToString();
            gpucore = getGpuCoreCounter().ToString();
            gputempuse = getGpuTemp().ToString();

            string lines = "CPU Usage: " + cpuuse + "%!" + "RAM Free: " + ramuse + "MB!" + "GPU Usage: " + gpuuse + "%!Core Clock: " + gpucore + "!GPU Temp: " + gputempuse + "C";

            System.IO.File.WriteAllText("C:\\Users\\ryanm\\Documents\\Programming\\Java\\ServerClient\\Server\\Usage.txt", lines);
        }
    }
}
