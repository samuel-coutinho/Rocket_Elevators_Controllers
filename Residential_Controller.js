// Classes varinition
var Column = function ( numberFloors, numberOfElevators) {
    this.status = 'Online';
    this.numberFloors = numberFloors;
    this.numberOfElevators = numberOfElevators;
    this.callButtonList = []
    this.elevatorList = []    
    
    this.createElevatorList = function ()  {
        for (var i = 0; i < this.numberOfElevators; i++)    {
            var elevator = new Elevator(i);            
            this.elevatorList.push(elevator)
        }
    }   
    this.createElevatorList();
    this.createcallButtonList = function()   {
        for (var i = 0; i < this.numberFloors + 1; i++) { 
            var callButton = new CallButton(i);                                     
            this.callButtonList.push(callButton)
        }
    }
    this.createcallButtonList();

    this.requestElevator = function (requestedFloor, requestedDirection)    {          
         bestElevator = this.findBestElevator(requestedFloor, requestedDirection);      
         this.requestFloor(bestElevator.id, requestedFloor );
    }    
    this.requestFloor = function (elevator, requestedFloor) {                                    
        if (requestedFloor > this.elevatorList[elevator].currentFloor)  {
            elevatorDirection = 'Up';
        } else  {
            elevatorDirection = 'Down';
        } 
        this.elevatorList[elevator].goToDestinationFloor(requestedFloor, elevatorDirection)         
    }
    this.findBestElevator = function(requestedFloor, requestedDirection)  {
        shortestDistance = this.numberFloors;
        bestElevatorId = 0;
        
        for (var i = 0; i < this.numberOfElevators; i++)    {
            if (requestedDirection === 'Up' && this.elevatorList[i].elevatorDirection === 'Up' && requestedFloor > this.elevatorList[i].currentFloor)   {
                distance = Math.abs(requestedFloor - this.elevatorList[i].currentFloor)    
                if (distance <= shortestDistance)   {
                    shortestDistance = distance
                    bestElevatorId = i
                }
            }
        }
        if  (bestElevatorId > 0)    {
            return this.elevatorList[bestElevatorId]
        }
        for (var i = 0; i < this.numberOfElevators; i++)    {
            if (requestedDirection == 'Down' && this.elevatorList[i].elevatorDirection == 'Down' && requestedFloor < this.elevatorList[i].currentFloor) {
                distance = Math.abs(requestedFloor - this.elevatorList[i].currentFloor)    
                if (distance <= shortestDistance)   {
                    shortestDistance = distance
                    bestElevatorId = i
                } 
            }
        }                   
        if  (bestElevatorId > 0)    {
            return this.elevatorList[bestElevatorId]
        }
        for (var i = 0; i < this.numberOfElevators; i++)    {
            if (this.elevatorList[i].elevatorDirection == 'None')   {
                distance = Math.abs(requestedFloor - this.elevatorList[i].currentFloor) 
                if (distance <= shortestDistance)   {
                    shortestDistance = distance
                    bestElevatorId = i            
                }
            }
        }            
        return this.elevatorList[bestElevatorId]
    }    
}; 

var Elevator = function (id)   {
    this.id = id;
    this.status = 'Online';
    this.elevatorDirection = 'None';
    this.currentFloor = 1;
    this.doors = 'Closed';   
    this.destinationList = [];
    
        
    this.goToDestinationFloor = function(requestedFloor, elevatorDirection)  {           
        this.destinationList.push(requestedFloor);
        this.elevatorDirection = elevatorDirection;        

        if (this.elevatorDirection === 'Up') {       
            this.destinationList.sort();
        }
        if (this.elevatorDirection === 'Down')   {       
            this.destinationList.reverse();
        }
        destination = this.destinationList[0];
        console.log('Elevator = ', this.id);
        console.log('Current floor = ', this.currentFloor);
        console.log('direction = ', this.elevatorDirection);                                             
        while (this.currentFloor != destination)    {                   
            if (this.elevatorDirection === 'Up')  {
                
                this.currentFloor += 1;                
            }
            if (this.elevatorDirection === 'Down')    {
                
                this.currentFloor -= 1;                
            }
        }                      
        this.destinationList.shift(); 
        this.elevatorDirection = 'None';
        console.log('Elevator = ', this.id);
        console.log('Destination floor = ', this.currentFloor);       
        console.log('Opening Doors\n');       
    }      
};
var CallButton = function (floor)   {
    this.direction = 'None';
    this.floor = floor;
};

 //Scenarios functions
var Scenario1 = function()  {
    var column1 = new Column(10, 2);
    var columnNumber = column1;     
    //Elevator A
    columnNumber.elevatorList[0].currentFloor = 2;
    columnNumber.elevatorList[0].elevatorDirection = 'None';
    //Elevator B
    columnNumber.elevatorList[1].currentFloor = 6;
    columnNumber.elevatorList[1].elevatorDirection = 'None';
     

    //Someone is on floor 3 and wants to go to the 7th floor.
    var floorOfPersonRequestingElevator = 3;
    var requestedFloor = 7;
    console.log('*****************************************');
    console.log('Scenario 1');
    console.log('Elevator A is Idle at floor 2');
    console.log('Elevator B is Idle at floor 6');
    console.log('Someone is on floor 3 and wants to go to the 7th floor.');
    console.log('Elevator A = 0 is expected to be sent.\n');
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor);    
    console.log('*****************************************');
}
var Scenario2 = function()  {       
    var column2 = new Column(10, 2);  
    var columnNumber = column2;   
    //Elevator A
    columnNumber.elevatorList[0].currentFloor = 10;
    columnNumber.elevatorList[0].elevatorDirection = 'None';
    //Elevator B
    columnNumber.elevatorList[1].currentFloor = 3;
    columnNumber.elevatorList[1].elevatorDirection = 'None';          

    //Someone is on the 1st floor and requests the 6th floor.    
    var floorOfPersonRequestingElevator = 1;
    var requestedFloor = 6;
    console.log('*****************************************');
    console.log('Scenario 2')
    console.log('Elevator A is Idle at floor 10')
    console.log('Elevator B is Idle at floor 3\n')
    console.log('Someone is on the 1st floor and requests the 6th floor.')
    console.log('Elevator B = 1 should be sent.\n')
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor);    
    
    // 2 minutes later, someone else is on the 3rd floor and requests the 5th floor. Elevator B should be sent    
    floorOfPersonRequestingElevator = 3;
    requestedFloor = 5;
    console.log('2 minutes later, someone else is on the 3rd floor and requests the 5th floor.')
    console.log('Elevator B = 1 should be sent.\n')
    //(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor)    
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor);    
    
    //Finally, a third person is at floor 9 and wants to go down to the 2nd floor. Elevator A should be sent.
    console.log('Finally, a third person is at floor 9 and wants to go down to the 2nd floor.')
    console.log('Elevator A = 0 should be sent.\n')    
    floorOfPersonRequestingElevator = 9;
    requestedFloor = 2;   
    //(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor)    
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor);    
    console.log('*****************************************');
}    

var Scenario3 = function()  {     
    var column3 = new Column(10, 2);  
    var columnNumber = column3;   
    //Elevator A
    columnNumber.elevatorList[0].currentFloor = 10;
    columnNumber.elevatorList[0].elevatorDirection = 'None';
    //Elevator B
    columnNumber.elevatorList[1].currentFloor = 3;
    columnNumber.elevatorList[1].elevatorDirection = 'Up';
    columnNumber.elevatorList[1].destinationList.push(6);      

    //Someone is on floor 3 and wants to go to the 2th floor.
    floorOfPersonRequestingElevator = 3
    requestedFloor = 2
    callButtonDirection = 'Up'
    console.log('*****************************************');
    console.log('Scenario 3');
    console.log('Elevator A is Idle at floor 10');
    console.log('Elevator B is Moving from floor 3 to floor 6');
    console.log('Someone is on floor 3 and requests the 2nd floor.');
    console.log('Elevator A = 0 should be sent.\n');
    //(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor)    
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor);    
    
    //5 minutes later, someone else is on the 10th floor and wants to go to the 3rd. Elevator B should be sent.
    console.log('5 minutes later, someone else is on the 10th floor and wants to go to the 3rd.')
    console.log('Elevator B = 1 should be sent.\n')
    floorOfPersonRequestingElevator = 10
    requestedFloor = 3
    callButtonDirection = 'Down'
    columnNumber.elevatorList[1].currentFloor = 6
    columnNumber.elevatorList[1].elevatorDirection = 'None'
    columnNumber.elevatorList[1].destinationList.shift();
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor);    
    console.log('*****************************************');
}

var runScenario = function(columnNumber, elevatorAinitialFloor, elevatorAinitialDirection, elevatorBinitialFloor, elevatorBinitialDirection, floor, requestedFloor, callButtonDirection )   {       
        //Elevator A
        columnNumber.elevatorList[0].currentFloor = elevatorAinitialFloor;
        columnNumber.elevatorList[0].elevatorDirection = elevatorAinitialDirection;
        //Elevator B
        columnNumber.elevatorList[1].currentFloor = elevatorBinitialFloor;
        columnNumber.elevatorList[1].elevatorDirection = elevatorBinitialDirection;       

        //Request Elevator        
        columnNumber.callButtonList[floor].direction = callButtonDirection;    
        columnNumber.requestElevator(floor, columnNumber.callButtonList[floor].direction);  
        console.log('Please wait a moment\n'); 
        //setTimeout(delay(), 2000); 
    
        //Request floor        
        console.log('Going to floor ',requestedFloor,'\n');
        var elevator = columnNumber.findBestElevator(floor, columnNumber.callButtonList[floor].direction);   
        columnNumber.requestFloor(elevator.id, requestedFloor);
        
}
//-------Test Section-------//
//Scenario1()
//Scenario2()
Scenario3()
