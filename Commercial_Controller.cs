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
                for (int i = 1; i < this.numberColumns; i++) 
                {
                    int firstServedFloor = 0;
                    int lastServedFloor = 0;
                    if (i == 1)
                    {
                        firstServedFloor = 2;
                        lastServedFloor = firstServedFloor + (numberOfFloorsPerColumn - 2);
                    }
                    else
                    {
                        firstServedFloor = 1 + (i - 1 ) * numberOfFloorsPerColumn;
                        lastServedFloor = firstServedFloor + (numberOfFloorsPerColumn - 1);
                    }                                         
                    Column newcolumn = new Column(i, numberOfFloorsPerColumn, numberElevatorsPerColumn, firstServedFloor, lastServedFloor);
                    columnList.Add(newcolumn);                        
                }   
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
            // Create a class constructor with multiple parameters
            public Column (int _id, int _numberFloors, int _numberOfElevators, int _firstServedFloor, int _lastServedFloor)
            {
                id = _id;
                status = "Online";
                numberFloors = _numberFloors;
                numberOfElevators = _numberOfElevators;
                firstServedFloor = _firstServedFloor;
                lastServedFloor = _lastServedFloor;
                //List<int> elevatorList = new List<int>();
            }
        }        
        static void Main(string[] args)
        {                     
            Battery B1 = new Battery(0, 4, 5, 6, 60);            
            Console.WriteLine("Test");            
            
        }
    }
}
