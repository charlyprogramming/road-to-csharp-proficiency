using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.VisualBasic;

class Program{

  static List<string> tasks = new List<string>();
  static List<string> compTasks = new List<string>();

  public static void Main(string[] args){

    LoadTasks(out tasks, out compTasks);

    while(true){

      Console.Clear();
      Console.WriteLine("To-Do List");
      Console.WriteLine("===========");
      DisplayTasks(tasks);
      Console.WriteLine("\nOptions: ");
      Console.WriteLine("1. Add Task");
      Console.WriteLine("2. Remove Task");
      Console.WriteLine("3. Mark Task as Complete");
      Console.WriteLine("4. Display Completed Tasks");
      Console.WriteLine("5. Exit");
      Console.WriteLine("Choose an option: ");

      string option = Console.ReadLine();

      switch (option){
        case "1":
          AddTask(tasks);
          break;
        case "2":
          Console.Write("Enter task number to remove: ");
          if (int.TryParse(Console.ReadLine(), out int removeIndex)){
            RemoveTask(tasks, removeIndex);
          }
          else{
            Console.WriteLine("Invalid task index. \n\n(NOT AN INT)");
            Console.ReadKey();
          }
          break;
        case "3":
          Console.Write("Enter task number to mark as complete: ");
          if (int.TryParse(Console.ReadLine(), out int completeIndex)){
            MarkTaskAsComplete(tasks, completeIndex);
          }
          else{
            Console.WriteLine("Invalid task index. \n\n(NOT AN INT)");
            Console.ReadKey();
          }
          break;
        case "4":
          DisplayCompletedTasks(compTasks);
          break;
        case "5":
          SaveTasks(tasks);
          return;
        default:
          Console.WriteLine("Invalid option, press any key to try again...");
          Console.ReadKey();
          break;
      }
    }
  }


// CASE FUNCTION METHODS
  public static void AddTask(List<string> tasks){

    Console.Clear();
    
    Console.Write("Enter a new task: ");
    string newTask = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newTask)){
      tasks.Add(newTask);
      Console.WriteLine("Task added successfully!");
    }
    else{
      Console.WriteLine("Task cannot be empty.");
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
  }
  
  public static void RemoveTask(List<string> tasks, int index){
    if (index > 0 && index <= tasks.Count){
      tasks.RemoveAt(index - 1);
      Console.WriteLine("Task removed succesfully.");
    }
    else{
     Console.WriteLine("Invalid task index.");
     Console.ReadKey();
    }
  }

  public static void MarkTaskAsComplete(List<string> tasks, int index){
    if (index > 0 && index <= tasks.Count){
      string completedTask = tasks[index - 1];
      tasks.RemoveAt(index - 1);
      compTasks.Add(completedTask);
    }
    else{
      Console.WriteLine("Invalid task index");
      Console.ReadKey();
    }
  }   
  
  public static void DisplayCompletedTasks(List<string> compTasks){
    Console.Clear();
    if (compTasks.Count == 0){
      Console.WriteLine("No tasks available");
    }
    else{
      Console.WriteLine("Your Completed Tasks: ");
      for (int i = 0; i < compTasks.Count; i++){
        Console.WriteLine($"{i + 1}. {compTasks[i]}");
      }
    }
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
  }

// BASE FUNCTION METHODS
  public static void DisplayTasks(List<string> tasks){
    if (OperatingSystem.IsWindows() && Console.IsOutputRedirected == false){
      Console.Clear();
    }
    if (tasks.Count == 0){
      Console.WriteLine("No tasks available.");
    }
    else{
      Console.WriteLine("Your Tasks:");
      for (int i = 0; i < tasks.Count; i++){
        Console.WriteLine($"{i + 1}. {tasks[i]}");
      }
    }
    
  }
  
  public static void SaveTasks(List<string> tasks){
    try{
      using (StreamWriter writer = new StreamWriter("tasks.txt")){
        foreach (var task in tasks){
            if (task != "" && task != "\n"){
              writer.WriteLine(task);
            }
        }
        writer.WriteLine("==================");
        writer.WriteLine("Completed Tasks: ");
        foreach (var task in compTasks){
          if (task != "" && task != "\n"){
            writer.WriteLine(task);
          }
        }
      }
    }
    catch (Exception ex){
      Console.WriteLine($"Error saving tasks: {ex.Message}");
    }
  }

  public static void LoadTasks(out List<string> list1, out List<string> list2){

    List<string> tasks = new List<string>();
    List<string> compTasks = new List<string>();

    try{
      if (File.Exists("tasks.txt")){
        using (StreamReader reader = new StreamReader("tasks.txt")){

          string line;
          bool isCompletedSection = false;

          while ( (line = reader.ReadLine() ) != null ){  

            line = line.Trim();

            if ( line == "==================" ){
              continue;
            }
            if (line == "Completed Tasks:"){
              isCompletedSection = true;
              continue;
            }
            
            if (!string.IsNullOrWhiteSpace(line)){
              if (isCompletedSection){
                compTasks.Add(line);
              }
              else{
                tasks.Add(line);
              } 
            }
          }
        }
      }
    }
    catch (Exception ex){
      Console.WriteLine($"Error loading tasks {ex.Message}");
    }
    list1 = tasks;
    list2 = compTasks;
  }
}
