import time


class Column:
    def __init__(self, numberFloors, numberOfElevators):
        self.status = 'Online'
        self.numberFloors = numberFloors
        self.numberOfElevators = numberOfElevators
        self.elevatorList = []
        self.callButtonList = [['none'] for i in range(numberFloors)]
        self.createElevatorList()   
    

    def createElevatorList(self):
        for i in range(self.numberOfElevators):                             
            self.elevatorList.append(Elevator(i))

    def requestFloor(self, i, requestedFloor):        
        if requestedFloor > self.elevatorList[i].currentFloor:
            direction = 'Up'
        else:
            direction = 'Down'
        self.elevatorList[i].goToDestinationFloor(requestedFloor, direction)
      

    def findBestElevator(self, requestedFloor, direction):
        shortestDistance = self.numberFloors
        bestElevator = 0
        for i in range(self.numberOfElevators):   #FOR EACH elevator in ElevatorList
            distance = self.elevatorList[i].calcDistanceToFloor(requestedFloor)
            if direction == 'Down' and self.elevatorList[i].currentFloor > requestedFloor:
                bestElevator = self.calcShortestDistance(distance, shortestDistance, i) # RETURNING bestElevator            
            if direction == 'Up' and self.elevatorList[i].currentFloor < requestedFloor:
                bestElevator = self.calcShortestDistance(distance, shortestDistance, i) # RETURNING bestElevator            
            if direction == 'None' and bestElevator == 0:
                bestElevator = self.calcShortestDistance(distance, shortestDistance, i) # RETURNING bestElevator
            else:
                bestElevator = self.calcShortestDistance(distance, shortestDistance, i) # RETURNING bestElevator   
        return bestElevator

    def calcShortestDistance(self, distance, shortestDistance, i): # RETURNING bestElevator
        if distance <= shortestDistance:
            shortestDistance = distance
            bestElevator = self.elevatorList[i]
        return bestElevator    

class Elevator:
    def __init__(self, id):
        self.id = id
        self.status = 'Online'  
        self.directon = 'None'
        self.currentFloor = 1
        self.doors = 'Closed'   
        self.destinationList = []

    def calcDistanceToFloor(self, requestedFloor): 
        distance = abs(requestedFloor - self.currentFloor)     
        return distance

    def goToDestinationFloor(self, requestedFloor, direction):
        self.destinationList.append(requestedFloor)
        self.directon = direction        
        self.moveElevatorToDestination()
      #  CALL openDoors WITH bestElevator   // not implemented
       # WAIT 5 seconds
       # CALL closeDoors WITH bestElevator
        
    def moveElevatorToDestination(self):
        destination = self.destinationList[0]                                     
        while self.currentFloor != destination:
            print('Current floor = ', self.currentFloor)
            print('direction = ', self.directon)  
            if self.directon =='Up':
                time.sleep(2)
                self.currentFloor += 1
            if self.directon =='Down':
                time.sleep(2)
                self.currentFloor -= 1                       
        self.destinationList.pop(0)
        self.directon = 'None'
        print('Destination floor = ', self.currentFloor)
        print('direction = ', self.directon)
        print('Opening Doors')        


class callButton:
    def __init__(self, direction, floor):    
        self.direction = direction
        self.floor = floor       


col1 = Column(10, 2)
#(elevator, floor)
col1.requestFloor(0, 5)
time.sleep(5)
col1.requestFloor(0, 3)




"""
    def requestElevator(requestedFloor, direction):
        pass
        bestElevator = findBestElevator(requestedFloor, direction)  #RETURNING bestElevator
        elevator.id[bestElevator].goToDestinationFloor(requestedFloor, direction)                        #goToDestinationFloor OF bestElevator WITH _requestedFloor AND _requestedDirection  
        
    def requestFloor(elevator, requestedFloor):
        if requestedFloor > currentFloor:
            direction = 'Down'
        else:
            direction = 'Up'
        #CALL goToDestinationFloor OF elevator WITH _requestedFloor AND _requestedDirection     


"""










