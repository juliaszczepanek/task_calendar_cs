using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace todolistUpdated
{
	public class Reader
	{
        public static List<Task> ReadFile()
        {
            string path = "taskcalendar.txt";

            bool fileExist = File.Exists(path);
            if (!fileExist)
            {
                FileStream fs = File.Create(path);
            }

            List<Task> tasks = new List<Task>();

            using (StreamReader sreader = File.OpenText(path))
            {             
                string line = " ";

                if ((line = sreader.ReadLine()) == null)
                {
                    Console.WriteLine("file is empty");
                }

                string[] lines = File.ReadAllLines(path);
                string[,] content = new string[lines.Length, 3]; 

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] values = lines[i].Split(' ');

                        if (values.Length > 0)
                        {
                            content[i, 0] = values[0];
                        }

                        if (values.Length > 1)
                        {
                            content[i, 1] = values[1];  
                        }

                        if (values.Length > 2)
                        {
                            content[i, 2] = string.Join(" ", values.Skip(2)); 
                        }
                    }

                    for (int b = 0; b < lines.Length; b++)
                    {
                        string date;
                        date = content[b, 0];
                        DateTime dateOfTask = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        string priorityOfTask;
                        priorityOfTask = content[b, 1];
                        TaskPriority priority;

                        switch (priorityOfTask)
                        {
                            case "Low":
                                priority = TaskPriority.Low;
                                break;
                            case "Medium":                           
                                priority = TaskPriority.Medium;
                                break;
                            case "High":                           
                                priority = TaskPriority.High;
                                break;
                            default:
                                priority = TaskPriority.Medium;
                                break;
                        }

                        string description;
                        description = content[b, 2];
                        Task task = new Task(dateOfTask, priority, description);
                        tasks.Add(task);
                    }
                
                return tasks;                
            }
        }

        public static void ClearFile()
        {
            string path = "taskcalendar.txt";
            System.IO.File.WriteAllText(path, string.Empty);
        }
            
	}

    public class Writer
    {
        public static void WriteToFile(List<Task> tasks)
        {
            string path = "taskcalendar.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                foreach (var task in tasks)
                {

                    writer.WriteLine($"{task.DateOfTask.ToString("dd/MM/yyyy")} {task.PriorityOfTask} {task.Description}");
                }
                writer.Close();
                writer.Dispose();
            }
        }

        //public static void WriteToFileAll(List<Task> listOfTask)
        //{
        //    string path = "taskcalendar.txt";
        //    using (StreamWriter writer = new StreamWriter(path, true))
        //    {
        //        foreach (var task in listOfTask)
        //        {
        //            writer.WriteLine($"{listOfTask}");
        //        }
        //        writer.Close();
        //        writer.Dispose();
        //    }
        //}
    }

}

