import time

#Main menu
def main():
    option1 = 0      
    while option1 != 4:
        option1 = int(input("""Select one of the following test scenarios option:
        1) Scenario 1
        2) Scenario 2
        3) Scenario 3
        4) Exit)\n"""))
        #Prevents numbers out of the range(1-3)
        while option1 < 1 or option1 > 4:
            option1 = int(input('Please provide a number between 1 and 4\n'))
        #Option 1 of menu
        if option1 == 1:
            Scenario1()           
        #Option 2 of menu
        if option1 == 2:
            Scenario2()
        if option1 == 3:
            Scenario3()

# Classes definition
class Column:
    def __init__(self, numberFloors, numberOfElevators):
        self.status = 'Online'
        self.numberFloors = numberFloors
        self.numberOfElevators = numberOfElevators
        self.elevatorList = []
        self.callButtonList = []
        self.createElevatorList()
        self.createcallButtonList()     

    def createElevatorList(self):
        for i in range(self.numberOfElevators):                             
            self.elevatorList.append(Elevator(i))
    
    def createcallButtonList(self):
        for i in range(self.numberFloors + 1):                                       
            self.callButtonList.append(callButton(i))

    def requestElevator(self,requestedFloor, requestedDirection):          
        bestElevator = self.findBestElevator(requestedFloor, requestedDirection)      
        self.requestFloor(bestElevator.id, requestedFloor )                        

    def requestFloor(self, elevator, requestedFloor):        
        print('Closing doors')                                    
        if requestedFloor > self.elevatorList[elevator].currentFloor:
            elevatorDirection = 'Up'
        else:
            elevatorDirection = 'Down'
        self.elevatorList[elevator].goToDestinationFloor(requestedFloor, elevatorDirection)    

    def findBestElevator(self, requestedFloor, requestedDirection):
        shortestDistance = self.numberFloors
        bestElevatorId = 0
        
        for i in range(self.numberOfElevators):
            if requestedDirection == 'Up' and self.elevatorList[i].elevatorDirection == 'Up' and requestedFloor > self.elevatorList[i].currentFloor:
                distance = abs(requestedFloor - self.elevatorList[i].currentFloor)    
                if distance <= shortestDistance:
                    shortestDistance = distance
                    bestElevatorId = i
        if  bestElevatorId > 0:
            return self.elevatorList[bestElevatorId]

        for i in range(self.numberOfElevators):
            if requestedDirection == 'Down' and self.elevatorList[i].elevatorDirection == 'Down' and requestedFloor < self.elevatorList[i].currentFloor:
                distance = abs(requestedFloor - self.elevatorList[i].currentFloor)    
                if distance <= shortestDistance:
                    shortestDistance = distance
                    bestElevatorId = i                    
        if  bestElevatorId > 0:
            return self.elevatorList[bestElevatorId]

        for i in range(self.numberOfElevators):
            if self.elevatorList[i].elevatorDirection == 'None':
                distance = abs(requestedFloor - self.elevatorList[i].currentFloor) 
                if distance <= shortestDistance:
                    shortestDistance = distance
                    bestElevatorId = i              
        return self.elevatorList[bestElevatorId]                           

class Elevator:
    def __init__(self, id):
        self.id = id
        self.status = 'Online'  
        self.elevatorDirection = 'None'
        self.currentFloor = 1
        self.doors = 'Closed'   
        self.destinationList = []    

    def goToDestinationFloor(self, requestedFloor, elevatorDirection):
        self.destinationList.append(requestedFloor)
        self.elevatorDirection = elevatorDirection 
        if self.elevatorDirection == 'Up':       
            self.destinationList.sort()
        if self.elevatorDirection == 'Down':       
            self.destinationList.sort(reverse=True)            
        destination = self.destinationList[0]
        print('Elevator = ', self.id)
        print('Current floor = ', self.currentFloor)
        print('Going ', self.elevatorDirection),'\n'                                      
        while self.currentFloor != destination:             
            if self.elevatorDirection =='Up':
                #time.sleep(1)
                self.currentFloor += 1
            if self.elevatorDirection =='Down':
                #time.sleep(1)
                self.currentFloor -= 1                       
        self.destinationList = self.destinationList[1:]
        self.elevatorDirection = 'None'
        print('Elevator = ', self.id)       
        print('Destination floor = ', self.currentFloor)        
        print('Opening Doors\n')            


class callButton:
    def __init__(self, floor):    
        self.direction = 'None'
        self.floor = floor       

#Scenarios functions
def Scenario1():
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
    print('***************************************************')
    print('Scenario 1')
    print('Elevator A is Idle at floor 2')
    print('Elevator B is Idle at floor 6')
    print('Someone is on floor 3 and wants to go to the 7th floor.')
    print('Elevator A = 0 is expected to be sent.\n')
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    
    print('***************************************************')
    del columnNumber
    del column1

def Scenario2():
    column2 = Column(10, 2)
    columnNumber = column2      
    #Elevator A
    columnNumber.elevatorList[0].currentFloor = 10
    columnNumber.elevatorList[0].elevatorDirection = 'None'
    #Elevator B
    columnNumber.elevatorList[1].currentFloor = 3
    columnNumber.elevatorList[1].elevatorDirection = 'None'  

    #Someone is on the 1st floor and requests the 6th floor.
    floorOfPersonRequestingElevator = 1
    requestedFloor = 6
    callButtonDirection = 'Up'
    print('***************************************************')
    print('Scenario 2')
    print('Elevator A is Idle at floor 10')
    print('Elevator B is Idle at floor 3\n')
    print('Someone is on the 1st floor and requests the 6th floor.')
    print('Elevator B = 1 should be sent.\n')
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    

    # 2 minutes later, someone else is on the 3rd floor and requests the 5th floor. Elevator B should be sent
    time.sleep(1)     
    print('2 minutes later, someone else is on the 3rd floor and requests the 5th floor.')
    print('Elevator B = 1 should be sent.\n')    
    floorOfPersonRequestingElevator = 3
    requestedFloor = 5
    callButtonDirection = 'Up'
    #(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor) 
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    

    #Finally, a third person is at floor 9 and wants to go down to the 2nd floor. Elevator A should be sent.
    print('Finally, a third person is at floor 9 and wants to go down to the 2nd floor.')
    print('Elevator A = 0 should be sent.\n')
    floorOfPersonRequestingElevator = 9
    requestedFloor = 2
    time.sleep(1)
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    
    print('***************************************************')
    del columnNumber
    del column2

def Scenario3():
    column3 = Column(10, 2)
    columnNumber = column3     
    #Elevator A
    columnNumber.elevatorList[0].currentFloor = 10
    columnNumber.elevatorList[0].elevatorDirection = 'None'
    #Elevator B
    columnNumber.elevatorList[1].currentFloor = 3
    columnNumber.elevatorList[1].elevatorDirection = 'Up'
    columnNumber.elevatorList[1].destinationList.append(6)    

    #Someone is on floor 3 and wants to go to the 7th floor.
    floorOfPersonRequestingElevator = 3
    requestedFloor = 2
    callButtonDirection = 'Up'
    print('***************************************************')
    print('Scenario 3')
    print('Elevator A is Idle at floor 10')
    print('Elevator B is Moving from floor 3 to floor 6')
    print('Someone is on floor 3 and requests the 2nd floor.')
    print('Elevator A = 0 should be sent.\n')
    #(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor)
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)

    #5 minutes later, someone else is on the 10th floor and wants to go to the 3rd. Elevator B should be sent.
    print('5 minutes later, someone else is on the 10th floor and wants to go to the 3rd.')
    print('Elevator B should be sent.\n')
    floorOfPersonRequestingElevator = 10
    requestedFloor = 3
    callButtonDirection = 'Down'
    columnNumber.elevatorList[1].currentFloor = 6
    columnNumber.elevatorList[1].elevatorDirection = 'None'
    columnNumber.elevatorList[1].destinationList = columnNumber.elevatorList[1].destinationList[1:]

    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)
    print('***************************************************')
    del columnNumber
    del column3

def runScenario(columnNumber, elevatorAinitialFloor, elevatorAinitialDirection, elevatorBinitialFloor, elevatorBinitialDirection, floor, requestedFloor, callButtonDirection ):        
      
        #Elevator A
        columnNumber.elevatorList[0].currentFloor = elevatorAinitialFloor
        columnNumber.elevatorList[0].elevatorDirection = elevatorAinitialDirection
        #Elevator B
        columnNumber.elevatorList[1].currentFloor = elevatorBinitialFloor
        columnNumber.elevatorList[1].elevatorDirection = elevatorBinitialDirection        

        #Request Elevator        
        columnNumber.callButtonList[floor].direction = callButtonDirection    
        columnNumber.requestElevator(floor, columnNumber.callButtonList[floor].direction)  
        print('Please wait a moment\n') 
        #time.sleep(1) 
    
        #Request floor        
        print('Going to floor ',requestedFloor,'\n')
        elevator = columnNumber.findBestElevator(floor, columnNumber.callButtonList[floor].direction)    
        columnNumber.requestFloor(elevator.id, requestedFloor)
        #time.sleep(1)        
#-------Test Section-------#
main()