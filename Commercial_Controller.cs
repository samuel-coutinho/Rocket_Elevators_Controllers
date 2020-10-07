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
            public void requestElevator(int requestedFloor, string requestedDirection)
            {          
                Column bestColumn = columnList[findBestColumn(requestedFloor)];                
                int bestElevatorId = bestColumn.findBestElevator(requestedFloor, requestedDirection);                                    
                bestColumn.requestFloor(bestElevatorId, requestedFloor );
            }    
            // public void requestFloor(int elevator, int requestedFloor)
            // {
            //     string elevatorDirection = "None";                                    
            //     if (requestedFloor > elevatorList[elevator].currentFloor)
            //     {
            //         elevatorDirection = "Up";
            //     } else  {
            //         elevatorDirection = "Down";
            //     } 
            //     elevatorList[elevator].goToDestinationFloor(requestedFloor, elevatorDirection);         
            // }
            public int findBestColumn(int _requestedFloor)
            {
                for (int i = 0 ; i < numberColumns; i++)
                {
                    if (_requestedFloor > columnList[i].firstServedFloor && _requestedFloor < columnList[i].lastServedFloor)
                    {
                        return i;
                    }
                }
                return _requestedFloor;
            }        
                       
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
                elevatorList[elevator].goToDestinationFloor(requestedFloor, elevatorDirection);         
            }           
            public int findBestElevator (int requestedFloor, string requestedDirection)
            {
                int shortestDistance = numberFloors;
                int bestElevatorId = 0;
                int distance = 0;
                
                for (int i = 0; i < numberOfElevators; i++)    
                {
                    if (requestedDirection == "Up" && elevatorList[i].elevatorDirection == "Up" && requestedFloor > elevatorList[i].currentFloor)
                    {
                        distance = Math.Abs(requestedFloor - elevatorList[i].currentFloor);    
                        if (distance <= shortestDistance)
                        {
                            shortestDistance = distance;
                            bestElevatorId = i;
                        }
                    }
                }
                if  (bestElevatorId > 0)
                {
                    return bestElevatorId;
                }
                for (int i = 0; i < numberOfElevators; i++)
                {
                    if (requestedDirection == "Down" && elevatorList[i].elevatorDirection == "Down" && requestedFloor < elevatorList[i].currentFloor)
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
                    if (elevatorList[i].elevatorDirection == "None")
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
            public void goToDestinationFloor (int _requestedFloor, string _elevatorDirection )
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
                int bestColumnId = BatteryOne.findBestColumn(_requestedFloor);
                Console.WriteLine ("Column = " + bestColumnId);
                Console.WriteLine ("Elevator = " + id);
                Console.WriteLine ("Current floor = " + currentFloor);
                Console.WriteLine ("Direction = " + elevatorDirection );
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
                Console.WriteLine ("Column = " + bestColumnId);
                Console.WriteLine ("Elevator = " + id);
                Console.WriteLine("Destination floor = " + currentFloor);       
                Console.WriteLine("Opening Doors\n"); 
            }             
        }     

        static void Main(string[] args)
        {                     
            Battery BatteryOne = new Battery(0, 4, 5, 6, 60);            
            //Console.WriteLine("Test");
            //Elevator E1 = new Elevator(9);
            //Column C1 = new Column(8, 20, 5, 1, 20);            
            int requestedFloor = 26;
            string requestedDirection = "Up";
            //int elevator = 4;
            //C1.requestFloor(elevator, requestedFloor);
            BatteryOne.requestElevator(requestedFloor, requestedDirection);                    
        }
    }
}
