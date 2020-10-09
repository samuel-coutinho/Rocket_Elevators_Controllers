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
                // Calls the functions that finds the bestColumn and bestElevator 
                Column bestColumn = columnList[findBestColumn(requestedFloor)];                                                              
                int bestElevatorId = bestColumn.findBestElevator(requestedFloor);

                // Calls the function to define the elevatorDirection
                string elevatorDirection = defineElevatorDirection(requestedFloor,  bestColumn.id, bestElevatorId);

                // Calls the function that routes the elevator to the person and then to the Lobby(1)                                 
                bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(requestedFloor, elevatorDirection, bestColumn.id);
                Console.WriteLine ("**********************");                
                bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(1, elevatorDirection, bestColumn.id);             
            }
            public void assignElevator (int requestedFloor)
            {
                // Calls the functions that finds the bestColumn and bestElevator 
                Column bestColumn = columnList[findBestColumn(requestedFloor)];
                int bestElevatorId = bestColumn.findBestElevator(1);
                // Calls the function to define the elevatorDirection
                string elevatorDirection = defineElevatorDirection(1, bestColumn.id, bestElevatorId);
                // Calls the function that routes the elevator to the Lobby(1)
                bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(1, elevatorDirection, bestColumn.id);
                // Sends the elevator to requested floor
                elevatorDirection = defineElevatorDirection(requestedFloor, bestColumn.id, bestElevatorId);
                Console.WriteLine ("**********************");
                bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(requestedFloor, elevatorDirection, bestColumn.id);
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
                return bestColumnId;
            }
            // Defines the elevatorDirection comparing the requestedFloor and the current elavatorFloor
            public string defineElevatorDirection(int requestedFloor, int bestColumnId, int bestElevatorId)
            {
                string elevatorDirection = columnList[bestColumnId].elevatorList[bestElevatorId].elevatorDirection;
                int elavatorFloor = columnList[bestColumnId].elevatorList[bestElevatorId].currentFloor;                                    
                if (requestedFloor > elavatorFloor)
                {
                    elevatorDirection = "Up";
                } 
                else if (requestedFloor < elavatorFloor)
                {
                    elevatorDirection = "Down";
                }                
                return elevatorDirection;

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
                               
            public int findBestElevator (int requestedFloor)
            {                
                int shortestDistance = numberFloors;
                int bestElevatorId = -100;
                int distance = 0;
                bool bestElevatorFound = false;                
                // Looks for an elevator going down (condition 1) on a higher level than the floor requested (condition 2) when it is not in the basements (condition 3)
                for (int i = 0; i < numberOfElevators; i++)
                {                   
                    if (elevatorList[i].elevatorDirection == "Down" && requestedFloor <= elevatorList[i].currentFloor && id > 0)
                    {   // Checks if there is an elevator on the requested floor with the doors still open
                        if (requestedFloor == elevatorList[i].currentFloor && elevatorList[i].doors == "Open")
                        {
                            bestElevatorId = i;
                            return bestElevatorId;
                        }                        
                        distance = Math.Abs(requestedFloor - elevatorList[i].currentFloor);    
                        if (distance <= shortestDistance && distance != 0)   {
                            shortestDistance = distance;
                            bestElevatorId = i;
                            bestElevatorFound = true;
                        } 
                    }
                }   // If the first search has a result, returns it
                if  (bestElevatorFound)
                {
                    return bestElevatorId;
                }
                for (int j = 0; j < numberOfElevators; j++)
                {   // Checks if there is an elevator on the requested floor with the doors still open
                    if (requestedFloor == elevatorList[j].currentFloor && elevatorList[j].doors == "Open")
                    {
                        bestElevatorId = j;
                        return bestElevatorId;
                    } // Looks for an elevator going up (condition 1) on a lower level than the floor requested (condition 2) when it is in the basements (condition 3)
                    if (elevatorList[j].elevatorDirection == "Up" && requestedFloor >= elevatorList[j].currentFloor && id < 1)
                    {
                        distance = Math.Abs(elevatorList[j].currentFloor - requestedFloor);    
                        if (distance <= shortestDistance && distance != 0)   {
                            shortestDistance = distance;
                            bestElevatorId = j;
                            bestElevatorFound = true;
                        } 
                    }
                }                     
                if  (bestElevatorFound)
                {
                    return bestElevatorId;
                } // If previous searches have not been successful, looks for the nearest elevator within the column
                for (int k = 0; k < numberOfElevators; k++)
                {                    
                    if (requestedFloor == elevatorList[k].currentFloor && elevatorList[k].doors == "Open")
                    {
                        bestElevatorId = k;
                        return bestElevatorId;
                    }
                    distance = Math.Abs(requestedFloor - elevatorList[k].currentFloor); 
                    if (distance <= shortestDistance)
                    {
                        shortestDistance = distance;
                        bestElevatorId = k;            
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
            public List<int> destinationList;            
            // Create a class constructor with multiple parameters
            public Elevator (int _id)
            {
                id = _id;
                status = "Online";
                elevatorDirection = "None";
                currentFloor = 1;
                doors = "Closed";                
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
                if (currentFloor == 1)
                {                
                    Console.WriteLine ("Current floor = Ground Floor");
                }                
                else
                {
                    Console.WriteLine ("Current floor = " + currentFloor);
                }                
                Console.WriteLine ("Direction = " + elevatorDirection + "\n" );
                doors = "Closed";                                 
                Console.WriteLine("Closing Doors\n");                                
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
                destinationList.RemoveAt(0);
                elevatorDirection = "None";
                Console.WriteLine ("Column = " + columnLetter);
                Console.WriteLine ("Elevator = " + columnLetter + (id + 1));
                if (currentFloor == 1)
                {                
                    Console.WriteLine ("Destination floor = Ground Floor");
                }                
                else
                {
                    Console.WriteLine ("Destination floor = " + currentFloor);
                }
                doors = "Open";                                 
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
            int requestedFloor = 20;            
            BatteryOne.assignElevator(requestedFloor);
            // Elevator B5 is expected to be sent.         
        }
        static void runScenarioTwo()
        {       
            Battery BatteryTwo = new Battery(0, 4, 5, 6, 60);

            // Scenario 2:
            // Elevator C1 at RC going to the 21st floor (not yet departed)
            BatteryTwo.columnList[2].elevatorList[0].currentFloor = 1;
            BatteryTwo.columnList[2].elevatorList[0].elevatorDirection = "Up";            

            // Elevator C2 at 23rd floor going to the 28th floor
            BatteryTwo.columnList[2].elevatorList[1].currentFloor = 23;
            BatteryTwo.columnList[2].elevatorList[1].elevatorDirection = "Up";
            // Elevator C3 at 33rd floor going to RC
            BatteryTwo.columnList[2].elevatorList[2].currentFloor = 33;
            BatteryTwo.columnList[2].elevatorList[2].elevatorDirection = "Down";
            // Elevator C4 at 40th floor going to the 24th floor
            BatteryTwo.columnList[2].elevatorList[3].currentFloor = 40;
            BatteryTwo.columnList[2].elevatorList[3].elevatorDirection = "Down";
            // Elevator C5 at 39th floor going to RC
            BatteryTwo.columnList[2].elevatorList[4].currentFloor = 39;
            BatteryTwo.columnList[2].elevatorList[4].elevatorDirection = "Down";

            // Someone at RC wants to go to the 36th floor.
            int requestedFloor = 36;            
            BatteryTwo.assignElevator(requestedFloor);            
            // Elevator C1 is expected to be sent.                   
        }
        static void runScenarioThree()
        {       
            Battery BatteryThree = new Battery(0, 4, 5, 6, 60);

            // Scenario 3:
            // Elevator D1 at 58th going to RC
            BatteryThree.columnList[3].elevatorList[0].currentFloor = 58;
            BatteryThree.columnList[3].elevatorList[0].elevatorDirection = "Down";
            // Elevator D2 at 50th floor going to the 60th floor
            BatteryThree.columnList[3].elevatorList[1].currentFloor = 50;
            BatteryThree.columnList[3].elevatorList[1].elevatorDirection = "Up";
            // Elevator D3 at 46th floor going to the 58th floor
            BatteryThree.columnList[3].elevatorList[2].currentFloor = 46;
            BatteryThree.columnList[3].elevatorList[2].elevatorDirection = "Up";
            // Elevator D4 at RC going to the 54th floor
            BatteryThree.columnList[3].elevatorList[3].currentFloor = 1;
            BatteryThree.columnList[3].elevatorList[3].elevatorDirection = "Up";
            // Elevator D5 at 60th floor going to RC
            BatteryThree.columnList[3].elevatorList[4].currentFloor = 60;
            BatteryThree.columnList[3].elevatorList[4].elevatorDirection = "Down";

            // Someone at 54e floor wants to go to RC.
            int requestedFloor = 54;            
            BatteryThree.requestElevator(requestedFloor);
            // Elevator D1 is expected to be sent.                  
        }
        static void runScenarioFour()
        {       
            Battery BatteryFour = new Battery(0, 4, 5, 6, 60);

            // Scenario 4:
            // Elevator A1 “Idle” at SS4
            BatteryFour.columnList[0].elevatorList[0].currentFloor = -4;
            BatteryFour.columnList[0].elevatorList[0].elevatorDirection = "None";
            // Elevator A2 “Idle” at RC
            BatteryFour.columnList[0].elevatorList[1].currentFloor = 1;
            BatteryFour.columnList[0].elevatorList[1].elevatorDirection = "None";
            // Elevator A3 at SS3 going to SS5
            BatteryFour.columnList[0].elevatorList[2].currentFloor = -3;
            BatteryFour.columnList[0].elevatorList[2].elevatorDirection = "Down";
            // Elevator A4 at SS6 going to RC
            BatteryFour.columnList[0].elevatorList[3].currentFloor = -6;
            BatteryFour.columnList[0].elevatorList[3].elevatorDirection = "Up";
            // Elevator A5 at SS1 going to SS6
            BatteryFour.columnList[0].elevatorList[4].currentFloor = -1;
            BatteryFour.columnList[0].elevatorList[4].elevatorDirection = "Down";

            // Someone at SS3 wants to go to RC.
            int requestedFloor = -3;            
            BatteryFour.requestElevator(requestedFloor);
            // Elevator A4 is expected to be sent.                 
        }       
        static void Main(string[] args)
        {            
            runScenarioOne();
            //runScenarioTwo();
            //runScenarioThree();
            //runScenarioFour();
        }        
    }
}
