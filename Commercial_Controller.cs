using System;
using System.Collections.Generic;

namespace Week_3
{
    class Program
    {
        class Battery
        {
            public int id;
            public string status;
            public int numberColumns;
            public int numberElevatorsPerColumn;
            public int numberBasements;
            public int numberFloors;
            public List<Column> columnList; 

            // Create a class constructor with multiple parameters             
            public Battery ( int _id, int _numberColumns, int _numberElevatorsPerColumn, int _numberBasements, int _numberFloors )
            {
                id = _id;
                status = "Online";
                numberColumns = _numberColumns;
                numberElevatorsPerColumn = _numberElevatorsPerColumn;
                numberBasements = _numberBasements;
                numberFloors = _numberFloors;
                columnList = new List<Column>();               

                //Creates the columnList, the first column is generated manually to contain all the basements, the others are generated with a FOR loop
                columnList.Add(new Column(0, numberBasements, numberElevatorsPerColumn, -numberBasements, -1));                             
                int numberOfFloorsPerColumn = numberFloors / (numberColumns -1); 
                
                for (int i = 1; i < numberColumns; i++) 
                {
                    int firstServedFloor = 0;
                    int lastServedFloor = 0;
                    if (i == 1)     // For the first column, the formula is diferent to exclude the Lobby
                    {
                        firstServedFloor = 2;
                        lastServedFloor = firstServedFloor + (numberOfFloorsPerColumn - 2);
                    }
                    else
                    {
                        firstServedFloor = 1 + (i - 1 ) * numberOfFloorsPerColumn; //Ex(i = 2): 1 + (2 -1) * 20 = 21
                        lastServedFloor = firstServedFloor + (numberOfFloorsPerColumn - 1); //EX(i = 2): 21 + (20 -1) = 40
                    }
                    //If there are floors left, they are all placed in the last column
                    int reminderFloors = numberFloors % (numberColumns -1); 
                    if (i ==  numberColumns - 1 && reminderFloors != 0)
                    {                        
                        lastServedFloor += reminderFloors;
                    }
                    //Creates the column object end add it to the columnList                                        
                    Column newcolumn = new Column(i, numberOfFloorsPerColumn, numberElevatorsPerColumn, firstServedFloor, lastServedFloor);
                    columnList.Add(newcolumn);                        
                }   
            }
            public void requestElevator(int requestedFloor)
            {          
                // Calls the function that returns the id of the appropriate column and use it to select the bestColumn
                Column bestColumn = columnList[findBestColumn(requestedFloor)];
                // Call the function that returns the id of the appropriate elevator                
                int bestElevatorId = bestColumn.findBestElevator(requestedFloor);
                // Call the function that routes the elevator to the person
                string elevatorDirection = "None";                                    
                if (requestedFloor > bestColumn.elevatorList[bestElevatorId].currentFloor)
                {
                    elevatorDirection = "Up";
                } else  {
                    elevatorDirection = "Down";
                }                                    
                bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(requestedFloor, elevatorDirection, bestColumn.id);
                // Adds the Lobby floor to destinationList of the selected elevator
                bestColumn.elevatorList[bestElevatorId].destinationList.Add(1);               
            }             
            public int findBestColumn(int _requestedFloor)
            {
                // Checks which column has the range to which the desired floor belongs
                int bestColumnId = 0;
                for (int i = 0 ; i < numberColumns; i++)
                {
                    if (_requestedFloor >= columnList[i].firstServedFloor && _requestedFloor <= columnList[i].lastServedFloor)
                    {
                        bestColumnId = i;
                    }
                }
                return bestColumnId;             }             
                            
        }       
        class Column
        {
            public int id;
            public string status;
            public int numberFloors;
            public int numberOfElevators;
            public int firstServedFloor;
            public int lastServedFloor;
            public List<Elevator> elevatorList; 
           
            // Create a class constructor with multiple parameters
            public Column (int _id, int _numberFloors, int _numberOfElevators, int _firstServedFloor, int _lastServedFloor)
            {
                id = _id;
                status = "Online";
                numberFloors = _numberFloors;
                numberOfElevators = _numberOfElevators;
                firstServedFloor = _firstServedFloor;
                lastServedFloor = _lastServedFloor;
                elevatorList = new List<Elevator>();

                //Creates the elevatorList
                for(int i = 0; i < numberOfElevators; i++)
                {                    
                    elevatorList.Add(new Elevator(i));  
                }
            }
            public void requestFloor(int elevator, int requestedFloor)
            {
                string elevatorDirection = "None";                                    
                if (requestedFloor > elevatorList[elevator].currentFloor)
                {
                    elevatorDirection = "Up";
                } else  {
                    elevatorDirection = "Down";
                } 
                elevatorList[elevator].goToDestinationFloor(requestedFloor, elevatorDirection, id);         
            }           
            public int findBestElevator (int requestedFloor)
            {
                int shortestDistance = numberFloors;
                int bestElevatorId = 0;
                int distance = 0;                
                
                for (int i = 0; i < numberOfElevators; i++)
                {
                    if (elevatorList[i].elevatorDirection == "Down" && requestedFloor < elevatorList[i].currentFloor)
                    {
                        distance = Math.Abs(requestedFloor - elevatorList[i].currentFloor);    
                        if (distance <= shortestDistance)   {
                            shortestDistance = distance;
                            bestElevatorId = i;
                        } 
                    }
                }                   
                if  (bestElevatorId > 0)
                {
                    return bestElevatorId;
                }
                for (var i = 0; i < numberOfElevators; i++)
                {
                    if (elevatorList[i].elevatorDirection != "Down")
                    {
                        distance = Math.Abs(requestedFloor - elevatorList[i].currentFloor); 
                        if (distance <= shortestDistance)
                        {
                            shortestDistance = distance;
                            bestElevatorId = i;            
                        }
                    }
                }            
                return bestElevatorId;
            }                    
        }
        class Elevator
        {
            public int id;
            public string status;
            public string elevatorDirection;
            public int currentFloor;
            public string doors;
            public int distanceToFloor;
            public List<int> destinationList;            
            // Create a class constructor with multiple parameters
            public Elevator (int _id)
            {
                id = _id;
                status = "Online";
                elevatorDirection = "None";
                currentFloor = 1;
                doors = "Closed";
                distanceToFloor = 0;
                destinationList = new List<int>();                 
            }
            // Set the next destination and move the elevator
            public void goToDestinationFloor (int _requestedFloor, string _elevatorDirection, int columnId)
            {
                destinationList.Add(_requestedFloor);
                elevatorDirection = _elevatorDirection;
                // If direction is Up, sorts the list from least to largest.
                if (elevatorDirection == "Up")  
                {       
                    destinationList.Sort();
                }
                // If direction is Down, sorts the list from largest to least.
                if (elevatorDirection == "Down")    
                {       
                    destinationList.Reverse();
                }
                // Set the next destination to the first element of sorted list.
                int destination = destinationList[0];
                string columnLetter = "None";
                switch (columnId)
                {
                    case 0:
                        columnLetter = "A";
                        break;
                    case 1:
                        columnLetter = "B";
                        break;
                    case 2:
                        columnLetter = "C";
                        break;
                    case 3:
                        columnLetter = "D";
                        break;
                }          
                
                Console.WriteLine ("Column = " + columnLetter);                
                Console.WriteLine ("Elevator = " + columnLetter + (id + 1));
                Console.WriteLine ("Current floor = " + currentFloor);
                Console.WriteLine ("Direction = " + elevatorDirection + "\n" );                                
                while (currentFloor != destination)
                {                   
                    if (elevatorDirection == "Up")
                    {                    
                        currentFloor++;                
                    }
                    if (elevatorDirection == "Down")
                    {                    
                        currentFloor--;
                    }                
                }
                // Removes the reached floor from destinationList and puts the direction in None (Idle)
                destinationList.Remove(0);
                elevatorDirection = "None";
                Console.WriteLine ("Column = " + columnLetter);
                Console.WriteLine ("Elevator = " + columnLetter + (id + 1));
                Console.WriteLine("Destination floor = " + currentFloor);       
                Console.WriteLine("Opening Doors\n"); 
            }             
        }
        static void runScenarioOne()
        {       
            Battery BatteryOne = new Battery(0, 4, 5, 6, 60);
            //Scenario 1:
            //Elevator B1 at 20th floor going to the 5th floor
            BatteryOne.columnList[1].elevatorList[0].currentFloor = 20;
            BatteryOne.columnList[1].elevatorList[0].elevatorDirection = "Down";
            // Elevator B2 at 3rd floor going to the 15th floor
            BatteryOne.columnList[1].elevatorList[1].currentFloor = 3;
            BatteryOne.columnList[1].elevatorList[1].elevatorDirection = "Up";
            // Elevator B3 at 13th floor going to RC
            BatteryOne.columnList[1].elevatorList[2].currentFloor = 13;
            BatteryOne.columnList[1].elevatorList[2].elevatorDirection = "Down";
            // Elevator B4 at 15th floor going to the 2nd floor
            BatteryOne.columnList[1].elevatorList[3].currentFloor = 15;            
            BatteryOne.columnList[1].elevatorList[3].elevatorDirection = "Down";
            // Elevator B5 at 6th floor going to RC
            BatteryOne.columnList[1].elevatorList[4].currentFloor = 6;
            BatteryOne.columnList[1].elevatorList[4].elevatorDirection = "Down";

            // Someone at RC wants to go to the 20th floor.
            int requestedFloor = 15;            
            BatteryOne.requestElevator(requestedFloor);
            // Elevator B5 is expected to be sent.         
        }  
        static void Main(string[] args)
        {            
            runScenarioOne();            
                               
        }
    }
}
