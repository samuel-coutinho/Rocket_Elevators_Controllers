import time

#Main menu
def main():
    option1 = 0
    print('Elevato 0 Floor = ', col1.elevatorList[0].currentFloor)
    print('Elevator 1 Floor = ', col1.elevatorList[1].currentFloor)     
    while True:    
        while option1 != 3:
            option1 = int(input("""Select from one of the following option:
            1) Request a Elevator
            2) Request a Floor
            3) Exit)\n"""))
            #Prevents numbers out of the range(1-3)
            while option1 < 1 or option1 > 3:
                option1 = int(input('Please provide a number between 1 and 3\n'))
            #Option 1 of menu
            if option1 == 1:
                requestedFloor = int(input('Please provide the Floor you are located(1-10): '))
                while requestedFloor < 1 or requestedFloor > 10:
                    requestedFloor = int(input('Please provide a Floor between 1 and 10\n'))
                requestedDirection = int(input('Please provide the Direction you want to go(1 - Up / 2 Down): '))
                while requestedDirection < 1 or requestedDirection > 2:
                    requestedDirection = int(input('Please provide a number between 1 and 2\n'))
                if requestedDirection == 1:
                    requestedDirection = 'Up'
                if requestedDirection == 2:
                    requestedDirection = 'Down'               
                col1.requestElevator(requestedFloor, requestedDirection)
            #Option 2 of menu
            if option1 == 2:
                elevator = int(input('Please provide the Elevator you are inside(0-1): '))
                while elevator < 0 or elevator > 1:
                    elevator = int(input('Please provide a number between 0 and 1\n'))
                requestedFloor = int(input('Please provide the Floor you want to go(1-10): '))
                while requestedFloor < 1 or requestedFloor > 10:
                    requestedFloor = int(input('Please provide a Floor between 1 and 10\n'))
                if requestedFloor == col1.elevatorList[elevator].currentFloor:
                    print('You are already on the floor', requestedFloor)
                    requestedFloor = int(input('Please provide the Floor you want to go(1-10): '))                                          
                col1.requestFloor(elevator, requestedFloor)
            print('Elevato 0 Floor = ', col1.elevatorList[0].currentFloor)
            print('Elevator 1 Floor = ', col1.elevatorList[1].currentFloor)
            
        if option1 == 3:
            break

# Classes definition
class Column:
    def __init__(self, numberFloors, numberOfElevators):
        self.status = 'Online'
        self.numberFloors = numberFloors
        self.numberOfElevators = numberOfElevators
        self.elevatorList = []
        self.callButtonList = []
        self.createElevatorList()     

    def createElevatorList(self):
        for i in range(self.numberOfElevators):                             
            self.elevatorList.append(Elevator(i))
    
    def createcallButtonList(self):
        for i in range(self.numberFloors):                             
            self.callButtonList.append(callButton(i))

    def requestElevator(self,requestedFloor, requestedDirection):          
        bestElevator = self.findBestElevator(requestedFloor, requestedDirection)      
        self.requestFloor(bestElevator.id, requestedFloor )                        

    def requestFloor(self, elevator, requestedFloor):                                    
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
        self.destinationList.sort()
        destination = self.destinationList[0]                                     
        if self.currentFloor != destination:
            print('Elevator = ', self.id)
            print('Current floor = ', self.currentFloor)
            print('direction = ', self.elevatorDirection)  
            if self.elevatorDirection =='Up':
                time.sleep(2)
                self.currentFloor += 1
            if self.elevatorDirection =='Down':
                time.sleep(2)
                self.currentFloor -= 1                       
        self.destinationList = self.destinationList[1:]
        self.elevatorDirection = 'None'        
        print('Destination floor = ', self.currentFloor)
        print('direction = ', self.elevatorDirection)
        print('Opening Doors')        

class callButton:
    def __init__(self, floor):    
        self.direction = 'None'
        self.floor = floor       

#Initialization
col1 = Column(10, 2)
main()