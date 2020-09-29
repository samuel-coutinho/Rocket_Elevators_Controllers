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
    
    def createcallButtonList(self):
        for i in range(self.numberFloors):                             
            self.callButtonList.append(callButton(i))

    def requestElevator(self,requestedFloor, direction):        
        bestElevator = self.findBestElevator(requestedFloor, direction)  #RETURNING bestElevator
        self.elevatorList[bestElevator.id].goToDestinationFloor(requestedFloor, direction)

    def requestFloor(self, elevator, requestedFloor):        
        if requestedFloor > self.elevatorList[elevator].currentFloor:
            direction = 'Up'
        else:
            direction = 'Down'
        self.elevatorList[elevator].goToDestinationFloor(requestedFloor, direction)
      

    def findBestElevator(self, requestedFloor, direction):
        shortestDistance = self.numberFloors
        bestElevator = 0
        for i in range(self.numberOfElevators):   #FOR EACH elevator in ElevatorList
            distance = self.elevatorList[i].calcDistanceToFloor(requestedFloor)
            if direction == 'Down' and self.elevatorList[i].direction == 'Down' and self.elevatorList[i].currentFloor > requestedFloor:
                bestElevator = self.calcShortestDistance(distance, shortestDistance, i) # RETURNING bestElevator            
            if direction == 'Up' and self.elevatorList[i].direction == 'Up' and self.elevatorList[i].currentFloor < requestedFloor:
                bestElevator = self.calcShortestDistance(distance, shortestDistance, i) # RETURNING bestElevator            
            if self.elevatorList[i].direction == 'None' and bestElevator == 0:
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
        self.direction = 'None'
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
        
    def moveElevatorToDestination(self):
        destination = self.destinationList[0]                                     
        while self.currentFloor != destination:
            print('Elevator = ', self.id)
            print('Current floor = ', self.currentFloor)
            print('direction = ', self.directon)  
            if self.directon =='Up':
                time.sleep(2)
                self.currentFloor += 1
            if self.directon =='Down':
                time.sleep(2)
                self.currentFloor -= 1                       
        self.destinationList = self.destinationList[1:]
        self.direction = 'None'        
        print('Destination floor = ', self.currentFloor)
        print('direction = ', self.directon)
        print('Opening Doors')
        print('destinationList = ', self.destinationList)      


class callButton:
    def __init__(self, floor):    
        self.direction = 'None'
        self.floor = floor       


col1 = Column(10, 2)
#(elevator, floor)
#col1.requestFloor(0, 5)
#time.sleep(5)
#col1.requestFloor(0, 3)
col1.requestElevator(4, 'Up')
time.sleep(5)
col1.requestElevator(7, 'Up')


"""
    def requestElevator(requestedFloor, direction):
        pass
        bestElevator = findBestElevator(requestedFloor, direction)  #RETURNING bestElevator
        elevator.id[bestElevator].goToDestinationFloor(requestedFloor, direction)       #goToDestinationFloor OF bestElevator WITH _requestedFloor AND _requestedDirection  
             
"""










