using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static todolistUpdated.Task;

namespace todolistUpdated
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public class Task
    {

        public string Description;
        public DateTime DateOfTask;
        public TaskPriority PriorityOfTask;

        public Task(DateTime dateOfTask, TaskPriority priorityOfTask, string description)
        {

            DateOfTask = dateOfTask;
            PriorityOfTask = priorityOfTask;
            Description = description;
        }
    }

    public class Calendar
    {
        private List<Task> tasks = new List<Task>();

        private List<Task> listOfTasks = Reader.ReadFile();

        public void AddTask()
		    {
                int listOfTasksCount = listOfTasks.Count;
                bool badValue = true;
                while (badValue == true)
                {
                    try
                    {
                        Console.WriteLine("Wprowadź datę zadania w formacie dd/mm/rrrr: ");
                        string date = Console.ReadLine();
                        DateTime dateOfTask = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        Console.WriteLine("Wprowadż treść zadania: ");
                        string description = Console.ReadLine();
                        Console.WriteLine("Określ priorytet zadania (high - h|medium - m|low - l): ");
                        string priorityOfTask = Console.ReadLine().ToLower();
                        TaskPriority priority;
                        switch (priorityOfTask)
                        {
                            case "low":
                            case "l":
                                priority = TaskPriority.Low;
                                break;
                            case "medium":
                            case "m":
                                priority = TaskPriority.Medium;
                                break;
                            case "high":
                            case "h":
                                priority = TaskPriority.High;
                                break;
                            default:
                                priority = TaskPriority.Medium;
                                return;
                        }

                        Task task = new Task(dateOfTask, priority, description);
                        tasks.Add(task);
                        Writer.WriteToFile(tasks);
                        tasks.Remove(task);
                        badValue = false;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                    }
                }
                
                }

        public void DisplayAllTasks()
        {
            List<Task> listOfTasks = Reader.ReadFile();

            if (listOfTasks.Count == 0) 
            {
                Console.WriteLine("Kalendarz jest pusty");
            }
            else
            {
                int i = 1;
                Console.WriteLine("Lista zadań: ");
                foreach (var task in listOfTasks)
                {
                    Console.WriteLine($" [{i}] {task.DateOfTask.ToString("dd/MM/yyyy")}, {task.PriorityOfTask}, {task.Description}");
                    i++;
                }
            }
        }


        public void EditTask()
        {
            DisplayAllTasks(); //wyświetlanie tasków
            Console.WriteLine("Wybierz numer zadania które chcesz edytować: ");

            try
            {
                string lineToEdit = Console.ReadLine();
                int line = int.Parse(lineToEdit);
                line = line -1;

                if (line < 0 || line >= listOfTasks.Count)
                {
                    Console.WriteLine("Wpisałeś niepoprawną wartość");
                    return;
                }

                Task taskToEdit = listOfTasks[line];


                Console.WriteLine($"Wprowadź nową datę zadania, poprzednia data to: {taskToEdit.DateOfTask.ToString("dd/MM/yyyy")}");
                string date = Console.ReadLine();
                DateTime dateOfTask = DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                taskToEdit.DateOfTask = dateOfTask;

                Console.WriteLine($"Wprowadź nową treść zadania, poprzednia treść to: {taskToEdit.Description}");
                taskToEdit.Description = Console.ReadLine();

                //if(taskToEdit.Description == " ")
                //{
                //    taskToEdit.Description == 
                //}

                Console.WriteLine($"Wprowadź nowy priorytet zadania, poprzedni priorytet to: {taskToEdit.PriorityOfTask}");
                string priorityOfTask = Console.ReadLine().ToLower();
                TaskPriority priority;

                switch (priorityOfTask)
                {
                    case "low":
                    case "l":
                        priority = TaskPriority.Low;
                        break;
                    case "medium":
                    case "m":
                        priority = TaskPriority.Medium;
                        break;
                    case "high":
                    case "h":
                        priority = TaskPriority.High;
                        break;
                    default:
                        priority = TaskPriority.Medium;
                        return;
                }

                taskToEdit.PriorityOfTask = priority;
                listOfTasks.Remove(taskToEdit);
                listOfTasks.Add(taskToEdit);
                Reader.ClearFile();
                Writer.WriteToFile(listOfTasks);

            }
            catch (Exception e)
            {
               Console.WriteLine();
            }
                       
        }

        public void RemoveTask()
        {
            DisplayAllTasks();
            Console.WriteLine("Wybierz numer zadania które chcesz usunąć: ");
            bool isOk = true;

            try
            {
                while (isOk)
                {
                    string numbToRemove = Console.ReadLine();
                    int line = int.Parse(numbToRemove);
                    line = line - 1;
                    Task taskToRemove = listOfTasks[line];

                    if (line < 0 || line >= listOfTasks.Count)
                    {
                        Console.WriteLine("Wpisałeś niepoprawną wartość");
                        return;
                    }                    

                    listOfTasks.Remove(taskToRemove);

                    Reader.ClearFile();
                    Writer.WriteToFile(listOfTasks);
                    isOk = false;
                  
                }
            }catch (Exception e)
            {
                Console.WriteLine();
            }

            //tu się musi modyfikować -> usuwać row , SZUKAMY IDEKSEM Z CALENDARZA JAK JEST TO WTEDY KOPIUJEMY KALENDARZ DO NOWEJ TABLICY, CZYSCIMY KALENDARZ I PRZEPIJEMY KALENDARZ PRZY TYM CZYSZCĄC 
        }

        public void PreviewTasks()
        {
            List<Task> listOfTasks = Reader.ReadFile();

            Console.WriteLine("Wybierz sposób w jaki chcesz wyświetlić taski\n | S - posortowane względem tygodnia/miesiąca/roku | P - posortowane względem priorytetu | W - wyświetl wszystkie taski: ");//IF WSZYTSKIE TASKI NORMALNIE TO PO PROSRU READER A JESLI NIE TO MUSI WYBRAC DATE A JAK WYBIERZE DATE TO SIE MUSZA SIRTOWAC W JAKIS SPOOSB W TYM KALNDRAZU Z TABLICA I MUSZE SIE WTEDY WYSIWETLAS KONKRETNE A NIE TE KTORE SA NORMALNIE W REDERZE

            string sort = Console.ReadLine().ToUpper();

            try
            {

                if (sort == "P")
                {
                    Console.Clear();
                    var sortedTasks = listOfTasks.OrderByDescending(t => t.PriorityOfTask);
                    int i = 1;

                    foreach (var task in sortedTasks)
                    {
                        Console.WriteLine($"[{i}]: {task.PriorityOfTask}, {task.DateOfTask.ToString("dd/MM/yyyy")}, {task.Description}");
                        i++;
                    }

                }else if (sort == "S")
                {
                    Console.Clear();
                    Console.WriteLine("Wprowadź rok dla którego chcesz wyświetlić taski ");
                    string year = Console.ReadLine();
                    //int yearToFind = int.Parse(year);
                    List<Task> matchingYear = listOfTasks.Where(y => y.DateOfTask.ToString("yyyy") == year).ToList();

                    Console.WriteLine("M - sortowanie względem miesiąca | T- sortowanie względem tygodnia");
                    string sortyBy = Console.ReadLine().ToUpper();

                        if(sortyBy == "M")
                        {
                            Console.WriteLine("Wprowadź nr. miesiąca dla którego chcesz wyświetlić taski ");
                            string month = Console.ReadLine();
                            List<Task> matchingMonth = matchingYear.Where(y => y.DateOfTask.ToString("MM") == month).ToList();
                            int i = 1;

                            foreach (var task in matchingMonth)
                            {
                                Console.WriteLine($"[{i}]: {task.DateOfTask.ToString("dd/MM/yyyy")}, {task.PriorityOfTask}, {task.Description}");
                                i++;
                            }

                        }else if(sortyBy == "T")
                        { 
                            Console.WriteLine("Wprowadź nr. tygodnia dla którego chcesz wyświetlić taski ");
                            string week = Console.ReadLine();
                            int weekToFind = int.Parse(week);

                            var filteredList = matchingYear.Where(d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d.DateOfTask, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == weekToFind);
                            int b = 1;
                            foreach (var task in filteredList)
                            {
                                Console.WriteLine($"[{b}]: {task.DateOfTask.ToString("dd/MM/yyyy")}, {task.PriorityOfTask}, {task.Description}");
                                b++;
                            }
                        }
                        else 
                        {
                            var sortedByYear = listOfTasks.OrderByDescending(t => t.DateOfTask);
                            int a = 1;

                            foreach (var task in sortedByYear)
                            {
                                Console.WriteLine($"[{a}]: {task.DateOfTask.ToString("dd/MM/yyyy")}, {task.PriorityOfTask}, {task.Description}");
                                a++;
                            }
                        }


                }else if(sort == "W")
                {
                    DisplayAllTasks();
                }
                else
                {
                    DisplayAllTasks();
                }
           }catch(Exception e)
           {
                DisplayAllTasks();
            }

        }
    }

}