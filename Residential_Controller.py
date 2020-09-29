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
      #  CALL openDoors WITH bestElevator   // not implemented
       # WAIT 5 seconds
       # CALL closeDoors WITH bestElevator        


class callButton:
    def __init__(self, direction, floor):    
        self.direction = direction
        self.floor = floor       


col1 = Column(10, 2)
#e1 = Elevator(8)
direction = 'Up'
floor = 5
#x = col1.elevatorList[0].calcDistanceToFloor(6)
#x = col1.findBestElevator(floor, direction)
col1.elevatorList[0].goToDestinationFloor(floor, direction)
print(col1.elevatorList[0].destinationList)




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
    
    

         

   

    def goToDestinationFloor(requestedFloor, direction):
        destinationList.append(requestedFloor)
      #  CALL openDoors WITH bestElevator   // not implemented
       # WAIT 5 seconds
       # CALL closeDoors WITH bestElevator
  

    def moveElevatorToDestination:
        destination = destinationList[1]        
        if currentFloor > destination:
            direction = 'Down'            
        else: 
            direction = 'Up'             
        while currentFloor != destination:
            pass
            #CALL moveElevator WITH direction
            #Call readCurrentFloor RETURNING currentFloor           
        
        destinationList.pop(0)

print("status = ", col1.status)
print("numberFloors = ", col1.numberFloors)
print("numberOfElevators = ", col1.numberOfElevators)
print("elevatorList = ", col1.elevatorList)
print("callButtonList = ", col1.callButtonList)
for i in range(10):
    print("callButtonList",i , "= ", col1.callButtonList[i]) 
print("elevatorList = ", col1.elevatorList[0].id)

"""










