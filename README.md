# Rocket_Elevators_Controllers
Contains the algorithm files for the elevator controllers for the New Rocket Elevator Solutions for both Residential and Commercial Offers

COMERCIAL CONTROLLER
--------------------

To run in both C# and Golang:

At the end of the code there are 4 functions: Scenario1(), Scenario2(), Scenario3() and Scenario4().
Leave the function you want to run uncommented and the others commented.

The functions creates a Baterry object and sets up the columns and elevators initial positions and directions.

The runScenario functions then runs the simulation with the values entered.




RESIDENCIAL CONTROLLER
----------------------


To run the program in Python:

Run the Residential_Controller.py file in a prompt or open it in Vscode and run it.
A menu with the scenario options provided will appear:

Select one of the following test scenarios option:
        1) Scenario 1
        2) Scenario 2
        3) Scenario 3
        4) Exit)

The function creates a column object and sets up the elevators initial positions and directions.
The floorOfPersonRequestingElevator, requestedFloor and callButtonDirection variables contain the simulation values.
The runScenario function then runs the simulation with the values ​​entered.

Ex:
    
    column1 = Column(10, 2)    
    #Elevator A
    column1.elevatorList[0].currentFloor = 2    
    column1.elevatorList[0].elevatorDirection = 'None'
    
    #Elevator B
    column1.elevatorList[1].currentFloor = 6    
    column1.elevatorList[1].elevatorDirection = 'None'    
    columnNumber = column1   

    #Someone is on floor 3 and wants to go to the 7th floor.
    floorOfPersonRequestingElevator = 3    
    requestedFloor = 7    
    callButtonDirection = 'Up'
    
    CALL runScenario
    
To run the program in JavaScript:
    
Open the file Residential_Controller.js in Vscode.
At the end of the code there are 3 functions: Scenario1 (), Scenario2 (), Scenario3 ().
Leave the function you want to run uncommented and the others commented.
The file was tested using the Code Runner extension.

Functions operation are the same as in Python
